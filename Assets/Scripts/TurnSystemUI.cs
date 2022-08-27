using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI turnText;
    [SerializeField] Button endTurnButton;

    void Start()
    {
        endTurnButton.onClick.AddListener(OnEndTurnButtonClicked);

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        UpdateTurnText();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e){
        UpdateTurnText();

    }
    private void OnEndTurnButtonClicked(){
        TurnSystem.Instance.NextTurn();

    }

    private void UpdateTurnText(){
        
        turnText.text = "TURN " + TurnSystem.Instance.GetTurnNumber();

    }
}
