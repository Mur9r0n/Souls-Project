using System.Collections.Generic;
using KI.Sheep;
using UnityEngine;

namespace KI
{
    public abstract class ABaseState
    {
        public delegate bool TransitionDelegate();

        public bool IsFinished { get; protected set; }

        public Dictionary<TransitionDelegate, ABaseState> Transitions
        {
            get { return new Dictionary<TransitionDelegate, ABaseState>(m_transitions); }
        }

        protected SheepController m_SheepController;
        protected BeeController m_BeeController;
        protected MushroomController m_MushroomController;

        private Dictionary<TransitionDelegate, ABaseState> m_transitions;

        public virtual void SheepInit(SheepController _sheepController,
            params KeyValuePair<TransitionDelegate, ABaseState>[] _transitions)
        {
            m_transitions = new Dictionary<TransitionDelegate, ABaseState>();
            foreach (var keyValuePair in _transitions)
            {
                m_transitions.Add(keyValuePair.Key, keyValuePair.Value);
            }

            m_SheepController = _sheepController;
        }

        public virtual void BeeInit(BeeController _beeController,
            params KeyValuePair<TransitionDelegate, ABaseState>[] _transitions)
        {
            m_transitions = new Dictionary<TransitionDelegate, ABaseState>();
            foreach (var keyValuePair in _transitions)
            {
                m_transitions.Add(keyValuePair.Key, keyValuePair.Value);
            }

            m_BeeController = _beeController;
        }

        public virtual void MushroomInit(MushroomController _mushroomController,
            params KeyValuePair<TransitionDelegate, ABaseState>[] _transitions)
        {
            m_transitions = new Dictionary<TransitionDelegate, ABaseState>();
            foreach (var keyValuePair in _transitions)
            {
                m_transitions.Add(keyValuePair.Key, keyValuePair.Value);
            }

            m_MushroomController = _mushroomController;
        }

        public virtual bool Enter()
        {
            Debug.Log($"Entered {this}!");
            IsFinished = false;
            return true;
        }

        public abstract void Update();

        public virtual void Exit()
        {
            Debug.Log($"Exit {this}!");
        }
    }
}