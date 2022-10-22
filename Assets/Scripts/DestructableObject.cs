using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : MonoBehaviour
{
    public static event EventHandler OnAnyDestroyed;
    [SerializeField] private Transform destroyedPrefab;
    private GridPosition gridPosition;

    void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }

    public GridPosition GetGridPosition(){
        return gridPosition;
    }
    public void Damage(){
        Transform objectDestroyed = Instantiate(destroyedPrefab, transform.position, transform.rotation);

        ApplyExplosionToChildren(objectDestroyed, 150f, transform.position, 10f);
        Destroy(gameObject);

        OnAnyDestroyed?.Invoke(this, EventArgs.Empty);
    }

    private void ApplyExplosionToChildren(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange){
        foreach(Transform child in root){
            if(child.TryGetComponent<Rigidbody>(out Rigidbody childRB)){
                childRB.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }
            ApplyExplosionToChildren(child, explosionForce, explosionPosition, explosionRange);
        }
    }
}
