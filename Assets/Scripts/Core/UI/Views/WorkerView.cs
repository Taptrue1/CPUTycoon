using System;
using Core.Team;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI.Views
{
    [RequireComponent(typeof(Button))]
    public class WorkerView : MonoBehaviour
    {
        public event Action<Worker> Clicked;
        public Worker Worker => _worker;
        //TODO add worker info display
        private Worker _worker;
        private Button _button;
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnButtonClick);
        }

        public void Init(Worker worker)
        {
            _worker = worker;   
        }
        
        private void OnButtonClick()
        {
            Clicked?.Invoke(_worker);
        }
    }
}