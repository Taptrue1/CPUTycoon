using UnityEngine;

namespace Core.GameSpeedStateMachines.States
{
    public class FastestGameSpeedState : IGameSpeedState
    {
        private readonly float _gameSpeed;
        
        public FastestGameSpeedState(float gameSpeed)
        {
            _gameSpeed = gameSpeed;
        }
        
        public void Enter()
        {
            Time.timeScale = _gameSpeed;
        }
    }
}