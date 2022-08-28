using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject 
{

    private GridSystem gridSystem;
    private GridPosition gridPosition;

    private List<Unit> unitsAtGrid;

    public GridObject(GridSystem gridSystem, GridPosition gridPosition){
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;

        unitsAtGrid = new List<Unit>();
        
    }

    public List<Unit> GetUnitsList(){
        return unitsAtGrid;
    }
    public void AddUnit(Unit unit){
        unitsAtGrid.Add(unit);
    }
    public void RemoveUnit(Unit unit){
        unitsAtGrid.Remove(unit);
    }

    public override string ToString()
    {
        string unitString = "";
        foreach(Unit unit in unitsAtGrid){
            unitString += unit + "\n";
        }
        return gridPosition.ToString() + "\n"+"unit: " +unitString;
    }

    public bool HasAnyUnit(){
        return unitsAtGrid.Count >0;
    }

    public Unit GetUnit(){
        if(HasAnyUnit()){
            return unitsAtGrid[0];
        }
        else{
            return null;
        }
    }

}
