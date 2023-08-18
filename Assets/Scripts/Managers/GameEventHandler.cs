using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GameEventHandler : Singleton<GameEventHandler>
{
    #region QuestEvents

    public Action OnCoinCollected;
    public Action OnReachedTheBox;
    public Action OnPlayerLogin;
    public Action<bool> OnPlayForTenMinutes;

    #endregion

    public Action OnGameStarted;

    public Action OnJumpPressed;
    public Action OnInitializeQuests;

    public Action<string> OnStartQuest;
    public Action<string> OnAdvanceQuest;
    public Action<string> OnFinishQuest;
    public Action<Quest> OnQuestChanged;

    public Action<int> OnPlayerLevelChanged;
    public Action<string, int, QuestStepState> OnQuesteStepStateChanged;
}
