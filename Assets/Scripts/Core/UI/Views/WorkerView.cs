using System;
using Core.Team;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI.Views
{
    [RequireComponent(typeof(Button))]
    public class WorkerView : MonoBehaviour
    {
        public event Action<Worker> Clicked;
        public Worker Worker => _worker;
        
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _nameTextObject;
        [SerializeField] private TextMeshProUGUI _descriptionTextObject;
        
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
            _icon.sprite = worker.Icon;
            _nameTextObject.text = $"{worker.Name} {worker.Surname}";
            _descriptionTextObject.text =
                $"Age: {worker.Age} years\nSalary: ${worker.Salary} / month\nPoints generation: {worker.PointsGeneration} / day";
        }
        
        private void OnButtonClick()
        {
            Clicked?.Invoke(_worker);
        }
    }
}