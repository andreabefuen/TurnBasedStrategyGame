using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadeProjectile : MonoBehaviour
{
    
    private Vector3 targetPosition;
    float moveSpeed = 15f;
    float reachedTargetDistance = .2f;
    float damageRadius = 4f;
    int damageAmount = 30;

    Action onGranadeBehaviourComplete;


    private void Update(){
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        if(Vector3.Distance(transform.position, targetPosition) < reachedTargetDistance){
            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, damageRadius);

            foreach(Collider collider in colliderArray){
                if(collider.TryGetComponent<Unit>(out Unit targetUnit)){
                    targetUnit.Damage(damageAmount);
                }
            }
            Destroy(gameObject);

            onGranadeBehaviourComplete();
        }
    }
    public void SetUp(GridPosition targetGridPosition, Action onGranadeBehaviourComplete){
        this.onGranadeBehaviourComplete = onGranadeBehaviourComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
    }
}
