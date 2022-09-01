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

    //C# standard
    public event EventHandler<OnShootEventArgs> OnShoot;

    public class OnShootEventArgs : EventArgs{
        public Unit targetUnit;
        public Unit shootingUnit;
    }
    private States state;
    private float stateTimer;
    private int maxShootDistance = 7;

    private int damageAmount = 40;

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
    public override List<GridPosition> GetValidActionGridPositionList(){
        return GetValidActionGridPositionList(unit.GetGridPosition());
    }
    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPos)
    {
        List<GridPosition> validGridPositions = new List<GridPosition>();

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
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        
        state = States.Aiming;
        float aimingStateTime = 1f;
        stateTimer = aimingStateTime;
        canShootBullet = true;

        ActionStart(onActionComplete);


    }

    public void Shoot(){
        OnShoot?.Invoke(this, new OnShootEventArgs{
            targetUnit = targetUnit, shootingUnit = unit 
        });
        targetUnit.Damage(damageAmount);

    }
    public Unit GetTargetUnit(){
        return targetUnit;
    }

    public int GetMaxShootDistance(){
        return maxShootDistance;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Unit targetUnit =  LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        float healthTargetUnit = targetUnit.GetHealthNormalized();
        return new EnemyAIAction{
            gridPosition = gridPosition,
            actionValue = 100 + Mathf.RoundToInt((1 - healthTargetUnit) * 100f),
        };
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition){
        return GetValidActionGridPositionList(gridPosition).Count;
    }
}
