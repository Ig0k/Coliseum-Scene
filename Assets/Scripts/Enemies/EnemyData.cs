using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EnemyData
{
    public int damage;
    public float moveSpeed;
    public Transform playerTransform;

    public float arrowSpeed;

    public EnemyData(int damage, float moveSpeed, Transform playerTransform)
    {
        this.damage = damage;
        this.moveSpeed = moveSpeed;
        this.playerTransform = playerTransform;
        arrowSpeed = 0;
    }

    public EnemyData(int damage, float moveSpeed, Transform playerTransform, float arrowSpeed)
    {
        this.damage = damage;
        this.moveSpeed = moveSpeed;
        this.playerTransform = playerTransform;
        this.arrowSpeed = arrowSpeed;
    }
}
