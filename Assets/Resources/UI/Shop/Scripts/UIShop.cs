using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIShop : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        ApplicationSDK.SendLogStepToService(102, "进入商店", true);
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

        Transform mTop = transform.Find("Body/Top");
        if (mTop != null)
        {
            UIEventListener.Get(mTop.gameObject).onClick += OnClickShopItem;
        }
	}

    Transform mCurBuy;
    /// <summary>
    /// 购买商品
    /// </summary>
    /// <param name="go"></param>
    void OnClickShopItem(GameObject go)
    {
        mCurBuy = go.transform;
        if (ApplicationSDK.isNeedTwoPop())
        {
            UIDialog.OpenBox("是否确认购买该商品？", Buy, null);
        }
        else
        {
            Buy();
        }
    }

    void Buy()
    {
        switch (mCurBuy.name)
        {
            case "Top":
                ApplicationSDK.PayMoney("Top", 2, "新手大礼包");
                Player.GetSingle().pGold += 100;
                break;
            case "Small":
                ApplicationSDK.PayMoney("Small", 5, "少量王者币");
                Player.GetSingle().pGold += 50;
                break;
            case "Middle":
                ApplicationSDK.PayMoney("Middle", 10, "中量王者币");
                Player.GetSingle().pGold += 100;
                break;
            case "Big":
                ApplicationSDK.PayMoney("Big", 20, "大量王者币");
                Player.GetSingle().pGold += 300;
                break;
            default:
                return;
        }

    }
}
