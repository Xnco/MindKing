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
    private Transform mRoundText;
    private Transform mQuestion;
    private List<Transform> mAllAnswer;

    // 当局的 AI
    private RedAI mAI;

    // 双方操作 - 玩家和AI是否完成答题
    private bool mPlayerFinish;
    private Transform mPlayerSelect;
    private bool mAIFinish;
    private Transform mAISelect;

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

        // 回合开始， 第一回合
        StartCoroutine(OneStart());
    }

    private void Init()
    {
        mTimeCircle = transform.Find("Top/Time/Sprite");
        mTimeLabel = transform.Find("Top/Time/Label");

        mBody = transform.Find("Body");
        mBlueSlider = mBody.Find("Blue");
        mRedSlider = mBody.Find("Red");

        mRoundText = mBody.Find("Round");
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
    IEnumerator OneStart()
    {
        // 先显示第几题 -- 动画播完才出题
        UIHelper.SetLabel(mRoundText, string.Format("第{0}题", mRound));
        mRoundText.GetComponent<TweenScale>().PlayForward();
        yield return new WaitForSeconds(1.3f);
        mRoundText.GetComponent<TweenScale>().PlayReverse();
        yield return new WaitForSeconds(0.3f);

        // 更新题目文本
        Question tmpQ = curQuestions[mRound - 1];
        UIHelper.SetLabel(mQuestion, tmpQ.text);
        UIHelper.SetLabel(mAllAnswer[0], "Label", tmpQ.A);
        UIHelper.SetLabel(mAllAnswer[1], "Label", tmpQ.B);
        UIHelper.SetLabel(mAllAnswer[2], "Label", tmpQ.C);
        UIHelper.SetLabel(mAllAnswer[3], "Label", tmpQ.D);

        // 通知AI回合开始，
        BaseEvent data = new BaseEvent();
        data.pEventID = (int)PlayerEvent.BeginReply;
        manager.NotifyEvent(data.pEventID, data);

        // 第一回合的处理
        if (mRound == 1)
        {
            mBody.GetComponent<UIPlayTween>().tweenGroup = 3;
            mBody.GetComponent<UIPlayTween>().Play(true);
        }

        // 题目先出来
        mBody.GetComponent<UIPlayTween>().tweenGroup = 1;
        mBody.GetComponent<UIPlayTween>().Play(true);

        // 1s 后再出选项
        yield return new WaitForSeconds(1f);
        mBody.GetComponent<UIPlayTween>().tweenGroup = 2;
        mBody.GetComponent<UIPlayTween>().Play(true);

        // 所有牌变白 - 圆圈隐藏
        for (int i = 0; i < mAllAnswer.Count; i++)
        {
            UIHelper.SetColor(mAllAnswer[i], Color.white);
            UIHelper.SetActive(mAllAnswer[i], "Right", false);
            UIHelper.SetActive(mAllAnswer[i], "Left", false);
        }
        // 所有牌能点
        AnswerEnable(true);

        // 计时从0开始
        mTimingCoroutine = StartCoroutine(Timing());

        mPlayerFinish = false;
        mAIFinish = false;
    }

    IEnumerator Timing()
    {
        time = 9.9f;
        UISprite timeSprite = mTimeCircle.GetComponent<UISprite>();
        while (time >= 0)
        {
            time -= 1 * Time.deltaTime;
            UIHelper.SetLabel(mTimeLabel, time.ToString("0.0"));
            timeSprite.fillAmount = time / 10f;
            yield return null;
        }

        // 时间到 -> 所有牌不能点 玩家不得分
        // 暂不考虑AI
        mPlayerFinish = true;
        AnswerEnable(false);

        OneEnd(); // 回合结束
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
        mPlayerSelect = go.transform; // 记录玩家的选择
        // 显示左边的圈
        UIHelper.SetActive(go.transform, "Left", true);

        if (mRound == 1) UIHelper.SetActive(mBlueSlider, "Score", true);
        // 判断正确与否
        if(curQuestions[mRound-1].real == click)
        {
            // 正确 判断加多少分
            mPlayerScore += 240 * time / 10f;
            UIHelper.SetColor(go.transform, Color.green);
        }
        else
        {
            // 错误 不加分
            UIHelper.SetColor(go.transform, Color.red);
        }
        UIHelper.SetLabel(mBlueSlider, "Score", mPlayerScore.ToString("0"));
        UIHelper.SetSlider(mBlueSlider, mPlayerScore / MaxScore);

        if (mAIFinish)
        {
            // AI先答， 现在再出AI的答案
            UIHelper.SetActive(mAISelect, "Right", true);
            if (click == mAISelect.name)
            {
                //AI 对了
                UIHelper.SetColor(mAISelect, Color.green);
            }
            else
            {
                UIHelper.SetColor(mAISelect, Color.red);
            }
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
        mAISelect = mAllAnswer[answer]; // 记录AI的选择
        
        if (mPlayerFinish) // 玩家先答题， 显示右边的圈
            UIHelper.SetActive(mAllAnswer[answer], "Right", true);

        mAIFinish = true;
        if (mRound == 1) UIHelper.SetActive(mRedSlider, "Score", true);
        // 判断正确与否
        if(curQuestions[mRound-1].real == ((char)(65+answer)).ToString())
        {
            // 正确 判断加多少分
            mAIScore += 240 * time / 10f;
            UIHelper.SetSlider(mRedSlider, mAIScore / MaxScore);
            if (mPlayerFinish) // 玩家先答题才出答案
                UIHelper.SetColor(mAllAnswer[answer], Color.green);
        }
        else
        {
            // 错误不加分
            if (mPlayerFinish) // 玩家先答题才出答案
                UIHelper.SetColor(mAllAnswer[answer], Color.red);
            else
            {
                // 玩家没答题， AI的进度条闪一下
                TweenColor tc = mRedSlider.Find("Sprite").GetComponent<TweenColor>();
                tc.ResetToBeginning();
                tc.PlayForward();
            }
        }

        UIHelper.SetLabel(mRedSlider, "Score", mAIScore.ToString("0"));

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

        // 显示正确答案
        Transform realA = mAllAnswer.Find((go)=> curQuestions[mRound - 1].real==go.name);
        UIHelper.SetColor(realA, Color.green);

        // 停止计时
        if (mTimingCoroutine != null)
            StopCoroutine(mTimingCoroutine);
        StartCoroutine(TweenFinish());
    }

    IEnumerator TweenFinish()
    {
        // 等一秒答案消失
        yield return new WaitForSeconds(1);
        mBody.GetComponent<UIPlayTween>().tweenGroup = 1;
        mBody.GetComponent<UIPlayTween>().Play(false);
        mBody.GetComponent<UIPlayTween>().tweenGroup = 2;
        mBody.GetComponent<UIPlayTween>().Play(false);
        // 等一会结算
        yield return new WaitForSeconds(1f);
        if (mRound < 5)
        {
            mRound++;
            // 回合开始， 第n回合
            StartCoroutine(OneStart());
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
