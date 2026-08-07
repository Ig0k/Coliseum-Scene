using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FirePowerUp : MonoBehaviour
{
    [SerializeField] private float _beastDuration = 0.01f, _beastCD = 0f;
    [SerializeField] private bool _onCD = false, _startCD = false, _canBeBeast = true;

    [SerializeField] private float _timeBetween = 5.5f;

    [SerializeField] private GameObject _text;

    public delegate void Beast();
    Beast beast = delegate { };

    //==============================

    [SerializeField]
    private Material _beastMat;

    float t;

    private Coroutine _shaderCoroutine;

    private void OnDisable()
    {
        _beastMat.SetFloat("_EffectIntensity", 1);

        Debug.Log("RR");
    }

    private void OnEnable()
    {
        _beastMat.SetFloat("_EffectIntensity", 1);

        Debug.Log("qq");
    }

    private void Update()
    {
        if (_startCD) Timer();
        else _startCD = false;

        beast();

        if (_canBeBeast && Input.GetKeyDown(KeyCode.Q) && _onCD)
        {
            beast = BeastMethod;
            _startCD = true;
            _beastCD = 0f;

            StartCoroutine(StartBeastMode());

            //=================================

            if(_shaderCoroutine != null) StopCoroutine(_shaderCoroutine);
            _shaderCoroutine = StartCoroutine(Shader());
        }
        else if (!_onCD)
        {
            beast = delegate { };
            _onCD = true;

            _startCD = true;

            //=================================

            //StartCoroutine(Shader(false));
        }

    }

    //private IEnumerator ShaderOff()
    //{
    //    float time = 0f;

    //    while (time < 1)
    //    {
    //        time += Time.deltaTime / 3;
    //        float d = t / 1;

    //        Debug.Log("tttttt");

    //        if (time <= 1) _beastMat.SetFloat("_EffectIntensity", Mathf.Lerp(0, 1, time));
    //        else _beastMat.SetFloat("_EffectIntensity", 1);
    //        yield return null;
    //    }
    //}

    private IEnumerator Shader()
    {
        float duration = 1.2f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(t / duration);
            
            _beastMat.SetFloat("_EffectIntensity", Mathf.Lerp(1f, 0f, normalizedTime));
            yield return null;
        }


        yield return new WaitForSeconds(4f);

        t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(t / duration);
            _beastMat.SetFloat("_EffectIntensity", Mathf.Lerp(0f, 1f, normalizedTime));
            yield return null;
        }

    }

    private IEnumerator StartBeastMode()
    {
        _text.SetActive(false);
        _canBeBeast = false;

        yield return new WaitForSeconds(_timeBetween);

        _text.SetActive(true);
        _canBeBeast = true;
    }

    private void BeastMethod()
    {
        //Debug.Log("Beast Mode");
        
        EnemyList<BaseEnemy>.BurnEnemies();
    }

    //private void NormalMode()
    //{
    //    //Debug.Log("Normal Mode");
    //}

    private void Timer()
    {
        if (_onCD)
        {
            _beastCD += Time.deltaTime;

            if (_beastCD > _beastDuration)
            {
                _onCD = false;
                _beastCD = 0f;
            }
        }
    }

}
