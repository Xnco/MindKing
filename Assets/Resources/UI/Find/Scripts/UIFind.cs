using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIFind : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(3);

        SceneManager.LoadScene("LoadFinish");
    }
}
