using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float _speed, _destroyTime;
    [SerializeField] private int _damage;

    [SerializeField] private bool _isPlayer = false;

    public void SetProperties(float speed, int damage)
    {
        _speed = speed;
        _damage = damage;
    }

    public bool isPlayer
    {
        set { _isPlayer = value; }
        get { return _isPlayer; }
    }

    private void Start()
    {
        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        transform.position += transform.forward * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!_isPlayer)
        {
            if (other.gameObject.layer == 6)
            {
                //if(other.gameObject.TryGetComponent<PlayerLife>(out PlayerLife playerLife))
                //{
                //    playerLife.Life -= _damage;
                //    Destroy(gameObject);
                //}
                if (other.gameObject.TryGetComponent<Player>(out Player player))
                {
                    player.Life -= _damage;
                    Destroy(gameObject);
                }

            }
        }
        else
        {
            if (other.gameObject.layer != 6)
            {
                if(other.gameObject.TryGetComponent<IDamageable>(out IDamageable damageInteface))
                {
                    damageInteface.Life -= _damage;
                    Destroy(gameObject);
                }
                
            }

            if(other.gameObject.CompareTag("Shield"))
            {
                Debug.Log("Triggering Shield");
                Destroy(gameObject);
            }
        }
        
    }
}
