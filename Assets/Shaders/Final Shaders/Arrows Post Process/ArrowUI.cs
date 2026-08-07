using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowUI : MonoBehaviour
{
    [SerializeField] private newCrossbow _crossBow;
    [SerializeField] private Material _mat;

    private bool _flag = true, _flag2 = true;
    private Coroutine _coroutine;

    private void Start()
    {
        _mat.SetInt("_ShowError", 0);
        _mat.SetInt("_ShowArrow", 0);
        _mat.SetInt("_ArrowOrError", 1);
    }
    private void Update()
    {
        
        if (Input.GetMouseButtonDown(1) && _flag && !_crossBow.hasArrowsLeft)
        {
            StartCoroutine(ShowError());
        }
    }

    public void CallShowArrow()
    {
        if(_flag2) _coroutine = StartCoroutine(ShowArrow());
    }

    private IEnumerator ShowArrow()
    {
        _flag2 = false;

        _mat.SetInt("_ShowError", 0);
        _mat.SetInt("_ArrowOrError", 1);

        yield return new WaitForSeconds(0.15f);

        _mat.SetInt("_ShowArrow", 0);
        yield return new WaitForSeconds(0.37f);
        _mat.SetInt("_ShowArrow", 1);
        yield return new WaitForSeconds(0.37f);
        _mat.SetInt("_ShowArrow", 0);
        yield return new WaitForSeconds(0.37f);
        _mat.SetInt("_ShowArrow", 1);
        yield return new WaitForSeconds(0.37f);
        _mat.SetInt("_ShowArrow", 0);
        yield return new WaitForSeconds(0.37f);
        _mat.SetInt("_ShowArrow", 1);
        yield return new WaitForSeconds(0.37f);
        _mat.SetInt("_ShowArrow", 0);
        yield return new WaitForSeconds(0.37f);

        _mat.SetInt("_ShowArrow", 0);

        _flag2 = true;
    }

    private IEnumerator ShowError()
    {
        _flag = false;

        yield return new WaitForSeconds(0.4f);

        _mat.SetInt("_ShowArrow", 0);
        _mat.SetInt("_ShowError", 1);

        _mat.SetInt("_ArrowOrError", 0);
        yield return new WaitForSeconds(0.27f);
        _mat.SetInt("_ArrowOrError", 1);
        yield return new WaitForSeconds(0.37f);
        _mat.SetInt("_ArrowOrError", 0);
        yield return new WaitForSeconds(0.27f);
        _mat.SetInt("_ArrowOrError", 1);
        yield return new WaitForSeconds(0.27f);
        _mat.SetInt("_ArrowOrError", 0);
        yield return new WaitForSeconds(0.27f);
        _mat.SetInt("_ArrowOrError", 1);

        _mat.SetInt("_ShowError", 0);

        _flag = true;
    }
}
