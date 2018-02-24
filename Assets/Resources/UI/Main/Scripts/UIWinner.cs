using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIWinner : MonoBehaviour
{
    Transform mCrown;
    EventManager manager;

    bool iswinner;

    void Awake()
    {
        mCrown = transform.Find("Crown");
        manager = EventManager.GetSinglon();
        manager.RegisterMsgHandler((int)PlayerEvent.GameOver, OnGameOver);

        Transform back = transform.Find("Body/Again");
        if (back != null)
        {
            UIEventListener.Get(back.gameObject).onClick +=
                (go) =>
                {
                    // 胜利 增加星星
                    if (iswinner)
                    {
                        Player.GetSingle().pExp++;
                    }
                    else
                    {
                        // 失败 隐藏皇冠
                        UIHelper.SetActive(mCrown, false);
                        Player.GetSingle().pExp--;
                    }

                    SceneManager.LoadScene("RankPK");
                };
        }
    }

    void OnGameOver(BaseEvent varData)
    {
        if (varData == null)
        {
            return;
        }

        ExData<bool> tmpdata = varData as ExData<bool>;
        iswinner = tmpdata.data;

        GetComponent<TweenScale>().PlayForward();
    }

    void OnDestroy()
    {
        manager.UnRegisterMsgHandler((int)PlayerEvent.GameOver, OnGameOver);
    }
}
