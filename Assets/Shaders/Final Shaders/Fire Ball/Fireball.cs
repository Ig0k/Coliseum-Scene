using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float _fallSpeed = 3f;
    [SerializeField] private GameObject _smokeParticles;

    Ray ray;
    RaycastHit hit;
    [SerializeField] private LayerMask _mask;

    private void Update()
    {
        transform.position += (-transform.up) * _fallSpeed * Time.deltaTime;

        ray = new Ray(transform.position, -transform.up);
        if(Physics.Raycast(ray.origin, ray.direction, out hit, 2f, _mask))
        {
            Vector3 pos = new Vector3(
                hit.point.x, 
                hit.point.y + 2.2f, 
                hit.point.z);

            Instantiate(_smokeParticles, pos,
                Quaternion.identity);  
            Destroy(gameObject);
        }
    }


}
