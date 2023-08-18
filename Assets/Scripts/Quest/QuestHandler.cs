using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class QuestHandler : Singleton<QuestHandler>
{
    public bool LoadSavedQuestStates = true;
    private Dictionary<string, Quest> _AllQuests = new Dictionary<string, Quest>();

    public int currentPlayerLevel = 1;

    private void Awake()
    {
        _AllQuests = CreateQuestMap();
    }

    private void OnEnable()
    {
        GameEventHandler.Instance.OnStartQuest += StartQuest;
        GameEventHandler.Instance.OnAdvanceQuest += AdvanceQuest;
        GameEventHandler.Instance.OnFinishQuest += FinishQuest;
        GameEventHandler.Instance.OnPlayerLevelChanged += UpdatePlayerLevel;
        GameEventHandler.Instance.OnQuesteStepStateChanged += QuestStepStateChange;
    }

    private void Start()
    {
        foreach (Quest quest in _AllQuests.Values)
        {
            if(quest.progress == QuestProgress.IN_PROGRESS)
            {
                quest.InstantiateCurrentQuestStep(transform);
            }
            GameEventHandler.Instance.OnQuestChanged?.Invoke(quest);
        }
    }

    private void Update()
    {
        foreach (Quest quest in _AllQuests.Values)
        {
            if (quest.progress == QuestProgress.REQUIREMENTS_NOT_MET && CheckRequirement(quest))
            {
                ChangeQuestState(quest.info.id, QuestProgress.CAN_START);
            }
        }
    }

    private Dictionary<string, Quest> CreateQuestMap()
    {
        QuestSO[] allQuestSOs = Resources.LoadAll<QuestSO>("Quests");

        Dictionary<string, Quest> idToQuest = new Dictionary<string, Quest>();
        foreach (QuestSO quest in allQuestSOs)
        {
            if(idToQuest.ContainsKey(quest.id))
            {
                Debug.LogWarning("Duplicate id found");
            }
            else
            {
                idToQuest.Add(quest.id, LoadQuest(quest));
            }
        }

        return idToQuest;
    }

    private Quest GetQuestByKey(string key)
    {
        Quest quest = _AllQuests[key];

        if (quest != null)  return quest;

        return null;
    }

    private void StartQuest(string id)
    {
        Debug.Log("Started Quest " +  id);
        Quest quest = GetQuestByKey(id);
        quest.InstantiateCurrentQuestStep(transform);
        ChangeQuestState(quest.info.id, QuestProgress.IN_PROGRESS);
    }

    private void AdvanceQuest(string id)
    {
        Debug.Log("Advance Quest " + id);
        Quest quest = GetQuestByKey(id);

        quest.MoveToNextStep();

        if (quest.CurrentStepExist())
        {
            quest.InstantiateCurrentQuestStep(transform);
        }
        else
        {
            ChangeQuestState(quest.info.id, QuestProgress.CAN_FINISH);
        }
    }

    private void FinishQuest(string id)
    {
        Debug.Log("Finish Quest " + id);

        Quest quest = GetQuestByKey(id);
        ClaimRewards(quest);
        ChangeQuestState(quest.info.id, QuestProgress.FINISHED);
    }

    public void ClaimRewards(Quest quest)
    {
        //Events for claiming the rewards;
        //GameEventHandler.Instance.OnPlayerLevelChanged?.Invoke(2);
    }

    private void ChangeQuestState(string _id, QuestProgress _progress)
    {
        Quest quest = GetQuestByKey(_id);
        quest.progress = _progress;
        GameEventHandler.Instance.OnQuestChanged?.Invoke(quest);
    }
    
    private void QuestStepStateChange(string id, int stepIndex, QuestStepState questStepState)
    {
        Quest quest = GetQuestByKey(id);
        quest.StoreQuestStepState(questStepState, stepIndex);
        ChangeQuestState(id, quest.progress);
    }

    public void UpdatePlayerLevel(int level)
    {
        currentPlayerLevel = level;
    }

    public bool CheckRequirement(Quest quest)
    {
        bool meetsRequirement = true;

        if (currentPlayerLevel < quest.info.levelRequirement)
        {
            meetsRequirement = false;
        }

        //check quest prerequisites for completion
        foreach(QuestSO subQuests in quest.info.questPrerequisites)
        {
            if(GetQuestByKey(subQuests.id).progress != QuestProgress.FINISHED)
            {
                meetsRequirement = false;
            }
        }

        return meetsRequirement;
    }
   

    private void OnDisable()
    {
        GameEventHandler.Instance.OnStartQuest -= StartQuest;
        GameEventHandler.Instance.OnAdvanceQuest -= AdvanceQuest;
        GameEventHandler.Instance.OnFinishQuest -= FinishQuest;
        GameEventHandler.Instance.OnPlayerLevelChanged -= UpdatePlayerLevel;
        GameEventHandler.Instance.OnQuesteStepStateChanged -= QuestStepStateChange;
    }

    private void OnApplicationQuit()
    {
        foreach (Quest quest in _AllQuests.Values)
        {
            SaveQuest(quest);
        }
    }

    private void SaveQuest(Quest quest)
    {
        try
        {
            QuestData questData = quest.GetQuestData();
            string serializeData = JsonUtility.ToJson(questData);

            //save the quest with a better way(not Player prefs)
            PlayerPrefs.SetString(quest.info.id, serializeData);

            Debug.Log(serializeData);
        }
        catch(Exception ex)
        {
            Debug.Log("Failed To Save Quest with id" + quest.info.id + ": " + ex);
        }
    }

    private Quest LoadQuest(QuestSO questInfo)
    {
        Quest quest = null;

        try
        {
            if (PlayerPrefs.HasKey(questInfo.id) && LoadSavedQuestStates)
            {
                string serializeData = PlayerPrefs.GetString(questInfo.id);
                QuestData questData = JsonUtility.FromJson<QuestData>(serializeData);
                quest = new Quest(questInfo, questData.progress, questData.questStepIndex, questData.questStepStates);
            }
            else
            {
                quest = new Quest(questInfo);
            }
        }
        catch( Exception ex )
        {
            Debug.LogError("Failed to load quest with id " + quest.info.id +" : " + ex);
        }

        return quest;
    }
}
