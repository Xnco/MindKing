using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIMain : MonoBehaviour
{
    // 最大得分
    private const float MaxScore = 1200f;

    // 时间
    private Transform mTimeSlider;
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

    private int mRound;

    private EventManager manager;

    void Start()
    {
        Init();
        InitEventManager();
        OneStart();
    }

    private void Init()
    {
        mTimeSlider = transform.Find("Top/Time/Sprite");
        mTimeLabel = transform.Find("Top/Time/Label");

        mBody = transform.Find("Body");
        mBlueSlider = mBody.Find("Blue");
        mRedSlider = mBody.Find("Red");

        mQuestion = mBody.Find("Question");
        mAllAnswer = new List<Transform>();
        for (int i = 0; i < 4; i++)
        {
            Transform tmpAnswer = mBody.Find("Answer/"+ i);
            if (tmpAnswer != null)
            {
                UIEventListener.Get(tmpAnswer.gameObject).onClick += OnClickAnswer;
                mAllAnswer.Add(tmpAnswer);
            }
        }

        // 新的一局新的AI
        GameObject ai = new GameObject("AI");
        mAI = ai.AddComponent<RedAI>();

        // 第一回合
        mRound = 1;
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

        mPlayerFinish = false;
        mAIFinish = false;
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
        UIHelper.SetColor(go.transform, Color.green);
        // 判断正确与否

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
        UIHelper.SetColor(mAllAnswer[answer], Color.red);

        mAIFinish = true;
        // 判断正确与否

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

        mBody.GetComponent<UIPlayTween>().Play(false);

        StartCoroutine(TweenFinish());
    }

    IEnumerator TweenFinish()
    {
        // 等两秒结算
        yield return new WaitForSeconds(2);
        if (mRound < 5)
        {
            mRound++;
            OneStart();
        }
        else
        {
            // 判断得分 -> 胜利或失败
            Debug.LogError("GameOver");
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
