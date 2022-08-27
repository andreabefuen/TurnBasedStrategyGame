using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;
    [SerializeField] private TextMeshProUGUI actionPointsText;

    private List<ActionButtonUI> actionButtonUIList;
    
    void Awake()
    {
        actionButtonUIList = new List<ActionButtonUI>();
    }
    void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChange;
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChange;
        UnitActionSystem.Instance.OnActionStarted += UnitActionSystem_OnActionStarted;

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;

        CreateUnitActionButtons();
        UpdateSelectedVisual();
        UpdateActionPoints();
    }
    void CreateUnitActionButtons(){
        foreach(Transform buttonTransform in actionButtonContainerTransform){
            Destroy(buttonTransform.gameObject);
        }

        actionButtonUIList.Clear();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        //if(selectedUnit == null) return;

        foreach(BaseAction action in selectedUnit.GetBaseActionArray()){
            Transform actionButton = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
            ActionButtonUI actionButtonUI = actionButton.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(action);
            actionButtonUIList.Add(actionButtonUI);
        }
    }

    private void UnitActionSystem_OnSelectedUnitChange(object sender, EventArgs e){
        CreateUnitActionButtons();
        UpdateSelectedVisual();
        UpdateActionPoints();
    }
    private void UnitActionSystem_OnSelectedActionChange(object sender, EventArgs e){
        UpdateSelectedVisual();
    }

    private void UnitActionSystem_OnActionStarted(object sender, EventArgs e){
        UpdateActionPoints();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e){
        UpdateActionPoints();
    }

    private void UpdateSelectedVisual(){
        foreach(ActionButtonUI actionButton in actionButtonUIList){
            actionButton.UpdateSelectedVisual();
        }
    }

    private void UpdateActionPoints(){
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        actionPointsText.text = "Action Points: " + selectedUnit.GetActionPoints();
    }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e){
        UpdateActionPoints();
    }
    
}
