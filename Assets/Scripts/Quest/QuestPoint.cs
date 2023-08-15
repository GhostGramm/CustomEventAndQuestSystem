using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class QuestPoint : MonoBehaviour
{
    [SerializeField] private QuestSO questInfo;

    [SerializeField] private bool isPlayerNear = false;
    [SerializeField] private bool isStartingPoint = false;
    [SerializeField] private bool isFinishedPoint = false;

    private string questId;
    private QuestProgress currentQuestProgress;

    public QuestIcon progressVisual;
    public bool hasStarted = false;

    private void Awake()
    {
        questId = questInfo.id;
        progressVisual = GetComponentInChildren<QuestIcon>();
    }

    private void OnEnable()
    {
        GameEventHandler.Instance.OnQuestChanged += QuestStateChanged;
        GameEventHandler.Instance.OnInitializeQuests += CanStartQuest;
    }

    private void OnDisable()
    {
        GameEventHandler.Instance.OnQuestChanged -= QuestStateChanged;
        GameEventHandler.Instance.OnInitializeQuests -= CanStartQuest;
    }

    private void Update()
    {
        CanStartQuest();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            
        }
    }

    private void CanStartQuest()
    {
        if (!isPlayerNear) return;

        if (hasStarted) return;
       
        if (currentQuestProgress.Equals(QuestProgress.CAN_START) && isStartingPoint)
        {
            Debug.Log("starting quuest");
            hasStarted = true;
            GameEventHandler.Instance.OnStartQuest?.Invoke(questId);
        }
        else if(currentQuestProgress.Equals(QuestProgress.CAN_FINISH) && isFinishedPoint)
        {
            GameEventHandler.Instance.OnFinishQuest?.Invoke(questId);
        }
    }

    private void QuestStateChanged(Quest quest)
    {
        if(quest.info.id.Equals(questId))
        {
            currentQuestProgress = quest.progress;
            progressVisual.ChangeState(currentQuestProgress, isStartingPoint, isFinishedPoint);
            Debug.Log("Quest with id: " + questId + " updated to state: " + currentQuestProgress);

            if(quest.progress == QuestProgress.CAN_FINISH)
            {
                GameEventHandler.Instance.OnFinishQuest?.Invoke(questId);
            }
        }
    }
}
