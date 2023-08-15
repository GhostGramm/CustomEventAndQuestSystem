using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public abstract void Enter();
    public abstract void Tick(float deltaTime);
    public abstract void Exit();
}

public class StateMachine : MonoBehaviour
{
    private State currentState;

    private void Start()
    {
        
    }

    private void Update()
    {
        currentState?.Tick(Time.deltaTime);
    }

    public void ChangeState(State newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }
}