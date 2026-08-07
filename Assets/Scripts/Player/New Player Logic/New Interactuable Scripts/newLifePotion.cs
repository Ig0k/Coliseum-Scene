using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newLifePotion : MonoBehaviour, IInteractuable
{
    [SerializeField] private Player _player;

    [SerializeField] private int _addLife = 5;

    private void Awake()
    {
        if(_player == null) _player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        EventManager.Subscribe(EventType.OnHealth, _player.PlayerHealth);
    }

    public void Interact()
    {
        EventManager.Trigger(EventType.OnHealth, _addLife);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        EventManager.UnSubscribe(EventType.OnHealth, _player.PlayerHealth);
    }
}
