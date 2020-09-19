using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInputs m_inputs;

    [SerializeField]
    Rigidbody m_rigidbody;
    [SerializeField]
    CharacterController m_controller;
    [SerializeField]
    Animator m_anim;

    [SerializeField]
    Transform m_groundCheck;
    [SerializeField]
    LayerMask m_groundMask;
    [SerializeField]
    bool m_isGrounded;

    [SerializeField]
    float m_groundDistance = 0.4f;
    [SerializeField]
    float m_movementSpeed = 5.0f;
    [SerializeField]
    float m_jumpHeight = 2.0f;
    [SerializeField]
    float m_gravity = -20f;

    private Vector3 m_velocity;

    private void Awake()
    {
        m_inputs = new PlayerInputs();
        //m_inputs.Player.Movement.started += _context => Movement(_context.ReadValue<Vector2>());
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        Movement(m_inputs.Player.Movement.ReadValue<Vector2>());
        //Jump(m_inputs.Player.Jump.performed);//Jump();
        Gravity();
    }

    public void Movement(Vector2 _direction)
    {
        Debug.Log("Movement " + _direction);
        m_isGrounded = Physics.CheckSphere(m_groundCheck.position, m_groundDistance, m_groundMask);

        if (m_isGrounded && m_velocity.y < 0.0f)
        {
            m_velocity.y = -2.0f;
        }
        //m_controller.Move(new Vector3(_direction.x, 0, _direction.y) * m_movementSpeed * Time.deltaTime);

        m_controller.Move(transform.forward * _direction.y * m_movementSpeed * Time.deltaTime);
        transform.Rotate(transform.up * _direction.x * 125 * Time.deltaTime);
    }

    //private void Jump()
    //{
    //    if (m_isGrounded)
    //    {
    //        m_velocity.y = Mathf.Sqrt(m_jumpHeight * -2.0f * m_gravity);
    //    }
    //}

    private void Gravity()
    {
        m_velocity.y += m_gravity * Time.deltaTime;

        m_controller.Move(m_velocity * Time.deltaTime);
    }

    private void OnEnable()
    {
        m_inputs.Enable();
    }

    private void OnDisable()
    {
        m_inputs.Disable();
    }
}
