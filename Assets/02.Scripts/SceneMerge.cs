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
        // 병합할 씬들의 경로 설정
        string[] scenePaths = new string[]
        {
            "Assets/Scenes/Scene1.unity",
            "Assets/Scenes/Scene2.unity"
            // 추가 씬 경로들
        };

        // 씬 병합 실행
    }
}
