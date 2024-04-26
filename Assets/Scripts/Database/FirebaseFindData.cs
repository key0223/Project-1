using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FirebaseFindData : MonoBehaviour
{
    public DatabaseReference reference;

    public void Awake()
    {
        SetUp();

    }
    public void Start()
    {
    }
    public async void SetUp()
    {
        await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp.Create();
            reference = FirebaseDatabase.DefaultInstance.RootReference;
        });
    }

    public async Task<bool> CheckifNameExists(string child, string name)
    {

        DataSnapshot snapshot = await reference.Child("players").GetValueAsync();
        bool isNameExists = false;

        foreach (var childSnapshot in snapshot.Children)
        {
            var value = childSnapshot.Child(child).Value;
            if (value != null)
            {
                string currentPlayerName = value.ToString();

                Debug.Log("검색");
                if (currentPlayerName == name)
                {
                    Debug.Log($"{child}하위에 이름 {name}이 존재합니다.");
                    isNameExists = true;
                    break;
                }
            }
        }
        return isNameExists;
    }
  
}

