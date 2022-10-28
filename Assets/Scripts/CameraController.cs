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
    [SerializeField]  float zoomIncreaseAmount = 1f;
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
        Vector2 inputMoveDirection = InputManager.Instance.GetCameraMoveVector();
        
        Vector3 moveVector = transform.forward * inputMoveDirection.y + transform.right * inputMoveDirection.x;
        transform.position += moveVector * moveSpeed * Time.deltaTime;
    }
    private void HandleRotation(){
        Vector3 rotation = new Vector3(0,InputManager.Instance.GetCameraRotateAmount(),0);

        transform.eulerAngles += rotation * rotationSpeed * Time.deltaTime;

    }
    private void HandleZoom(){


        targetFollowOffset.y += InputManager.Instance.GetCameraZoomAmount() * zoomIncreaseAmount;

        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);

        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset, zoomSpeed * Time.deltaTime);

    }
}
