using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridDebugObject : MonoBehaviour
{
    private GridObject gridObject;
    [SerializeField] private TextMeshPro textMesh;

    public void SetGridObject(GridObject gridObject){
        this.gridObject = gridObject;
    }

    void Update()
    {
        textMesh.text = gridObject.ToString();
    }
}
