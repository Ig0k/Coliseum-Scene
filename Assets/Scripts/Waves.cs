using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Waves : MonoBehaviour
{
    [SerializeField] GameObject[] _wave1, _wave2, _wave3;
    [SerializeField] GameObject[] _waveObject = new GameObject[3];

    [SerializeField] private bool _wave1Wined = false, _wave2Wined = false, _wave3Wined = false;

    [SerializeField] private int i1 = 0, i2 = 0, i3 = 0;

    [SerializeField] private GameObject _potions, _potions2;

    [Header("Portal")]
    [SerializeField] private GameObject _portalW1, _portalW2, _portalW3;

    public void StartFirstWave()
    {
        _portalW1.SetActive(true);
        _waveObject[0].SetActive(true);
        Debug.Log("Oleada empezada");

        if(!_potions.activeSelf)_potions.SetActive(true);
    }

    private void Update()
    {
        i1 = 0;

        for (int index = 0; index < _wave1.Length; index++)
        {
            if (_wave1[index] == null)
            {
                i1++;
            }
            
        }

        if (_wave1Wined)
        {
            _portalW1.SetActive(false);
            _portalW2.SetActive(true);
            _waveObject[1].SetActive(true);

            i2 = 0;

            for (int index = 0; index < _wave2.Length; index++)
            {
                if (_wave2[index] == null)
                {
                    i2++;
                }
            }
        }

        i3 = 0;

        if (_wave2Wined)
        {
            _potions2.SetActive(true);

            _portalW1.SetActive(false);
            _portalW2.SetActive(false);
            _portalW3.SetActive(true);
            _waveObject[2].SetActive(true);

            for (int index = 0; index < _wave3.Length; index++)
            {
                if (_wave3[index] == null)
                {
                    i3++;
                }

            }
        }
        

        if (i1 >= _wave1.Length) _wave1Wined = true;
        if (i2 >= _wave2.Length) _wave2Wined = true;
        if (i3 >= _wave3.Length) _wave3Wined = true;

        if(_wave1Wined && _wave2Wined && _wave3Wined)
        {
            SceneManager.LoadScene("End");
        }
    }
}
