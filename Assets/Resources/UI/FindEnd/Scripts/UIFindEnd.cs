using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIFindEnd : MonoBehaviour {

	// Use this for initialization
	void Start () {
        // 测试， 直接开始游戏
        //Invoke("StartGame", 3);
        UIPlayTween bg = transform.Find("BG").GetComponent<UIPlayTween>();

        Transform blue = transform.Find("Blue");
        if (blue != null)
        {
            EventDelegate.Add(blue.GetComponent<TweenPosition>().onFinished,() => bg.Play(true));
        }
	}

    public void StartGame()
    {
        Invoke("GOMain", 1);
    }

    private void GOMain()
    {
        SceneManager.LoadScene("Main");
    }
}
