using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM 
{
    public delegate void FSMTranslationCallFunc();
    public FSMState currentState;
    Dictionary<string, FSMState> StateDict = new Dictionary<string, FSMState>();

    public class FSMState
    {
        public string name;

        public FSMState(string name)
        {
            this.name = name;
        }

        public Dictionary<string, FSMTranslation> TranslationDict = new Dictionary<string, FSMTranslation>();
    }

    public class FSMTranslation
    {
        public FSMState fromState;
        public string name;
        public FSMState toState;
        public FSMTranslationCallFunc callFunc;

        public FSMTranslation(FSMState fromState,string name,FSMState toState,FSMTranslationCallFunc callFunc)
        {
            this.fromState = fromState;
            this.toState = toState;
            this.name = name;
            this.callFunc = callFunc;
        }
    

    }

    public void AddState(FSMState state)
    {
        StateDict[state.name] = state;
    }

    public void AddTransaltion(FSMTranslation translation)
    {
        StateDict[translation.fromState.name].TranslationDict[translation.name] = translation;
    }

    public void Start(FSMState state)
    {
        currentState = state;
    }

    public void HandleEvent(string name)
    {
        if (currentState != null && currentState.TranslationDict.ContainsKey(name))
        {
            Debug.Log("fromState: " + currentState.name);
            currentState.TranslationDict[name].callFunc();
            currentState = currentState.TranslationDict[name].toState;
            Debug.Log("toState: " + currentState.name);
        }
    }



}
