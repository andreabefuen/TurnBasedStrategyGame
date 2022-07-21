using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour
{
    [SerializeField] private Unit unit;

    private MeshRenderer meshRenderer;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged+= UnitActionSystem_OnSelectedUnitChanged;

        UpdateVisual();
        
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
       UpdateVisual();
    }

    private void UpdateVisual(){
        if(UnitActionSystem.Instance.GetSelectedUnit() == unit){
            meshRenderer.enabled = true;
        }
        else{
            meshRenderer.enabled = false;

        }
    }
}
