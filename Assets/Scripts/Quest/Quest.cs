using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum QuestType
{
    Daily,
    Weekly
}
public enum QuestProgress
{
    REQUIREMENTS_NOT_MET,
    CAN_START,
    IN_PROGRESS,
    CAN_FINISH,
    FINISHED
}

public class QuestName
{
    public static string COLLECT_COINS = "CollectCoins";
}



public class Quest
{
    public QuestSO info;

    public QuestProgress progress;

    private int currentQuestStepIndex;

    public QuestStepState[] questStepStates;

    public Quest(QuestSO questInfo)
    {
        info = questInfo;
        progress = questInfo.initalProgress;
        currentQuestStepIndex = 0;
        questStepStates = new QuestStepState[info.questStepPrefabs.Length];
        for (int i = 0; i < questStepStates.Length; i++)
        {
            questStepStates[i] = new QuestStepState();
        }
    }

    public Quest(QuestSO info, QuestProgress progress, int currentQuestStepIndex, QuestStepState[] questStepStates)
    {
        this.info = info;
        this.progress = progress;
        this.currentQuestStepIndex = currentQuestStepIndex;
        this.questStepStates = questStepStates;

        if(questStepStates.Length != info.questStepPrefabs.Length)
        {
            Debug.LogWarning("Quest step prefabs and quest step states are of different lengths " + info.id);
        }
    }

    public void MoveToNextStep() => currentQuestStepIndex++;

    public bool CurrentStepExist() => currentQuestStepIndex < info.questStepPrefabs.Length;

    public void InstantiateCurrentQuestStep(Transform parentTransform)
    {
        GameObject questPrefab = GetCurrentQuestStepPrefab();
        if (questPrefab != null)
        {
            QuestStep questStep = UnityEngine.Object.Instantiate(questPrefab, parentTransform).GetComponent<QuestStep>();
            questStep.initializeQuestStep(info.id, currentQuestStepIndex, questStepStates[currentQuestStepIndex].state);
        }
    }

    private GameObject GetCurrentQuestStepPrefab()
    {
        GameObject questStepPrefab = null;

        if(CurrentStepExist())
        {
            questStepPrefab = info.questStepPrefabs[currentQuestStepIndex];
        }
        else
        {
            Debug.LogWarning("Tried to get quest step prefab, but it was exceeded");
        }
        
        return questStepPrefab;
    }

    public void StoreQuestStepState(QuestStepState questStepState, int stepIndex)
    {
        if(stepIndex < questStepStates.Length)
        {
            questStepStates[stepIndex] = questStepState;
        }
        else
        {
            Debug.LogWarning("Can't Save, StepIndex is out of Range");
        }
    }

    public QuestData GetQuestData()
    {
        return new QuestData(progress, currentQuestStepIndex, questStepStates);
    }
}

[System.Serializable]
public class QuestStepState
{
    public string state;

    public QuestStepState() => state = "";

    public QuestStepState(string _state) => state = _state;
}

[System.Serializable]
public class QuestData
{
    public QuestProgress progress;
    public int questStepIndex;
    public QuestStepState[] questStepStates;

    public QuestData(QuestProgress progress, int questStepIndex, QuestStepState[] questStepStates)
    {
        this.progress = progress;
        this.questStepIndex = questStepIndex;
        this.questStepStates = questStepStates;
    }
}
