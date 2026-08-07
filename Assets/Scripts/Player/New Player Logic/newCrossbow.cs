using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class newCrossbow : Weapon
{
    [SerializeField] private Animator _animator, _swordAnimator;
    [SerializeField] private GameObject _playerArrow;
    [SerializeField] private Transform _sight, _sight2, _sight3;

    [SerializeField] private Arrow _arrowScript;

    private float _speed;
    private int _damage;

    public bool isPowerUp = false;

    private const bool _isPlayer = true;

    [SerializeField] private int _arrowsLeft = 10;
    [SerializeField] private int _maxArrows = 20;

    [SerializeField] private TMP_Text _arrowsText;

    [SerializeField] private bool _canShootCrossBow = false;

    [SerializeField] private GameObject _sword;

    [SerializeField] private float _maxCD = 1f, _currentCD = 0f;
    [SerializeField] private bool _onCD = false;

    public bool hasArrowsLeft = true;

    public int ArrowsAmount
    {
        get { return _arrowsLeft; }
        set
        {
            _arrowsLeft = value;
            _arrowsLeft = Mathf.Clamp(_arrowsLeft, 0, _maxArrows);
        }
    }

    public override void SetProperties(float speed, int damage)
    {
        _speed = speed;
        _damage = damage;
    }

    private void Start()
    {
        _arrowScript.SetProperties(_speed, _damage);
    }

    public override void Attack()
    {
        if (!_onCD)
        {
            #region Animations
            if (_swordAnimator.GetBool("Attack") == false)
            {
                _animator.SetBool("Shoot", true);
            }
            else
            {
                _animator.SetBool("Shoot", false);
            }
            _onCD = true;

            //if(_animator.GetBool("Shoot") == true) StartCoroutine(WaitToDisable());
        }


        #endregion Animations
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

        if(_arrowsLeft <= 0)
        {
            hasArrowsLeft = false;
        }
        else
        {
            hasArrowsLeft = true;
        }
    }

    public void CancelShootAnimation() 
    {
        _animator.SetBool("Shoot", false);
    }

    public void ShootForAnimationEvent()
    {
        if (!isPowerUp)
        {
            StartCoroutine(Shoot());
        }
        else
        {
            StartCoroutine(ShootPowerUp());
        }

    }


    public override IEnumerator Shoot()
    {
        _arrowScript.isPlayer = _isPlayer;
        if (_arrowsLeft > 0)
        {
            _arrowsLeft--;
            Instantiate(_playerArrow, _sight.position, _sight.rotation);
        }
        Debug.Log("Flechas Restantes:" + _arrowsLeft);
        yield return null;
    }



    public IEnumerator PowerUpTimer(float powerUpDuration)
    {
        yield return new WaitForSeconds(8f);
        isPowerUp = false;
    }

    public override IEnumerator ShootPowerUp()
    {
        _arrowScript.isPlayer = _isPlayer;
        if (_arrowsLeft >= 3)
        {
            _arrowsLeft -= 3;

            Instantiate(_playerArrow, _sight.position, _sight.rotation);
            yield return new WaitForSeconds(.13f);
            Instantiate(_playerArrow, _sight2.position, _sight.rotation);
            yield return new WaitForSeconds(.13f);
            Instantiate(_playerArrow, _sight3.position, _sight.rotation);
            yield return new WaitForSeconds(.13f);
        }
        else if (_arrowsLeft > 0 && _arrowsLeft <= 2)
        {
            _arrowsLeft--;
            Instantiate(_playerArrow, _sight.position, _sight.rotation);
        }
        Debug.Log("Flecha instanciada con Power Up.");
        Debug.Log("Flechas Restantes:" + _arrowsLeft);
    }

}
