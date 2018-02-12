using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class UIMain : MonoBehaviour
{
    // 最大得分
    private const float MaxScore = 1200f;

    // 时间
    private float time;
    private Coroutine mTimingCoroutine;
    private Transform mTimeCircle;
    private Transform mTimeLabel;

    // 双方得分
    private Transform mBody;
    private Transform mBlueSlider;
    private Transform mRedSlider;

    // 问题和回答
    private Transform mQuestion;
    private List<Transform> mAllAnswer;

    // 当局的 AI
    private RedAI mAI;

    // 双方操作
    private bool mPlayerFinish;
    private bool mAIFinish;

    private float mPlayerScore;
    private float mAIScore;

    private int mRound;

    private EventManager manager;

    // 当局的所有题目
    List<Question> curQuestions;

    void Start()
    {
        Init();
        InitEventManager();
        OneStart();
    }

    private void Init()
    {
        mTimeCircle = transform.Find("Top/Time/Sprite");
        mTimeLabel = transform.Find("Top/Time/Label");

        mBody = transform.Find("Body");
        mBlueSlider = mBody.Find("Blue");
        mRedSlider = mBody.Find("Red");

        mQuestion = mBody.Find("Question");
        mAllAnswer = new List<Transform>();
        for (int i = 0; i < 4; i++)
        {
            Transform tmpAnswer = mBody.Find("Answer/"+ (char)(65+i));
            if (tmpAnswer != null)
            {
                UIEventListener.Get(tmpAnswer.gameObject).onClick += OnClickAnswer;
                mAllAnswer.Add(tmpAnswer);
            }
        }

        Transform back = transform.Find("Top/Back");
        if (back != null)
        {
            UIEventListener.Get(back.gameObject).onClick += (go) => SceneManager.LoadScene("RankPK");
        }

        // 新的一局新的AI
        GameObject ai = new GameObject("AI");
        mAI = ai.AddComponent<RedAI>();

        // 得分从0开始
        //UIHelper.SetLabel(mBlueSlider, "Score", "0");
        //UIHelper.SetLabel(mRedSlider, "Score", "0");
        UIHelper.SetSlider(mBlueSlider, 0);
        UIHelper.SetSlider(mRedSlider, 0);
        //UIHelper.SetActive(mBlueSlider, "Score", false);
        //UIHelper.SetActive(mRedSlider, "Score", false);

        // 第一回合
        mRound = 1;

        mPlayerScore = 0;
        mAIScore = 0;

        // 初始化题目 -- 5道题测试
        int[] tmpIndex = new int[5] { 0,1,2,3,4};
        curQuestions = LocalInfo.GetSinglon().GetQuestion(tmpIndex);
    }

    private void InitEventManager()
    {
        manager = EventManager.GetSinglon();

        manager.RegisterMsgHandler((int)PlayerEvent.AI_Reply, AIAnswer);
    }

    // 一回合开始
    private void OneStart()
    {
        // 通知UI回合开始，
        BaseEvent data = new BaseEvent();
        data.pEventID = (int)PlayerEvent.BeginReply;
        manager.NotifyEvent(data.pEventID, data);

        mBody.GetComponent<UIPlayTween>().Play(true);

        // 所有牌变白
        for (int i = 0; i < mAllAnswer.Count; i++)
        {
            UIHelper.SetColor(mAllAnswer[i], Color.white);
        }
        // 所有牌能点
        AnswerEnable(true);

        // 计时从0开始
        mTimingCoroutine = StartCoroutine(Timing());

        // 更新题目
        Question tmpQ = curQuestions[mRound-1];
        UIHelper.SetLabel(mQuestion, tmpQ.text);
        UIHelper.SetLabel(mAllAnswer[0], "Label", tmpQ.A);
        UIHelper.SetLabel(mAllAnswer[1], "Label", tmpQ.B);
        UIHelper.SetLabel(mAllAnswer[2], "Label", tmpQ.C);
        UIHelper.SetLabel(mAllAnswer[3], "Label", tmpQ.D);

        mPlayerFinish = false;
        mAIFinish = false;
    }

    IEnumerator Timing()
    {
        time = 10;
        UISprite timeSprite = mTimeCircle.GetComponent<UISprite>();
        while (time >= 0)
        {
            time -= 1 * Time.deltaTime;
            UIHelper.SetLabel(mTimeLabel, time.ToString("0.0"));
            timeSprite.fillAmount = time / 10f;
            yield return null;
        }
    }

    // 玩家答题
    private void OnClickAnswer(GameObject go)
    {
        if (mPlayerFinish)
        {
            return;
        }

        // 所有牌不能点
        mPlayerFinish = true;
        AnswerEnable(false);

        string click = go.name;
        
        if (mRound == 1) UIHelper.SetActive(mBlueSlider, "Score", true);
        // 判断正确与否

        if(curQuestions[mRound-1].real == click)
        {
            // 正确 判断加多少分
            mPlayerScore += 240 * time / 10f;
            UIHelper.SetLabel(mBlueSlider, "Score", mPlayerScore.ToString("0"));
            UIHelper.SetSlider(mBlueSlider, mPlayerScore / MaxScore);

            UIHelper.SetColor(go.transform, Color.green);
        }
        else
        {
            // 错误 不加分
            UIHelper.SetColor(go.transform, Color.red);
        }

        // 判断游戏是否结束
        OneEnd();
    }

    // 获取 AI 的答案
    private void AIAnswer(BaseEvent varData)
    {
        if (varData == null || mAIFinish)
        {
            return;
        }

        ExData<int> data = varData as ExData<int>;
        int answer = data.data;
        
        mAIFinish = true;
        if (mRound == 1) UIHelper.SetActive(mRedSlider, "Score", true);
        // 判断正确与否

        if(curQuestions[mRound-1].real == ((char)(65+answer)).ToString())
        {
            // 正确 判断加多少分
            mAIScore += 240 * time / 10f;
            UIHelper.SetLabel(mRedSlider, "Score", mAIScore.ToString("0"));
            UIHelper.SetSlider(mRedSlider, mAIScore / MaxScore);
            UIHelper.SetColor(mAllAnswer[answer], Color.green);
        }
        else
        {
            // 错误不加分
            UIHelper.SetColor(mAllAnswer[answer], Color.red);
        }

        // 判断游戏是否结束
        OneEnd();
    }

    // 一回合结束
    private void OneEnd()
    {
        if (!mAIFinish || !mPlayerFinish)
        {
            return;
        }

        // 停止计时
        if (mTimingCoroutine != null)
            StopCoroutine(mTimingCoroutine);
        StartCoroutine(TweenFinish());
    }

    IEnumerator TweenFinish()
    {
        // 等一秒答案消失
        yield return new WaitForSeconds(1);
        mBody.GetComponent<UIPlayTween>().Play(false);
        // 等0.5秒结算
        yield return new WaitForSeconds(0.5f);
        if (mRound < 5)
        {
            mRound++;
            OneStart();
        }
        else
        {
            // 判断得分 -> 胜利或失败
            Debug.LogError("GameOver");
            transform.Find("UIFinish").GetComponent<TweenScale>().PlayForward();
        }
    }

    private void AnswerEnable(bool enable)
    {
        for (int i = 0; i < mAllAnswer.Count; i++)
        {
            mAllAnswer[i].GetComponent<BoxCollider>().enabled = enabled;
        }
    }
}
