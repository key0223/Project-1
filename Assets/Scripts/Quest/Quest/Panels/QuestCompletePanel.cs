using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class QuestCompletePanel : MonoBehaviour
{
    //�ڵ� ����
    QuestSystem questSystem;

    //�ν����� 
    [SerializeField] QuestChoosePyosaPanel questChoosePyosaPanel;
    [SerializeField] Button cancelBtn;
    [SerializeField] Button completeBtn;

    QuestTracker questTracker;

    public QuestTracker QuestTracker { get { return questTracker; } private set { questTracker = value; } }

    private void Awake()
    {
        questSystem = QuestSystem.Instance;

        cancelBtn.onClick.AddListener(() => OnCancelBtn());
        completeBtn.onClick.AddListener(() => OnCompleteBtn());

        gameObject.SetActive(false);
    }
    public void Setup(QuestTracker tracker)
    {
        QuestTracker = tracker;
        questChoosePyosaPanel = QuestChoosePyosaPanel.instance;

    }

    void EscortsPlace()
    {
        if (questTracker.TargetQuest.Pyodu.IsPlayer)
        {
            //������ ó��
            if (questTracker.GetawayEscort.Count > 0)
            {
                float randomPercent = Random.Range(50f, 101f);
                int returnEscrot = Mathf.FloorToInt(questTracker.GetawayEscort.Count * randomPercent);

                for (int i = 0; i < returnEscrot; i++)
                {
                    Pyosa escort = questTracker.GetawayEscort.Dequeue();

                    Toggle toggle = escort.GetComponent<Toggle>();
                    toggle.isOn = false;
                    escort.IsToggleOn = false;

                    questChoosePyosaPanel.AvailableEscortList.Add(escort);
                    //questChoosePyosaPanel.RegisteredEscortList.Remove(escort);
                    questTracker.TargetQuest.AttendedEscortList.Remove(escort);
                }

                if(questTracker.GetawayEscort.Count>0)
                {
                    foreach (var escort in questTracker.GetawayEscort)
                    {
                        questTracker.GetawayEscort.Dequeue();
                        questChoosePyosaPanel.RegisteredEscortList.Remove(escort);
                    }
                }
            }
            questTracker.GetawayEscort.Clear();

            //����� ó��
            foreach (var escort in questTracker.DeathEscort)
            {
                Toggle toggle = escort.GetComponent<Toggle>();
                toggle.isOn = false;
                escort.IsToggleOn = false;

                questChoosePyosaPanel.RegisteredEscortList.Remove(escort);
                questTracker.TargetQuest.AttendedEscortList.Remove(escort);
                escort.gameObject.SetActive(false);
            }
            questTracker.DeathEscort.Clear();

            //�߻��� ó��
            if (questTracker.SeriousInjuryEscort.Count > 0)
            {
                foreach (var escort in questTracker.SeriousInjuryEscort)
                {
                    Toggle toggle = escort.GetComponent<Toggle>();
                    toggle.isOn = false;
                    escort.IsToggleOn = false;

                    escort.SetRecoveryDate();

                    questChoosePyosaPanel.InjuredEscortsList.Add(escort);
                    questChoosePyosaPanel.RegisteredEscortList.Remove(escort);
                    questTracker.TargetQuest.AttendedEscortList.Remove(escort);
                    escort.gameObject.SetActive(false);
                }
            }
            questTracker.SeriousInjuryEscort.Clear();

            //����� ó��
            if (questTracker.SlightInjuryEscort.Count > 0)
            {
                foreach (var escort in questTracker.SlightInjuryEscort)
                {
                    Toggle toggle = escort.GetComponent<Toggle>();
                    toggle.isOn = false;
                    escort.IsToggleOn = false;

                    questChoosePyosaPanel.AvailableEscortList.Add(escort);
                    questChoosePyosaPanel.RegisteredEscortList.Remove(escort);
                    questTracker.TargetQuest.AttendedEscortList.Remove(escort);
                }
            }
            questTracker.SlightInjuryEscort.Clear();

            foreach (var  escort in questTracker.TargetQuest.AttendedEscortList)
            {
                questChoosePyosaPanel.AvailableEscortList.Add(escort);
                questChoosePyosaPanel.RegisteredEscortList.Remove(escort);
            }
        }
        else
        {
            if (questTracker.TargetQuest.AttendedEscortList.Count > 0)
            {
                foreach (var escort in questTracker.TargetQuest.AttendedEscortList)
                {
                    questChoosePyosaPanel.AvailableEscortList.Add(escort);
                    questChoosePyosaPanel.RegisteredEscortList.Remove(escort);
                }
            }
        }
    }
    void OnCancelBtn()
    {
        gameObject.SetActive(false);
    }
    void OnCompleteBtn()
    {
        EscortsPlace();

        questSystem.CompleteWaitingQuests(questTracker.TargetQuest);
    }
}
