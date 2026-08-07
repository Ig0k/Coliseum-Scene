using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    private float vRotation = 0f;

    [SerializeField] private Player _player;

    private void Update()
    {
        if(!_player.isDead)
        {
            float mouseY = Input.GetAxis("Mouse Y");

            vRotation -= mouseY;
            vRotation = Mathf.Clamp(vRotation, -90, 90);

            transform.localEulerAngles =
                new Vector3(vRotation, transform.localEulerAngles.y, 0f);
        }
        else
        {
            StartCoroutine(LookToSky());

            
        }
    }

    private IEnumerator LookToSky()
    {
        float t = 0;
        float duration = 0.5f;

        while(t < duration)
        {
            t += Time.deltaTime;

            
            vRotation -= Time.deltaTime;
            vRotation = Mathf.Clamp(vRotation, -90, 90);

            transform.localEulerAngles =
                new Vector3(vRotation, transform.localEulerAngles.y, 0f);

        }
        yield return null;  
        
    }
}
