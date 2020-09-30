using UnityEngine;

public class SheepWalkState : ABaseState
{
    public override void Update()
    {
        Debug.Log(m_controller);
        Debug.Log("Hai wir d Update!");
    }
}