using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class newSword : Weapon
{
    [SerializeField] private int _damage;

    [SerializeField] private Collider _collider;
    [SerializeField] private Animator _animator, _crossbowAnimatior;

    //COOLDOWN PARA EVITAR QUE GOLPEE DOS VECES EN UNA
    [SerializeField] private float _maxCD = 1f, _currentCD = 0f;
    [SerializeField] private bool _onCD = false;

    private float _commonDamageProb = 50f, _criticalDamageProb = 30f, _bloodDamageProb = 20f;

    [Header("Shaders")]
    [SerializeField] private Material _bloodMaskMat;
    private float _bloodAmount = 0f;

    /*[SerializeField] private ParticleSystem _swordParticles;
    [SerializeField] private Transform _swordPos;*/

    private void Start()
    {
        _bloodAmount = 0f;
        _bloodMaskMat.SetFloat("_BloodAmount", 0f);

        //_swordParticles.transform.position = _swordPos.position;
    }

    Dictionary<string, int> damageType = new Dictionary<string, int> //Tipo de daño y daño
    {
        {"Common", 1},
        {"Blood", 2},
        {"Critical", 3}
    };

    private void Awake()
    {
        if (_collider == null) _collider = GetComponent<Collider>();
    }

    /*public void ShowParticles()
    {
        _swordParticles.transform.position = _swordPos.position;

        _swordParticles.Play();
    }*/

    public override void Attack()
    {
        #region Animations

        if (_crossbowAnimatior.GetBool("Shoot") == false)
        {
            _animator.SetBool("Attack", true);
        }
        else
        {
            _animator.SetBool("Attack", false);
        }

        if (_animator.GetBool("Attack") == true) StartCoroutine(WaitToDisable());


        #endregion
    }

    private IEnumerator WaitToDisable()
    {
        yield return new WaitForSeconds(1.5f);
        _animator.SetBool("Attack", false);
    }


    public void ActiveCollider1()
    {
        _collider.enabled = true;
    }

    public void DisableCollider()
    {
        _collider.enabled = false;
    }

    public void CancelAttackAnimation()
    {
        _animator.SetBool("Attack", false);
    }

    public void DamageType()
    {
        var probabilities = Random.Range(0, 101);
        //Debug.Log("Damge chances" + probabilities);

        if (probabilities >= _commonDamageProb)
        {
            SetProperties(damageType["Common"]);
        }
        else if (probabilities < _commonDamageProb && probabilities >= _criticalDamageProb)
        {
            SetProperties(damageType["Blood"]);
        }
        else if (probabilities < _criticalDamageProb && probabilities >= _bloodDamageProb)
        {
            SetProperties(damageType["Critical"]);
        }
    }

    public void SetProperties(int damage)
    {
        _damage = damage;
    }

    private void Update()
    {

        if (_onCD)
        {
            _currentCD += Time.deltaTime;

            if (_currentCD >= _maxCD)
            {
                _onCD = false;
                _currentCD = 0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 6)
        {
            if (!_onCD)
            {
                if (other.gameObject.TryGetComponent<IDamageable>(out IDamageable damageInteface))
                {
                    damageInteface.Life -= _damage;

                    _bloodMaskMat.SetFloat("_BloodAmount", _bloodAmount);
                    _bloodAmount = Mathf.Clamp(_bloodAmount, 0f, 0.5f);

                    _bloodAmount += 0.05f;

                    DamageType();
                }
                _onCD = true;
            }

        }
    }
}
