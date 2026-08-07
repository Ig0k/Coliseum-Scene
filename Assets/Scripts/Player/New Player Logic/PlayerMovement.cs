using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    //[SerializeField] private float _walkSpeed = 5f;
    //[SerializeField] private float _rotationSpeed = 2f;

    private MovementData _movementData = new MovementData(5f, 2f);

    [SerializeField] private Animator _animator;

    private float _ogSpeed;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        //if (Cursor.lockState != CursorLockMode.Locked) Debug.Log("Cursor is NOT locked");
        //if (Cursor.visible == true) Debug.Log("Cursor is Visible");

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.None;
    }

    private void Start()
    {
        //_ogSpeed = _walkSpeed;
        _ogSpeed = _movementData.walkSpeed;
    }

    public IEnumerator SpeedPowerUp(float newSpeed, float powerUpDuration)
    {
        //_walkSpeed = newSpeed;
        _movementData.walkSpeed = newSpeed;
        yield return new WaitForSeconds(powerUpDuration);
        //_walkSpeed = _ogSpeed;
        _movementData.walkSpeed = _ogSpeed;
    }

    private void Update()
    {      
        float mouseX = Input.GetAxis("Mouse X");
        //transform.Rotate((transform.up * mouseX) * _rotationSpeed);      
        transform.Rotate((transform.up * mouseX) * _movementData.rotationSpeed);
    }

    private void FixedUpdate()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        Vector3 move = (transform.forward * ver + transform.right * hor).normalized;
        //_rb.velocity = move * _walkSpeed;
        _rb.velocity = move * _movementData.walkSpeed;

        _animator.SetFloat("X", hor);
        _animator.SetFloat("Z", ver);

        if (hor != 0 || ver != 0)
        {
            _animator.SetBool("Run", true);
            
        }
        else if (hor == 0 && ver == 0)
        {
            _animator.SetBool("Run", false);
        }

    }
}
