using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MovementData
{
    public float walkSpeed;
    public float rotationSpeed;

    public MovementData(float walkSpeed, float rotationSpeed)
    {
        this.walkSpeed = walkSpeed;
        this.rotationSpeed = rotationSpeed;
    }
}
