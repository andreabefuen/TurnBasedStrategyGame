using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSettings : MonoBehaviour
{
    public static LevelSettings Instance {get; private set;}

    [System.Serializable]
    public class DoorElementsHided{
        public Door doorTrigger;
        public List<GameObject> hiders;
        public List<GameObject> enemies;
    }

    [System.Serializable]
    public class CloseElementsHided{
        public Transform triggerPosition;
        public List<GameObject> hiders;
        public List<GameObject> enemies;
    }

    private List<GameObject> AllEnemiesInLevel = new List<GameObject>();

    [SerializeField] private List<DoorElementsHided> hiders;
    [SerializeField] private List<CloseElementsHided> hidersClose;

    void Awake()
    {
        if(Instance !=null){
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start(){
        foreach(DoorElementsHided doorElements in hiders){
            doorElements.doorTrigger.OnDoorOpened += (object sender, EventArgs e) =>
            {
                SetActiveGameObjectList(doorElements.hiders, false);
                SetActiveGameObjectList(doorElements.enemies, true);
            };
            AllEnemiesInLevel.AddRange(doorElements.enemies);
        }
        foreach(CloseElementsHided closeElement in hidersClose){
            AllEnemiesInLevel.AddRange(closeElement.enemies);
        }

        LevelGrid.Instance.OnAnyUnitMoveGridPosition += LevelGrid_OnAnyUnitMoveGridPosition;
    }

    private void LevelGrid_OnAnyUnitMoveGridPosition(object sender, LevelGrid.OnAnyUnitMovedGridPositionEventArgs e)
    {
        foreach(CloseElementsHided closeElement in hidersClose){
            GridPosition triggerGridPosition = LevelGrid.Instance.GetGridPosition(closeElement.triggerPosition.position);
            if(e.toGridPos.z >= triggerGridPosition.z && (e.toGridPos.x == triggerGridPosition.x - 1 || e.toGridPos.x == triggerGridPosition.x + 1)){
                SetActiveGameObjectList(closeElement.hiders, false);
                SetActiveGameObjectList(closeElement.enemies, true);
            }
        }
    }

    private void SetActiveGameObjectList(List<GameObject> gameObjectList, bool isActive)
    {
        if(gameObjectList.Count<=0) return;
        foreach (GameObject gameObject in gameObjectList)
        {
            gameObject.SetActive(isActive);
        }
    }

    public List<GameObject> GetAllEnemiesInLevel(){
        return AllEnemiesInLevel;
    }

    public void RemoveEnemyFromAllEnemiesInLevel(GameObject unitToRemove){
        AllEnemiesInLevel.Remove(unitToRemove);
    }

}
