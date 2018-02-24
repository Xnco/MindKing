using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player
{
    private static Player mPlayer;

    public static Player GetSingle()
    {
        if (mPlayer == null)
        {
            mPlayer = new Player();
        }
        return mPlayer;
    }

    private Player()
    {
        mNeedExp = new List<int>() { 0,1,2,3,3 };
        mNeedGold = new List<int>() { 0, 20,50,90, 140 };

        pGold = PlayerPrefs.GetInt("Gold", 500);
        pLevel = PlayerPrefs.GetInt("Level", 1);
        mExp = PlayerPrefs.GetInt("Exp", 0);
    }

    private int mGold;
    private int mLevel;
    private int mExp;
    private int mCurLevel;
    private List<int> mNeedExp;
    public List<int> mNeedGold;

    public int pGold
    {
        get
        {
            return mGold;
        }
        set
        {
            mGold = value;
            PlayerPrefs.SetInt("Gold", mGold);
        }
    }

    public int pLevel
    {
        get
        {
            return mLevel;
        }
        set
        {
            mLevel = value > 4 ? 4 : value;
            PlayerPrefs.SetInt("Level", mLevel);
        }
    }

    public int pExp
    {
        get
        {
            return mExp;
        }
        set
        {
            mExp = value;
            if (mCurLevel != 0 && mExp >= mNeedExp[mCurLevel])
            {
                mExp = 0;
                pLevel++;
            }

            PlayerPrefs.SetInt("Exp", mExp);
        }
    }

    public int pCurLevel
    {
        get
        {
            return mCurLevel;
        }
        set
        {
            mCurLevel = value;
        }
    }
}
