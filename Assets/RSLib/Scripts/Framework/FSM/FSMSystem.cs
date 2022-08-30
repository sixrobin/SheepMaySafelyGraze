﻿namespace RSLib.Framework.FSM
{
    public class FSMSystem
    {
        private System.Collections.Generic.List<FSMState> _states = new System.Collections.Generic.List<FSMState>();

        public FSMStateId CurrentStateID { get; private set; }
        public FSMState CurrentState { get; private set; }

        public FSMSystem()
        {
        }

        public FSMSystem(params FSMState[] states)
        {
            for (int i = 0; i < states.Length; ++i)
                AddState(states[i]);
        }

        public void AddState(FSMState state)
        {
            UnityEngine.Assertions.Assert.IsNotNull(state, $"Null {typeof(FSMStateId)} is not allowed.");

            if (_states.Count == 0)
            {
                _states.Add(state);
                CurrentState = state;
                CurrentStateID = state.Id;
                return;
            }

            for (int i = _states.Count - 1; i >= 0; --i)
                UnityEngine.Assertions.Assert.IsTrue(state.Id != _states[i].Id, $"{typeof(FSMStateId)} {state.Id} has already been added.");

            _states.Add(state);
        }

        public void DeleteState(FSMStateId id)
        {
            UnityEngine.Assertions.Assert.IsTrue(id != FSMStateId.NONE, $"{id} {typeof(FSMStateId)} is not allowed.");

            for (int i = _states.Count - 1; i >= 0; --i)
            {
                if (_states[i].Id == id)
                {
                    _states.Remove(_states[i]);
                    return;
                }
            }

            throw new System.Exception($"Could not found {typeof(FSMStateId)} {id} to delete it.");
        }

        public void PerformTransition(FSMTransition transition)
        {
            UnityEngine.Assertions.Assert.IsTrue(transition != FSMTransition.NONE, $"{transition} {typeof(FSMTransition)} is not allowed.");

            FSMStateId id = CurrentState.GetTransitionOutputState(transition);
            UnityEngine.Assertions.Assert.IsTrue(id != FSMStateId.NONE, $"{transition} to {typeof(FSMStateId)} {nameof(FSMStateId.NONE)} is not allowed to perform a state transition.");

            for (int i = _states.Count - 1; i >= 0; --i)
            {
                if (_states[i].Id == id)
                {
                    CurrentStateID = id;
                    CurrentState.OnStateExit();
                    CurrentState = _states[i];
                    CurrentState.OnStateEntered();
                    return;
                }
            }

            throw new System.Exception($"Could not found {typeof(FSMStateId)} {id} to perform a state transition.");
        }
    }
}