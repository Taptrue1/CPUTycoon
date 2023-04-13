using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI.Windows
{
    public class CoreWindow : WindowPresenter
    {
        [Header("Game Speed")]
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _normalSpeedButton;
        [SerializeField] private Button _fastSpeedButton;
        [SerializeField] private Button _fastestSpeedButton;
        
        [Header("Windows Open Buttons")]
        [SerializeField] private Button _openResearchWindowButton;
        [SerializeField] private Button _openDevelopmentWindowButton;

        //TODO change text to custom number view or something like that
        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI _moneyTextObject;
        [SerializeField] private TextMeshProUGUI _companyNameTextObject;
        
        private void Awake()
        {
            _pauseButton.onClick.AddListener(() => { });
            _normalSpeedButton.onClick.AddListener(() => { });
            _fastSpeedButton.onClick.AddListener(() => { });
            _fastestSpeedButton.onClick.AddListener(() => { });
            
            _openResearchWindowButton.onClick.AddListener(() => { });
            _openDevelopmentWindowButton.onClick.AddListener(() => { });
        }
    }
}