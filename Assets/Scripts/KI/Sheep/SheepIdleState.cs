﻿namespace KI.Sheep
{
    public class SheepIdleState : ABaseState
    {
        public override void Update()
        {
            IsFinished = true;
        }
    }
}
