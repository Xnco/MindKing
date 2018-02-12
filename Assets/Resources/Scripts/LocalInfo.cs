using UnityEngine;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public class LocalInfo
{
    private static LocalInfo instance;

    public static LocalInfo GetSinglon()
    {
        if (instance == null)
        {
            instance = new LocalInfo();
        }
        return instance;
    }

#if UNITY_EDITOR
    string textPath = Application.dataPath + "/StreamingAssets/Topic/Topic.xml";
#elif UNITY_ANDROID
    string textPath = Application.dataPath;
#endif

    List<Question> allQuestion;

    private LocalInfo()
    {
        allQuestion = new List<Question>();

        // 读取 XML
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(textPath);
        XmlNode root = xmlDoc.SelectSingleNode("root/questionbank");
        XmlNodeList list = root.SelectNodes("topic");
        foreach (XmlNode item in list)
        {
            Question q = new Question();
            q.text = item.SelectSingleNode("question").InnerText;
            q.A = item.SelectSingleNode("A").InnerText;
            q.B = item.SelectSingleNode("B").InnerText;
            q.C = item.SelectSingleNode("C").InnerText;
            q.D= item.SelectSingleNode("D").InnerText;
            q.real = item.SelectSingleNode("real").InnerText;
            allQuestion.Add(q);
        }
    }

    public List<Question> GetQuestion(int[] indexs)
    {
        List<Question> tmpQ = new List<Question>();
        for (int i = 0; i < indexs.Length; i++)
        {
            tmpQ.Add(allQuestion[i]);
        }
        return tmpQ;
    }
}

public class Question
{
    public string text;
    public string A;
    public string B;
    public string C;
    public string D;
    public string real;

    public override string ToString()
    {
        return string.Format("题目：{0} \n A: {1}\nB:{2}\nC:{3}\nD:{4}\nreal:{5}", text, A, B, C, D,  real);
    }
}
