using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.UI;

#region QuestEnums
public enum QuestType
{
    Transport = 1,
    Store,
    Escort,
    Guard,
}
public enum OwnerType
{
    Govermentv = 1,
    Gangho,
    General,
}
public enum QuestLevel
{
    Normal,
    High,
    Dangerous,
}
public enum Dest
{
    Sanseo = 1,
    Shanxi,
    Henan,
    Shandong,
    Jigye,
    Zhejiang,
    Fujian,
    Szechuan,
    Hubei,
    Hunan,
    Jiangxi,
    Anhui,
    Jiangsu,
    Returning,
}
public enum Season
{
    Spring = 1,
    Summer,
    Fall,
    Winter,
}
public enum Special
{
    Rich = 1,
    Poor,
    Epidemic,
    Insurrection
}
public enum Province
{
    Sanseo = 1,
    Shanxi,
    Henan,
    Shandong,
    Jigye,
    Zhejiang,
    Fujian,
    Szechuan,
    Hubei,
    Hunan,
    Jiangxi,
    Anhui,
    Jiangsu,
}
#endregion

public class QuestSystem : MonoBehaviour
{
    #region Events
    public delegate void QuestAvailableHandler(Quest newQuest);
    public delegate void QuestRegisteredHandeler(Quest quest);
    public delegate void QuestCompletedHandeler(Quest quest);
    //public delegate void QuestCanceledHandeler(Quest quest);

    public event QuestAvailableHandler onQuestAvailable;
    public event QuestRegisteredHandeler onQuestRegistered;
    public event QuestCompletedHandeler onQuestCompleted;
    //public event QuestCanceledHandeler onQuestCanceled;
    #endregion

    public static QuestSystem Instance
    {
        get
        {
            if (!isApplicationQuitting && instance == null)
            {
                instance = FindObjectOfType<QuestSystem>();
                if (instance == null)
                {
                    instance = new GameObject("Quest System").AddComponent<QuestSystem>();
                    DontDestroyOnLoad(instance.gameObject);
                }
            }
            return instance;

        }
    }

    [SerializeField] TableManager tableManager;
    [SerializeField] GameObject questPrefab;
    [SerializeField] GameObject availableQuestListPanel;

    private ToggleGroup toggleGroup;

    private static QuestSystem instance;
    private static bool isApplicationQuitting;

    [SerializeField] List<Quest> availableQuests = new List<Quest>();
    [SerializeField] List<Quest> activeQuests = new List<Quest>();
    [SerializeField] List<Quest> completedQuests = new List<Quest>();

    public IReadOnlyList<Quest> AvailableQuests => availableQuests;
    public IReadOnlyList<Quest> ActiveQuests => activeQuests;
    public IReadOnlyList<Quest> CompletedQuests => completedQuests;

    public bool ContainsInAvailableQuests(Quest quest) => availableQuests.Any(x => x.QuestId == quest.QuestId);
    public bool ContainsInActiveQuests(Quest quest) => activeQuests.Any(x => x.QuestId == quest.QuestId);
    public bool ContatinsInCompletedQuests(Quest quest) => completedQuests.Any(x => x.QuestId == quest.QuestId);


    [Space(10)]
    [SerializeField] GameObject robberPrefab;
    [SerializeField] Transform robberParent;
    [SerializeField] List<Robber> robberGroups = new List<Robber>();

    
    public List<Robber> RobberGroups { get {  return robberGroups; }}

    public void Awake()
    {
        toggleGroup = availableQuestListPanel.GetComponent<ToggleGroup>();
        Invoke("CreateNewQuest", 0.1f);
        Invoke("CreateNewRobber", 0.1f);
    }

    private void OnApplicationQuit()
    {
        isApplicationQuitting = true;
    }
    void CreateNewQuest()
    {
        for (int i = 0; i < tableManager.questList.array.Count; i++)
        {
            Quest newQuest = Instantiate(questPrefab).GetComponent<Quest>();

            newQuest.gameObject.transform.SetParent(availableQuestListPanel.transform, false);
            newQuest.gameObject.SetActive(true);

            newQuest.questUnit = tableManager.questList.array[i];

            newQuest.QuestId = newQuest.questUnit.QuestId;
            newQuest.OwnerName = tableManager.GetQuestValue("QuestOwner_" + newQuest.QuestId.ToString());
            newQuest.QuestDesc = tableManager.GetQuestValue("QuestDesc_" + newQuest.QuestId.ToString());

            newQuest.Deadline = GetDeadline(newQuest.questUnit);
            newQuest.Reward = GetReward(newQuest.questUnit);

            newQuest.QuestType = (QuestType)newQuest.questUnit.QuestType;
            newQuest.OwnerType = (OwnerType)newQuest.questUnit.OwnerType;
            newQuest.QuestLevel = SetLevel(newQuest.questUnit);
            newQuest.Dest = (Dest)newQuest.questUnit.Dest;
            
            //위험도

            newQuest.gameObject.GetComponent<Toggle>().group = toggleGroup;


            newQuest.Setup();

            newQuest.onRegistered += OnQuestRegistered;
            newQuest.onCompleted += OnQuestCompleted;
            //newQuest.onCanceled += OnQuestCanceled;


            availableQuests.Add(newQuest);

            onQuestAvailable?.Invoke(newQuest);
        }
    }
    public void ReceiveReport(int questId, Dest dest, int value)
    {
        ReceiveReport(activeQuests, questId, dest, value);
    }
    private void ReceiveReport(List<Quest> quests, int questId, Dest dest, int value)
    {
        foreach (var quest in quests.ToArray())
            quest.ReceiveReport(questId, dest, value);
    }

    public void CompleteWaitingQuests(Quest quest)
    {
        if (quest.IsCompleteable)
            quest.Complete();
        //foreach (var quest in activeQuests.ToList())
        //{
        //    if (quest.IsCompleteable)
        //        quest.Complete();
        //}
    }


    #region SetDatas
    public int GetDeadline(QuestUnit questUnit)
    {
        //QuestUnit의 속성을 모두 가져온다-> Date가 포함된 속성을 필터링 -> 필터링된 속성들의 값을 선택
        var dates = questUnit.GetType().GetFields()
         .Where(field => field.Name.Contains("Date"))
         .Select(field =>
         {
             var value = field.GetValue(questUnit);
             return value != null ? (int)value : 0;
         });

        int dateSum = dates.Sum();
        return dateSum;
    }

    public int GetReward(QuestUnit questUnit)
    {
        int rewardSum;
        int deposit = questUnit.Deposit;
        int remainder = questUnit.Remainder;

        rewardSum = deposit + remainder;

        return rewardSum;
    }

    public QuestLevel SetLevel(QuestUnit quest)
    {
        QuestLevel questLevel = QuestLevel.Normal;
        int needFame = quest.Fame;
        //임시 데이터
        int currentFame = 20;

        int level = needFame / currentFame;

        if (level < 1.5)
        {
            questLevel = QuestLevel.Normal;
        }
        else if (level >= 1.5)
        {
            questLevel = QuestLevel.High;
        }
        else if (level >= 2)
        {
            questLevel = QuestLevel.Dangerous;

            return questLevel;
        }
        return questLevel;

    }
    #endregion

    #region Callback

    private void OnQuestRegistered(Quest quest)
    {
        if (availableQuests.Contains(quest))
        {
            availableQuests.Remove(quest);
            activeQuests.Add(quest);
            onQuestRegistered?.Invoke(quest);
        }
    }
    private void OnQuestCompleted(Quest quest)
    {
        if (activeQuests.Contains(quest))
        {
            activeQuests.Remove(quest);
            completedQuests.Add(quest);
            onQuestCompleted?.Invoke(quest);
        }
    }
    //private void OnQuestCanceled(Quest quest)
    //{
    //    if (activeQuests.Contains(quest))
    //    {
    //        activeQuests.Remove(quest);
    //        onQuestCanceled?.Invoke(quest);
    //    }

    //    Destroy(quest, Time.deltaTime);
    //}
    #endregion

    #region Robber

    void CreateNewRobber()
    {
        for (int i = 0; i < tableManager.robberList.array.Count; i++)
        {
            Robber newRobber = Instantiate(robberPrefab).GetComponent<Robber>();
            newRobber.gameObject.transform.SetParent(robberParent);

            newRobber.Setup(tableManager.robberList.array[i]);
            newRobber.RobberName = tableManager.GetRobberValue("RobberName_" + newRobber.RobberId.ToString());
            
            robberGroups.Add(newRobber);
        }
    }

    public Robber GetRobber(Province province)
    {
       List<Robber> filteredRobbers = new List<Robber>();

       filteredRobbers = robberGroups.Where(robber => robber.Province == province).ToList();

        if(filteredRobbers.Count <=0)
            return null;

       int randRobber = Random.Range(0, filteredRobbers.Count);
        return filteredRobbers[randRobber];
    }
    #endregion

}
