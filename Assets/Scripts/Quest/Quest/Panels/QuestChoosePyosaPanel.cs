using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.UI;

public class QuestChoosePyosaPanel : MonoBehaviour
{
    #region Panel - UI Text
    [Header("Paenl UI Set")]
    [SerializeField] Text titleChooseEscortText;
    [SerializeField] Text subtitleEscortNameText;
    [SerializeField] Text subtitleEscortLoyaltyText;
    [SerializeField] Text subtitleEscortCommandText;
    [SerializeField] Text subtitleEscortIntelligenceText;
    [SerializeField] Text subtitleEscortMartialText;
    [SerializeField] Text subtitleEscortLevelText;
    [SerializeField] Text subtitleEscortCharmText;
    [Space(10)]
    [SerializeField] Text chooseAllBtnText;
    [SerializeField] Text cancelBtnText;
    [SerializeField] Text confirmBtnText;
    [Space(10)]
    [SerializeField] Button chooseAllBtn;
    [SerializeField] Button cancelBtn;
    [SerializeField] Button confirmBtn;
    [SerializeField] Text currentEscortText;
    #endregion

    #region Events
    public delegate void EscortAvailableHandler(Pyosa newPyosa);
    public delegate void EscortRegisteredHandler(Pyosa pyosa);
    public delegate void EscortTerminatedHandler(Pyosa pyosa);

    public event EscortAvailableHandler onEscortAvailable;
    public event EscortRegisteredHandler onEscortRegistered;
    public event EscortTerminatedHandler onEscortTerminated;
    #endregion

    public static QuestChoosePyosaPanel instance;

    //인스펙터 세팅
    [Header("Referenced Class ")]
    [SerializeField] QuestDetailPanel questDetailPanel;
    [SerializeField] QuestInformationPanel questInformationPanel;
    [SerializeField] CurrentPyosaList currentPyosaList;

    [Header("Other Setting")]
    [SerializeField] GameObject pyosaPrefab;
    [SerializeField] Transform listParent;
    
    [Header("Escort - 데이터 확인용")]
    [SerializeField] List<Pyosa> injuredEscortsList = new List<Pyosa>();
    [Space(10)]
    [SerializeField] List<Pyosa> availableEscortList = new List<Pyosa>();
    [SerializeField] List<Pyosa> registeredEscortList = new List<Pyosa> ();
    [SerializeField] List<Pyosa> tempEscortList = new List<Pyosa> ();

    public List<Pyosa> InjuredEscortsList { get { return injuredEscortsList; } set { injuredEscortsList = value; } }
    public List<Pyosa> AvailableEscortList => availableEscortList;
    public List<Pyosa> RegisteredEscortList { get { return registeredEscortList; } set { registeredEscortList = value; } }
    public List<Pyosa> TempEscortList { get { return tempEscortList; } set { tempEscortList = value; } }

    private int needEscort;
    private int selectedEscort;

    public Quest Target { get; private set; }

    public int NeedEscort { get { return needEscort; } set { needEscort = value; } }
    public int SelectedEscort
    {
        get { return selectedEscort; }
        set
        {
            selectedEscort = value;
            SetEscortCount();
        }
    }

    private void Awake()
    {
        titleChooseEscortText.text = Manager.ins.stringTableManager.GetValue("Quest_QuestChooseEscort");
        subtitleEscortNameText.text = Manager.ins.stringTableManager.GetValue("Name");
        subtitleEscortLoyaltyText.text= Manager.ins.stringTableManager.GetValue("Loyalty");
        subtitleEscortCommandText.text = Manager.ins.stringTableManager.GetValue("Command");
        subtitleEscortIntelligenceText.text = Manager.ins.stringTableManager.GetValue("Intelligence");
        subtitleEscortMartialText.text = Manager.ins.stringTableManager.GetValue("Martial");
        subtitleEscortLevelText.text = Manager.ins.stringTableManager.GetValue("Level");
        subtitleEscortCharmText.text = Manager.ins.stringTableManager.GetValue("Charm");

        chooseAllBtnText.text = Manager.ins.stringTableManager.GetValue("ChooseAll");
        cancelBtnText.text = Manager.ins.stringTableManager.GetValue("Cancel");
        confirmBtnText.text = Manager.ins.stringTableManager.GetValue("Confirm");

        if(instance ==null)
            instance = this;


    }

    private void Start()
    {

        confirmBtn.interactable = false;

        chooseAllBtn.onClick.AddListener(() => OnChooseAllBtn());
        cancelBtn.onClick.AddListener(() => OnCancelBtn());
        confirmBtn.onClick.AddListener(() => OnConfirmBtn());

        Invoke("SetPyosaList", 0.05f);
    }

    private void OnEnable()
    {
        AvailablePyosa();
    }

    public void SetQuest(Quest quest)
    {
        Target = quest;
    }
    public void SetPyosaList()
    {
        SetPlayerAbility();

        OrderByLoyalty();

        for (int i = 0; i < currentPyosaList.pyosaDatas.pyosas.Count; i++)
        {
            Pyosa pyosa = Instantiate(pyosaPrefab).GetComponent<Pyosa>();

            pyosa.gameObject.transform.SetParent(listParent, false);
            pyosa.gameObject.SetActive(true);

            pyosa.pyosaData = currentPyosaList.pyosaDatas.pyosas[i];

            pyosa.PyosaName = currentPyosaList.pyosaDatas.pyosas[i].name;
            pyosa.Loyalty = currentPyosaList.pyosaDatas.pyosas[i].royalty;
            pyosa.Command = currentPyosaList.pyosaDatas.pyosas[i].command;
            pyosa.Intelligence = currentPyosaList.pyosaDatas.pyosas[i].intelligence;
            pyosa.Martial = currentPyosaList.pyosaDatas.pyosas[i].martialArts;
            pyosa.Level = currentPyosaList.pyosaDatas.pyosas[i].level;
            pyosa.Charm = currentPyosaList.pyosaDatas.pyosas[i].charm;

            pyosa.IsPyodu = false;

            Toggle toggle = pyosa.gameObject.GetComponent<Toggle>();

            toggle.onValueChanged.AddListener(isOn => OnToggleValueChanged(toggle, isOn));
            pyosa.Setup();

            pyosa.onRegistered += OnEscortRegistered;
            pyosa.onTerminated += OnEscortTerminated;

            availableEscortList.Add(pyosa);

            onEscortAvailable?.Invoke(pyosa);
        }

        SetEscortCount();
    }

    void SetPlayerAbility()
    {
        Pyosa player = Instantiate(pyosaPrefab).GetComponent<Pyosa>();

        player.gameObject.transform.SetParent(listParent, false);
        player.gameObject.SetActive(true);

        player.IsPlayer = true;
        player.PyosaName = Manager.ins.playerManager.playerName;
        player.Loyalty = 80;
        player.Command = 98;
        player.Intelligence = 82;
        player.Martial = 87;
        player.Level = 75;
        player.Charm = 77;

        player.IsPyodu = false;

        Toggle toggle = player.gameObject.GetComponent<Toggle>();

        toggle.onValueChanged.AddListener(isOn => OnToggleValueChanged(toggle, isOn));
        player.Setup();

        player.onRegistered += OnEscortRegistered;
        player.onTerminated += OnEscortTerminated;

        availableEscortList.Add(player);

        onEscortAvailable?.Invoke(player);
    }
    void AvailablePyosa()
    {
        List<Pyosa> tempList = new List<Pyosa>(tempEscortList);

        if (tempEscortList.Count > 0)
        {
            for (int i = 0; i < tempList.Count; i++)
            {
                Toggle toggle = tempList[i].GetComponent<Toggle>();
                toggle.isOn = false;
                tempList[i].IsToggleOn = false;
            }
        }
        else if (availableEscortList.Count > 0)
        {
            for (int i = 0; i < availableEscortList.Count; i++)
            {
                Toggle toggle = availableEscortList[i].GetComponent<Toggle>();
                toggle.isOn = false;
                availableEscortList[i].IsToggleOn = false;
            }
        }
       
        for (int i = 0; i < registeredEscortList.Count; i++)
        {
            registeredEscortList[i].gameObject.SetActive(false);
        }
    }

    void OrderByLoyalty()
    {
        currentPyosaList.pyosaDatas.pyosas = currentPyosaList.pyosaDatas.pyosas.OrderByDescending(l => l.royalty).ToList();
    }
    public bool ContainsInRegisteredEscortList(Pyosa pyosa) => tempEscortList.Any(x => x.PyosaName == pyosa.PyosaName);

    void OnToggleValueChanged(Toggle toggle, bool isOn)
    {
        Pyosa pyosa = toggle.gameObject.GetComponent<Pyosa>();

        if (isOn && !ContainsInRegisteredEscortList(pyosa))
        {
            if (tempEscortList.Count == 0)
            {
                pyosa.IsPyodu = true;
                Target.Pyodu = pyosa;
            }

            pyosa.OnRegister();
        }
        else
        {
            pyosa.Terminate();

            if (pyosa.IsPyodu)
            {
                pyosa.IsPyodu = false;
                if(tempEscortList.Count >0)
                {
                    tempEscortList.Sort((a, b) => b.Loyalty.CompareTo(a.Loyalty));
                    tempEscortList[0].IsPyodu = true;
                    Target.Pyodu = tempEscortList[0];

                    //Target.Pyodu = pyosa;
                }
            }
        }
        availableEscortList.Sort((a, b) => b.Loyalty.CompareTo(a.Loyalty));
        SelectedEscort = tempEscortList.Count;
    }

    void SetEscortCount()
    {
        currentEscortText.text = $"{tempEscortList.Count}/{needEscort}";

        if (selectedEscort < needEscort)
        {
            chooseAllBtn.interactable = true;
            confirmBtn.interactable = false;
        }
        if (selectedEscort >= needEscort)
        {
            chooseAllBtn.interactable = false;
            confirmBtn.interactable = true;
        }
    }
    void OnChooseAllBtn()
    {
        tempEscortList.Sort((a, b) => b.Loyalty.CompareTo(a.Loyalty));
        List<Pyosa> tempAvailableList = new List<Pyosa>(availableEscortList);

        int remainingNeeded = needEscort - SelectedEscort; // 남은 필요한 표사의 수

        for (int i = 0; i < tempAvailableList.Count; i++)
        {
            Toggle toggle = tempAvailableList[i].gameObject.GetComponent<Toggle>();

            if (SelectedEscort < needEscort && !toggle.isOn)
            {
                toggle.isOn = true;
                tempAvailableList[i].IsToggleOn = true;

                if (!ContainsInRegisteredEscortList(tempAvailableList[i]))
                {
                    tempEscortList.Add(tempAvailableList[i]);
                    SelectedEscort++;
                }
            }
        }
        chooseAllBtn.interactable = false;
    }

    void OnCancelBtn()
    {
        //foreach (var escort in TempEscortList)
        //{
        //    Toggle toggle = escort.gameObject.GetComponent<Toggle>();
        //    toggle.isOn = false;
        //    escort.IsToggleOn = false;
        //}

        //questDetailPanel.Show(Target);
        //questDetailPanel.gameObject.SetActive(true);

        //Target = null;
        //gameObject.SetActive(false);

        foreach (var escort in tempEscortList)
        {
            availableEscortList.Add(escort); // tempEscortList의 요소를 availableEscortList로 이동
        }


        //tempEscortList.Clear(); // tempEscortList 비우기

        questDetailPanel.Show(Target);
        questDetailPanel.gameObject.SetActive(true);

        chooseAllBtn.interactable = true;

        Target = null;
        gameObject.SetActive(false);
    }

    void OnConfirmBtn()
    {
        gameObject.SetActive(false);
        questInformationPanel.Setup(Target);
        questInformationPanel.gameObject.SetActive(true);
    }

    #region Callback

    void OnEscortRegistered(Pyosa pyosa)
    {
        if(availableEscortList.Contains(pyosa))
        {
            pyosa.IsToggleOn = true;
            availableEscortList.Remove(pyosa);
            //registeredEscortList.Add(pyosa);

            Target.AttendedEscortList.Add(pyosa);
            tempEscortList.Add(pyosa);
            onEscortRegistered?.Invoke(pyosa);
        }
    }

    void OnEscortTerminated(Pyosa pyosa)
    {
        if(tempEscortList.Contains(pyosa))
        {
            pyosa.IsToggleOn = false;
            //registeredEscortList.Remove(pyosa);
            availableEscortList.Add(pyosa);

            Target.AttendedEscortList.Remove(pyosa);
            tempEscortList.Remove(pyosa);
            onEscortTerminated?.Invoke(pyosa);
        }
    }
   
    #endregion
}
