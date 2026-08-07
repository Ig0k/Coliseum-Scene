using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    Ray ray;
    [SerializeField] private LayerMask _mask;

    [SerializeField] private int _damage = 10;
    [SerializeField] private float _radius = 20f;

    private bool _canMakeDmg = true;

    float maxT = 0.5f;
    float t = 0f;

    private void Update()
    {
        if (t < maxT) t += Time.deltaTime;
        else return;

       Vector3 pos = new Vector3(
                    transform.position.x,
                    transform.position.y + 6f,
                    transform.position.z);

        ray = new Ray(pos, transform.forward);

        Collider[] col = Physics.OverlapSphere(pos, _radius, _mask);
        foreach(var collider in col)
        {
            if(collider.TryGetComponent<IDamageable>(out IDamageable dmg))
            {
                if(dmg != null) dmg.Life -= _damage;
            }
        }
    }
}
