using System.Collections.Generic;
using _1_Scripts.Leo_s_Tools.GameManagers;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    private float elapsedTime = 0;
    private int elapsedDays = 0;
    private int eventCounter = 0;
    [SerializeField] private int LengthTick = 60;

    [SerializeField] private TextMeshProUGUI daysPassedText;


// Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        elapsedTime += Time.deltaTime;

        elapsedDays = (int) elapsedTime / LengthTick;

        if (eventCounter < elapsedDays)
        {
            eventCounter++;
            EventManager.TriggerEvent("dayPassed", new Dictionary<string, object>()
            {
                {"daysPassed", eventCounter}
            });
        }
        
        daysPassedText.text = "Days Passed: " + eventCounter.ToString();
    }
}
