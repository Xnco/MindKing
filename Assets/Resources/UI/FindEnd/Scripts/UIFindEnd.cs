using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIFindEnd : MonoBehaviour {

	// Use this for initialization
	void Start () {
        // 测试， 直接开始游戏
        Invoke("StartGame", 3);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void StartGame()
    {
        SceneManager.LoadScene("Main");
    }
}
