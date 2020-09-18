using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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

    float m_horizontal;
    float m_vertical;
    Vector3 m_direction;

    private Vector3 m_velocity;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        Movement();
        Jump();
        Gravity();

        if(Input.GetMouseButton(0))
        {
            m_anim.SetTrigger("Attack");
        }
    }

    private void Movement()
    {
        m_isGrounded = Physics.CheckSphere(m_groundCheck.position, m_groundDistance, m_groundMask);

        if (m_isGrounded && m_velocity.y < 0.0f)
        {
            m_velocity.y = -2.0f;
        }

        m_horizontal = Input.GetAxis("Horizontal");
        m_vertical = Input.GetAxis("Vertical");

        //TODO:
        if(m_vertical > 0)
        {
            m_anim.SetFloat("Gei no fiir", 1);
        }
        else
        {
            m_anim.SetFloat("Gei no fiir", 0);
        }

        //MAYBE
        if(Input.GetMouseButton(1))
        {
            m_direction = (transform.right * m_horizontal + transform.forward * m_vertical).normalized;
            m_controller.Move(m_direction * m_movementSpeed * Time.deltaTime);
        }
        else
        {
            m_controller.Move(transform.forward * m_vertical * m_movementSpeed * Time.deltaTime);
            transform.Rotate(transform.up * m_horizontal * 125 * Time.deltaTime);
        }
    }

    private void Jump()
    {
        if (m_isGrounded && Input.GetButtonDown("Jump"))
        {
            m_anim.SetTrigger("Jump");
            m_velocity.y = Mathf.Sqrt(m_jumpHeight * -2.0f * m_gravity);
        }
    }

    private void Gravity()
    {
        m_velocity.y += m_gravity * Time.deltaTime;

        m_controller.Move(m_velocity * Time.deltaTime);
    }
}
