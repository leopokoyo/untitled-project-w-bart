using System;
using System.Collections.Generic;
using _1_Scripts.Leo_s_Tools.GameManagers;
using UnityEngine;
using TMPro;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI totalMoney;
    [SerializeField] private int totalMoneyAmount;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        EventManager.StartListening("dayPassed", PayUp);  
    }

    private void OnDisable()
    {
        EventManager.StopListening("dayPassed", PayUp); 
    }

    // Update is called once per frame
    void Update()
    {
        totalMoney.text = "Money: " + totalMoneyAmount;
    }

    void PayUp(Dictionary<string, object> dictionary)
    {
        int moneyOwed = (int) dictionary["daysPassed"];
        Debug.Log(moneyOwed);
        totalMoneyAmount -= moneyOwed;
    }
}
