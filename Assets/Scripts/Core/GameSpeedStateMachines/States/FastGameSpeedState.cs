using UnityEngine;

namespace Core.GameSpeedStateMachines.States
{
    public class FastGameSpeedState : IGameSpeedState
    {
    private readonly float _gameSpeed;

    public FastGameSpeedState(float gameSpeed)
    {
        _gameSpeed = gameSpeed;
    }

    public void Enter()
    {
        Time.timeScale = _gameSpeed;
    }
    }
}