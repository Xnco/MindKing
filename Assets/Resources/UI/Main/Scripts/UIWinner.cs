using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIWinner : MonoBehaviour {

    void Awake()
    {
        Transform back = transform.Find("Body/Again");
        if (back != null)
        {
            UIEventListener.Get(back.gameObject).onClick +=
                (go) =>
                {
<<<<<<< HEAD
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

=======
>>>>>>> parent of c794ebd... Update Logic and Atlas
                    SceneManager.LoadScene("RankPK");

                    // 胜利 增加星星
                    Player.GetSingle().pExp++;
                };
        }
    }
}
