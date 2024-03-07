using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class StateManagerBase : MonoBehaviour
{
    public bool LogStateTransition;
    protected StateBase currentState;
    protected Dictionary<StateType, StateBase> type2StateDict = new Dictionary<StateType, StateBase>();

    public SceneUnitInfo SceneUnitInfo;

    public Action OnDeathStateOver = delegate{};

    protected void AddState(StateBase state)
    {
        state.Manager = this;
        type2StateDict.Add(state.Type, state);
    }

    protected T GetState<T>()
        where T : StateBase
    {
        foreach(var state in type2StateDict.Values)
        {
            if(state is T)
                return(T)state;
        }
        return null;
    }


    public void Update()
    {
        currentState.OnUpdate();

        if(TryGetNewState(out var newState))
            EnterNewState(newState);
    }

    public void EnterNewState(StateType type)
    {
        if (currentState != null)
        {
            currentState.OnExit();
            if(LogStateTransition)
                Debug.Log($"State OnExit {currentState.GetType().Name}");
        }


        currentState = type2StateDict[type];

        if(LogStateTransition)
            Debug.Log($"State OnEnter {currentState.GetType().Name}");
        currentState.OnEnter();
    }

    protected abstract bool TryGetNewState(out StateType newState);

}


