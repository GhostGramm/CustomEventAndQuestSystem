using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GameEventHandler : Singleton<GameEventHandler>
{
    public Action OnGameStarted;
    public Action OnCoinCollected;
    public Action OnReachedTheBox;
    public Action OnJumpPressed;
    public Action OnInitializeQuests;

    public Action<string> OnStartQuest;
    public Action<string> OnAdvanceQuest;
    public Action<string> OnFinishQuest;
    public Action<Quest> OnQuestChanged;

    public Action<int> OnPlayerLevelChanged;
    public Action<string, int, QuestStepState> OnQuesteStepStateChanged;


    private void Start()
    {

    }

    private void StartGame() => OnGameStarted?.Invoke();

    private void initializeQuest() => OnInitializeQuests?.Invoke();
}
