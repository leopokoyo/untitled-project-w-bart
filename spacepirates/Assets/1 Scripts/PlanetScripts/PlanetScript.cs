using System;
using System.Collections.Generic;
using _1_Scripts.Leo_s_Tools.GameManagers;
using UnityEngine;

public enum eventTypes
{
    DaysPassed,
}
public class PlanetScript : MonoBehaviour
{
    [SerializeField] string resource = "";
    private int AmountOfResource = 0;
    [SerializeField] private int BaseResourceStorage = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        EventManager.StartListening("dayPassed", ResetResources);
    }

    
    private void OnDisable()
    {
        EventManager.StopListening("dayPassed", ResetResources);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(AmountOfResource);
    }

    public void ResetResources(Dictionary<string, object> dictionary)
    {
        AmountOfResource = BaseResourceStorage;
        Debug.Log(AmountOfResource);
    }
}
