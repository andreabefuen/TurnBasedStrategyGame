using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float MIN_FOLLOW_Y_OFFSET = 1f;
    private const float MAX_FOLLOW_Y_OFFSET = 20f;

    [SerializeField] bool invertZoomControl = false;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float rotationSpeed = 100f;
    [SerializeField] float zoomAmount = 4f;
    [SerializeField] float zoomSpeed = 5f;
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    CinemachineTransposer cinemachineTransposer;
    Vector3 targetFollowOffset;

    void Awake()
    {
        cinemachineTransposer =  virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetFollowOffset = cinemachineTransposer.m_FollowOffset;
        
    }
    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleZoom();  

    }
    private void HandleMovement(){
        Vector3 inputMoveDirection = new Vector3(0, 0, 0);
        


        if(Input.GetKey(KeyCode.W)){
            inputMoveDirection.z += 1f;
        }
        if(Input.GetKey(KeyCode.S)){
            inputMoveDirection.z -= 1f;
        }
        if(Input.GetKey(KeyCode.A)){
            inputMoveDirection.x -= 1f;
        }
        if(Input.GetKey(KeyCode.D)){
            inputMoveDirection.x += 1f;
        }

        
        Vector3 moveVector = transform.forward * inputMoveDirection.z + transform.right * inputMoveDirection.x;
        transform.position += moveVector * moveSpeed * Time.deltaTime;
    }
    private void HandleRotation(){
        Vector3 rotation = new Vector3(0,0,0);
        if(Input.GetKey(KeyCode.Q)){
            rotation.y += 1f;
        }
        if(Input.GetKey(KeyCode.E)){
            rotation.y -= 1f;
        }
        transform.eulerAngles += rotation * rotationSpeed * Time.deltaTime;

    }
    private void HandleZoom(){
         int invert = invertZoomControl?1:-1;
        if(Input.mouseScrollDelta.y > 0){
            targetFollowOffset.y -= zoomAmount *invert;
        }
        if(Input.mouseScrollDelta.y < 0){
            targetFollowOffset.y += zoomAmount*invert;
        }

        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);

        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset, zoomSpeed * Time.deltaTime);

    }
}
