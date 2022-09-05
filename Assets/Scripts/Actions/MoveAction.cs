using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;
    [SerializeField] int maxMoveDistance = 5;

    private List<Vector3> positionList;
    private int currentPositionIndex = 0;


    void Update()
    {
        if(!isActive) return;

        Vector3 targetPosition = positionList[currentPositionIndex];
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

        float stoppingDistance = 0.1f;

         if(Vector3.Distance(transform.position, targetPosition)> stoppingDistance){
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

             

        }
        else{
            currentPositionIndex ++;
            if(currentPositionIndex >= positionList.Count){
                OnStopMoving?.Invoke(this, EventArgs.Empty);
                ActionCompleted();
            }


        }
        
    }
    public override void TakeAction(GridPosition gridPos, Action onActionComplete){
        List<GridPosition> gridPositionList = Pathfinding.Instance.FindPath(unit.GetGridPosition(), gridPos, out int pathLenght);
        currentPositionIndex = 0;
        positionList = new List<Vector3>();

        foreach(GridPosition gridPosition in gridPositionList){
            positionList.Add(LevelGrid.Instance.GetWorldPosition(gridPosition));
        }

        OnStartMoving?.Invoke(this, EventArgs.Empty);

        ActionStart(onActionComplete);

                
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
                if(!Pathfinding.Instance.IsWalkable(testGridPosition)){
                    continue;
                }
                if(!Pathfinding.Instance.HasPath(unitGridPos, testGridPosition)){
                    continue;
                }
                int pathfindDistanceMultiplier = 10;
                if(Pathfinding.Instance.GetPathLength(unitGridPos, testGridPosition) > maxMoveDistance * pathfindDistanceMultiplier){
                    //Path lenght too long
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
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCountAtGridPosition = unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);
        return new EnemyAIAction{
            gridPosition = gridPosition,
            actionValue = targetCountAtGridPosition * 10,
        };
    }
}
