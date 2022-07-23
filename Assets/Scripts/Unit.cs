using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Animator unitAnimator;

    private Vector3 targetPosition;
    private GridPosition gridPosition;

    public void Move(Vector3 targetPosition){
        this.targetPosition = targetPosition;
        
    }
    void Awake()
    {
        targetPosition = transform.position;
    }
    void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
    }

    void Update()
    {

        float stoppingDistance = 0.1f;

         if(Vector3.Distance(transform.position, targetPosition)> stoppingDistance){
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            float rotateSpeed = 10f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
            
            unitAnimator.SetBool("isWalking", true);

        }
        else{
            unitAnimator.SetBool("isWalking", false);

        }

        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        
        if(gridPosition != newGridPosition){
            LevelGrid.Instance.UnitMovedGridPosition(this, newGridPosition, gridPosition);
            gridPosition= newGridPosition;
    
        }

    }
}
