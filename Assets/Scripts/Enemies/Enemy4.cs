using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class Enemy4 : BaseEnemy, IDamageable
{
    private EnemyData _enemyData;

    [SerializeField] NavMeshAgent _agent;

    [SerializeField] private Player _player;
    [SerializeField] Transform _playerTransform;
    [SerializeField] Transform _hand;

    [SerializeField] private Transform _shieldPosition, _shieldAtBack;
    [SerializeField] private Transform _shieldTransform;
    [SerializeField] private BoxCollider _shieldCollider;

    Ray _attackRay;
    RaycastHit _attackHit;
    [SerializeField] float _attackRayDistance = 3f;
    [SerializeField] LayerMask _attackMask;

    [Header("Distances")]

    [SerializeField] float _minDistanceToFollowPlayer = 5f;
    [SerializeField] float _minDistanceToAttackPlayer = 5f;
    [SerializeField] float _minDistanceToStop = 2;

    int i = 0;

    //=========COOLDOWN PROPERTIES===========
    [Header("Cooldown Properties")]

    [SerializeField] private float _maxCD = 3f;
    [SerializeField] private float _currentCD = 0f;
    [SerializeField] private bool _onCD = false;

    [Header("Booleans")]

    [SerializeField] bool _isAttacking = false;
    public bool isFollowningPlayer = false;
    [SerializeField] bool _isGoingToAWayPoint = false;

    [SerializeField] Animator _animator;

    [SerializeField] private int _life;

    public bool shootedByPlayer = false;

    [Header("Particles")]

    [SerializeField] private ParticleSystem _blood;
    [SerializeField] private Transform _bloodPosition;

    [SerializeField] private ParticleSystem _fire;

    private bool _canBurn = true;

    [Header("Shaders")]
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Material _ogMat, _disolveMat;
    [SerializeField] private bool _disolveOn = false;
    [SerializeField] private float _disolveValue = -2f;

    [SerializeField] private Material _bloodMat;
    private bool _showBlood = true;
    [SerializeField] bool isRed = false;
    float _bloodSpeed = 1f;
    float t = 0f;

    [SerializeField] private Material _fadeMat;
    float fadeT = 0f;

    public int Life
    {
        get
        {
            if (_blood != null && _bloodPosition != null) ShowBloodParticles();

            if (_showBlood)
            {
                isRed = false;
                StartCoroutine(RedDamageOn());
            }

            return _life;
        }
        set
        {
            _life = value;
            _life = Mathf.Clamp(_life, 0, 100);

            if (_life <= 0)
            {
                _disolveOn = true;
                Die();
            }

        }
    }

    protected override void Die()
    {
        Destroy(gameObject, 2f);
    }

    private void Awake()
    {
        _enemyData = new EnemyData(2, 3, _playerTransform);

        if(_agent == null) _agent = GetComponent<NavMeshAgent>();
        if(_animator == null) _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _agent.speed = _enemyData.moveSpeed;

        //=========================================

        _renderer.material = _fadeMat;

        _fadeMat.SetFloat("_t", 0);

        _disolveValue = -2f;

        _fire.Stop();

        _blood.transform.position = _bloodPosition.position;
    }
    private IEnumerator RedDamageOn()
    {
        _showBlood = false;
        _renderer.material = _bloodMat;

        while (_bloodMat.GetFloat("_Instensity") > 0.1 && !isRed)
        {
            //t = 0f;
            t += Time.deltaTime * 2;
            float lerp = Mathf.Lerp(1f, 0.1f, t);
            yield return null;

            _bloodMat.SetFloat("_Instensity", (lerp));
            if (t >= 1)
            {
                isRed = true;
                t = 0f;
            }
        }
        yield return null;


        while (_bloodMat.GetFloat("_Instensity") > 0.1 && isRed)
        {
            //t = 0f;
            t += Time.deltaTime * 2;
            float lerp = Mathf.Lerp(0.1f, 1f, t);
            yield return null;

            _bloodMat.SetFloat("_Instensity", (lerp));
            if (t >= 1)
            {
                isRed = false;
                t = 0f;
            }
        }


        yield return new WaitForSeconds(0.2f);

        //_renderer.material = _ogMat;

        _showBlood = true;
    }
    private void ShowBloodParticles()
    {
        _blood.transform.position = _bloodPosition.position;
        _blood.Play();
    }
    private IEnumerator FadeAtSpawn()
    {
        _renderer.material = _fadeMat;
        _fadeMat.SetFloat("_t", 0);

        while (_fadeMat.GetFloat("_t") < 1)
        {
            fadeT += Time.deltaTime * 1.3f;
            float lerp = Mathf.Lerp(0f, 1f, fadeT);
            yield return null;

            Debug.Log("Fading");

            _fadeMat.SetFloat("_t", lerp);
        }
        yield return new WaitForSeconds(2f);
        _renderer.material = _ogMat;
    }

    private void OnEnable()
    {
        StartCoroutine(FadeAtSpawn());
    }
    private void Update()
    {
        if (_disolveOn)
        {
            _renderer.material = _disolveMat;

            if (_life <= 0)
            {
                _disolveValue += Time.deltaTime * 2f;
            }

            _disolveMat.SetFloat("_DisolveHeight", _disolveValue);
        }

        //===========================================================

        Timer();

        Vector3 directionToPlayer = _playerTransform.position - transform.position;
        directionToPlayer.y = 0f;
        Quaternion rotationToPlayer = Quaternion.LookRotation(directionToPlayer);

        float distanceFromPlayer = Vector3.Distance(transform.position, _playerTransform.position);
        if (_life > 0)
        {
            if (distanceFromPlayer <= _minDistanceToFollowPlayer || shootedByPlayer)
            {
                isFollowningPlayer = true;
                _isGoingToAWayPoint = false;

                FollowPlayer();
                LookPlayer(rotationToPlayer);
            }
            else
            {
                _isGoingToAWayPoint = true;
                isFollowningPlayer = false;
            }
        }

        //========STOP AGENT============

        if (_life > 0)
        {
            if (_player.state == AttackState.SHOOT) //ENUM
            {
                _agent.isStopped = true;

                _animator.SetBool("shield", true);

                _shieldTransform.position = _shieldPosition.position;
                _shieldTransform.rotation = transform.rotation;

                _shieldCollider.enabled = true;

            }
            else
            {
                _agent.isStopped = false;

                _animator.SetBool("shield", false);

                _shieldTransform.position = _shieldAtBack.position;

                _shieldCollider.enabled = false;
            }

            if (distanceFromPlayer <= _minDistanceToStop)
            {
                _agent.isStopped = true;

                _animator.SetBool("idle legs", true);
            }
            else
            {
                _agent.isStopped = false;

                _animator.SetBool("idle legs", false);
            }
        }

        //========ATTACK PLAYER=========

        if (!_onCD && distanceFromPlayer < _minDistanceToAttackPlayer)
        {
            //AttackPlayer();

            _animator.SetBool("attack", true);

            _onCD = true;

        }
        else if(distanceFromPlayer > _minDistanceToAttackPlayer)
        {
            _animator.SetBool("attack", false);
        }

        else if (_onCD) _isAttacking = false;

    }

    protected override void LookPlayer(Quaternion rotation)
    {
        transform.rotation = rotation;
    }

    void Timer()
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

    void FollowPlayer()
    {
        _agent.SetDestination(_playerTransform.position);
    }

    public override void Attack()
    {
        _attackRay = new Ray(_hand.position, transform.forward);

        if (Physics.Raycast(_attackRay, out _attackHit, _attackRayDistance, _attackMask))
        {
            Debug.Log(name + " golpeó a: " + _attackHit.collider.name);

            Player playerLifeScript = _attackHit.collider.GetComponent<Player>();
            playerLifeScript.Life -= _enemyData.damage;
        }

    }

    public override void Burn()
    {
        if (_canBurn && Life > 0) StartCoroutine(BurnCorrutine());
    }

    private IEnumerator BurnCorrutine()
    {
        _canBurn = false;

        _fire.gameObject.SetActive(true);
        _fire.Play();

        Life -= 1;

        yield return new WaitForSeconds(1f);

        Life -= 1;

        yield return new WaitForSeconds(1f);

        _fire.Stop();

        _canBurn = true;
    }
}
