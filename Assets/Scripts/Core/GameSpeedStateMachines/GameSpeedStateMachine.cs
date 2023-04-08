using System;
using System.Collections.Generic;
using Core.GameSpeedStateMachines.States;

namespace Core.GameSpeedStateMachines
{
    public class GameSpeedStateMachine
    {
        private IGameSpeedState _currentState;
        private readonly Dictionary<Type, IGameSpeedState> _allStates;

        public GameSpeedStateMachine(IGameSpeedState initialState, Dictionary<Type, IGameSpeedState> allStates)
        {
            _currentState = initialState;
            _allStates = allStates;
            
            _currentState.Enter();
        }
        
        public void SwitchState<T>() where T : IGameSpeedState
        {
            _currentState = _allStates[typeof(T)];
            _currentState.Enter();
        }
    }
}