using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using static Weapon;

public enum AttackState
{
    IDLE,
    ATTACK,
    SHOOT
}

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private int _life;
    private int _maxLife;

    [SerializeField] private Animator _animator; //RedDamage
    [SerializeField] private Animator _greenScreenAnim;
 
    [SerializeField] private PostProcessVolume _postProcessVolume;
    private ColorGrading _colorGrading;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _heartBeatClip;
    [SerializeField] private AudioClip _healthClip;

    [SerializeField] private TMP_Text _lifeText;

    WeaponManager[] _weaponManager = new WeaponManager[2];

    [SerializeField] private newSword _sword;
    [SerializeField] private newCrossbow _crossBow;

    [SerializeField] private string _mouseButtonSword, _mouseButtonCrossbow;
    [SerializeField] private KeyCode _keyCodeSword, _keyCodeCrossbow;

    [SerializeField] private float _arrowSpeed;
    [SerializeField] private int _arrowDamage;

    [SerializeField] private LayerMask _interactLayer;
    private RaycastHit _interactHit;
    private Ray _ray;

    public AttackState state;
    [SerializeField] private Animator _swordAnimator, _crossbowAnimator;

    [SerializeField]
    private Material _damageMat;
    private bool _canShowDamagePostProcess = true;

    [SerializeField] private DieFeedback _dieFeedbackScript;

    public bool isDead = false;

    [SerializeField] private Material _dyingMat;

    private bool _canShowGreenScreen = true;

    [SerializeField] private Material _greenScreenMat;

    private void Awake()
    {
        EventManager.ResetEventDictionary();

        _weaponManager[0] = new WeaponManager(_sword);
        _weaponManager[1] = new WeaponManager(_crossBow);

        SetInputs();
        _weaponManager[1].SetArrowProperties(_arrowSpeed, _arrowDamage);

        if(_audioSource == null) _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _maxLife = _life;

        if(_postProcessVolume.profile.TryGetSettings(out ColorGrading colorGrading))
        {
            _colorGrading = colorGrading;
        }

        _damageMat.SetFloat("_AlphaValue", 0f);
        _dyingMat.SetFloat("_SaturationValue", 1);
        _greenScreenMat.SetFloat("_t", 1f);

        EventManager.Subscribe(EventType.OnPlayerDamaged, PlayerDamaged);
    }

    private void OnDestroy()
    {
        EventManager.UnSubscribe(EventType.OnPlayerDamaged, PlayerDamaged);
    }

    private IEnumerator DamagePostProcess()
    {
        _canShowDamagePostProcess = false;

        float duration = 0.2f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(t / duration);
            if(_life > 8)
                _damageMat.SetFloat("_AlphaValue", Mathf.Lerp(0f, 0.1f, normalizedTime));
            else if(_life <= 8 && _life > 4)
                _damageMat.SetFloat("_AlphaValue", Mathf.Lerp(0f, 0.3f, normalizedTime));
            else if(_life <= 4 && _life > 2)
                _damageMat.SetFloat("_AlphaValue", Mathf.Lerp(0f, 0.5f, normalizedTime));
            else if(_life <= 2)
            {
                _damageMat.SetFloat("_AlphaValue", Mathf.Lerp(0f, 0.8f, normalizedTime));
            }
                
            yield return null;
        }

        t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(t / duration);

            if (_life <= 3)
            {
                _dyingMat.SetFloat("_SaturationValue", Mathf.Lerp(1, 0.4f, normalizedTime));
            }
            //else
            //{
            //    _dyingMat.SetFloat("_SaturationValue", Mathf.Lerp(0.4f, 1f, normalizedTime));
            //}
        }     

        yield return new WaitForSeconds(0.1f);

        t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(t / duration);
            if (_life > 8)
                _damageMat.SetFloat("_AlphaValue", Mathf.Lerp(0.1f, 0f, normalizedTime));
            else if (_life <= 8 && _life > 4)
                _damageMat.SetFloat("_AlphaValue", Mathf.Lerp(0.3f, 0f, normalizedTime));
            else if (_life <= 4 && _life > 2)
                _damageMat.SetFloat("_AlphaValue", Mathf.Lerp(0.5f, 0f, normalizedTime));
            else if (_life <= 2)
                _damageMat.SetFloat("_AlphaValue", Mathf.Lerp(0.8f, 0f, normalizedTime));
            yield return null;
        }

        _canShowDamagePostProcess = true;
    }

    public int Life
    {
        get
        {
            //ShowRedPanel();
            if(_canShowDamagePostProcess)StartCoroutine(DamagePostProcess());

            Debug.Log("2");
            return _life;
        }

        set
        {
            _life = value;
            _life = Mathf.Clamp(_life, 0, 10);

            //if (Life <= 0) Die();
            if (Life <= 0)
            {
                isDead = true;

                _dieFeedbackScript.CallDieEffects();

                Camera.main.gameObject.transform.parent = null;

                gameObject.SetActive(false);

                  
            }
            _lifeText.text = _life.ToString();

            if (_life <= 3)
            {               
                EventManager.Trigger(EventType.OnPlayerDamaged);
            }
            if(_life > 3)
            {
                ResetEffects();
            }
        }
    }

    //private void Die()
    //{
    //    SceneManager.LoadScene("Nivel Parcial 2");
    //}

    

    private void PlayerDamaged(params object[] parameter)
    {
        //AUDIO

        if(_audioSource != null)
        {
            _audioSource.loop = true;
            _audioSource.clip = _heartBeatClip;
            _audioSource.Play();
        }
        
        //====================================
        //POST PROCESS

        if(_colorGrading != null)
        {
            _colorGrading.saturation.value = -100;
        }
        
    }

    public void PlayerHealth(params object[] parameter)
    {       
        _life += (int)parameter[0];

        _life = Mathf.Clamp(_life, 0, 10);

        _lifeText.text = _life.ToString();

        //_greenScreenAnim.SetTrigger("GreenScreen");
        if(_canShowGreenScreen) StartCoroutine(GreenScreenShader());

        if(_audioSource != null)
        {
            _audioSource.loop = false;
            _audioSource.clip = _healthClip;
            _audioSource.Play(); 
        }

        _dyingMat.SetFloat("_SaturationValue", 1);

        ResetEffects();
    }

    private IEnumerator GreenScreenShader()
    {
        _canShowGreenScreen = false;

        float duration = 0.7f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(t / duration);
            _greenScreenMat.SetFloat("_t", Mathf.Lerp(1f, 0.45f, normalizedTime));
            yield return null;
        }


        yield return new WaitForSeconds(0.25f);

        t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(t / duration);
            _greenScreenMat.SetFloat("_t", Mathf.Lerp(0.45f, 1f, normalizedTime));
            yield return null;
        }

        _canShowGreenScreen = true;
    }

    private void ResetEffects()
    {
        if (_audioSource != null && _audioSource.loop)
        {
            _audioSource.Stop();
            _audioSource.loop = true;
        }

        if (_colorGrading != null)
        {
            _colorGrading.saturation.value = 0; 
        }
    }

    private void SetInputs()
    {
        _weaponManager[0].ButtonToAttack(_mouseButtonSword);
        _weaponManager[1].ButtonToAttack(_mouseButtonCrossbow);

        _weaponManager[0].KeysToAttack(_keyCodeSword);
        _weaponManager[1].KeysToAttack(_keyCodeCrossbow);
    }

    private void Update()
    {
        if (_crossbowAnimator.GetBool("Shoot") == true) state = AttackState.SHOOT;
        else if (_swordAnimator.GetBool("Attack") == true) state = AttackState.ATTACK;
        else state = AttackState.IDLE;

        if (Input.GetButtonDown(_weaponManager[0]._buttonToAttack) ||
            Input.GetKeyDown(_weaponManager[0]._keyToAttack))
        {
            _weaponManager[0].Attack();
        }
        else if (Input.GetButtonDown(_weaponManager[1]._buttonToAttack) ||
            Input.GetKeyDown(_weaponManager[1]._keyToAttack))
        {
            _weaponManager[1].Attack();
        }
        

        #region Interact Raycast
        Vector3 rayPos = new Vector3(transform.position.x, transform.position.y + 1.7f, transform.position.z);
        _ray = new Ray(rayPos, Vector3.down);
        Debug.DrawRay(_ray.origin, _ray.direction);
      
        if (Physics.Raycast(_ray, out _interactHit, 10f))
        {
            if(_interactHit.transform.gameObject.TryGetComponent
                <IInteractuable>(out IInteractuable interactuable))
            {
                interactuable.Interact();
            }
        }
        #endregion 
    }

    public void ShowRedPanel()
    {
        _animator.SetTrigger("RedScreen");

    }

}

