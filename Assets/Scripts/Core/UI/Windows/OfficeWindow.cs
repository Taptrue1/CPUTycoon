using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Services;
using Core.Team;
using Core.UI.Views;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.UI.Windows
{
    public class OfficeWindow : WindowPresenter
    {
        [Header("Panels")]
        [SerializeField] private GameObject _scientistsTeamPanel;
        [SerializeField] private GameObject _programmersTeamPanel;
        [Header("Containers")]
        [SerializeField] private Transform _scientistsContainer;
        [SerializeField] private Transform _programmersContainer;
        [SerializeField] private Transform _freeScientistsContainer;
        [SerializeField] private Transform _freeProgrammersContainer;
        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI _officeInfoTextObject;
        [SerializeField] private TextMeshProUGUI _hiredScientistsCountTextObject;
        [SerializeField] private TextMeshProUGUI _hiredProgrammersCountTextObject;
        [Header("TextFormats")]
        [SerializeField] private string _hiredScientistsTitleTextFormat = "Hired Scientists: {0}/{1}";
        [SerializeField] private string _hiredProgrammersTitleTextFormat = "Hired Programmers: {0}/{1}";
        [SerializeField] private string _scientistsPlacesTextFormat = "Scientists places: {0}";
        [SerializeField] private string _programmersPlacesTextFormat = "Programmers places: {0}";
        [SerializeField] private string _rpProducingCountTextFormat = "RP / Day: {0}";
        [SerializeField] private string _dpProducingCountTextFormat = "DP / Day: {0}";
        [SerializeField] private string _totalSalaryTextFormat = "Total Salary: ${0}";
        [SerializeField] private string _officeLevelTextFormat = "Office Level {0}";
        [Header("Prefabs")]
        [SerializeField] private WorkerView _workerViewPrefab;
        [Header("Other")]
        [SerializeField] private Button _openScientistsTeamButton;
        [SerializeField] private Button _openProgrammersTeamButton;
        [SerializeField] private Button _upgradeOfficeButton;
        [SerializeField] private Button _exitButton;
        
        private UIService _uiService;
        private TeamService _teamService;
        
        private List<WorkerView> _freeScientistsViews;
        private List<WorkerView> _freeProgrammersViews;
        private List<WorkerView> _hiredScientistsViews;
        private List<WorkerView> _hiredProgrammersViews;
        private void Awake()
        {
            _openScientistsTeamButton.onClick.AddListener(OnOpenScientistsTeamButtonClick);
            _openProgrammersTeamButton.onClick.AddListener(OnOpenProgrammersTeamButtonClick);
            _upgradeOfficeButton.onClick.AddListener(OnUpgradeOfficeButtonClick);
            _exitButton.onClick.AddListener(OnExitButtonClick);
        }

        [Inject]
        public void InjectDependencies(UIService uiService, TeamService teamService)
        {
            _uiService = uiService;
            _teamService = teamService;
            _freeScientistsViews = new List<WorkerView>();
            _freeProgrammersViews = new List<WorkerView>();
            _hiredScientistsViews = new List<WorkerView>();
            _hiredProgrammersViews = new List<WorkerView>();
        }
        public override void Show()
        {
            base.Show();
            //TODO if free workers count changed
            ClearFreeWorkersViews();
            SpawnFreeWorkerViews();
            UpdateTeamInformation();
            UpdateOfficeInformation();
        }
        
        #region Callbacks
        
        private void OnOpenScientistsTeamButtonClick()
        {
            _scientistsTeamPanel.SetActive(true);
        }
        private void OnOpenProgrammersTeamButtonClick()
        {
            _programmersTeamPanel.SetActive(true);
        }
        private void OnUpgradeOfficeButtonClick()
        {
            if(!_teamService.CanUpgradeOffice()) return;
            _teamService.UpgradeOffice();
            //TODO update scientists and programmers count text objects
        }
        private void OnExitButtonClick()
        {
            _uiService.ShowWindow<CoreWindow>();
        }
        private void OnWorkerViewClicked(Worker worker)
        {
            var isHiredScientist = _teamService.HiredScientists.Contains(worker);
            var isHiredProgrammer = _teamService.HiredProgrammers.Contains(worker);
            var isFreeScientist = _teamService.FreeScientists.Contains(worker);
            var isFreeProgrammer = _teamService.FreeProgrammers.Contains(worker);
            var canHireScientist = _hiredScientistsViews.Count < _teamService.Office.ScientistsPlaces.Length;
            var canHireProgrammer = _hiredProgrammersViews.Count < _teamService.Office.ProgrammersPlaces.Length;

            if (isHiredScientist)
                FireScientist(worker);
            else if (isHiredProgrammer)
                FireProgrammer(worker);
            else if (isFreeScientist && canHireScientist)
                HireScientist(worker);
            else if (isFreeProgrammer && canHireProgrammer)
                HireProgrammer(worker);
            
            UpdateOfficeInformation();
            UpdateTeamInformation();
        }

        #endregion
        
        #region Other

        private void SpawnFreeWorkerViews()
        {
            _freeProgrammersViews ??= new List<WorkerView>();
            _freeScientistsViews ??= new List<WorkerView>();
            foreach (var worker in _teamService.FreeProgrammers)
            {
                var workerView = Instantiate(_workerViewPrefab, _freeProgrammersContainer);
                workerView.Init(worker);
                workerView.Clicked += OnWorkerViewClicked;
                _freeProgrammersViews.Add(workerView);
            }
            foreach (var worker in _teamService.FreeScientists)
            {
                var workerView = Instantiate(_workerViewPrefab, _freeScientistsContainer);
                workerView.Init(worker);
                workerView.Clicked += OnWorkerViewClicked;
                _freeScientistsViews.Add(workerView);
            }
        }
        private void ClearFreeWorkersViews()
        {
            foreach (var workerView in _freeProgrammersViews)
            {
                workerView.Clicked -= OnWorkerViewClicked;
                Destroy(workerView.gameObject);
            }
            foreach (var workerView in _freeScientistsViews)
            {
                workerView.Clicked -= OnWorkerViewClicked;
                Destroy(workerView.gameObject);
            }
            _freeProgrammersViews.Clear();
            _freeScientistsViews.Clear();
        }
        private void UpdateOfficeInformation()
        {
            var rpProducing = _teamService.HiredScientists.Sum(worker => worker.PointsGeneration);
            var dpProducing = _teamService.HiredProgrammers.Sum(worker => worker.PointsGeneration);
            var totalSalary = _teamService.HiredScientists.Sum(worker => worker.Salary) +
                              _teamService.HiredProgrammers.Sum(worker => worker.Salary);
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(string.Format(_officeLevelTextFormat, _teamService.OfficeLevel));
            stringBuilder.AppendLine(string.Format(_scientistsPlacesTextFormat,
                _teamService.Office.ScientistsPlaces.Length));
            stringBuilder.AppendLine(string.Format(_programmersPlacesTextFormat,
                _teamService.Office.ProgrammersPlaces.Length));
            stringBuilder.AppendLine(string.Format(_rpProducingCountTextFormat, rpProducing));
            stringBuilder.AppendLine(string.Format(_dpProducingCountTextFormat, dpProducing));
            stringBuilder.AppendLine(string.Format(_totalSalaryTextFormat, totalSalary));
            _officeInfoTextObject.text = stringBuilder.ToString();
        }
        private void UpdateTeamInformation()
        {
            _hiredScientistsCountTextObject.text = string.Format(_hiredScientistsTitleTextFormat,
                _hiredScientistsViews.Count, _teamService.Office.ScientistsPlaces.Length);
            _hiredProgrammersCountTextObject.text = string.Format(_hiredProgrammersTitleTextFormat,
                _hiredProgrammersViews.Count, _teamService.Office.ProgrammersPlaces.Length);
        }
        private void FireScientist(Worker worker)
        {
            var view = _hiredScientistsViews.First(view => view.Worker == worker);
            view.transform.parent = _freeScientistsContainer;
            _hiredScientistsViews.Remove(view);
            _freeScientistsViews.Add(view);
            _teamService.FireScientist(worker);
        }
        private void FireProgrammer(Worker worker)
        {
            var view = _hiredProgrammersViews.First(view => view.Worker == worker);
            view.transform.parent = _freeProgrammersContainer;
            _hiredProgrammersViews.Remove(view);
            _freeProgrammersViews.Add(view);
            _teamService.FireProgrammer(worker);
        }
        private void HireScientist(Worker worker)
        {
            var view = _freeScientistsViews.First(view => view.Worker == worker);
            view.transform.parent = _scientistsContainer;
            _freeScientistsViews.Remove(view);
            _hiredScientistsViews.Add(view);
            _teamService.HireScientist(worker);
        }
        private void HireProgrammer(Worker worker)
        {
            var view = _freeProgrammersViews.First(view => view.Worker == worker);
            view.transform.parent = _programmersContainer;
            _freeProgrammersViews.Remove(view);
            _hiredProgrammersViews.Add(view);
            _teamService.HireProgrammer(worker);
        }

        #endregion
    }
}