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
        //������Ʈ ���� Ȯ��
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp.Create();
            //�����ͺ��̽� ��θ� ������ �ν��Ͻ��� �ʱ�ȭ
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
        //������ ����ȭ
        string jsonPosition = JsonUtility.ToJson(serializableData);
        // �÷��̾� ���̵� �ؿ� Database�̸����� ����ȭ�� ������ ����
        _reference.Child("players").Child(Manager.ins.playerManager.playerId).UpdateChildrenAsync(nameData);
        _reference.Child("players").Child(Manager.ins.playerManager.playerId).Child("Player").SetValueAsync(jsonPosition);

    }
    public void Load()
    {
        Database database = Manager.ins.databaseManager;

        string playerId = Manager.ins.playerManager.playerId;

        //����� ������ ��������
        _reference.Child("players").Child(playerId).Child("Player").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            //�۾� ó�� �Ϸ� ����
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to Load Database");
            }
            //�۾��� �Ϸ�Ǹ�
            else if (task.IsCompleted)
            {
                //DataSnapshot -> �����͸� �����ϴ� ����
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
        Debug.Log("�����͸� �ҷ��Խ��ϴ�.");

    }

    /*
    public void Save()
    {
        string playerId = Manager.ins.playerManager.playerId;

        PlayerManager playerManager = Manager.ins.playerManager;
        QuestManager questManager = Manager.ins.questManager;

        //����Ʈ, �÷��̾� ���� ��... ���̺�

        SerializableData serializableData = new SerializableData(playerManager.player, questManager.questData);
        //������ ����ȭ
        string jsonPosition = JsonUtility.ToJson(serializableData);
        // �÷��̾� ���̵� �ؿ� Database�̸����� ����ȭ�� ������ ����
        _reference.Child(playerId).Child("Database").SetValueAsync(jsonPosition);

        Debug.Log("�����͸� �����߽��ϴ�");
    }

    public void Load()
    {
        Database database = Manager.ins.databaseManager;

        string playerId = Manager.ins.playerManager.playerId;

        //����� ������ ��������
        _reference.Child(playerId).Child("Database").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            //�۾� ó�� �Ϸ� ����
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to Load Database");
            }
            //�۾��� �Ϸ�Ǹ�
            else if (task.IsCompleted)
            {
                //DataSnapshot -> �����͸� �����ϴ� ����
                DataSnapshot snapshot = task.Result;

                if (!snapshot.Exists)
                    database.Create();

                string jsonPosition = snapshot.Value.ToString();
                SerializableData serializablePosition = JsonUtility.FromJson<SerializableData>(jsonPosition);
                serializablePosition.Load();
            }
        });

        Debug.Log("�����͸� �ҷ��Խ��ϴ�.");

    }
    */


    public void Create()
    {
        Debug.Log("�����ͺ��̽��� �����մϴ�.");

        //Save();
    }
}