using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballManager : MonoBehaviour
{
    [SerializeField] private GameObject _fireBall;
    [SerializeField] private Transform[] _spawnPoints;
    private bool _canThrow = true;

    float t = 0f;
    [SerializeField] private float _cooldown = 2f;

    private void Update()
    {
        if(t < _cooldown)
        {
            t += Time.deltaTime;
            _canThrow = false;
        }
        else
        {
            _canThrow = true;
        }

        if ((Input.GetKeyDown(KeyCode.G) || Input.GetKeyUp(KeyCode.Alpha1))
            && _canThrow)
        {
            Instantiate(_fireBall, 
                _spawnPoints[Random.Range(0,_spawnPoints.Length)].position,
                transform.rotation);

            Debug.Log("Throwing Fire Ball!");

            t = 0f;
        }
    }
}
