using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class Enemy1 : BaseEnemy, IDamageable
{
    private EnemyData _enemyData;

    [SerializeField] Transform[] _wayPoints;

    [SerializeField] NavMeshAgent _agent;

    //[SerializeField] float _speed;

    [SerializeField] Transform _player;
    [SerializeField] Transform _hand;

    //[SerializeField] int _damage;
    Ray _attackRay;
    RaycastHit _attackHit;
    [SerializeField] float _attackRayDistance = 3f;
    [SerializeField] LayerMask _attackMask;


    [Header("Distances")]

    [SerializeField] float _minDistanceToChangeWp = 2;
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

    public int Life
    {
        get
        {
            if (_blood != null && _bloodPosition != null) ShowBloodParticles();
            return _life; 
        }
        set
        {
            _life = value;
            _life = Mathf.Clamp(_life, 0, 100);

            if (_life <= 0) Die();

        }
    }

    protected override void Die()
    {
        Destroy(gameObject);
    }

    private void Awake()
    {
        _enemyData = new EnemyData(2, 3, _player);

        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        //_agent.speed = _speed;
        _agent.speed = _enemyData.moveSpeed;

        _fire.Stop();

        _blood.transform.position = _bloodPosition.position;
    }
    
    private void ShowBloodParticles()
    {
        _blood.transform.position = _bloodPosition.position;
        _blood.Play();
        if (_blood.isPaused || _blood.isStopped)
        {
            
        }
        
    }

    private void Update()
    {
        Timer();

        Vector3 directionToPlayer = _player.position - transform.position;
        directionToPlayer.y = 0f;
        Quaternion rotationToPlayer = Quaternion.LookRotation(directionToPlayer);


        float distanceFromPlayer = Vector3.Distance(transform.position, _player.position);
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

                GoToNextWayPoint();
            }
        }

        //========STOP AGENT============

        if (_life > 0)
        {
            if (distanceFromPlayer <= _minDistanceToStop)
            {
                _agent.isStopped = true;

                _animator.SetBool("stopped", true);
            }
            else
            {
                _agent.isStopped = false;

                _animator.SetBool("stopped", false);
            }
        }

        //========ATTACK PLAYER=========

        if (!_onCD && distanceFromPlayer < _minDistanceToAttackPlayer)
        {
            AttackPlayer();

            _onCD = true;

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

    void GoToNextWayPoint()
    {
        float distanceFromWps = Vector3.Distance(transform.position, _wayPoints[i].position);

        //_agent.speed = _speed;
        _agent.speed = _enemyData.moveSpeed;

        _agent.SetDestination(_wayPoints[i].position);

        if (distanceFromWps <= _minDistanceToChangeWp)
        {
            i = Random.Range(0, _wayPoints.Length);
        }
    }

    void FollowPlayer()
    {
        _agent.SetDestination(_player.position);
    }

    void AttackPlayer()
    {
        _isAttacking = true;
        _animator.SetBool("isAttacking", true);
        StartCoroutine(FinishAttackAnimation());

        if (_isAttacking) Debug.Log(name + "Attacking!");

    }


    public override void Attack()
    {
        _attackRay = new Ray(_hand.position, transform.forward);

        if (Physics.Raycast(_attackRay, out _attackHit, _attackRayDistance, _attackMask))
        {
            Debug.Log(name + " golpeó a: " + _attackHit.collider.name);

            //PlayerLife playerLifeScript = _attackHit.collider.GetComponent<PlayerLife>();
            //playerLifeScript.Life -= _damage;

            Player playerLifeScript = _attackHit.collider.GetComponent<Player>();
            //playerLifeScript.Life -= _damage;
            playerLifeScript.Life -= _enemyData.damage;
        }

    }

    IEnumerator FinishAttackAnimation()
    {
        yield return new WaitForSeconds(2.19f);
        _isAttacking = false;
        _animator.SetBool("isAttacking", false);
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
