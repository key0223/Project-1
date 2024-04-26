using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City_MainUI : MonoBehaviour
{

    CanvasGroup cityMainCanvas;

    public GameObject myPyogukUI;
    public GameObject myCastleUI;
    public GameObject myLocationUI;

    [Header("임시 데이터")]
    public int trust = 75;
    public int fame = 75;
    public int money = 10000;

    public int population = 2555;
    public int security = 84;
    public string state;

    private void Awake()
    {
        cityMainCanvas = GetComponent<CanvasGroup>();
    }
    public void SetInteractable(bool isInteractable)
    {
        cityMainCanvas.blocksRaycasts = isInteractable;
    }


    public void OnGameStart()
    {
        Invoke("GameStart", 0.5f);
    }
    public void GameStart()
    {
        myPyogukUI.SetActive(true);
        myCastleUI.SetActive(true);
        myLocationUI.SetActive(true);
    }
}
