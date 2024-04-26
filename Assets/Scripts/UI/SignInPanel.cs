using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignInPanel : MonoBehaviour
{
    public InputField playerIdInput;
    City_MainUI cityMainUI;

    private void Awake()
    {
        cityMainUI = FindObjectOfType<City_MainUI>();
    }
    public void Btn_Confirm()
    {
        Manager.ins.playerManager.playerId = playerIdInput.text;
        Manager.ins.databaseManager.Load();
        //임시코드
        //cityMainUI.OnGameStart();

        DestroyPanel();
    }
    public void DestroyPanel()
    {
        Destroy(gameObject);
    }
}
