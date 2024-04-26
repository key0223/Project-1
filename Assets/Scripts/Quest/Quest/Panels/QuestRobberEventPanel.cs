using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum NegotiationResult
{
    GreatSuccess,
    Success,
    Fail,
    GreatFail,
}
public class QuestRobberEventPanel : MonoBehaviour
{
    //코드 셋팅
    [Header("Referenced Object -  데이터 확인용")]
    [SerializeField] RobberEvent robberEvent;
    [SerializeField] RobberOptionType currentRobberOption = RobberOptionType.None;
    [SerializeField] QuestOption questOption;

    ToggleGroup toggleGroup;
    Toggle currentToggle;

    //values
    [SerializeField] int robberSetNumber;

    #region Panel - UI Set
    [SerializeField] GameObject questDirectingPanel;
    [Header("UI Text")]
    [SerializeField] Button acceptBtn;
    [Space(10)]
    [SerializeField] Text robberGroupNameText;
    [SerializeField] Text numberText;
    [SerializeField] Text armingText;
    [Space(10)]
    [SerializeField] Text ourGroupNameText;
    [SerializeField] Text ourNumberText;
    [SerializeField] Text confidenceText;
    #endregion

    #region Properties
    public RobberOptionType CurrentRobberOption { get { return currentRobberOption; } }
    public QuestTracker Owner { get; private set; }
    public Robber Robber { get; private set; }
    public int RobberSetNumber { get { return robberSetNumber; } set { robberSetNumber = value; } }
    public Button AcceptBtn { get { return acceptBtn; } set { acceptBtn = value; } }
    #endregion

    private void Awake()
    {
        toggleGroup = GetComponent<ToggleGroup>();
        acceptBtn.onClick.AddListener(() => OnAcceptBtn());
        robberEvent = Manager.ins.questEventManager.RobberEvent;
    }
   
    public void Setup(QuestTracker questTracker, Robber robber)
    {
        foreach(Toggle toggle in toggleGroup.GetComponentsInChildren<Toggle>())
        {
            toggle.interactable = true;
        }

        acceptBtn.interactable = false;

        questDirectingPanel.SetActive(false);
        toggleGroup.SetAllTogglesOff();

        Owner = questTracker;
        Robber = robber;
        RobberSetNumber = SetRobberNumber();

        robberGroupNameText.text = robber.RobberName;
        numberText.text = RobberSetNumber.ToString();
        armingText.text = robber.ArmingExtent.ToString();

    }

    int SetRobberNumber()
    {
        float randomPercentage = Random.Range(0.8f, 1.2f);
        int result = Mathf.FloorToInt(Robber.Number * randomPercentage);

        Debug.Log($"기준 인원: {Robber.Number}, 랜덤화된 인원: {result}");
        return result;
    }

    public void OnOptionSelected(Toggle toggle, RobberOptionType robberOptionType)
    {
        currentRobberOption = robberOptionType;
        currentToggle = toggle;
        questOption = currentToggle.gameObject.GetComponent<QuestOption>();

        acceptBtn.interactable = true;
    }
    void OnAcceptBtn()
    {
        if (acceptBtn == null) return;

        currentToggle.isOn = false;
        currentToggle.interactable = false;
        questOption.selectedImage.SetActive(false);

        robberEvent.OnRobberOption(Owner,currentRobberOption);

        gameObject.SetActive(false);
    }
}
