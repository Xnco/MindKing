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
                    SceneManager.LoadScene("RankPK");

                    // 胜利 增加星星
                    Player.GetSingle().pExp++;
                };
        }
    }
}
