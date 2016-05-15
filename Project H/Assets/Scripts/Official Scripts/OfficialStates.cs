using UnityEngine;
using System.Collections;
using System;

public interface IState
{
    void Enter(AIControl _official);
    void Execute(AIControl _official);
    void Exit(AIControl _official);
}

public class GoToDoor : IState
{
    GameObject door = GameObject.Find("DoorLookAt");
    HideWaveScript waveTimer = GameObject.Find("LevelManager").GetComponent<HideWaveScript>();
    bool targetSet = false;



    public void Enter(AIControl _official)
    {

    }
    public void Execute(AIControl _official)
    {
        if (waveTimer.hideTime <= 0 && !targetSet)
        {
            _official.SetTarget(door.transform);
            //To only set target once.
            targetSet = false;
        }

        if (Vector3.Distance(_official.transform.position, door.transform.position) < 0.5f)
        {

        }

    }

    public void Exit(AIControl _official)
    {

    }
}

public class OfficialStateMachine
{
    private IState currentState;
    private IState previousState;
    private AIControl official;

    public OfficialStateMachine(AIControl _currentOfficial)
    {
        currentState = null;
        previousState = null;

        official = _currentOfficial;
    }

    public void ExecuteSM()
    {
        if (currentState != null)
        {
            currentState.Execute(official);
        }

    }

    public void RevertToPreviousState()
    {
        ChangeCurrentState(previousState);
    }

    public void ChangeCurrentState(IState newState)
    {
        if (currentState != null)
        {
            previousState = currentState;

            currentState.Exit(official);
        }

        currentState = newState;
        currentState.Enter(official);
    }
}