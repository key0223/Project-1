using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager ins;

    public QuestEventManager questEventManager;
    public PlayerManager playerManager;
    public ResourceManager resourceManager;
    public Database databaseManager;

    public FirebaseFindData firebaseFindData;
    public TableManager stringTableManager;

    [Header("임시 세팅")]
    public City_MainUI city_MainUI;

    private void Awake()
    {
        if (ins != null)
        {
            ins = null;
            Destroy(gameObject);
            return;
        }

        ins = this;

    }
}
