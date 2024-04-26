using Firebase;
using Firebase.Extensions;
using Firebase.Database;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using JetBrains.Annotations;
using System.Collections.Generic;

[System.Serializable]
public class SerializableData
{
    public Player player;
   // public QuestData questData;


    public SerializableData( Player player/*, QuestData questData*/)
    {
      
        this.player = player;
      //  this.questData = questData;
    }

    public void Load()
    {
        PlayerManager playerManager = Manager.ins.playerManager;
       // QuestManager questManager = Manager.ins.questManager;

        playerManager.player = player;
        //questManager.questData = questData;
    }
}

public class FirebaseManager : IDatabase
{
    private DatabaseReference _reference;
    public InputField text;


    public void Setup()
    {
        //업데이트 내용 확인
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp.Create();
            //데이터베이스 경로를 설정해 인스턴스를 초기화
            _reference = FirebaseDatabase.DefaultInstance.RootReference;

        });
    }

    public void Save()
    {
        PlayerManager playerManager = Manager.ins.playerManager;
        //QuestManager questManager = Manager.ins.questManager;

        Dictionary<string, object> nameData = new Dictionary<string, object>();
        nameData["playerName"] = playerManager.playerName;
        nameData["playerPyogukName"] = playerManager.playerPyogukName;

        SerializableData serializableData = new SerializableData(playerManager.player /*, questManager.questData*/);
        //데이터 직렬화
        string jsonPosition = JsonUtility.ToJson(serializableData);
        // 플레이어 아이디 밑에 Database이름으로 직렬화된 데이터 저장
        _reference.Child("players").Child(Manager.ins.playerManager.playerId).UpdateChildrenAsync(nameData);
        _reference.Child("players").Child(Manager.ins.playerManager.playerId).Child("Player").SetValueAsync(jsonPosition);

    }
    public void Load()
    {
        Database database = Manager.ins.databaseManager;

        string playerId = Manager.ins.playerManager.playerId;

        //저장된 데이터 가져오기
        _reference.Child("players").Child(playerId).Child("Player").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            //작업 처리 완료 여부
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to Load Database");
            }
            //작업이 완료되면
            else if (task.IsCompleted)
            {
                //DataSnapshot -> 데이터를 저장하는 단위
                DataSnapshot snapshot = task.Result;

                if (!snapshot.Exists)
                    database.Create();

                string jsonPosition = snapshot.Value.ToString();
                SerializableData serializablePosition = JsonUtility.FromJson<SerializableData>(jsonPosition);
                serializablePosition.Load();

                Manager.ins.city_MainUI.OnGameStart();
                
            }
        });
        #region playerName
        _reference.Child("players").Child(playerId).Child("playerName").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            DataSnapshot snapshot = task.Result;

            string jsonPosition = snapshot.Value.ToString();
            Manager.ins.playerManager.playerName = jsonPosition;
            Debug.Log(jsonPosition);
        });
        _reference.Child("players").Child(playerId).Child("playerPyogukName").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            DataSnapshot snapshot = task.Result;

            string jsonPosition = snapshot.Value.ToString();
            Manager.ins.playerManager.playerPyogukName = jsonPosition;
            Debug.Log(jsonPosition);
        });

        #endregion
        Debug.Log("데이터를 불러왔습니다.");

    }

    /*
    public void Save()
    {
        string playerId = Manager.ins.playerManager.playerId;

        PlayerManager playerManager = Manager.ins.playerManager;
        QuestManager questManager = Manager.ins.questManager;

        //퀘스트, 플레이어 정보 등... 세이브

        SerializableData serializableData = new SerializableData(playerManager.player, questManager.questData);
        //데이터 직렬화
        string jsonPosition = JsonUtility.ToJson(serializableData);
        // 플레이어 아이디 밑에 Database이름으로 직렬화된 데이터 저장
        _reference.Child(playerId).Child("Database").SetValueAsync(jsonPosition);

        Debug.Log("데이터를 저장했습니다");
    }

    public void Load()
    {
        Database database = Manager.ins.databaseManager;

        string playerId = Manager.ins.playerManager.playerId;

        //저장된 데이터 가져오기
        _reference.Child(playerId).Child("Database").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            //작업 처리 완료 여부
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to Load Database");
            }
            //작업이 완료되면
            else if (task.IsCompleted)
            {
                //DataSnapshot -> 데이터를 저장하는 단위
                DataSnapshot snapshot = task.Result;

                if (!snapshot.Exists)
                    database.Create();

                string jsonPosition = snapshot.Value.ToString();
                SerializableData serializablePosition = JsonUtility.FromJson<SerializableData>(jsonPosition);
                serializablePosition.Load();
            }
        });

        Debug.Log("데이터를 불러왔습니다.");

    }
    */


    public void Create()
    {
        Debug.Log("데이터베이스를 생성합니다.");

        //Save();
    }
}