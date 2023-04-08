using System;
using System.Collections;
using System.Collections.Generic;
using Core.GameSpeedStateMachines;
using Core.GameSpeedStateMachines.States;
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
        private readonly CoroutineRunner _coroutineRunner;
        private readonly WaitForSeconds _waitForTick = new(1f);

        [Inject]
        public TickService(CoreSettings coreSettings, CoroutineRunner coroutineRunner)
        {
            _gameSpeedSettings = coreSettings.GameSpeedSettings;
            _coroutineRunner = coroutineRunner;
            
            var gameSpeedStates = new Dictionary<Type, IGameSpeedState>()
            {
                { typeof(PauseGameSpeedState), new PauseGameSpeedState(_gameSpeedSettings.PauseTimeScale) },
                { typeof(NormalGameSpeedState), new NormalGameSpeedState(_gameSpeedSettings.NormalTimeScale) },
                { typeof(FastGameSpeedState), new FastGameSpeedState(_gameSpeedSettings.FastTimeScale) },
                { typeof(FastestGameSpeedState), new FastestGameSpeedState(_gameSpeedSettings.FastestTimeScale) }
            };
            _gameSpeedStateMachine = new GameSpeedStateMachine(gameSpeedStates[typeof(PauseGameSpeedState)], gameSpeedStates);

            _tickCoroutine = _coroutineRunner.StartCoroutine(StartTicking());
        }
        
        public void ChangeGameSpeedState<T>() where T : IGameSpeedState
        {
            _gameSpeedStateMachine.SwitchState<T>();
        }

        private IEnumerator StartTicking()
        {
            while (true)
            {
                yield return _waitForTick;
                Tick?.Invoke();
            }
        }
    }
}