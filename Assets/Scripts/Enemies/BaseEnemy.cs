using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    public abstract void Burn();

    public abstract void Attack();

    protected abstract void LookPlayer(Quaternion rotation);

    protected abstract void Die();


}
