using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{

    [SerializeField] private bool invert;
    private Transform cameraTransform;

    void Awake()
    {
        cameraTransform = Camera.main.transform;
    }
    void LateUpdate()
    {
        if(invert){
            Vector3 directionCam = (cameraTransform.position - transform.position).normalized;
            transform.LookAt(transform.position + directionCam * -1);
        }
        else{
            transform.LookAt(cameraTransform);

        }
    }
}
