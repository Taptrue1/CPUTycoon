using System;
using System.Collections;
using Core.CPU;
using Core.Custom;
using Core.Levels.GameStateMachines;
using UnityEngine;

namespace Core.Levels
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private GameSpeedView _gameSpeedView;
        [SerializeField] private ProcessorBuilderView _processorBuilderView;
        [SerializeField] private CustomDateTimeView _dateTimeView;

        private GameSpeedStateMachine _gameSpeedStateMachine;
        private ProcessorBuilder _processorBuilder;
        private CustomDateTime _dateTime;
        
        private void Awake()
        {
            _gameSpeedStateMachine = new();
            _processorBuilder = new(_processorBuilderView);
            _dateTime = new(new DateTime(1990, 1, 1));
            
            _gameSpeedStateMachine.SetState(GameSpeedState.Pause);
            
            _gameSpeedView.Init(_gameSpeedStateMachine);
            _dateTimeView.Init(_dateTime);

            _processorBuilder.ProcessorCreated += OnProcessorCreated;

            StartCoroutine(TickDay());
        }

        private IEnumerator TickDay()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);

                _dateTime.AddDays(1);
            }
        }
        private void OnProcessorCreated(Processor processor)
        {
            Debug.Log("Processor power score> " + processor.GetPowerScore());
        }
    }
}