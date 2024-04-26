using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountryDate : MonoBehaviour
{

    public static CountryDate Instance;

    public delegate void ReportHandler();

    public event ReportHandler OnReport;

    [Header("Timer")]
    [SerializeField] bool isTimeFlowing = true;
    [SerializeField] float interval = 2f;
    [SerializeField] int reportDate = 1;

    Coroutine reportCoroutine;

    [Header("UI Text Set")]
    [SerializeField] Text clockText;

    #region Date Set - Value
    int yearValue;
    int monthValue;
    int dayValue;
    #endregion

    public bool IsTimeFlowing
    { get { return isTimeFlowing; } private set { isTimeFlowing = value; } }

    public void TimeFlowingSet(bool timeFlowing)
    {
        isTimeFlowing = timeFlowing;
    }
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        UpdateClockText();
        StartReport();
    }
    IEnumerator ReportDate()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(interval);

            if(isTimeFlowing&& OnReport != null) 
            {
                // 날짜 업데이트
                dayValue++;
                if (dayValue > GetMaxDayOfMonth(monthValue))
                {
                    dayValue = 1;
                    monthValue++;
                    if (monthValue > 12)
                    {
                        monthValue = 1;
                        yearValue++;
                    }
                }

                UpdateClockText();

                OnReport?.Invoke();
            }
        }
    }

    private void UpdateClockText()
    {
        string timeString = string.Format("{0:D4}년 {1:D2}월 {2:D2}일", yearValue, monthValue, dayValue);
        clockText.text = timeString;
    }
    private int GetMaxDayOfMonth(int month)
    {
        switch (month)
        {
            case 2: // February
                if (yearValue % 4 == 0 && (yearValue % 100 != 0 || yearValue % 400 == 0))
                    return 29; // Leap year
                else
                    return 28;
            default: // 월별 최대 일수 변경 없음
                return 30;
        }
    }

    bool HasActiveQuest()
    {
        if(QuestSystem.Instance.ActiveQuests.Count>0)
            return true;
        else return false;
    }
    public void StartReport()
    {
        if (reportCoroutine == null)
            reportCoroutine = StartCoroutine(ReportDate());
    }

    public void StopReport()
    {
        if (reportCoroutine != null)
        {
            StopCoroutine(reportCoroutine);
            reportCoroutine = null;
        }
    }
}
