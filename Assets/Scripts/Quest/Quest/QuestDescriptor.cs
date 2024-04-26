using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuestDescriptor : MonoBehaviour
{
    [SerializeField] Text escortText;
    [SerializeField] Text questTypeText;
    [SerializeField] Text currentPathText;
    [SerializeField] Image pyoduImage;
    
    public void UpdateCurrentPath(Quest quest, Route currentRoute)
    {
        //int escortCount = Mathf.Max(0, quest.questUnit.Escort - 1);
        int escortCount = quest.AttendedEscortList.Count-1;
        escortText.text = $"{quest.Pyodu.PyosaName} ¿Ü {escortCount}";
        questTypeText.text = quest.QuestType.ToString();
        currentPathText.text = currentRoute.Dest.ToString();
        //currentPathText.text = quest.CurrentRoute.Dest.ToString();
    }
  

    public void OnComplete()
    {
        currentPathText.text = "¿Ï·á";
    }

}
