using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected Unit unit;
    protected bool isActive;
    protected Action onActionComplete;
    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public abstract string GetActionName();

    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);
    public virtual bool IsValidActionGridPosition(GridPosition gridPos){
         List<GridPosition> validPositions = GetValidActionGridPositionList();
        return validPositions.Contains(gridPos);
     }
    public abstract List<GridPosition> GetValidActionGridPositionList();

    public virtual int GetActionPointsCost(){
        return 1;
    }

    protected void ActionStart(Action onActionComplete){
        isActive = true;
        this.onActionComplete = onActionComplete;
    }
    protected void ActionCompleted(){
        isActive = false;
        onActionComplete();
    }
}
