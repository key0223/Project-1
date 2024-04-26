using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public string playerId { get; set; }
    public string playerName { get; set; }
    public string playerPyogukName { get; set; }
    public Player player;
    

    //public SignInPanel signInPanel { get; set; }

    private void Start()
    {
        ShowPlayerLoginPanel();
    }

    public async void ShowPlayerLoginPanel()
    {
        GameObject obj = ResourceManager.Instantiate(await ResourceManager.LoadResource<GameObject>(AddressSetting.LOGIN_PANEL_ADDRESS));
        
        //signInPanel = obj.GetComponent<SignInPanel>();
    }
    
    public async void ShowPlayerSetupPanel()
    {
        GameObject obj = ResourceManager.Instantiate(await ResourceManager.LoadResource<GameObject>(AddressSetting.PLAYER_SETUP_PANEL_ADDRESS));

        //signInPanel = obj.GetComponent<SignInPanel>();
    }




    public void GetReward()
    {

    }

    public void Decrese()
    {

    }

    public void Increase()
    {

    }
}
