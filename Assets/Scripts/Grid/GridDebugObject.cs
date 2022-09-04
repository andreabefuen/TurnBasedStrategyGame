using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridDebugObject : MonoBehaviour
{
    private object gridObject;
    [SerializeField] private TextMeshPro textMesh;

    public virtual void SetGridObject(object gridObject){
        this.gridObject = gridObject;
    }

    protected virtual void Update()
    {
        textMesh.text = gridObject.ToString();
    }
}
