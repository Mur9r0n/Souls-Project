using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KI.Sheep
{
    public class SheepController : MonoBehaviour
    {
        public string m_CurrentState;
        public NavMeshAgent m_Agent;
        public Vector3? TargetPosition { get; set; }
        public Vector3 OriginPosition { get; set; }
        public Quaternion OriginRotation { get; set; }

        public float AttackDistance
        {
            get { return m_attackDistance; }
        }

        [SerializeField] private float m_attackDistance;
        [SerializeField] private float m_maxHealthPoints;
        [SerializeField] private float m_currentHealthPoints;
        private bool m_playerFound = false;

        private ABaseState m_activeState;
        private SheepIdleState m_idleState;

        private void Start()
        {
            m_Agent = GetComponent<NavMeshAgent>();
            OriginPosition = transform.position;
            OriginRotation = transform.rotation;

            m_idleState = new SheepIdleState();
            SheepAttackState m_attackState = new SheepAttackState();
            SheepResetState m_resetState = new SheepResetState();
            SheepWalkState m_walkState = new SheepWalkState();

            // m_idleState.SheepInit(this, new KeyValuePair<ABaseState.TransitionDelegate, ABaseState>
            // (
            //     () => 
            // ));
            //
            // m_attackState.SheepInit(this, new KeyValuePair<ABaseState.TransitionDelegate, ABaseState>
            // (
            //     () => 
            // ));
            //
            // m_resetState.SheepInit(this, new KeyValuePair<ABaseState.TransitionDelegate, ABaseState>
            // (
            //     () => 
            // ));
            //
            // m_walkState.SheepInit(this, new KeyValuePair<ABaseState.TransitionDelegate, ABaseState>
            // (
            //     () => 
            // ));
        }

        private void Update()
        {
            LookForPlayer();
            if (m_activeState is object)
            {
                m_activeState.Update();

                if (m_activeState.IsFinished)
                {
                    m_activeState.Exit();
                    m_activeState = m_idleState;
                    m_activeState.Enter();
                }

                foreach (var keyValuePair in m_activeState.Transitions)
                {
                    if (keyValuePair.Key())
                    {
                        m_activeState.Exit();
                        m_activeState = keyValuePair.Value;
                        if (!m_activeState.Enter())
                            Debug.Log($"Konnte {m_activeState} nicht betretet!");
                        break;
                        ;
                    }
                }
            }
        }

        public void LookForPlayer()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + new Vector3(0, 1, 1),
                transform.TransformDirection(Vector3.forward), out hit,
                Mathf.Infinity))
            {
                Debug.Log(("Hit Something!"));
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    Debug.DrawRay(transform.position + new Vector3(0, 1, 1),
                        transform.TransformDirection(Vector3.forward) * hit.distance, Color.green);
                    Debug.Log($"Hit Player {hit.collider.gameObject.name}!");
                }
                else
                {
                    Debug.DrawRay(transform.position + new Vector3(0, 1, 1),
                        transform.TransformDirection(Vector3.forward) * 1000, Color.blue);
                    Debug.Log($"Hit was {hit.collider.gameObject.name}!");
                }
            }
            else
            {
                Debug.DrawRay(transform.position + new Vector3(0, 1, 1),
                    transform.TransformDirection(Vector3.forward) * 1000, Color.blue);
                Debug.Log("Hit wasn't Player!");
            }
        }
    }
}