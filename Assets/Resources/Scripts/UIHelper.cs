using UnityEngine;
using System.Collections;

public static class UIHelper
{
    // 改变 Slider 
    public static void SetSlider(Transform parent, string path, float value)
    {
        if (parent == null)
        {
            return;
        }

        Transform target = parent.Find(path);
        SetSlider(target, value);
    }

    public static void SetSlider(Transform slider, float value)
    {
        if (slider == null)
        {
            return;
        }

        UISlider tmp = slider.GetComponent<UISlider>();
        if (tmp == null)
        {
            return;
        }

        tmp.value = value;
    }

    // 改变 Label
    public static void SetLabel(Transform parent, string path, string value)
    {
        if (parent == null)
        {
            return;
        }

        Transform target = parent.Find(path);
        SetLabel(target, value);
    }

    public static void SetLabel(Transform label, string value)
    {
        if (label == null)
        {
            return;
        }

        UILabel tmp = label.GetComponent<UILabel>();
        if (tmp == null)
        {
            return;
        }
        tmp.text = value;
    }

    // 改变 Color
    public static void SetColor(Transform parent, string path, Color value)
    {
        if (parent == null)
        {
            return;
        }

        Transform target = parent.Find(path);
        SetColor(target, value);
    }

    public static void SetColor(Transform target, Color value)
    {
        if (target == null)
        {
            return;
        }

        UIWidget tmp = target.GetComponent<UIWidget>();
        if (tmp == null)
        {
            return;
        }
        tmp.color = value;
    }

    // 设置激活状态
    public static void SetActive(Transform parent, string path, bool active)
    {
        if (parent == null)
        {
            return;
        }

        Transform target = parent.Find(path);
        SetActive(target, active);
    }

    public static void SetActive(Transform target, bool active)
    {
        if (target == null)
        {
            return;
        }

        target.gameObject.SetActive(active);
    }

    // 设置激活状态
    public static void SetSpriteName(Transform parent, string path, string name)
    {
        if (parent == null)
        {
            return;
        }

        Transform target = parent.Find(path);
        SetSpriteName(target, name);
    }

    public static void SetSpriteName(Transform target, string name)
    {
        if (target == null)
        {
            return;
        }

        UISprite sp = target.GetComponent<UISprite>();
        if (sp != null)
        {
            sp.spriteName = name;
        }
    }
}
