using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    private enum States{
        Aiming,
        Shooting,
        Cooloff,
    }
    private States state;
    private float stateTimer;
    private int maxShootDistance = 7;

    private Unit targetUnit;
    private bool canShootBullet;
    void Update()
    {
        if(!isActive)return;
        stateTimer -= Time.deltaTime;
        switch(state){
            case States.Aiming:
                float rotateSpeed = 10f;
                Vector3 aimDirection = (targetUnit.GetWorldPosition()  - unit.GetWorldPosition()).normalized;

                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * rotateSpeed);
 
                break;
            case States.Shooting:
                if(canShootBullet){
                    Shoot();
                    canShootBullet = false;
                }

                break;
            case States.Cooloff:

                break;

        }
        if(stateTimer <= 0f){
            NextState();
        }
       
        
    }
    private void NextState(){
        switch(state){
            case States.Aiming:
                state = States.Shooting;
                float shootingStateTime = .1f;
                stateTimer = shootingStateTime;
                break;
            case States.Shooting:
                state = States.Cooloff;
                float cooloffStateTime = .5f;
                stateTimer = cooloffStateTime;
                break;
            case States.Cooloff:
                
                ActionCompleted();
                break;
        }
    }
    public override string GetActionName()
    {
        return "Shoot";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositions = new List<GridPosition>();
        GridPosition unitGridPos = unit.GetGridPosition();

        for (int x = -maxShootDistance; x <= maxShootDistance; x++){
            for(int z = -maxShootDistance; z<= maxShootDistance; z++){
                GridPosition offsetGridPos = new GridPosition(x,z);
                GridPosition testGridPosition = unitGridPos + offsetGridPos;
                
                if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition)){
                    continue;
                }
                int testDistance =Mathf.Abs(x)+ Mathf.Abs(z);

                if(testDistance > maxShootDistance){
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
        ActionStart(onActionComplete);
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        
        state = States.Aiming;
        float aimingStateTime = 1f;
        stateTimer = aimingStateTime;
        canShootBullet = true;

    }

    public void Shoot(){
        targetUnit.Damage();

    }
}
