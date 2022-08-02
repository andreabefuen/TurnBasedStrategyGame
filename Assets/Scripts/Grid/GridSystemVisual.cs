using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance {get; private set;}

    [SerializeField] public Transform gridSystemVisualSinglePrefab;

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

    }
    void Update()
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
    public void ShowAllPositionsList(List<GridPosition> gridPositions){
        foreach(GridPosition gridPos in gridPositions){
            gridSystemVisualSimpleArray[gridPos.x, gridPos.z].Show();
        }
    }

    private void UpdateGridVisual(){
        HideAllGridPositions();
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        ShowAllPositionsList(selectedUnit.GetMoveAction().GetValidActionGridPositionList());
    }

}
