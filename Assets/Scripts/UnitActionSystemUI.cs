using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;

    void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChange;
        CreateUnitActionButtons();
    }
    void CreateUnitActionButtons(){
        foreach(Transform buttonTransform in actionButtonContainerTransform){
            Destroy(buttonTransform.gameObject);
        }

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        if(selectedUnit == null) return;

        foreach(BaseAction action in selectedUnit.GetBaseActionArray()){
            Transform actionButton = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
            ActionButtonUI actionButtonUI = actionButton.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(action);
        }
    }

    private void UnitActionSystem_OnSelectedUnitChange(object sender, EventArgs e){
        CreateUnitActionButtons();
        
    }
    
}
