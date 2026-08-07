using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _eKeyText;
    [SerializeField] private bool _playerInRange;

    [SerializeField] private GameObject[] _texts;

    [SerializeField] private int i = 0;

    [SerializeField] private Waves _waves;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6) //layer player
        {
            _eKeyText.SetActive(true);

            _playerInRange = true;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6) //layer player
        {
            _eKeyText.SetActive(false);

            _playerInRange = false;
        }
    }

    private void Update()
    {

        if(Input.GetKeyDown(KeyCode.E) && _playerInRange && i < _texts.Length)
        {
            Time.timeScale = 0;
            CameraRotation cam = Camera.main.GetComponent<CameraRotation>();
            cam.enabled = false;
            PlayerMovement pMov = FindObjectOfType<PlayerMovement>();
            pMov.enabled = false;

            _eKeyText.SetActive(false);
            _texts[i].SetActive(true);
            if(i > 0) _texts[i - 1].SetActive(false);

            i++;
        }
        else if(i >= _texts.Length && Input.GetKeyDown(KeyCode.E))
        {
            _texts[_texts.Length - 1].SetActive(false);
            i = 0;

            //ACTIVA OLEADA 
            _waves.StartFirstWave();

            CameraRotation cam = Camera.main.GetComponent<CameraRotation>();     
            cam.enabled = true;
            PlayerMovement pMov = FindObjectOfType<PlayerMovement>();
            pMov.enabled = true;
            Time.timeScale = 1;
        }
    }

}
