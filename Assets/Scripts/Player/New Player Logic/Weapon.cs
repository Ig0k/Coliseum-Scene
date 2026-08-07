using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{   
    public virtual void Attack()
    {
        Debug.Log("attack");
    }

    public virtual IEnumerator Shoot()
    {
        yield return null;
    }

    public virtual IEnumerator ShootPowerUp()
    {
        yield return null;
    }

    public virtual void SetProperties(float speed, int damage)
    {

    }
}
