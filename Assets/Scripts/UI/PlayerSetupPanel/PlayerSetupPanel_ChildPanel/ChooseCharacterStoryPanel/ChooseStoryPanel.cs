using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Stat
{
    public int vitality;
    public int patience;
    public int scheme;
    public int social;
    public int intelligence;
}
[System.Serializable]
public class Job
{
    public string jobName;
    public string jobDescription;
    public int vitality;
    public int patience;
    public int scheme;
    public int social;
    public int intelligence;
    public string benefit;
}
[System.Serializable]
public class jobList
{
    public List<Job> jobs;
}
public class ChooseStoryPanel : PlayerSetupPanel_ChildPanel
{
    jobList jobList;

    [Header("Inspector Set")]
    public Transform jobRoot;
    public GameObject jobPrefab;
    // UI
    public Text jobDescriptionText;
    public Text statsText;
    public Text benefitText;

    bool jobSelected;

    public override void Start()
    {
        nextButton.interactable = jobSelected;
        base.Start();
        SetStory();
    }

    //UI ¼¼ÆÃ
    public void SetStoryData(string jobDescription, Stat stat, string benefit )
    {
        jobSelected = true;
        nextButton.interactable = jobSelected;

        jobDescriptionText.text = jobDescription;
        statsText.text = $"vitality : {stat.vitality} \n " +
            $"patience : {stat.patience} \n" +
            $"scheme : {stat.scheme} \n" +
            $"social : {stat.social} \n" +
            $"intelligence : {stat.intelligence}";

        benefitText.text = benefit;
    }

    async void SetStory()
    {
        TextAsset textAsset = await ResourceManager.LoadResource<TextAsset>("PlayerStory");
        jobList = JsonUtility.FromJson<jobList>(textAsset.text);
        
        CreateStroy();
    }
    void CreateStroy()
    {
        for (int i = 0; i < jobList.jobs.Count-1; i++)
        {
            GameObject go = Instantiate(jobPrefab, jobRoot);
            
            PlayerStoryItem story = go.GetComponent<PlayerStoryItem>();
            story.stat.vitality = jobList.jobs[i].vitality;
            story.stat.patience = jobList.jobs[i].patience;
            story.stat.scheme = jobList.jobs[i].scheme;
            story.stat.social = jobList.jobs[i].social;
            story.stat.intelligence = jobList.jobs[i].intelligence;

            story.SetUp(this, jobList.jobs[i].jobName, jobList.jobs[i].jobDescription, jobList.jobs[i].benefit);
        }
    }
    public override void Next()
    {
        base.Next();
    }

    public override void Previous()
    {
        base.Previous();
    }
    public override void BtnText()
    {
        base.BtnText();
    }
    public override void SetPanelText()
    {
        panelTitleText.text = Manager.ins.stringTableManager.GetValue("PlayerInitSetTitle_CharBack");
        panelSubTitleText.text = Manager.ins.stringTableManager.GetValue("PlayerInitSetSubTitle_CharBack");
    }
}
