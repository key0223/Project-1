using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SuppressionResultPanel : MonoBehaviour
{
    #region Result Panel - UI Text
    //Inspector Set
    [Header("Visible Info")]
    [SerializeField] Text suppressionPanelTittleText;

    [Space(20)]
    [SerializeField] Text robberNameText;
    [SerializeField] Text robberCountText;
    [SerializeField] Text robberArimingText;
    [Space(10)]
    [SerializeField] Text robberDeathText;
    [SerializeField] Text robberSeriousInjuryText;
    [SerializeField] Text robberSlightInjuryText;
    [SerializeField] Text robberGetawayText;

    [Space(20)]
    [SerializeField] Text pyogukNameText;
    [SerializeField] Text escortText;
    [SerializeField] Text confidenceText;
    [Space(10)]
    [SerializeField] Text deathText;
    [SerializeField] Text seriousInjuryText;
    [SerializeField] Text slightInjuryText;
    [SerializeField] Text getawayText;

    [Space(20)]
    [SerializeField] Text rewardTrustText;
    [SerializeField] Text rewardFameText;
    [SerializeField] Text rewardMoneyText;

    [Space(10)]
    [SerializeField] Button confirmBtn;
    #endregion

    #region Each group result - Value
    [SerializeField] float confidenceValue;
    [SerializeField] float robberConfidenceValue;

    [SerializeField] bool win;
    [SerializeField] float power;
    [SerializeField] float robberPower;
    [SerializeField] int getawayValue;
    [SerializeField] int robberGetawayValue;
    [SerializeField] int deathValue;
    [SerializeField] int robberDeathValue;
    [SerializeField] int seriouesInjuryValue;
    [SerializeField] int robberSeriouseInjuryValue;
    [SerializeField] int slightInjuryValue;
    [SerializeField] int robberSlightInjuryValue;
    int tempEscortCount = 0;
    bool isReturn = false;
    #endregion

    #region Reward - Value
    [SerializeField] int rewardTrustValue;
    [SerializeField] int rewardFameValue;
    [SerializeField] int rewardMoneyValue;
    #endregion

    public QuestTracker Owner { get; private set; }

    #region ContainerCheck
    bool ContainsGetawayEscort(Pyosa pyosa) => Owner.GetawayEscort.Any(x => x.PyosaName == pyosa.PyosaName);
    bool ContainsDeathEscort(Pyosa pyosa) => Owner.DeathEscort.Any(x => x.PyosaName == pyosa.PyosaName);
    bool ContainsSeriousInjuryEscort(Pyosa pyosa) => Owner.SeriousInjuryEscort.Any(x => x.PyosaName == pyosa.PyosaName);
    bool ContainsSlightInjuryEscort(Pyosa pyosa) => Owner.SlightInjuryEscort.Any(x => x.PyosaName == pyosa.PyosaName);
    #endregion


    private void Awake()
    {
        confirmBtn.onClick.AddListener(() => onConfirmBtn());
    }
    public void Setup(QuestTracker questTracker)
    {
        Owner = questTracker;
        tempEscortCount = Owner.TargetQuest.AttendedEscortList.Count;

        BattleResult();
        RewardResult();
        SetUIText();
    }

    void onConfirmBtn()
    {
        if (isReturn)
        {
            Owner.TargetQuest.ForceToReturn();
            Manager.ins.questEventManager.EndEvent();
            Debug.Log("국주가 중상을 입었습니다. 복귀합니다.");
        }
        else
        {
            Owner.QuestDirectingPanel.gameObject.SetActive(true);
            Owner.QuestDirectingPanel.OnDirecting(Manager.ins.questEventManager.ProgressEvent.CurrentProgressType);
            Manager.ins.questEventManager.EndEvent();
           // Owner.QuestReporter.StartReport();
        }

        gameObject.SetActive(false);

        InitFields();
    }

    void InitFields()
    {
        power = 0;
        robberPower = 0;
        getawayValue = 0;
        robberGetawayValue = 0;
        deathValue = 0;
        robberDeathValue = 0;
        seriouesInjuryValue = 0;
        robberSeriouseInjuryValue = 0;
        slightInjuryValue = 0;
        robberSlightInjuryValue = 0;

        tempEscortCount = Owner.TargetQuest.AttendedEscortList.Count;

    }
    void SetUIText()
    {
        if (win)
            suppressionPanelTittleText.text = "토벌 완료!";
        else suppressionPanelTittleText.text = "토벌 실패";

        robberNameText.text = Owner.RobberEventPanel.Robber.RobberName;
        robberCountText.text = $"{Owner.RobberEventPanel.RobberSetNumber}";
        robberArimingText.text = $"{Owner.RobberEventPanel.Robber.ArmingExtent}";

        robberDeathText.text = $"{robberDeathValue}";
        robberSeriousInjuryText.text = $"{robberSeriouseInjuryValue}";
        robberSlightInjuryText.text = $"{robberSlightInjuryValue}";
        robberGetawayText.text = $"{robberGetawayValue}";

        pyogukNameText.text = Manager.ins.playerManager.playerPyogukName;
        escortText.text = $"{tempEscortCount}";
        confidenceText.text = "수정해야됨";

        deathText.text = $"{deathValue}";
        seriousInjuryText.text = $"{seriouesInjuryValue}";
        slightInjuryText.text = $"{slightInjuryValue}";
        getawayText.text = $"{getawayValue}";

        //Reward result
        rewardTrustText.text = win ? $"+ {rewardTrustValue}" : $"- {rewardTrustValue}";
        rewardFameText.text = $"{rewardFameValue}";
        rewardMoneyText.text = win? $"{rewardMoneyValue}" : $"+ 0" ;
    }
    #region Suppression result set - Value
    void BattleResult()
    {
        SetPower();

        //power가 robberPower보다 크거나 같을 경우 true 
        win = power >= robberPower;

        SetGetawayValue();
        SetDeathValue();
        SetSeriousInjury();
        SetSlightInjury();
    }
    void SetPower()
    {
        int SumEscortsMartialValue()
        {
            int result = 0;

            for (int i = 0; i < Owner.TargetQuest.AttendedEscortList.Count; i++)
            {
                int martailValue = Owner.TargetQuest.AttendedEscortList[i].Martial;

                result += martailValue;
            }
            return result;
        }

        int escortsMartialValue = SumEscortsMartialValue();
        int escortReaderCommandValue = Owner.TargetQuest.Pyodu.Command;
        int escortReaderIntelligence = Owner.TargetQuest.Pyodu.Intelligence;
        float confidenceValue = SetConfidenceValue();
        float CalculatePower()
        {
            float result = ((escortsMartialValue * (escortReaderCommandValue / 50)) + escortReaderIntelligence) * confidenceValue;

            return Mathf.FloorToInt(result);
        }
        power = CalculatePower();


        int robberNumber = Owner.RobberEventPanel.Robber.Number;
        float robberConfidence = SetRobberConfidenceValue();

        float CaculateRobberPower()
        {
            float result = robberNumber * 50 * robberConfidence;
            return Mathf.FloorToInt(result);
        }

        robberPower = CaculateRobberPower();

    }
    void SetGetawayValue()
    {
        int escortCount = Owner.TargetQuest.AttendedEscortList.Count;
        int robberCount = Owner.RobberEventPanel.Robber.Number;

        float winValue = Random.Range(0.0f, 5.1f);
        float loseValue = Random.Range(10.0f, 30.0f);

        getawayValue = win ? Mathf.FloorToInt((escortCount * winValue / 100) * confidenceValue) : Mathf.FloorToInt((escortCount * loseValue / 100) * confidenceValue);
        robberGetawayValue = win ? Mathf.FloorToInt(robberCount * loseValue / 100) * 2 : Mathf.FloorToInt(robberCount * winValue / 100) * 2;

        Owner.TargetQuest.AttendedEscortList.Sort((a, b) => a.Loyalty.CompareTo(b.Loyalty));

        if (getawayValue > 0)
        {
            for (int i = 0; i < getawayValue; i++)
            {
                Pyosa pyosa = Owner.TargetQuest.AttendedEscortList[i];
                Owner.GetawayEscort.Enqueue(pyosa);
                Owner.TargetQuest.AttendedEscortList.Remove(pyosa);

                Debug.Log(string.Format("{0}번째 도주자의 충성도는 {1}입니다.", i, pyosa.Loyalty));
            }

            Debug.Log(string.Format("도주자는 총 {0}명 입니다.", Owner.GetawayEscort.Count));
        }
    }
    void SetDeathValue()
    {
        float count = tempEscortCount - getawayValue;
        float robberCount = Owner.RobberEventPanel.Robber.Number - robberGetawayValue;

        float B = win ? power : robberPower;
        float C = win ? robberPower : power;

        float D = B / C;
        D = Mathf.Clamp(D, 0.0f, 2.0f);

        float d = (C / B) - 0.5f;
        d = Mathf.Max(d, 0.0f);

        float E = 0.5f;
        float F = 1.0f;

        deathValue = win ? Mathf.FloorToInt(count * d * E) : Mathf.FloorToInt(count * (D - 1) * F);
        robberDeathValue = win ? Mathf.FloorToInt(robberCount * (D - 1) * F) : Mathf.FloorToInt(robberCount * d * E);

        if (deathValue > 0)
        {
            Owner.TargetQuest.AttendedEscortList.Sort((a, b) => (a.Martial + a.Intelligence).CompareTo(b.Martial + b.Intelligence));

            List<Pyosa> deathEscorts = new List<Pyosa>();
            int deathCount = 0;

            Pyosa player;

            for (int i = 0; i < tempEscortCount && deathCount < deathValue; i++)
            {
                if (ContainsGetawayEscort(Owner.TargetQuest.AttendedEscortList[i]))
                    continue;

                if (Owner.TargetQuest.AttendedEscortList[i].IsPlayer)
                    player = Owner.TargetQuest.AttendedEscortList[i];

                Pyosa pyosa = Owner.TargetQuest.AttendedEscortList[i];
                deathEscorts.Add(pyosa);

                deathCount++;

                //Debug.Log(string.Format("{0}번째 사망자의 값은 {1}입니다.", i, pyosa.Martial + pyosa.Intelligence));
            }

            deathCount = Mathf.Min(deathEscorts.Count, deathValue);

            for (int i = 0; i < deathCount; i++)
            {
                Pyosa pyosa = deathEscorts[i];

                if (tempEscortCount == deathValue-1)
                    isReturn = true;

                if (pyosa.IsPlayer)
                {
                    Owner.SeriousInjuryEscort.Enqueue(pyosa);
                    Owner.TargetQuest.AttendedEscortList.Remove(pyosa);
                    deathValue = deathValue - 1;

                    isReturn = true;

                    Debug.Log("Death함수 플레이어 사망 - 중상으로 이동");
                }
                else
                {
                    Owner.DeathEscort.Enqueue(pyosa);
                    Owner.TargetQuest.AttendedEscortList.Remove(pyosa);
                }

                if(tempEscortCount == deathValue -1)
                {
                    Owner.SeriousInjuryEscort.Enqueue(pyosa);
                    Owner.TargetQuest.AttendedEscortList.Remove(pyosa);

                    isReturn = true;

                    Debug.Log("표사가 전멸하고 국주가 중상을 입어 복귀합니다");
                }
            }

            Debug.Log(string.Format("사망자는 총 {0}명 입니다.", Owner.DeathEscort.Count));
        }
    }

    void SetSeriousInjury()
    {
        float count = tempEscortCount - getawayValue - deathValue;
        float robberCount = Owner.RobberEventPanel.Robber.Number - robberGetawayValue - robberDeathValue;

        float B = win ? power : robberPower;
        float C = win ? robberPower : power;

        float D = 0.5f;
        float E = 1.0f;

        seriouesInjuryValue = win ? Mathf.FloorToInt(count * C / (B + C) * D) : Mathf.FloorToInt(count * B / (B + C) * E);
        robberSeriouseInjuryValue = win ? Mathf.FloorToInt(robberCount * B / (B + C) * E) : Mathf.FloorToInt(robberCount * C / (B + C) * D);

        if (seriouesInjuryValue > 0)
        {
            int seriousCount = 0;

            for (int i = 0; i < tempEscortCount && seriousCount < seriouesInjuryValue; i++)
            {
                if (ContainsGetawayEscort(Owner.TargetQuest.AttendedEscortList[i]) || ContainsDeathEscort(Owner.TargetQuest.AttendedEscortList[i]))
                    continue;

                Pyosa pyosa = Owner.TargetQuest.AttendedEscortList[i];

                Owner.SeriousInjuryEscort.Enqueue(pyosa);
                Owner.TargetQuest.AttendedEscortList.Remove(pyosa);

                if (pyosa.IsPlayer)
                {
                    isReturn = true;
                    Debug.Log("플레이어 중상");
                }
                seriousCount++;
            }

            Debug.Log(string.Format("중상자는 총 {0}명 입니다.", Owner.SeriousInjuryEscort.Count));
        }

        if (isReturn)
            seriouesInjuryValue++;


        if (seriouesInjuryValue < 0)
            seriouesInjuryValue = 0;
    }

    void SetSlightInjury()
    {
        float count = Owner.TargetQuest.AttendedEscortList.Count - getawayValue - deathValue - seriouesInjuryValue;
        float robberCount = Owner.RobberEventPanel.Robber.Number - robberGetawayValue - robberDeathValue - robberSeriouseInjuryValue;

        float B = Random.Range(0.0f, 0.4f);
        float C = Random.Range(0.3f, 1.1f);

        slightInjuryValue = win ? Mathf.FloorToInt(count * B) : Mathf.FloorToInt(count * C);
        robberSlightInjuryValue = win ? Mathf.FloorToInt(robberCount * C) : Mathf.FloorToInt(robberCount * B);

        if (slightInjuryValue > 0)
        {
            int slightCount = 0;

            for (int i = 0; i < Owner.TargetQuest.AttendedEscortList.Count && slightCount < slightInjuryValue; i++)
            {
                if (ContainsGetawayEscort(Owner.TargetQuest.AttendedEscortList[i]) || ContainsDeathEscort(Owner.TargetQuest.AttendedEscortList[i])
                    || ContainsSeriousInjuryEscort(Owner.TargetQuest.AttendedEscortList[i]))
                    continue;

                Pyosa pyosa = Owner.TargetQuest.AttendedEscortList[i];

                //pyosa.Loyalty -= Mathf.FloorToInt(pyosa.Loyalty * 0.3f);
                //pyosa.Command -= Mathf.FloorToInt(pyosa.Command * 0.3f);
                //pyosa.Intelligence -= Mathf.FloorToInt(pyosa.Intelligence * 0.3f);
                //pyosa.Martial -= Mathf.FloorToInt(pyosa.Martial*0.3f);
                //pyosa.Level -= Mathf.FloorToInt(pyosa.Level * 0.3f);
                //pyosa.Charm -= Mathf.FloorToInt(pyosa.Charm * 0.3f);

                pyosa.DecreaseAbility();

                Owner.SlightInjuryEscort.Enqueue(pyosa);

                slightCount++;
            }
            Debug.Log(string.Format("경상자는 총 {0}명 입니다.", Owner.SlightInjuryEscort.Count));
        }

        if (slightInjuryValue < 0)
            slightInjuryValue = 0;

    }

    float SetConfidenceValue()
    {
        float value = 0.0f;
        if (Manager.ins.questEventManager.ProgressEvent.CurrentProgressType == ProgressOptionType.Wimoo)
            value = 1.3f;
        else if (Manager.ins.questEventManager.ProgressEvent.CurrentProgressType == ProgressOptionType.Ineui)
            value = 1.0f;
        else
            value = 0.7f;

        return value;
    }

    float SetRobberConfidenceValue()
    {
        float value = 0.0f;
        if (Owner.RobberEventPanel.Robber.ArmingExtent == ArmingExtent.Strong)
            value = 1.3f;
        else if (Owner.RobberEventPanel.Robber.ArmingExtent == ArmingExtent.Normal)
            value = 1.0f;
        else
            value = 0.7f;

        return value;
    }
    #endregion

    #region Suppression reward set - Value

    void RewardResult()
    {
        SetTrustValue();
        SetFameValue();
        SetMoneyValue();
    }
    void SetTrustValue()
    {
        //현재 의뢰의 총 보수
        int questReward = Owner.TargetQuest.questUnit.Deposit + Owner.TargetQuest.questUnit.Remainder;
        float percent = questReward * 0.02f;

        int result  = Mathf.FloorToInt(percent);

        rewardTrustValue = result;
    }

    void SetFameValue()
    {
        int robberGetaway = robberGetawayValue * 10;
        int robberDeath = robberDeathValue * 20;
        int robberSerious = robberSeriouseInjuryValue * 15;
        int robberSlight = robberSlightInjuryValue * 5;
        int robberSum = robberGetaway+robberDeath+robberSerious+robberSlight;

        int escortGetaway = getawayValue * 10;
        int escortDeath = deathValue * 20;
        int escortSerious = seriouesInjuryValue *15;
        int escortSlight = slightInjuryValue*5;
        int escortSum = escortGetaway+escortDeath+escortSerious+escortSlight;

        int result = robberSum - escortSum;

        rewardFameValue = result;
    }

    void SetMoneyValue()
    {
        // A = 도적 인원, B = 도적 무장도 보정값, C = 랜덤값
        int A = Owner.RobberEventPanel.RobberSetNumber;
        int B = 0;
        #region Set B
        if (Owner.RobberEventPanel.Robber.ArmingExtent == ArmingExtent.Strong)
            B = 3;
        else if (Owner.RobberEventPanel.Robber.ArmingExtent == ArmingExtent.Normal)
            B = 2;
        else
            B = 1;
        #endregion
        int C = Random.Range(50, 201);

        int result = A * B * C;

        rewardMoneyValue = result;
    }
    #endregion
}
