using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    private GridPosition gridPosition;
    private MoveAction moveAction;

    private SpinAction spinAction;
    private BaseAction[] baseActionArray;
    private int actionPoints = 2;

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
    }

    public int GetActionPoints(){
        return actionPoints;
    }
}
