using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DieFeedback : MonoBehaviour
{
    private bool _canShowEffects = true;

    [SerializeField] private Material _dyingMat;

    private void Start()
    {
        _dyingMat.SetFloat("_LifeIs0", 0);
        _dyingMat.SetFloat("_Blink", 1);
    }

    public void CallDieEffects()
    {
        if(_canShowEffects) StartCoroutine(DieEffects());
    }
    private IEnumerator DieEffects()
    {
        _canShowEffects = false;

        Debug.Log("Effectos de muerte 1");

        yield return new WaitForSeconds(1.3f);

        Debug.Log("Effectos de muerte 2");

        _dyingMat.SetFloat("_LifeIs0", 1);

        float duration = 1.2f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(t / duration);
            _dyingMat.SetFloat("_Blink", Mathf.Lerp(1f, 0f, normalizedTime));
            yield return null;
        }


        yield return new WaitForSeconds(0.2f);

        t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(t / duration);
            _dyingMat.SetFloat("_Blink", Mathf.Lerp(0f, 0.6f, normalizedTime));
            yield return null;
        }

        t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(t / duration);
            _dyingMat.SetFloat("_Blink", Mathf.Lerp(0.6f, 0f, normalizedTime));
            yield return null;
        }


        yield return new WaitForSeconds(0.5f);

        t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(t / duration);
            _dyingMat.SetFloat("_Blink", Mathf.Lerp(0f, 0.35f, normalizedTime));
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);

        t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(t / duration);
            _dyingMat.SetFloat("_Blink", Mathf.Lerp(0.35f, 0f, normalizedTime));
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        Die();

        _canShowEffects = true;
    }

    

    private void Die()
    {
        SceneManager.LoadScene("Nivel Parcial 2");
    }
}
