using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newShootBoost : MonoBehaviour, IInteractuable
{
    [SerializeField] private newCrossbow _crossBow;
    [SerializeField] private float _crossBowPowerUpDuration = 10f;

    private void Awake()
    {
        if(_crossBow == null) _crossBow = FindObjectOfType<newCrossbow>();
    }

    public void Interact()
    { 
        _crossBow.isPowerUp = true;
        _crossBow.StartCoroutine(_crossBow.PowerUpTimer(_crossBowPowerUpDuration));
        Destroy(gameObject);
    }
}
