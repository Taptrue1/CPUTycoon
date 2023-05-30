using System;
using System.Collections.Generic;
using Core.Games.GameSpeedStateMachines;
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
        public DateTime StartDate { get; private set; }
        public DateTime CurrentDate => _currentDate;
        
        private DateTime _currentDate;
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
            _currentDate = DateTime.Now; //TODO add date save/load and default date constant
            StartDate = _currentDate;
            //TODO add cancellation token
            StartTicking().Forget();
        }
        
        public void SetGameSpeedState<T>() where T : IGameSpeedState
        {
            _gameSpeedStateMachine.SwitchState<T>();
        }
        public void SetLastGameSpeedState()
        {
            _gameSpeedStateMachine.SwitchLastState();
        }

        private async UniTaskVoid StartTicking()
        {
            while (true)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(TickInterval));
                _currentDate = _currentDate.AddDays(TickInterval);
                DateTimeChanged?.Invoke(_currentDate);
                Tick?.Invoke();
            }
        }
    }
}