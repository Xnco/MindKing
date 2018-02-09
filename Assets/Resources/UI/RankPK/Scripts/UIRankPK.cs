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
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
