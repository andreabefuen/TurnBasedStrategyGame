using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance {get; private set;}

    [Serializable]
    public struct GridVisualTypeMaterial{
        public GridVisualType gridVisualType;
        public Material material;
    }
    public enum GridVisualType{
        White,
        Blue,
        Red,
        Yellow,
        RedSoft,
    }
    [SerializeField] private Transform gridSystemVisualSinglePrefab;

    [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterialList;

    private GridSystemVisualSingle [,] gridSystemVisualSimpleArray;

    void Awake()
    {
        if(Instance != null){
            Debug.LogError("There is more than one gridsystemvisual in this scene!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        gridSystemVisualSimpleArray = new GridSystemVisualSingle[LevelGrid.Instance.GetWitdh(), LevelGrid.Instance.GetHeight()];
        for(int x = 0; x < LevelGrid.Instance.GetWitdh(); x++){
            for(int z= 0; z < LevelGrid.Instance.GetHeight(); z++ ){
                GridPosition gridPosition = new GridPosition(x,z);
                Transform gridSystemVisualSingleTransform = Instantiate(gridSystemVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);
                gridSystemVisualSimpleArray[x,z] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();

            }
        }

        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        LevelGrid.Instance.OnAnyUnitMoveGridPosition += LevelGrid_OnAnyUnitMoveGridPosition;

        UpdateGridVisual();

    }

    private void LevelGrid_OnAnyUnitMoveGridPosition(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }


    public void HideAllGridPositions(){
        for(int x = 0; x < LevelGrid.Instance.GetWitdh(); x++){
            for(int z= 0; z < LevelGrid.Instance.GetHeight(); z++ ){
                gridSystemVisualSimpleArray[x,z].Hide();
            }
        }
    }

    private void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType){
        List<GridPosition> gridPositionList = new List<GridPosition>();
        for(int x = -range; x <= range; x++){
            for(int z = -range; z <= range; z++){
                GridPosition testGridPos = gridPosition + new GridPosition(x, z);
                if(!LevelGrid.Instance.IsValidGridPosition(testGridPos)){
                     continue;
                }
                int distance = Mathf.Abs(x) + Mathf.Abs(z);
                if(distance > range){
                    continue;
                }
                gridPositionList.Add(testGridPos);
            }
        }
        ShowAllPositionsList(gridPositionList, gridVisualType);
    }
    public void ShowAllPositionsList(List<GridPosition> gridPositions, GridVisualType gridVisualType){
        foreach(GridPosition gridPos in gridPositions){
            gridSystemVisualSimpleArray[gridPos.x, gridPos.z].Show(GetGridVisualTypeMaterial(gridVisualType));
        }
    }

    private void UpdateGridVisual(){
        HideAllGridPositions();
        
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
        GridVisualType gridVisualType;
        switch (selectedAction){
            default:
            case MoveAction moveAction:
                gridVisualType = GridVisualType.White;
                break;
            case ShootAction shootAction:
                gridVisualType = GridVisualType.Red;
                ShowGridPositionRange(selectedUnit.GetGridPosition(), shootAction.GetMaxShootDistance(), GridVisualType.RedSoft);
                break;
            case SpinAction spinAction:
                gridVisualType = GridVisualType.Blue;
                break;
        }
        ShowAllPositionsList(selectedAction.GetValidActionGridPositionList(), gridVisualType);
    }
    private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType){
        foreach(GridVisualTypeMaterial gridVisualTypeMaterial in gridVisualTypeMaterialList){
            if(gridVisualTypeMaterial.gridVisualType == gridVisualType){
                return gridVisualTypeMaterial.material;
            }
        }
        Debug.LogError("Material not found GridSystemVisual");
        return null;

    }
}
