using System;
using System.Collections.Generic;
using Core.GameSpeedStateMachines;
using Core.GameSpeedStateMachines.States;
using Cysharp.Threading.Tasks;
using Settings;
using Zenject;

namespace Core.Services
{
    public class TimeService
    {
        public event Action Tick;
        public event Action<DateTime> DateTimeChanged;
        public DateTime CurrentDateTime => _currentDateTime;
        
        private DateTime _currentDateTime;
        private readonly GameSpeedStateMachine _gameSpeedStateMachine;
        private const int TickInterval = 1;

        [Inject]
        public TimeService(CoreSettings coreSettings)
        {
            var gameSpeedStates = new Dictionary<Type, IGameSpeedState>()
            {
                { typeof(PauseGameSpeedState), new PauseGameSpeedState(coreSettings.GameSpeedSettings.PauseTimeScale) },
                { typeof(NormalGameSpeedState), new NormalGameSpeedState(coreSettings.GameSpeedSettings.NormalTimeScale) },
                { typeof(FastGameSpeedState), new FastGameSpeedState(coreSettings.GameSpeedSettings.FastTimeScale) },
                { typeof(FastestGameSpeedState), new FastestGameSpeedState(coreSettings.GameSpeedSettings.FastestTimeScale) }
            };
            
            _gameSpeedStateMachine = new(gameSpeedStates[typeof(PauseGameSpeedState)], gameSpeedStates);
            _currentDateTime = DateTime.Now; //TODO add date save/load and default date constant
            
            //TODO add cancellation token
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
                _currentDateTime = _currentDateTime.AddDays(TickInterval);
                DateTimeChanged?.Invoke(_currentDateTime);
                Tick?.Invoke();
            }
        }
    }
}