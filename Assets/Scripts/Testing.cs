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


        }
    }

}
