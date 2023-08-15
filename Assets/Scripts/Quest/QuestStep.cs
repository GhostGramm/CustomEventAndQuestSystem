using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished = false;
    private string questId;
    private int stepIndex;

    public void initializeQuestStep(string _questId, int _stepIndex, string questStepState)
    {
        questId = _questId;
        stepIndex = _stepIndex;
        if(questStepState != null && !string.IsNullOrEmpty(questStepState))
        {
            SetQuestStepState(questStepState);
        }
    }

    protected void FinishedQuestStep()
    {
        if (!isFinished)
        {
            isFinished = true;

            // send an event to advance
            GameEventHandler.Instance.OnAdvanceQuest?.Invoke(questId);

            Destroy(gameObject);
        }
    }

    protected abstract void SetQuestStepState(string _state);

    protected void ChangeState(string newState) => GameEventHandler.Instance.OnQuesteStepStateChanged?.Invoke(questId, stepIndex, new QuestStepState(newState));
}
