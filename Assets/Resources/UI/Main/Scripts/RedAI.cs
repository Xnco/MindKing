using UnityEngine;
using System.Collections;

public class RedAI : MonoBehaviour
{
    EventManager manager;

    public void Awake()
    {
        manager = EventManager.GetSinglon();

        // 注册回合开始的时间
        manager.RegisterMsgHandler((int)PlayerEvent.BeginReply, AI_BeginReply);
    }

    void AI_BeginReply(BaseEvent varData)
    {
        // AI开始答题， n 秒后回答 n
        float time = Random.Range(0f, 10f);

        StartCoroutine(AI_Reply(time));
    }

    IEnumerator AI_Reply(float time)
    {
        yield return new WaitForSeconds(time);

        // AI 随机答一题
        ExData<int> data = new ExData<int>();
        data.data = Random.Range(0,4);
        data.pEventID = (int)PlayerEvent.AI_Reply;

        manager.NotifyEvent(data.pEventID, data);
    }

    void OnDestroy()
    {
        manager.UnRegisterMsgHandler((int)PlayerEvent.BeginReply, AI_BeginReply);
    }
}
