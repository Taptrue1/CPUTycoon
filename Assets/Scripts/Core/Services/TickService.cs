using System;
using System.Collections.Generic;
using Core.GameSpeedStateMachines;
using Core.GameSpeedStateMachines.States;
using Cysharp.Threading.Tasks;
using Settings;
using UnityEngine;
using Zenject;

namespace Core.Services
{
    public class TickService
    {
        public event Action Tick;
        
        //TODO add cancellation token for StartTicking
        private Coroutine _tickCoroutine;
        
        private readonly GameSpeedStateMachine _gameSpeedStateMachine;
        private const int TickInterval = 1;

        [Inject]
        public TickService(CoreSettings coreSettings)
        {
            var gameSpeedStates = new Dictionary<Type, IGameSpeedState>()
            {
                { typeof(PauseGameSpeedState), new PauseGameSpeedState(coreSettings.GameSpeedSettings.PauseTimeScale) },
                { typeof(NormalGameSpeedState), new NormalGameSpeedState(coreSettings.GameSpeedSettings.NormalTimeScale) },
                { typeof(FastGameSpeedState), new FastGameSpeedState(coreSettings.GameSpeedSettings.FastTimeScale) },
                { typeof(FastestGameSpeedState), new FastestGameSpeedState(coreSettings.GameSpeedSettings.FastestTimeScale) }
            };
            
            _gameSpeedStateMachine = new(gameSpeedStates[typeof(PauseGameSpeedState)], gameSpeedStates);
            
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