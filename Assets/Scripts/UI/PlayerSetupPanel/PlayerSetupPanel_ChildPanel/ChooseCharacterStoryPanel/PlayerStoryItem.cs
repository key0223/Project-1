using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStoryItem : MonoBehaviour
{
    public ChooseStoryPanel chooseStory;

    public Text jobName;
    public string jobDescription;
    public Stat stat;
    public string benefit;

    public Button storyBtn;

    
   public void SetUp(params object[] objects)
    {
        this.chooseStory = (ChooseStoryPanel)objects[0];
        jobName.text = (string)objects[1];
        jobDescription = (string)objects[2];
        //stat = (Stat)objects[3];
        benefit = (string)objects[3];

        SetButton();
    }

    void SetButton()
    {
        if (storyBtn == null) return;

        storyBtn.onClick.AddListener(() =>
        {
            chooseStory.SetStoryData(jobDescription, stat, benefit);
            PlayerSetupPanel.ins.playerHistory.characterStory = jobName.text;
        });
    }
}
