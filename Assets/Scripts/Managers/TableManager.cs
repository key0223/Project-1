using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class StringTable
{
    public string stringId;
    public string ko;
    public string en;
}
[System.Serializable]
public class StringTables
{
    public string md5;
    public List<StringTable> array;
}

#region Quest
[System.Serializable]
public class QuestStringTables
{
    public string md5;
    public StringTable[] array;
}

[System.Serializable]
public class QuestList
{
    public string md5;
    public List<QuestUnit> array;
}
#endregion

#region Robber
[System.Serializable]
public class RobberGroupStringTables
{
    public string md5;
    public StringTable[] array;
}
[System.Serializable]
public class RobberList
{
    public string md5;
    public List<RobberUnit> array;
}

#endregion
[System.Serializable]

public class TableManager : MonoBehaviour
{
    string path_gui = "DataTable/StringTable_GUI";

    string path_questList = "DataTable/QuestList";
    string path_stringTable_Quest = "DataTable/StringTable_Quest";

    string path_robberList = "DataTable/RobberList";
    string path_stringTable_Group = "DataTable/StringTable_Group";

    public StringTables stringTable_GUI = new StringTables();

    public QuestStringTables questStringTables = new QuestStringTables();
    public QuestList questList = new QuestList();

    public RobberGroupStringTables robberGroupStringTable = new RobberGroupStringTables();
    public RobberList robberList = new RobberList();

    Dictionary<string, string> gui_stringDict;
    Dictionary<string, string> quest_stringDict;
    Dictionary<string, string> robber_stringDict;

    private void Awake()
    {
        LoadStringTable(path_gui);
        LoadQuestUnit(path_questList);
        LoadQuestStringTable(path_stringTable_Quest);

        LoadRobberUnit(path_robberList);
        LoadRobberGroupStringTable(path_stringTable_Group);
    }

    #region GUI
    public void LoadStringTable(string path)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        stringTable_GUI = JsonUtility.FromJson<StringTables>(textAsset.text);
        GetPlayerLanguage(stringTable_GUI);
    }

  public void GetPlayerLanguage(StringTables stringTable)
    {
        gui_stringDict = stringTable.array.ToDictionary(s => s.stringId, s => s.ko, StringComparer.OrdinalIgnoreCase);
    }

    public string GetValue(string key)
    {
        string a = gui_stringDict[key];
        return a;
    }
    #endregion
    #region Quest
    void LoadQuestUnit(string path)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        questList = JsonUtility.FromJson<QuestList>(textAsset.text);

    }

    public QuestUnit GetQuestUnitById(int questId)
    {
        QuestUnit questUnit = questList.array.FirstOrDefault(q => q.QuestId == questId);
        return questUnit;
    }

    void LoadQuestStringTable(string path)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        questStringTables = JsonUtility.FromJson<QuestStringTables>(textAsset.text);

        ListToDict(questStringTables);
    }

    void ListToDict(QuestStringTables questStringTables)
    {
        quest_stringDict = questStringTables.array.ToDictionary(s => s.stringId, s => s.ko);
    }

    public string GetQuestValue(string key)
    {
        string a = quest_stringDict[key];
        return a;
    }
    #endregion

    #region Robber
    void LoadRobberUnit(string path)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        robberList = JsonUtility.FromJson<RobberList>(textAsset.text);
    }

    void LoadRobberGroupStringTable(string path)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        robberGroupStringTable = JsonUtility.FromJson<RobberGroupStringTables>(textAsset.text);

        ListToDict(robberGroupStringTable);
    }
    void ListToDict(RobberGroupStringTables robberGroupStringTables)
    {
        robber_stringDict = robberGroupStringTables.array.ToDictionary(s=>s.stringId, s => s.ko);
    }
    public string GetRobberValue(string key)
    {
        string a = robber_stringDict[key];
        return a;
    }
    #endregion
}
