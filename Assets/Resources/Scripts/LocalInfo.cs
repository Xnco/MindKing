using UnityEngine;
using System.IO;
using System.Collections;

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
    string textPath = Application.dataPath + "/StreamingAssets/Topic";
#elif UNITY_ANDROID
    string textPath = Application.dataPath;
#endif

    private LocalInfo()
    {
        DirectoryInfo info = new DirectoryInfo(textPath);

        
    }
}
