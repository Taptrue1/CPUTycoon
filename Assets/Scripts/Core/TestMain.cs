using Core.CPU;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class TestMain : MonoBehaviour
    {
        [SerializeField] private ProcessorBuilder _processorBuilder;
        [SerializeField] private Button _openProcessorBuilderButton;

        private void Awake()
        {
            _processorBuilder.ProcessorCreated += OnProcessorCreated;

            _openProcessorBuilderButton.onClick.AddListener(OnOpenProcessorBuilderButtonClick);
        }

        private void OnOpenProcessorBuilderButtonClick()
        {
            _processorBuilder.Activate();
        }
        private void OnProcessorCreated(Processor processor)
        {
            Debug.Log($"Вы создали процессор с производительностью {processor.GetPowerScore()}");
        }
    }
}