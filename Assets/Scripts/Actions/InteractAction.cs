using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractAction : BaseAction
{
    private int maxInteractDistance = 1;

    void Update()
    {
        if(!isActive)
            return;
    }
    public override string GetActionName()
    {
        return "Interact";
    }
    

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositions = new List<GridPosition>();
        GridPosition unitGridPos = unit.GetGridPosition();

        for (int x = -maxInteractDistance; x <= maxInteractDistance; x++){
            for(int z = -maxInteractDistance; z<= maxInteractDistance; z++){
                GridPosition offsetGridPos = new GridPosition(x,z);
                GridPosition testGridPosition = unitGridPos + offsetGridPos;
                
                if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition)){
                    continue;
                }
                IInteractable interactable = LevelGrid.Instance.GetInteractableAtGridPosition(testGridPosition);
                if(interactable == null){
                    continue;
                }
                validGridPositions.Add(testGridPosition);
                
            }
        }
        return validGridPositions;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        //Debug.Log("InteractAction");
        IInteractable interactable = LevelGrid.Instance.GetInteractableAtGridPosition(gridPosition);
        interactable.Interact(OnInteractComplete);

        ActionStart(onActionComplete);
    }

    private void OnInteractComplete(){
        ActionCompleted();
    }
}
