using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{

    public static event EventHandler OnAnyDoorOpened;
    public event EventHandler OnDoorOpened;
    [SerializeField] private bool isOpen;
    private GridPosition gridPosition;
    private Animator animator;
    private Action onInteractionComplete;
    private bool isActive;
    private float timer;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(this.transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);

        if(isOpen){

            isOpen = true;
            Pathfinding.Instance.SetIsWalkable(gridPosition, isOpen);


        }
        else{
            isOpen = false;
            Pathfinding.Instance.SetIsWalkable(gridPosition, isOpen);
            animator.SetTrigger("doorCloseInstantly");
        }
    }
    void Update()
    {
        if(!isActive) return;
        timer -= Time.deltaTime;
        if(timer <= 0f){
            isActive = false;
            onInteractionComplete();
        }

    }
    public void Interact(Action onInteractionComplete){
        this.onInteractionComplete = onInteractionComplete;
        isActive = true;
        timer = .5f;
        if(isOpen){
            CloseDoor();
        }
        else{
            OpenDoor();
        }
    }

    void OpenDoor(){
        isOpen = true;
        animator.SetBool("isOpen", isOpen);
        Pathfinding.Instance.SetIsWalkable(gridPosition, isOpen);

        OnDoorOpened?.Invoke(this, EventArgs.Empty);
        OnAnyDoorOpened?.Invoke(this, EventArgs.Empty);
    }
    void CloseDoor(){
        isOpen = false;
        animator.SetBool("isOpen", isOpen);
        Pathfinding.Instance.SetIsWalkable(gridPosition, isOpen);
    }
}
