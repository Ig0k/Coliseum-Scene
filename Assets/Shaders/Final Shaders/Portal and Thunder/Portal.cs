using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Material _mat;
    [SerializeField] private Material _beastMat;
    private float t = 0f;

    [SerializeField] private GameObject _particles;

    private void OnEnable()
    {
        StartCoroutine(SetA());
        _beastMat.SetInt("_IsPortal", 1);
    }

    private IEnumerator SetA()
    {
        while (_mat.GetFloat("_tAlpha") < 1f)
        {
            t += Time.deltaTime * 1.3f;
            float lerp = Mathf.Lerp(0f, 1f, t);
            float lerpBeast = Mathf.Lerp(1f, 0f, t);
            yield return null;

            _mat.SetFloat("_tAlpha", lerp);
            _beastMat.SetFloat("_EffectIntensity", lerpBeast);
            if (t >= 1)
            {
                t = 0f;
            }
        }

        yield return new WaitForSeconds(2f);

        while (_mat.GetFloat("_tAlpha") > 0f)
        {
            t += Time.deltaTime * 1.3f;
            float lerp = Mathf.Lerp(1f, 0f, t);
            float lerpBeast = Mathf.Lerp(0f, 1f, t);
            yield return null;

            _mat.SetFloat("_tAlpha", lerp);
            _beastMat.SetFloat("_EffectIntensity", lerpBeast);
            if (t >= 1)
            {
                t = 0f;
            }
        }
        yield return null;
        if(_mat.GetFloat("_tAlpha") <= 0f)
        {
            _particles.SetActive(false);
            _beastMat.SetFloat("_EffectIntensity", 1f);
            _beastMat.SetInt("_IsPortal", 0);
            gameObject.SetActive(false);
        }
    }

}
