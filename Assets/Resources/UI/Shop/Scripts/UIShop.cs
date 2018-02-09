using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIShop : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        Transform backBtn = transform.Find("Back");
        if (backBtn != null)
        {
            UIEventListener.Get(backBtn.gameObject).onClick += (go) => SceneManager.LoadScene("Start");
        }

        Transform mGrid = transform.Find("Body/Grid");
        BoxCollider[] allItem = mGrid.GetComponentsInChildren<BoxCollider>();
        foreach (var item in allItem)
        {
            UIEventListener.Get(item.gameObject).onClick += OnClickShopItem;
        }

        Transform mFree = transform.Find("Free");
        if (mFree != null)
        {
            UIEventListener.Get(mFree.gameObject).onClick += (go) => { };
        }
	}
    
    /// <summary>
    /// 购买商品
    /// </summary>
    /// <param name="go"></param>
    void OnClickShopItem(GameObject go)
    {

    }
}
