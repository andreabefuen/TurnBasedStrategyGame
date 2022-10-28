using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private const int ACTION_POINTS_MAX = 3;

    public static event EventHandler OnAnyActionPointsChanged;

    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;


    [SerializeField] private bool isEnemy;

    private GridPosition gridPosition;

    private HealthSystem healthSystem;

    private BaseAction[] baseActionArray;
    private int actionPoints = ACTION_POINTS_MAX;

    void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();

        baseActionArray = GetComponents<BaseAction>();

    }
    void Start()
    {
        
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        healthSystem.OnDeath += HealthSystem_OnDeath;

        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);

    }

    void Update()
    {

        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        
        if(gridPosition != newGridPosition){
            GridPosition oldGridPos = gridPosition;
            gridPosition= newGridPosition;
            LevelGrid.Instance.UnitMovedGridPosition(this, newGridPosition, oldGridPos);
    
        }

    }

    //Generic for getting any action
    public T GetAction<T>() where T : BaseAction{
        foreach(BaseAction baseAction in baseActionArray){
            if(baseAction is T){
                return (T)baseAction;
            }
        }
        return null;
    }

    public GridPosition GetGridPosition(){
        return gridPosition;
    }

    public Vector3 GetWorldPosition(){
        return transform.position;
    }

    public BaseAction[] GetBaseActionArray(){
        return baseActionArray;
    }

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction){
        if(actionPoints >= baseAction.GetActionPointsCost()){
            return true;
        }
        else{
            return false;
        }
    }

    public bool TrySpendActionPointToTakeAction(BaseAction baseAction){
        if(CanSpendActionPointsToTakeAction(baseAction)){
            SpendActionPoints(baseAction.GetActionPointsCost());
            return true;
        }else{
            return false;
        }
    }
    private void SpendActionPoints(int amount){
        actionPoints -= amount;
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetActionPoints(){
        return actionPoints;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e){
        if((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn())|| 
        (!IsEnemy() && !TurnSystem.Instance.IsPlayerTurn())){
            actionPoints = ACTION_POINTS_MAX;
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void HealthSystem_OnDeath(object sender, EventArgs e){
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition,this);
        Destroy(gameObject);

        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }

    public bool IsEnemy(){
        return isEnemy;
    }

    public void Damage(int damageAmount){
        Debug.Log(transform + " damaged!");
        healthSystem.Damage(damageAmount);
    }

    public float GetHealthNormalized(){
        return healthSystem.GetHealthNormalized();
    }

    public void HealUnit(int healAmount){
        healthSystem.Heal(healAmount);
    }
}
