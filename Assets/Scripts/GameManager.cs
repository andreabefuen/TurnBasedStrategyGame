using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    public GameObject gameOverScreen;
    public GameObject winScreen;

    private void Awake() {
        if(Instance !=null){
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start() {
        UnitManager.Instance.OnAllEnemiesDead += UnitManager_OnAllEnemiesDead;
        UnitManager.Instance.OnAllUnitsDead += UnitManager_OnAllUnitsDead;
    }

    private void UnitManager_OnAllUnitsDead(object sender, EventArgs e)
    {
        Debug.Log("We lost!");
        gameOverScreen.SetActive(true);
    }

    private void UnitManager_OnAllEnemiesDead(object sender, EventArgs e)
    {
        Debug.Log("We won!");
        winScreen.SetActive(true);

    }
}
