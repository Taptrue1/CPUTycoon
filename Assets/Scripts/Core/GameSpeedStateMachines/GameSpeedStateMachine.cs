using System;
using System.Collections.Generic;
using Core.GameSpeedStateMachines.States;

namespace Core.Games.GameSpeedStateMachines
{
    public class GameSpeedStateMachine
    {
        private IGameSpeedState _currentState;
        private IGameSpeedState _lastState;
        private readonly Dictionary<Type, IGameSpeedState> _allStates;

        public GameSpeedStateMachine(IGameSpeedState initialState, Dictionary<Type, IGameSpeedState> allStates)
        {
            _lastState = initialState;
            _currentState = initialState;
            _allStates = allStates;
            
            _currentState.Enter();
        }
        
        public void SwitchState<T>() where T : IGameSpeedState
        {
            _lastState = _currentState;
            _currentState = _allStates[typeof(T)];
            _currentState.Enter();
        }
        public void SwitchLastState()
        {
            _currentState = _lastState;
            _currentState.Enter();
        }
    }
}