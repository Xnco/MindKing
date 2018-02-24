using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class UIRankPK : MonoBehaviour
{

    Player mPlayer;

    // Use this for initialization
    void Start () {
        mPlayer = Player.GetSingle();

        UIHelper.SetLabel(transform.Find("Title/Gold"), mPlayer.pGold.ToString());

        Transform backBtn = transform.Find("Top/Back");
        if (backBtn != null)
        {
            UIEventListener.Get(backBtn.gameObject).onClick += (go) => SceneManager.LoadScene("Start");
        }

        List<Transform> mAllItem = new List<Transform>();

        Debug.Log("Level : " + mPlayer.pLevel);
        for (int i = 0; i < mPlayer.pLevel + 1; i++)
        {
            Transform tmpItem = transform.Find("List/Grid/Item" + i);
            if (tmpItem != null)
            {
                UIHelper.SetActive(tmpItem, true);
                UIHelper.SetActive(tmpItem, "BG/Mask", false);

                // 低于当前等级的星星都变白
                if (i < mPlayer.pLevel - 1)
                {
                    Transform tmpStar = tmpItem.Find("Star");
                    for (int j = 0; j < tmpStar.childCount; j++)
                    {
                        UIHelper.SetSpriteName(tmpStar.GetChild(j), "star_white");
                    }

                    UIEventListener.Get(tmpItem.gameObject).onClick += OnClickPK;
                }
                else if (i == mPlayer.pLevel - 1)
                {
                    UIEventListener.Get(tmpItem.gameObject).onClick += OnClickPK;
                }

                mAllItem.Add(tmpItem);
            }
        }

        // 最后一个遮罩
        UIHelper.SetActive(mAllItem[mAllItem.Count - 1], "BG/Mask", true);

        // 当前等级的星星
        Transform curStar = mAllItem[mPlayer.pLevel - 1].Find("Star");
        for (int j = 0; j < mPlayer.pExp; j++)
        {
            UIHelper.SetSpriteName(curStar.GetChild(j), "star_white");
        }
    }
	
    private void OnClickPK(GameObject go)
    {
        // 得到当前选中的关卡
        mPlayer.pCurLevel = int.Parse(go.name.Substring(go.name.Length - 1)) + 1;

        // 减去关卡需要的金币
        mPlayer.pGold -= mPlayer.mNeedGold[mPlayer.pCurLevel];

        SceneManager.LoadScene("Find");
    }
}
