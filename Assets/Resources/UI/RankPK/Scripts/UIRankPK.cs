using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIRankPK : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Transform backBtn = transform.Find("Top/Back");
        if (backBtn != null)
        {
            UIEventListener.Get(backBtn.gameObject).onClick += (go) => SceneManager.LoadScene("Start");
        }

        for (int i = 0; i < 3; i++)
        {
            Transform tmpItem = transform.Find("List/Item" + i);
            if (tmpItem != null)
            {
                UIEventListener.Get(tmpItem.gameObject).onClick += (go) => SceneManager.LoadScene("Find") ;
            }
        }
	}
	

}
