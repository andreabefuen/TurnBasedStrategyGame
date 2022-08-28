using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private const int actionPointsMax = 2;

    public static event EventHandler OnAnyActionPointsChanged;

    [SerializeField] private bool isEnemy;

    private GridPosition gridPosition;
    private MoveAction moveAction;

    private SpinAction spinAction;
    private BaseAction[] baseActionArray;
    private int actionPoints = actionPointsMax;

    void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActionArray = GetComponents<BaseAction>();

    }
    void Start()
    {
        
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

    }

    void Update()
    {

       

        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        
        if(gridPosition != newGridPosition){
            LevelGrid.Instance.UnitMovedGridPosition(this, newGridPosition, gridPosition);
            gridPosition= newGridPosition;
    
        }

    }

    public MoveAction GetMoveAction(){
        return moveAction;
    }
    public SpinAction GetSpinAction(){
        return spinAction;
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
            actionPoints = actionPointsMax;
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
      

    }

    public bool IsEnemy(){
        return isEnemy;
    }

    public void Damage(){
        Debug.Log(transform + " damaged!");
    }
}
