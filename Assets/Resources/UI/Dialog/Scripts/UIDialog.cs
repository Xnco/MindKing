using UnityEngine;
using System.Collections;

public delegate void ClickDelegate();

public class UIDialog : MonoBehaviour 
{
    static UIDialog()
    {
        mDialogPrefab = new Transform[2];
        mOpenDialog = new UIDialog[2];
    }

    static Transform[] mDialogPrefab;   // 弹框预设

    static UIDialog[] mOpenDialog; // 已打开窗口

    Transform BackButton;
    Transform ConButton;
    Transform BGText;

    event ClickDelegate mBackEvent;
    event ClickDelegate mConEvent;

	Transform money;

    private void Awake()
    {
        Init();
    }

    void Init()
    {
        BGText = transform.Find("Label");
        BackButton = transform.Find("Canel");
        if (BackButton != null)
        {
            UIEventListener.Get(BackButton.gameObject).onClick += OnClickBack;
        }
        ConButton = transform.Find("OK");
        if (ConButton != null)
        {
            UIEventListener.Get(ConButton.gameObject).onClick += OnClickCon;
        }
    }

    /// <summary>
    /// 传入确定和取消的回调方法
    /// </summary>
    /// <param name="Confirm"></param>
    /// <param name="Back"></param>
    public static void OpenBox(string text, ClickDelegate Confirm, ClickDelegate Back)
    {
        if (mOpenDialog[0] == null)
        {
            // 窗口没打开 -> 要打开窗口
            // 有没有这个资源 
            if (mDialogPrefab[0] == null)
            {
                mDialogPrefab[0] = Resources.Load<GameObject>("UI/Dialog").transform.Find("Dialog");
            }
            Transform tmpBox = Instantiate(mDialogPrefab[0]) ;
            UIDialog mUIPopover = tmpBox.GetComponent<UIDialog>();
            mOpenDialog[0] = mUIPopover;

            // 设置到当前的UIRoot
            tmpBox.SetParent(GameObject.FindObjectOfType<UIRoot>().transform);
            tmpBox.transform.localScale = Vector3.one;
			tmpBox.transform.position = Vector3.zero;
        }
        mOpenDialog[0].gameObject.SetActive(true);
        UIHelper.SetLabel(mOpenDialog[0].BGText, text);

        mOpenDialog[0].mBackEvent = Back;
        mOpenDialog[0].mConEvent = Confirm;
    }

    /// <summary>
    /// 点 X
    /// </summary>
    /// <param name="go"></param>
    void OnClickBack(GameObject go)
    {
        if (mBackEvent != null)
        {
            mBackEvent();
        }
        Close();
    }

    /// <summary>
    /// 点确认
    /// </summary>
    /// <param name="go"></param>
    void OnClickCon(GameObject go)
	{
        if (mConEvent != null)
        {
            mConEvent();
        }
        Close();
    }

    void Close()
    {
        // 点击后清空事件
        mBackEvent = null;
        mConEvent = null;

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        mOpenDialog[0] = null;
    }
}
