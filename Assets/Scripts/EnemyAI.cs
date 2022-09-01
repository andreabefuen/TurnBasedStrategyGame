using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    public enum State{
        WaitingForEnemyTurn,
        TakingTurn,
        Busy,
    }
    private State state;
    private float timer;

    void Awake()
    {
        state = State.WaitingForEnemyTurn;
    }
    void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }
    void Update()
    {
        if(TurnSystem.Instance.IsPlayerTurn()){
            return;
        }
        switch(state){
            case State.WaitingForEnemyTurn:
                break;
            case State.TakingTurn:
                timer -= Time.deltaTime;
                if(timer < 0f){
                    if(TryTakeEnemyAIAction(SetStateTakingTurn)){
                        state = State.Busy;
                    }
                    else{
                        TurnSystem.Instance.NextTurn();
                    }
                }
                break;
            case State.Busy:
                break;
        }
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e){
        if(!TurnSystem.Instance.IsPlayerTurn()){
            state = State.TakingTurn;
            timer = 2f;

        }
    }
    private void SetStateTakingTurn(){
        timer = 0.5f;
        state = State.TakingTurn;
    }

    private bool TryTakeEnemyAIAction (Action onEnemyAIActionComplete){
        Debug.Log("Taking enemy actions");
        foreach(Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList()){
            if(TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete)){
                return true;
            }
        }
        return false;

    }
    private bool TryTakeEnemyAIAction (Unit enemyUnit, Action onEnemyAIActionComplete){
        EnemyAIAction bestEnemyAIAction = null;
        BaseAction bestAction = null;
        foreach(BaseAction action in enemyUnit.GetBaseActionArray()){
            if(!enemyUnit.CanSpendActionPointsToTakeAction(action)){
                //Enemy cannot afford the cost of that action
                continue;
            }
            if(bestEnemyAIAction == null){
                bestEnemyAIAction = action.GetBestEnemyAIAction();
                bestAction = action;

            }
            else{
                EnemyAIAction testEnemyAIAction = action.GetBestEnemyAIAction();
                if(testEnemyAIAction != null && testEnemyAIAction.actionValue > bestEnemyAIAction.actionValue){
                    bestEnemyAIAction = testEnemyAIAction;
                    bestAction = action;
                }
            }
        }
        if(bestEnemyAIAction != null && enemyUnit.TrySpendActionPointToTakeAction(bestAction)){
            bestAction.TakeAction(bestEnemyAIAction.gridPosition, onEnemyAIActionComplete);
            return true;
        }
        else{
            return false;
        }
       
    }
}
