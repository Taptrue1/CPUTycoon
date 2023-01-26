using System;
using UnityEngine;

namespace Core.StateMachines
{
    public class GameStateMachine
    {
        private GameState _currentGameState;
        
        public void SetState(GameState gameState)
        {
            ExitCurrentState();
            
            switch (gameState)
            {
                case GameState.Pause:
                    EnterPauseState();
                    break;
                case GameState.X1:
                    EnterX1State();
                    break;
                case GameState.X2:
                    EnterX2State();
                    break;
                case GameState.X4:
                    EnterX4State();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gameState), gameState, null);
            }
            
            Time.timeScale = (float)_currentGameState;
        }
        
        private void EnterPauseState()
        {
            _currentGameState = GameState.Pause;
            Debug.Log("Enter Pause");   
        }
        private void ExitPauseState()
        {
            Debug.Log("Exit Pause");
        }
        private void EnterX1State()
        {
            _currentGameState = GameState.X1;
            Debug.Log("Enter X1");
        }
        private void ExitX1State()
        {
            Debug.Log("Exit X1");
        }
        private void EnterX2State()
        {
            _currentGameState = GameState.X2;
            Debug.Log("Enter X2");
        }
        private void ExitX2State()
        {
            Debug.Log("Exit X2");
        }
        private void EnterX4State()
        {
            _currentGameState = GameState.X4;
            Debug.Log("Enter X4");
        }
        private void ExitX4State()
        {
            Debug.Log("Exit X4");
        }
        
        private void ExitCurrentState()
        {
            switch (_currentGameState)
            {
                case GameState.Pause:
                    ExitPauseState();
                    break;
                case GameState.X1:
                    ExitX1State();
                    break;
                case GameState.X2:
                    ExitX2State();
                    break;
                case GameState.X4:
                    ExitX4State();
                    break;
            }
        }
    }
    
    public enum GameState
    {
        Pause = 0,
        X1 = 1,
        X2 = 2,
        X4 = 4
    }
}