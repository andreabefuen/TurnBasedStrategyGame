using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    [SerializeField] private Animator unitAnimator;

    [SerializeField] int maxMoveDistance = 5;

    private Vector3 targetPosition;
    private Unit unit;
    
    void Awake()
    {
        targetPosition = transform.position;
        unit = GetComponent<Unit>();
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
    }
    public void Move(Vector3 targetPosition){
        this.targetPosition = targetPosition;
        
    }

    public List<GridPosition> GetValidActionGridPositionList(){
        List<GridPosition> validGridPositions = new List<GridPosition>();
        GridPosition unitGridPos = unit.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++){
            for(int z = -maxMoveDistance; z<= maxMoveDistance; z++){
                GridPosition offsetGridPos = new GridPosition(x,z);
                GridPosition testGridPosition = unitGridPos + offsetGridPos;
                Debug.Log(testGridPosition);
                
            }
        }
        return validGridPositions;
    }

}
