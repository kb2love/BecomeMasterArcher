using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;

public class SceneMerge
{
    [MenuItem("Tools/Merge Scenes")]
    public static void MergeScenes()
    {
        // ������ ������ ��� ����
        string[] scenePaths = new string[]
        {
            "Assets/Scenes/Scene1.unity",
            "Assets/Scenes/Scene2.unity"
            // �߰� �� ��ε�
        };

        // �� ���� ����
    }
}
