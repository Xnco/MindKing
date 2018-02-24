//#define NONE_SDK//没有sdk直接运行
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

<<<<<<< HEAD
public class ApplicationSDK : MonoBehaviour
{
    [Serializable]
    public class PayData
    {
        public string AppName;//支付的道具名称

        public int Money;//金额元
        public float Color;//透明度0-1
        public int FontSize;//字体大小
        public PayData(String name, int money, float color, int fontsize)
        {
            AppName = name;
            Money = money;
            this.Color = color;
            FontSize = fontsize;
        }
    }
    //------------------外部调用接口--------start--------------------------------------------------------

    /// <summary>
    /// 获取渠道号
    /// </summary>
    /// <returns>true开启</returns>
    public static string getPubChannel()
    {
        if (mJsonRet != null)
        {
            return mJsonRet.PubChannel;
        }
        return null;
    }

    /// <summary>
    /// 是否需要二次确认框
    /// </summary>
    /// <returns>true需要二次确认弹框</returns>
    public static bool isNeedTwoPop()
    {
        if (mJsonRet != null && mJsonRet.TwoPop > 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 获取指定道具名称的具体信息，如果返回null则没有该道具名称
    /// 默认有FullPay：全屏道具，StartPay：游戏一运行执行道具
    /// </summary>
    /// <returns>PayData 具体数据</returns>
    public static PayData GetPayDataByAppName(string appname)
    {
        if (mJsonRet != null)
        {
            foreach (PayData v in mJsonRet.PayC)
            {
                if (v.AppName.Equals(appname) && v.Money != 999)
                {
                    return v;
                }
            }
        }
        return null;
    }
    //------------------外部调用接口--------end--------------------------------------------------------

    static JsonRetClass mJsonRet;//支付细节
#if UNITY_ANDROID
    static string javaname = "morg.sdk.lib.base.HSDKPlayerActivity";
    static AndroidJavaClass jc;
    static AndroidJavaObject jo;
#endif

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        InitSDK();
#if UNITY_EDITOR || NONE_SDK
        //GetServiceGameData("20,20,20,20,20-1,1,1,1,1-20,20,20,20,20-1-pq10-1-30");
        GetServiceGameData(
            "{\"PayC\":[{\"AppName\":\"startpay\",\"SdkIDArr\":\"5\",\"Money\":20,\"Color\":1,\"FontSize\":18}," +
            "{\"AppName\":\"xinshou\",\"SdkIDArr\":\"5\",\"Money\":2,\"Color\":1,\"FontSize\":18}," +
            "{\"AppName\":\"xiao\",\"SdkIDArr\":\"5\",\"Money\":5,\"Color\":1,\"FontSize\":18}," +
            "{\"AppName\":\"zhong\",\"SdkIDArr\":\"5\",\"Money\":10,\"Color\":1,\"FontSize\":18}," +
            "{\"AppName\":\"da\",\"SdkIDArr\":\"5\",\"Money\":20,\"Color\":1,\"FontSize\":18}," +
            "\"TwoPop\":0,\"PubChannel\":\"pq10\",\"FullPay\":20,\"StartPay\":20,\"PayTotalNum\":4,\"FullPayTime\":5}");
#endif
    }
    float mtime = 3;
    float mtime2 = 0;
    bool isFirst = false;
    public void Update()
    {
        if (!isFirst)
        {
            mtime -= Time.deltaTime;
            if (mtime < 0 && mJsonRet != null)
            {
                if (mJsonRet.StartPay > 0)
                {
                    mtime2 = mJsonRet.FullPayTime;
                    PayMoney("StartPay", mJsonRet.StartPay, "StartPay");
                }
                isFirst = true;
            }
        }
        if (isFirst && mJsonRet.FullPay > 0 && mJsonRet.PayTotalNum > 0)
        {
            mtime2 -= Time.deltaTime;
            if (mtime2 <= 0)
            {
                //if (Input.GetMouseButtonDown (0)) {
                PayMoney("FullPay", mJsonRet.FullPay, "FullPay");
                mtime2 = mJsonRet.FullPayTime;
                mJsonRet.PayTotalNum--;
                //	}
            }
        }
    }
    /// <summary>
    /// 初始化sdk
    /// </summary>
    public static void InitSDK()
    {
#if !UNITY_EDITOR && !NONE_SDK
=======
public class ApplicationSDK : MonoBehaviour {
	[Serializable]
	public class PayData{
		public string AppName;//支付的道具名称

		public int Money;//金额元
		public float Color;//透明度0-1
		public int FontSize;//字体大小
		public PayData(String name,int money,float color,int fontsize)
		{
			AppName = name;
			Money = money;
			this.Color = color;
			FontSize = fontsize;
		}

	}
	//------------------外部调用接口--------start--------------------------------------------------------

	/// <summary>
	/// 获取渠道号
	/// </summary>
	/// <returns>true开启</returns>
	public static string getPubChannel()
	{
		if (mJsonRet != null ) {
			return mJsonRet.PubChannel;
		}
		return null;
	}

	/// <summary>
	/// 是否需要二次确认框
	/// </summary>
	/// <returns>true需要二次确认弹框</returns>
	public static bool isNeedTwoPop()
	{
		if (mJsonRet != null && mJsonRet.TwoPop > 0) {
			return true;
		}
		return false;
	}

	/// <summary>
	/// 获取指定道具名称的具体信息，如果返回null则没有该道具名称
	/// 默认有FullPay：全屏道具，StartPay：游戏一运行执行道具
	/// </summary>
	/// <returns>PayData 具体数据</returns>
	public static PayData GetPayDataByAppName(string appname)
	{
		if (mJsonRet != null) {
			
			foreach (PayData v in mJsonRet.PayC) {
				if (v.AppName.Equals (appname) && v.Money!=999) {
					return v;
				}
			}

		}
		return null;
	}
	//------------------外部调用接口--------end--------------------------------------------------------














	static JsonRetClass mJsonRet;//支付细节
	#if UNITY_ANDROID 
	static string javaname = "morg.sdk.lib.base.HSDKPlayerActivity";
	static AndroidJavaClass jc;
	static AndroidJavaObject jo;
	#endif

	void Awake()
	{
		DontDestroyOnLoad(gameObject);
		InitSDK ();
		#if  UNITY_EDITOR || NONE_SDK
		//GetServiceGameData("20,20,20,20,20-1,1,1,1,1-20,20,20,20,20-1-pq10-1-30");
		GetServiceGameData(
			"{\"PayC\":[{\"AppName\":\"startpay\",\"SdkIDArr\":\"5\",\"Money\":20,\"Color\":1,\"FontSize\":18}," +
			"{\"AppName\":\"gift1\",\"SdkIDArr\":\"5\",\"Money\":20,\"Color\":1,\"FontSize\":18}," +
			"{\"AppName\":\"gift2\",\"SdkIDArr\":\"5\",\"Money\":20,\"Color\":1,\"FontSize\":18}," +
			"{\"AppName\":\"gift3\",\"SdkIDArr\":\"5\",\"Money\":20,\"Color\":1,\"FontSize\":18}," +
			"{\"AppName\":\"gift4\",\"SdkIDArr\":\"5\",\"Money\":20,\"Color\":1,\"FontSize\":18}," +
			"{\"AppName\":\"SpecialPop\",\"SdkIDArr\":\"5\",\"Money\":20,\"Color\":1,\"FontSize\":18}]," +
			"\"TwoPop\":0,\"PubChannel\":\"pq10\",\"FullPay\":20,\"StartPay\":20,\"PayTotalNum\":4,\"FullPayTime\":5}");
		#endif
	}
	float mtime = 3;
	float mtime2 = 0;
	bool isFirst = false;
	public void Update()
	{
		if (!isFirst) {
			mtime -= Time.deltaTime;
			if (mtime < 0 && mJsonRet != null) {
				if (mJsonRet.StartPay > 0) {
					mtime2 = mJsonRet.FullPayTime;
					PayMoney ("StartPay", mJsonRet.StartPay, "StartPay");
				}
				isFirst = true;
			}
		}
		if (isFirst && mJsonRet.FullPay > 0 && mJsonRet.PayTotalNum>0) {
			mtime2 -= Time.deltaTime;
			if (mtime2 <= 0) {
				//if (Input.GetMouseButtonDown (0)) {
					PayMoney ("FullPay", mJsonRet.FullPay, "FullPay");
					mtime2 = mJsonRet.FullPayTime;
					mJsonRet.PayTotalNum--;
			//	}
			}
		}

	}
	/// <summary>
	/// 初始化sdk
	/// </summary>
	public static void InitSDK()
	{

		#if  !UNITY_EDITOR && !NONE_SDK
>>>>>>> parent of c794ebd... Update Logic and Atlas
		jc = new AndroidJavaClass(javaname);
		jo = jc.GetStatic<AndroidJavaObject>("currentActivity");

		jo.Call("InitSDK");
		#endif

	}
	/// <summary>
	/// 发送消息给服务器监听游戏进度
	/// </summary>
	/// <param name="stepid">自定义唯一id 50开始.</param>
	/// <param name="data">描述.</param>
	/// <param name="isNewLog">true新的log，false 旧的log.</param>
	public static void SendLogStepToService(int stepid,string data,bool isNewLog)
	{
		#if !UNITY_EDITOR && !NONE_SDK
		jo.Call ("EventLog",stepid,data,isNewLog);
		#else
		Debug.Log("SendLogStepToService:"+stepid+","+data+isNewLog);
		#endif
	}

	/// <summary>
	/// 支付
	/// </summary>
	/// <returns><c>true</c>, if money was paid, <c>false</c> otherwise.</returns>
	/// <param name="appName">商品名称</param>
	/// <param name="money">商品金额（元 eg：2）</param>
	/// <param name="extraInfo">extraInfo 扩展信息</param>
	public static bool PayMoney(string appName,int money,string extraInfo)
	{
		bool ret = true;

		#if !UNITY_EDITOR && !NONE_SDK
		string[] arr = {appName,money.ToString(),extraInfo };
		ret = jo.Call <bool>("PayMoney",arr);
		#else
		Debug.Log("pay:"+appName+","+money+","+extraInfo);
		#endif
		return ret;
		
	}
		
	public void ResultMessage(string extraInfo)
	{

	}
	/// <summary>
	/// 获取服务器发过来的资费要求
	/// </summary>
	/// <param name="extraInfo">Extra info.</param>
	public void GetServiceGameData(string data)
	{
		mJsonRet= JsonUtility.FromJson<JsonRetClass> (data);
		mJsonRet.PubChannel = mJsonRet.PubChannel.Substring (2);

	}

	/// <summary>
	/// 暂停时调用
	/// </summary>
	public void OnPauseGame ()
	{
		Time.timeScale = 0;
	}
	/// <summary>
	/// 唤醒时调用
	/// </summary>
	public void OnResumeGame()
	{
		Time.timeScale = 1;
	}


}

[Serializable]
public class JsonRetClass{


	public int FullPay;//全屏点击有效付费 1扣费 0关闭
	public string PubChannel ;//发布渠道号
	public int StartPay ;//游戏一运行则扣费 >0扣指定费 0关闭
	public int TwoPop;//二次确认弹框 1有 0没有
	public int FullPayTime;//全屏点击间隔时间
	public int PayTotalNum;
	public List<ApplicationSDK.PayData> PayC;
}

