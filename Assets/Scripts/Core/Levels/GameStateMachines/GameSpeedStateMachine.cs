using UnityEngine;

namespace Core.Levels.GameStateMachines
{
    public class GameSpeedStateMachine
    {
        private float _pauseTimeScale = 0;
        private float _x1TimeScale = 1;
        private float _x2TimeScale = 2;
        private float _x4TimeScale = 4;
        
        public void Init(float pauseTimeScale, float x1TimeScale, float x2TimeScale, float x4TimeScale)
        {
            _pauseTimeScale = pauseTimeScale;
            _x1TimeScale = x1TimeScale;
            _x2TimeScale = x2TimeScale;
            _x4TimeScale = x4TimeScale;
        }
        
        public void SetState(GameSpeedState state)
        {
            Time.timeScale = state switch
            {
                GameSpeedState.Pause => _pauseTimeScale,
                GameSpeedState.X1 => _x1TimeScale,
                GameSpeedState.X2 => _x2TimeScale,
                GameSpeedState.X4 => _x4TimeScale,
                _ => Time.timeScale
            };
        }
    }
    
    public enum GameSpeedState
    {
        Pause,
        X1,
        X2,
        X4
    }
}