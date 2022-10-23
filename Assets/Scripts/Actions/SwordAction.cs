using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : BaseAction
{
    public static event EventHandler OnAnySwordHit;
    public event EventHandler OnSwordActionStarted;
    public event EventHandler OnSwordActionCompleted;

    [SerializeField] private int damageAmount;
    private enum State{
        SwingingSwordBeforeHit,
        SwingingSwordAfterHit,
    }
    private State state;
    private float stateTimer;
    int maxSwordDistance = 1;

    private Unit targetUnit;
    void Update()
    {
        if(!isActive){
            return;
        }
        stateTimer -= Time.deltaTime;
        switch(state){
            case State.SwingingSwordBeforeHit:
                float rotateSpeed = 10f;
                Vector3 aimDirection = (targetUnit.GetWorldPosition()  - unit.GetWorldPosition()).normalized;
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * rotateSpeed);
                break;
            case State.SwingingSwordAfterHit:
                
                break;

        }
        if(stateTimer <= 0f){
            NextState();
        }
    }
    private void NextState(){
        switch(state){
            case State.SwingingSwordBeforeHit:
                state = State.SwingingSwordAfterHit;
                float afterHitStateTime = .5f;
                stateTimer = afterHitStateTime;
                targetUnit.Damage(damageAmount);
                OnAnySwordHit?.Invoke(this, EventArgs.Empty);
                break;
            case State.SwingingSwordAfterHit:
                OnSwordActionCompleted?.Invoke(this, EventArgs.Empty);
                ActionCompleted();
                break;
        }
    }
    public override string GetActionName()
    {
        return "Sword";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 200,

        };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositions = new List<GridPosition>();
        GridPosition unitGridPos = unit.GetGridPosition();

        for (int x = -maxSwordDistance; x <= maxSwordDistance; x++){
            for(int z = -maxSwordDistance; z<= maxSwordDistance; z++){
                GridPosition offsetGridPos = new GridPosition(x,z);
                GridPosition testGridPosition = unitGridPos + offsetGridPos;
                
                if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition)){
                    continue;
                }
                if(!LevelGrid.Instance.HasAnyUnitInGridPosition(testGridPosition)){
                    //Grid position empty, without enemy

                    continue;
                }
                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                if(targetUnit.IsEnemy() == unit.IsEnemy()){
                    //Both units are in the same team, no shooting
                    continue;
                }
               

                validGridPositions.Add(testGridPosition);
                
            }
        }
        return validGridPositions;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        state = State.SwingingSwordBeforeHit;
        float beforeHitStateTime = .7f;
        stateTimer = beforeHitStateTime;

        OnSwordActionStarted?.Invoke(this, EventArgs.Empty);

        ActionStart(onActionComplete);
    }

    public int GetMaxSwordDistance(){
        return maxSwordDistance;
    }
}
