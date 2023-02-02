using UnityEngine;
using UnityEngine.UI;

namespace Core.Levels.GameStateMachines
{
    public class GameSpeedView : MonoBehaviour
    {
        [SerializeField] private Color32 _activeColor;
        [SerializeField] private Color32 _inactiveColor;
        
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _x1Button;
        [SerializeField] private Button _x2Button;
        [SerializeField] private Button _x4Button;

        public void Init(GameSpeedStateMachine gameSpeedStateMachine)
        {
            ActivateButton(_pauseButton);

            _pauseButton.onClick.AddListener(() =>
            {
                ActivateButton(_pauseButton);
                gameSpeedStateMachine.SetState(GameSpeedState.Pause);
            });
            _x1Button.onClick.AddListener(() =>
            {
                ActivateButton(_x1Button);
                gameSpeedStateMachine.SetState(GameSpeedState.X1);
            });
            _x2Button.onClick.AddListener(() =>
            {
                ActivateButton(_x2Button);
                gameSpeedStateMachine.SetState(GameSpeedState.X2);
            });
            _x4Button.onClick.AddListener(() =>
            {
                ActivateButton(_x4Button);
                gameSpeedStateMachine.SetState(GameSpeedState.X4);
            });
        }

        private void ActivateButton(Button button)
        {
            _pauseButton.image.color = _inactiveColor;
            _x1Button.image.color = _inactiveColor;
            _x2Button.image.color = _inactiveColor;
            _x4Button.image.color = _inactiveColor;
            
            button.image.color = _activeColor;
        }
    }
}