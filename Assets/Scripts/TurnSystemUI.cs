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

    [SerializeField] GameObject enemyTurnVisualGameObject;

    void Start()
    {
        endTurnButton.onClick.AddListener(OnEndTurnButtonClicked);

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e){
        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
 

    }
    private void OnEndTurnButtonClicked(){
        TurnSystem.Instance.NextTurn();

    }

    private void UpdateTurnText(){
        
        turnText.text = "TURN " + TurnSystem.Instance.GetTurnNumber();

    }
    private void UpdateEnemyTurnVisual(){
        enemyTurnVisualGameObject.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    }

    private void UpdateEndTurnButtonVisibility(){
        endTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }
}
