using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Button button;
    [SerializeField] private GameObject selectedAction;

    private BaseAction baseAction;

    public void SetBaseAction(BaseAction action){
        this.baseAction = action;
        text.text = action.GetActionName().ToUpper();

        button.onClick.AddListener(()=> {
            UnitActionSystem.Instance.SetSelectedAction(action);
        });
    }

    public void UpdateSelectedVisual(){
        BaseAction selectedBaseAction = UnitActionSystem.Instance.GetSelectedAction();
        
        selectedAction.SetActive(selectedBaseAction == baseAction);
    }


}
