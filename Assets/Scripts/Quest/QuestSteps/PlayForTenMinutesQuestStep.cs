using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayForTenMinutesQuestStep : QuestStep
{
    private float timeLeft = 300;

    public bool canStartCountDown = false;
    private void OnEnable()
    {
        GameEventHandler.Instance.OnPlayForTenMinutes += setBool;
    }

    private void OnDisable()
    {
        GameEventHandler.Instance.OnPlayForTenMinutes -= setBool;
    }

    private void Update()
    {
        if (canStartCountDown)
        {
            Debug.Log(timeLeft);
            timeLeft -= Time.deltaTime;
        }
    }

    private void setBool(bool value) => canStartCountDown = value;


    protected override void SetQuestStepState(string _state)
    {
        
    }
}