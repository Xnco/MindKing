using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIWinner : MonoBehaviour {

    void Awake()
    {
        Transform back = transform.Find("Body/Again");
        if (back != null)
        {
            UIEventListener.Get(back.gameObject).onClick += (go) => SceneManager.LoadScene("RankPK");
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
