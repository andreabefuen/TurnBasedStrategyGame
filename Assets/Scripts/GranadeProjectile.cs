using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadeProjectile : MonoBehaviour
{
    public static event EventHandler OnAnyGranadeExploted;
    [SerializeField] public Transform granadeExplosionVFX;
    [SerializeField] public TrailRenderer trailRenderer;
    [SerializeField] private AnimationCurve arcYAnimationCurve;
    private Vector3 targetPosition;
    float moveSpeed = 15f;
    float reachedTargetDistance = .2f;
    float damageRadius = 4f;
    int damageAmount = 30;
    float maxHeight = 3f;

    private float totalDistance;
    private Vector3 positionXZ;

    Action onGranadeBehaviourComplete;


    private void Update(){
        Vector3 moveDirection = (targetPosition - positionXZ).normalized;
        positionXZ += moveDirection * moveSpeed * Time.deltaTime;
        float distance = Vector3.Distance(positionXZ, targetPosition);
        float distanceNormalize = 1 - distance / totalDistance;

        float maxHeightClamp = totalDistance / maxHeight;

        float postionY = arcYAnimationCurve.Evaluate(distanceNormalize) * maxHeightClamp;
        transform.position = new Vector3(positionXZ.x, postionY, positionXZ.z);

        if(Vector3.Distance(positionXZ, targetPosition) < reachedTargetDistance){
            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, damageRadius);

            foreach(Collider collider in colliderArray){
                if(collider.TryGetComponent<Unit>(out Unit targetUnit)){
                    targetUnit.Damage(damageAmount);
                }
                if(collider.TryGetComponent<DestructableObject>(out DestructableObject destructableObject)){
                    destructableObject.Damage();
                }
            }
            OnAnyGranadeExploted?.Invoke(this, EventArgs.Empty);
            trailRenderer.transform.parent = null;
            Instantiate(granadeExplosionVFX, targetPosition + Vector3.up * 1f, Quaternion.identity);
            Destroy(gameObject);
            onGranadeBehaviourComplete();
        }
    }
    public void SetUp(GridPosition targetGridPosition, Action onGranadeBehaviourComplete){
        this.onGranadeBehaviourComplete = onGranadeBehaviourComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);

        positionXZ = transform.position;
        positionXZ.y = 0;
        totalDistance = Vector3.Distance(positionXZ, targetPosition);
    }
}
