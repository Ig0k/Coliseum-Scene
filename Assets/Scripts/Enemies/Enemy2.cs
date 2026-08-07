using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Enemy2 : BaseEnemy, IDamageable
{
    private EnemyData _enemyData;

    [SerializeField] private float _minDistanceToShoot = 5f;
    [SerializeField] private Animator _animator;

    [SerializeField] private Transform _player;
    [SerializeField] private Transform _targetToRotate;
    [SerializeField] private Transform _playerTargetAtShoot;

    [SerializeField] private GameObject _arrow;
    [SerializeField] private Arrow _arrowScript;
    [SerializeField] private Transform _sight;

    private const bool _isPlayer = false;

    [Header("Shoot Properties")]

    //[SerializeField] private float _arrowSpeed = 4f;
    //[SerializeField] private int _damage = 2;

    [SerializeField] private int _life = 5;

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
    [SerializeField]bool isRed = false;
    float _bloodSpeed = 1f;
    float t = 0f;

    [SerializeField] private Material _fadeMat;
    float fadeT = 0f;

    public int Life
    {
        get
        {
            if(_blood != null && _bloodPosition != null) ShowBloodParticles();

            
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

            //if (_life <= 0) Die();
            if (_life <= 0)
            {
                //StartCoroutine(Disolve());
                _disolveOn = true;
                Die();
            }
        }
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
            if(t >= 1)
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
    protected override void Die()
    {
        Destroy(gameObject, 2f); //VERIFICAR QUE SE DESTRUYA DESPUES DE QUE SE APLIQUE EL DISOLVE
    }

    private void Awake()
    {
        _enemyData = new EnemyData(1, 0f, _player, 12f);

        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        StartCoroutine(FadeAtSpawn());
    }
    private void Start()
    {
        _renderer.material = _fadeMat;

        _fadeMat.SetFloat("_t", 0);

        _disolveValue = -2f;

        _fire.Stop();

        _blood.transform.position = _bloodPosition.position;

        
    }

    private void ShowBloodParticles()
    {
        _blood.transform.position = _bloodPosition.position;
        _blood.Play();
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

        //========================================================================

        float distance = Vector3.Distance(_player.position, transform.position);

        Vector3 distanceVector = _targetToRotate.position - transform.position;
        distanceVector.y = 0f;
        Quaternion rotationToPlayer = Quaternion.LookRotation(distanceVector);

        LookPlayer(rotationToPlayer);

        _sight.LookAt(_playerTargetAtShoot.position);

        if (distance <= _minDistanceToShoot)
        {
            _animator.SetTrigger("Shoot");
            _animator.SetBool("Idle", false);
        }
        else
        {
            _animator.ResetTrigger("Shoot");
            _animator.SetBool("Idle", true);
        }
    }
    protected override void LookPlayer(Quaternion rotation)
    {
        transform.rotation = rotation;
    }

    //public void Shoot()
    //{
    //    _arrowScript.SetProperties(_arrowSpeed, _damage);
    //    _arrowScript.isPlayer = _isPlayer;
    //    Instantiate(_arrow, _sight.position, _sight.rotation);
    //}

    public override void Attack()
    {
        //_arrowScript.SetProperties(_arrowSpeed, _damage);
        _arrowScript.SetProperties(_enemyData.arrowSpeed, _enemyData.damage);
        _arrowScript.isPlayer = _isPlayer;
        Instantiate(_arrow, _sight.position, _sight.rotation);
    }

    public override void Burn()
    {
        if (_canBurn && Life > 0) StartCoroutine(BurnCorrutine());
        //_fire.transform.position = transform.position;
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
