using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;
    [SerializeField] int maxMoveDistance = 5;

    private Vector3 targetPosition;

    
    protected override void Awake()
    {
        base.Awake();
        
        targetPosition = transform.position;
       
    }
    void Update()
    {
        if(!isActive) return;
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        float stoppingDistance = 0.1f;

         if(Vector3.Distance(transform.position, targetPosition)> stoppingDistance){
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

             

        }
        else{

            OnStopMoving?.Invoke(this, EventArgs.Empty);
            ActionCompleted();

        }
        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
    }
    public override void TakeAction(GridPosition gridPos, Action onActionComplete){
        ActionStart(onActionComplete);

        this.targetPosition = LevelGrid.Instance.GetWorldPosition(gridPos);

        OnStartMoving?.Invoke(this, EventArgs.Empty);
                
    }
     
    public override List<GridPosition> GetValidActionGridPositionList(){
        List<GridPosition> validGridPositions = new List<GridPosition>();
        GridPosition unitGridPos = unit.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++){
            for(int z = -maxMoveDistance; z<= maxMoveDistance; z++){
                GridPosition offsetGridPos = new GridPosition(x,z);
                GridPosition testGridPosition = unitGridPos + offsetGridPos;
                
                if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition)){
                    continue;
                }

                if(unitGridPos == testGridPosition){
                    //Same grid pos
                    continue;
                }
                if(LevelGrid.Instance.HasAnyUnitInGridPosition(testGridPosition)){
                    continue;
                }
                

                validGridPositions.Add(testGridPosition);
                
            }
        }
        return validGridPositions;
    }

    public override string GetActionName()
    {
        return "Move";
    }
}
