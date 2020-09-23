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

    //private variables
    [SerializeField, Tooltip("Speed in which the Character moves.")] private float m_movementSpeed = 5.0f;
    private float m_currentSpeed = 0f;
    private float m_speedSmoothVelocity = 0f;
    private float m_speedSmoothTime = 0.1f;
    private float m_rotationSpeed = 0.1f;


    //public variables
    public int m_maxHealth = 100;
    public int m_currentHealth = 80;

    //TEST
    bool isSprinting = false;

    private void Awake()
    {
        m_inputs = new PlayerInputs();
        m_controller = GetComponent<CharacterController>();
        m_anim = GetComponent<Animator>();
        m_mainCameraTransform = Camera.main.transform;

        #region Input Action
        m_inputs.Player.Dodge.performed += _ => Dodge();
        m_inputs.Player.LightAttack.performed += _ => LightAttack();
        m_inputs.Player.HeavyAttack.performed += _ => HeavyAttack();
        m_inputs.Player.TargetSystem.performed += _ => TargetSystem();
        m_inputs.Player.Sprint.performed += _ => Sprint();
        m_inputs.Player.Use.performed += _ => Use();
        m_inputs.Player.Interaction.performed += _ => Interaction();
        m_inputs.Player.SwitchItems.performed += _ => SwitchItems(m_inputs.Player.SwitchItems.ReadValue<float>());
        m_inputs.Player.OpenInventory.performed += _ => OpenInventory();
        m_inputs.Player.OpenMenu.performed += _ => OpenMenu();
        #endregion
    }

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        Debug.Log(Application.persistentDataPath);
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
            if (isSprinting)
                m_currentSpeed = 13;
            else
                m_currentSpeed = Mathf.SmoothDamp(m_currentSpeed, TargetSpeed, ref m_speedSmoothVelocity, m_speedSmoothTime);

            m_controller.Move(desiredMoveDirection * m_currentSpeed * Time.deltaTime);

        }
        //m_anim.SetFloat("MovementSpeed", _context.magnitude);
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

    public void Dodge()
    {
        Debug.Log("Dodge");
    }

    public void LightAttack()
    {
        Debug.Log("Light Attack");
    }

    public void HeavyAttack()
    {
        Debug.Log("Heavy Attack");
    }

    public void TargetSystem()
    {
        Debug.Log("Target System");
    }

    public void Sprint()
    {
        Debug.Log("Sprint");

        isSprinting = !isSprinting;
    }

    public void Use()
    {
        Debug.Log("Use");
    }

    public void Interaction()
    {
        Debug.Log("Interaction");
    }

    public void SwitchItems(float _context)
    {
        Debug.Log("Switch Items");
    }

    public void OpenInventory()
    {
        Debug.Log("Open Inventory");
    }

    public void OpenMenu()
    {
        Debug.Log("Open Menu");
    }

    private void OnEnable()
    {
        m_inputs.Enable();
    }

    private void OnDisable()
    {
        m_inputs.Disable();
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 100), "Damage"))
        {
            m_currentHealth -= 10;
        }

        if (GUI.Button(new Rect(10, 150, 150, 100), "Save" ))
        {
            DataManager.Instance.SavePlayer(this);
            Debug.Log("Saved");
        }

        if (GUI.Button(new Rect(10, 300, 150, 100), "Load"))
        {
            PlayerData temp = DataManager.Instance.LoadPlayer();

            m_currentHealth = temp.m_CurrentHealth;
            m_maxHealth = temp.m_MaxHealth;

            Vector3 temppos = new Vector3(temp.m_Position[0], temp.m_Position[1], temp.m_Position[2]);
            m_controller.enabled = false;
            transform.position = temppos;
            m_controller.enabled = true;

            Debug.Log(temppos);
        }

    }
}
