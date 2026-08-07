using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            //Debug.Log("Triggereando " + gameObject.name);

            EnemyList<BaseEnemy>.AddEnemies(other.gameObject.GetComponent<BaseEnemy>());

        }
    }
}
