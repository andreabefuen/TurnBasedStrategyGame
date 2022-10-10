using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScreenShake : MonoBehaviour
{

    public static ScreenShake Instance {get; private set;}
    private CinemachineImpulseSource cinemachineImpulseSource;

    void Awake()
    {
        if(Instance !=null){
            Debug.LogError("There is another screen shake instance! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }


    public void Shake(float intensity = 1f){
        cinemachineImpulseSource.GenerateImpulse(intensity);
    }
}
