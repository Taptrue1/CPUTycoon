using UnityEngine;

namespace Core.GameSpeedStateMachines.States
{
    public class PauseGameSpeedState : IGameSpeedState
    {
        private readonly float _gameSpeed;
        
        public PauseGameSpeedState(float gameSpeed)
        {
            _gameSpeed = gameSpeed;
        }
        
        public void Enter()
        {
            Time.timeScale = _gameSpeed;
        }
    }
}