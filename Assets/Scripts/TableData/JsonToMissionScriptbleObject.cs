//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using Unity.VisualScripting;
//using UnityEditor;
//using UnityEngine;
//using UnityEngine.UI;
//[System.Serializable]
//public class MissionData
//{
//    public string category;
//    public string questId;
//    public string description;
//    public string action;
//    public string[] targets;
//    public string initialSuccessValue;
//    public int needSuccessToComplete;
//    public bool canReceiveReportsDuringCompletion;
//    public int fame;
//    public string destination;
//    public int date;
//    public int deposit;
//    public int remainder;
//    public string route1;
//    public string route2;
//    public string route3;
//    public string route4;
//    public string route5;
//    public string date1;
//    public string date2;
//    public string date3;
//    public string date4;
//    public string date5;
//    public int escort;
//    public int laborer;
//    public int horse;
//    public int cart;
//    public int ship;
//}

//[System.Serializable]
//public class MissionDataList
//{
//    public MissionData[] array;
//}

//public class JsonToMissionScriptableObject : EditorWindow
//{
//    string quest_stringTablePath = "Assets/Res/StringTable/StringTable_QuestTest.json";
//    string missionSavePath = "Assets/Scripts/Quest/Scriptable/Mission";

//    [MenuItem("Custom Tools/Convert Json to ScriptableObject")]

//    private static void OpenWindow()
//    {
//        EditorWindow.GetWindow(typeof(JsonToMissionScriptableObject));
//    }

//    private void OnGUI()
//    {
//        GUILayout.Label("ScriptableObject Generator", EditorStyles.boldLabel);

//        GUILayout.Space(10f);

//        quest_stringTablePath = EditorGUILayout.TextField("Json File Path", quest_stringTablePath);
//        missionSavePath = EditorGUILayout.TextField("Save Path", missionSavePath);

//        GUILayout.Space(10f);

//        if (GUILayout.Button("Generate ScriptableObject"))
//        {
//            GenerateScriptableObject();
//        }
//    }
//    private void GenerateScriptableObject()
//    {
//        if (string.IsNullOrEmpty(quest_stringTablePath) || string.IsNullOrEmpty(missionSavePath))
//        {
//            Debug.LogError("Json file path or save path is empty!");
//            return;
//        }

//        if (!File.Exists(quest_stringTablePath))
//        {
//            Debug.LogError($"Json file does not exist at path: {quest_stringTablePath}");
//            return;
//        }

//        string json = File.ReadAllText(quest_stringTablePath);
//        MissionDataList missionDataList = JsonUtility.FromJson<MissionDataList>(json);

//        if (missionDataList == null || missionDataList.array == null)
//        {
//            Debug.LogError("Failed to parse JSON data!");
//            return;
//        }

//        for (int i = 0; i < missionDataList.array.Length; i++)
//        {
//            MissionData missionData = missionDataList.array[i];
//            Mission mission = ScriptableObject.CreateInstance<Mission>();

//            // mission.Category = Enum.TryParse(missionData.category, out Category resultType) ? resultType : Category.None;
//            mission.QuestId = missionData.questId;
//            mission.Description = missionData.description;
//            //  mission.Action = missionData.action;
//            //  mission.Targets = missionData.targets;
//            // mission.InitialSuccessValue = Enum.TryParse(missionData.initialSuccessValue, out SuccessValue resultValue) ? resultValue : SuccessValue.None;
//            mission.NeedSuccessToComplete = missionData.needSuccessToComplete;
//            mission.CanReceiveReportsDuringCompletion = missionData.canReceiveReportsDuringCompletion;
//            mission.Fame = missionData.fame;
//            mission.Destination = missionData.destination;
//            mission.Date = missionData.date;
//            mission.Deposit = missionData.deposit;
//            mission.Remainder = missionData.remainder;
//            mission.Route1 = missionData.route1;
//            mission.Route2 = missionData.route2;
//            mission.Route3 = missionData.route3;
//            mission.Route4 = missionData.route4;
//            mission.Route5 = missionData.route5;
//            mission.Date1 = missionData.date1;
//            mission.Date2 = missionData.date2;
//            mission.Date3 = missionData.date3;
//            mission.Date4 = missionData.date4;
//            mission.Date5 = missionData.date5;
//            mission.Escort = missionData.escort;
//            mission.Laborer = missionData.laborer;
//            mission.Horse = missionData.horse;
//            mission.Cart = missionData.cart;
//            mission.Ship = missionData.ship;

//            string saveFilePath = $"{missionSavePath}/Mission{i}.asset";
//            AssetDatabase.CreateAsset(mission, saveFilePath);
//            AssetDatabase.SaveAssets();
//        }

//        AssetDatabase.Refresh();
//        Debug.Log("ScriptableObjects generated and saved!");
//    }
//}


