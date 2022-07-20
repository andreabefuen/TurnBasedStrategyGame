using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private Vector3 targetPosition;
    [SerializeField] private Animator unitAnimator;

    private void Move(Vector3 targetPosition){
        this.targetPosition = targetPosition;
        
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

        if(Input.GetMouseButtonDown(0)){
            Move(MouseWorld.GetPosition());
                //Move(new Vector3(4,0,4));

        }
        

    }
}
