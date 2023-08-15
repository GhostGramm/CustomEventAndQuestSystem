using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ReachedTheBoxQuestStep : QuestStep
{
    internal Transform player;
    private void OnEnable()
    {
        player = FindAnyObjectByType<PlayerStateMachine>().transform;
    }

    private void OnDisable()
    {
        
    }

    public void Update()
    {
        float dist = Vector3.Distance(transform.position, player.position);
        Debug.Log(dist);
        if(dist < 2f) ReachedTheBox();

    }

    public void ReachedTheBox() => FinishedQuestStep();

    protected override void SetQuestStepState(string _state)
    {
        
    }
}