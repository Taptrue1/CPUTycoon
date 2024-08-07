using UnityEngine;

namespace Core.GameSpeedStateMachines.States
{
    public class NormalGameSpeedState : IGameSpeedState
    {
        private readonly float _gameSpeed;
        
        public NormalGameSpeedState(float gameSpeed)
        {
            _gameSpeed = gameSpeed;
        }
        
        public void Enter()
        {
            Time.timeScale = _gameSpeed;
        }
    }
}