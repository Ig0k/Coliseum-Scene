using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newArrowReload : MonoBehaviour, IInteractuable
{
    [SerializeField] private newCrossbow _crossBow;
    [SerializeField] private int _arrowsToAdd = 7;

    [SerializeField] private ArrowUI _arrowUI;

    private void Awake()
    {
        if(_crossBow == null) _crossBow = FindObjectOfType<newCrossbow>();
    }

    public void Interact()
    {
        _crossBow.ArrowsAmount += _arrowsToAdd;
        _arrowUI.CallShowArrow();
        Destroy(gameObject);
    }


}
