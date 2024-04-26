using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public enum EscortState
{
    Avaliable,
    Registered,
}
public class Pyosa : MonoBehaviour
{
    #region Events
    public delegate void RegisteredHandler(Pyosa pyosa);
    public delegate void TerminatedHandler(Pyosa pyosa);

    public event RegisteredHandler onRegistered;
    public event TerminatedHandler onTerminated;
    #endregion

    [SerializeField] bool isPlayer = false;

    [Header("Visiable Info")]
    [SerializeField] GameObject toggleOn;
    [SerializeField] GameObject pyodu;
    [SerializeField] Text nameText;
    [SerializeField] Text loyaltyText;
    [SerializeField] Text commandText;
    [SerializeField] Text intelligenceText;
    [SerializeField] Text martialText;
    [SerializeField] Text levelText;
    [SerializeField] Text charmText;

    public PyosaData pyosaData;


    [SerializeField] string pyosaName;
    [SerializeField] int loyalty;
    [SerializeField] int command;
    [SerializeField] int intelligence;
    [SerializeField] int martial;
    [SerializeField] int level;
    [SerializeField] int charm;
    [SerializeField] bool isPyodu;
    [SerializeField] bool isToggleOn;

    [Space(20)]
    [SerializeField] bool isAbilityModified = false;

    int originalLoyalty;
    int originalCommand;
    int originalIntelligence;
    int originalMartial;
    int originalLevel;
    int originalCharm;

    int decreasedLoyalty;
    int decreasedCommand;
    int decreasedIntelligence;
    int decreasedMartial;
    int decreasedLevel;
    int decreasedCharm;

    [SerializeField] int injuredDate= 0;
    [SerializeField] int injuredCount =0;

    [SerializeField] int recoveryDate=0;
    [SerializeField] int recoveryCount=0;

    #region Properties
    public bool IsPlayer { get { return isPlayer; } set { isPlayer = value; } }
    public string PyosaName { get { return pyosaName; } set { pyosaName = value; } }
    public int Loyalty { get { return loyalty; } set { loyalty = value; } }
    public int Command { get { return command; } set { command = value; } }
    public int Intelligence { get { return intelligence; } set { intelligence = value; } }
    public int Martial { get { return martial; } set { martial = value; } }
    public int Level { get { return level; } set { level = value; } }
    public int Charm { get { return charm; } set { charm = value; } }
    public bool IsPyodu
    {
        get { return isPyodu; }
        set
        {
            isPyodu = value;
            PyoduState();
        }
    }
    public bool IsToggleOn
    {
        get { return isToggleOn; }
        set
        {
            isToggleOn = value;
            ToggleState();
        }
    }
    public EscortState State { get; private set; }
    public bool IsAvaliable => State == EscortState.Avaliable;
    public bool IsRegistered => State == EscortState.Registered;

    #endregion

    public void Setup()
    {
        injuredDate = 0;
        injuredCount = 0;

        originalLoyalty = loyalty;
        originalCommand = command;
        originalIntelligence = intelligence;
        originalMartial = martial;
        originalLevel = level;
        originalCharm = charm;

        nameText.text = PyosaName;
        loyaltyText.text = $"{Loyalty}";
        commandText.text = $"{Command}";
        intelligenceText.text = $"{Intelligence}";
        martialText.text = $"{Martial}";
        levelText.text = $"{Level}";
        charmText.text = $"{Charm}";

    }

    public void OnRegister()
    {
        Debug.Assert(!IsRegistered, "This escort has already been registered ");
        onRegistered?.Invoke(this);
        State = EscortState.Registered;

    }
    public void Terminate()
    {
        CheckIsRegistered();

        State = EscortState.Avaliable;
        onTerminated?.Invoke(this);

        //onRegistered = null;
        //onTerminated = null;
    }

    void PyoduState()
    {
        pyodu.SetActive(IsPyodu);
    }
    void ToggleState()
    {
        toggleOn.SetActive(IsToggleOn);
    }

    public void CountInjuryDate()
    {
        injuredCount++;
        Debug.Log($"{injuredCount} + 번째 CountInjuryDate 험수 호출");

        if (injuredCount == injuredDate)
        {
            RestoreAbility();
            injuredDate = 0;
            injuredCount = 0;
        }
    }

     public void DecreaseAbility()
    {
        CountryDate.Instance.OnReport += CountInjuryDate;

        float decreaseRatio = 0.3f;

        injuredDate = Random.Range(1, 6);

        decreasedLoyalty = Mathf.FloorToInt(Loyalty * (1 - decreaseRatio));
        decreasedCommand = Mathf.FloorToInt(Command * (1 - decreaseRatio));
        decreasedIntelligence = Mathf.FloorToInt(Intelligence * (1 - decreaseRatio));
        decreasedMartial = Mathf.FloorToInt(Martial * (1 - decreaseRatio));
        decreasedLevel = Mathf.FloorToInt(Level * (1 - decreaseRatio));
        decreasedCharm = Mathf.FloorToInt(Charm * (1 - decreaseRatio));

        Loyalty = decreasedLoyalty;
        Command = decreasedCommand;
        Intelligence = decreasedIntelligence;
        Martial = decreasedMartial;
        Level = decreasedLevel;
        Charm = decreasedCharm;

        isAbilityModified = true;
    }

    public void RestoreAbility()
    {
        Loyalty = originalLoyalty;
        Command = originalCommand;
        Intelligence = originalIntelligence;
        Martial = originalMartial;
        Level = originalLevel;
        Charm = originalCharm;

        isAbilityModified = false;
        CountryDate.Instance.OnReport -= CountInjuryDate;
        Debug.Log("회복 완료");
    }

    public void SetRecoveryDate()
    {
        int randomDate = Random.Range(15, 46);

        recoveryDate = randomDate;
    }

    public void CountRecoveryDate()
    {
        recoveryCount++;

        //회복 후 Avalilable List 이동
        if(recoveryCount== recoveryDate)
            RecoverComplete();
    }
    public void RecoverComplete()
    {
        QuestChoosePyosaPanel.instance.AvailableEscortList.Add(this);
        QuestChoosePyosaPanel.instance.InjuredEscortsList.Remove(this);
    }

    [Conditional("UNITY_EDITOR")] //인자로 전달한 Simbol값이 선언되어있으면 함수를 실행하고 아니라면 함수를 무시하게 해주는 Attribute
    private void CheckIsRegistered()
    {
        Debug.Assert(IsRegistered, "This escort has already been registered");
        Debug.Assert(!IsAvaliable, "This escort is available");
    }

   
}
