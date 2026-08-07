using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newSpeedBoost : MonoBehaviour, IInteractuable
{
    [SerializeField] private float _newSpeed = 10, _speedUpDuration = 10f;
    [SerializeField] private PlayerMovement _playerMovement;

    private void Awake()
    {
        if(_playerMovement == null) _playerMovement = FindObjectOfType<PlayerMovement>();
    }

    public void Interact()
    {
        _playerMovement.StartCoroutine(_playerMovement.SpeedPowerUp(_newSpeed, _speedUpDuration));
        Destroy(gameObject);
    }
}
