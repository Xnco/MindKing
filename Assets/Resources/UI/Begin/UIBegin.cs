using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIBegin : MonoBehaviour {

	// Use this for initialization
	IEnumerator Start () {
        // 清空数据， 方便测试
        PlayerPrefs.DeleteAll();

        // 读取本地题库 -- 测试
        StartCoroutine(LocalInfo.LoadXML());
        Player.GetSingle();
        yield return null;

        SceneManager.LoadScene("Start");
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
