
using UnityEngine;

public class LoginQuestStep : QuestStep
{
    private void OnEnable()
    {
        GameEventHandler.Instance.OnPlayerLogin += LoggedIn;
    }

    private void OnDisable()
    {
        GameEventHandler.Instance.OnPlayerLogin -= LoggedIn;
    }

    private void Update()
    {
        
    }

    private void LoggedIn()
    {
        Debug.Log("Player has logged in");
        FinishedQuestStep();
    }

    protected override void SetQuestStepState(string _state)
    {
        
    }
}
