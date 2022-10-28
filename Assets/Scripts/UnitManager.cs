using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance {get; private set;}
    private List<Unit> unitList;
    private List<Unit> friendlyUnitList;
    private List<Unit> enemyUnitList;

    public event EventHandler OnUnitRemovedFromFriendlyList;
    void Awake()
    {
        if(Instance !=null){
            Destroy(gameObject);
            return;
        }
        Instance = this;


        unitList = new List<Unit>();
        friendlyUnitList = new List<Unit>();
        enemyUnitList = new List<Unit>();
    }
    void Start()
    {
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }

    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {

        Unit unit = sender as Unit;
        Debug.Log(unit + " dead");

        unitList.Remove(unit);

        if(unit.IsEnemy()){
            enemyUnitList.Remove(unit);
        }
        else{
            friendlyUnitList.Remove(unit);
            OnUnitRemovedFromFriendlyList?.Invoke(this, EventArgs.Empty);
        }

    }

    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;
        Debug.Log(unit + " spawned");
        unitList.Add(unit);

        if(unit.IsEnemy()){
            enemyUnitList.Add(unit);
        }
        else{
            friendlyUnitList.Add(unit);
        }
        
    }

    public List<Unit> GetUnitList(){
        return unitList;
    }
    
    public List<Unit> GetFriendlyUnitList(){
        return friendlyUnitList;
    }
    
    public List<Unit> GetEnemyUnitList(){
        return enemyUnitList;
    }
}
