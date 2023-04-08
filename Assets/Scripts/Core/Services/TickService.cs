using System;
using System.Collections;
using System.Collections.Generic;
using Core.GameSpeedStateMachines;
using Core.GameSpeedStateMachines.States;
using Cysharp.Threading.Tasks;
using Infrastructure;
using Settings;
using UnityEngine;
using Zenject;

namespace Core.Services
{
    public class TickService
    {
        public event Action Tick;
        
        private Coroutine _tickCoroutine;
        
        private readonly GameSpeedStateMachine _gameSpeedStateMachine;
        private readonly GameSpeedSettings _gameSpeedSettings;
        private const int TickInterval = 1;

        [Inject]
        public TickService(CoreSettings coreSettings)
        {
            _gameSpeedSettings = coreSettings.GameSpeedSettings;

            var gameSpeedStates = new Dictionary<Type, IGameSpeedState>()
            {
                { typeof(PauseGameSpeedState), new PauseGameSpeedState(_gameSpeedSettings.PauseTimeScale) },
                { typeof(NormalGameSpeedState), new NormalGameSpeedState(_gameSpeedSettings.NormalTimeScale) },
                { typeof(FastGameSpeedState), new FastGameSpeedState(_gameSpeedSettings.FastTimeScale) },
                { typeof(FastestGameSpeedState), new FastestGameSpeedState(_gameSpeedSettings.FastestTimeScale) }
            };
            _gameSpeedStateMachine = new GameSpeedStateMachine(gameSpeedStates[typeof(PauseGameSpeedState)], gameSpeedStates);
            
            StartTicking().Forget();
        }
        
        public void ChangeGameSpeedState<T>() where T : IGameSpeedState
        {
            _gameSpeedStateMachine.SwitchState<T>();
        }

        private async UniTaskVoid StartTicking()
        {
            while (true)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(TickInterval));
                Tick?.Invoke();
            }
        }
    }
}