using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController m_controller = null;
    private Animator m_anim = null;
    private PlayerInputs m_inputs = null;
    private Transform m_mainCameraTransform = null;

    [SerializeField, Tooltip("Speed in which the Character moves.")] private float m_movementSpeed = 5.0f;
    private float m_currentSpeed = 0f;
    private float m_speedSmoothVelocity = 0f;
    private float m_speedSmoothTime = 0.1f;
    private float m_rotationSpeed = 0.1f;

    private void Awake()
    {
        m_inputs = new PlayerInputs();
        m_controller = GetComponent<CharacterController>();
        m_anim = GetComponent<Animator>();
        m_mainCameraTransform = Camera.main.transform;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        Movement(m_inputs.Player.Movement.ReadValue<Vector2>());
        Gravity();
    }

    public void Movement(/*InputAction.CallbackContext*/Vector2 _context)
    {
        if (_context != Vector2.zero)
        {
            //Vector2 movementInput = new Vector2(_context.ReadValue<Vector2>().x, _context.ReadValue<Vector2>().y);
            Vector2 movementInput = new Vector2(_context.x, _context.y);

            Vector3 forward = m_mainCameraTransform.forward;
            Vector3 right = m_mainCameraTransform.right;
            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();

            Vector3 desiredMoveDirection = (forward * movementInput.y + right * movementInput.x).normalized;

            if (desiredMoveDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), m_rotationSpeed);
            }

            float TargetSpeed = m_movementSpeed * movementInput.magnitude;
            m_currentSpeed = Mathf.SmoothDamp(m_currentSpeed, TargetSpeed, ref m_speedSmoothVelocity, m_speedSmoothTime);

            m_controller.Move(desiredMoveDirection * m_currentSpeed * Time.deltaTime);
        }
    }

    private void Gravity()
    {
        Vector3 gravityVector = Vector3.zero;

        if (!m_controller.isGrounded)
        {
            gravityVector.y += Physics.gravity.y;
        }

        m_controller.Move(gravityVector * Time.deltaTime);
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
