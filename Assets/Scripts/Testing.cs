using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private Transform gridDebugObjectPrefab;

    [SerializeField] private Unit unit;
    private GridSystem<GridObject> gridSystem;
    void Start()
    {
        /*
        gridSystem = new GridSystem(10,10, 2f);
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab);

        Debug.Log(new GridPosition(5,7));
        */

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T)){
            GridPosition mouseGridPos = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            GridPosition startGridPosition = new GridPosition(0,0);
            
            List<GridPosition> gridPositionList = Pathfinding.Instance.FindPath(startGridPosition, mouseGridPos);

            for(int i = 0; i < gridPositionList.Count - 1; i++){
                Debug.DrawLine(LevelGrid.Instance.GetWorldPosition(gridPositionList[i]),
                LevelGrid.Instance.GetWorldPosition(gridPositionList[i +1]),
                Color.magenta,
                10f
                );
            }

        }
    }

}
