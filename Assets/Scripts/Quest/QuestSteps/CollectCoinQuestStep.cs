using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CollectCoinQuestStep : QuestStep
{
    public int coinCollected = 0;
    public int coinsToComplete = 5;

    private void OnEnable()
    {
        GameEventHandler.Instance.OnCoinCollected += CoinCollected;
    }

    private void OnDisable()
    {
        GameEventHandler.Instance.OnCoinCollected -= CoinCollected;
    }

    public void CoinCollected()
    {
        if(coinCollected < coinsToComplete)
        {
            coinCollected++;
            UpdateState();
        } 

        if(coinCollected >= coinsToComplete) FinishedQuestStep();
    }

    private void UpdateState()
    {
        string state = coinCollected.ToString();
        ChangeState(state);
    }

    protected override void SetQuestStepState(string _state)
    {
        coinCollected = int.Parse(_state);
        UpdateState();
    }
}
