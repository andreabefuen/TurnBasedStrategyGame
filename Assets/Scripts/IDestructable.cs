using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDestructable
{

    void Damage();
    GridPosition GetGridPosition();

}
