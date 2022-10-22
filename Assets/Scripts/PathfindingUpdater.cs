using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingUpdater : MonoBehaviour
{
    void Start()
    {
        DestructableObject.OnAnyDestroyed += DestructableObject_OnAnyDestroyed;
    }

    private void DestructableObject_OnAnyDestroyed(object sender, EventArgs e)
    {
        DestructableObject destructableObject = sender as DestructableObject;
        Pathfinding.Instance.SetIsWalkable(destructableObject.GetGridPosition(), true);
    }
}
