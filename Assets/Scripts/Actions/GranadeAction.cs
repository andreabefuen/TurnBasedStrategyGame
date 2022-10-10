using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadeAction : BaseAction
{
    [SerializeField]
    private Transform grenadeProjectilePrefab;
    private int maxThrowDistance = 7;
    void Update()
    {
        if(!isActive){
            return;
        }

    }
    public override string GetActionName()
    {
        return "Grenade";
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

        for (int x = -maxThrowDistance; x <= maxThrowDistance; x++){
            for(int z = -maxThrowDistance; z<= maxThrowDistance; z++){
                GridPosition offsetGridPos = new GridPosition(x,z);
                GridPosition testGridPosition = unitGridPos + offsetGridPos;
                
                if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition)){
                    continue;
                }
                int testDistance =Mathf.Abs(x)+ Mathf.Abs(z);

                if(testDistance > maxThrowDistance){
                    continue;
                }
               

                validGridPositions.Add(testGridPosition);
                
            }
        }
        return validGridPositions;
    
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        Debug.Log("GrenadeAction!");
        Transform grenadeProjectileTransform = Instantiate(grenadeProjectilePrefab, unit.GetWorldPosition(), Quaternion.identity);
        GranadeProjectile granadeProjectile = grenadeProjectileTransform.GetComponent<GranadeProjectile>();
        granadeProjectile.SetUp(gridPosition, OnGranadeBehaviourComplete);
        ActionStart(onActionComplete);

    }

    private void OnGranadeBehaviourComplete(){
        ActionCompleted();
    }
}
