using UnityEngine;

public class SheepAttackState : ABaseState
{
    public override bool Enter()
    {
        m_controller.m_Agent.SetDestination(GameManager.Instance.PlayerTransform.position);
        return base.Enter();
    }

    public override void Update()
    {
        Debug.Log(m_controller);
    }
}