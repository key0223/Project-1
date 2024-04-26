using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSetupPanel : MonoBehaviour
{
    public static PlayerSetupPanel ins;

    
    public PlayerHistory playerHistory;

    [SerializeField] private PlayerSetupPanel_ChildPanel[] childPanels;

    

    private void Awake()
    {
        if(ins != null)
        {
            ins = null;
            Destroy(gameObject);
            return;
        }

        ins = this;

       
        
    }

    private void Start()
    {
        for (int i = 0; i < childPanels.Length; i++)
        {
            childPanels[i].gameObject.SetActive(false);

            if (i == 0)
                childPanels[i].gameObject.SetActive(true);
        }
    }

    public void SetInitialPlayerHistory()
    {
        Manager.ins.playerManager.player.playerHistory = this.playerHistory;
        Manager.ins.playerManager.playerName = this.playerHistory.characterName;
        Manager.ins.playerManager.playerPyogukName = this.playerHistory.pyogukName;
        Manager.ins.databaseManager.Save();
    }
}
