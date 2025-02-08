using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Movement : MonoBehaviour
{
    public InputSystem_Actions inputSystem;
    public GameObject Player;

    [SerializeField]private float _MouseSensativityX; 
    [SerializeField] private float _MouseSensativityY;

    [SerializeField] private float _Speed;
    float _RotationX = 0.0f;
    Vector2 _LookToInput;
    Rigidbody rb;


    private Vector2 _PrevMousePos;
    void Start()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        _RotationX = 0;
        transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        //Rotation();

    }
    private void Update()
    {
        Rotation();
    }
    private void OnEnable()
    {
        rb = Player.GetComponent<Rigidbody>();

        inputSystem = new InputSystem_Actions();
        inputSystem.Enable();
        inputSystem.Player.Look.performed += OnRotate;
        inputSystem.Player.Look.canceled += OnStopRotate;
    }
    private void OnDisable()
    {
        inputSystem.Disable();
    }

    private void Move()
    {
        Vector2 move = inputSystem.Player.Move.ReadValue<Vector2>();
        Vector3 moveIn3D = new Vector3(_Speed * move.x, 0, _Speed * move.y);
        rb.velocity = moveIn3D;
    }
    public void OnRotate(InputAction.CallbackContext context)
    {
        _LookToInput = context.ReadValue<Vector2>();
    }
    public void OnStopRotate(InputAction.CallbackContext context)
    {
        _LookToInput = Vector2.zero;
    }
    private void Rotation()
    {
        float mouseX = _LookToInput.x * _MouseSensativityX * Time.deltaTime;
        float mouseY = _LookToInput.y * _MouseSensativityY * Time.deltaTime;

        _RotationX -= mouseY;
        _RotationX = Mathf.Clamp(_RotationX, -90f, 90f);

        transform.localRotation = Quaternion.Euler(_RotationX, 0, 0);
        Player.transform.Rotate(Vector3.up * mouseX);
    }
}
