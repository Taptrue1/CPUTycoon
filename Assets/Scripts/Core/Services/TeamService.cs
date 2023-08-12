using System;
using System.Collections.Generic;
using System.Linq;
using Core.Team;
using Settings;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Services
{
    public class TeamService
    {
        public int OfficeLevel { get; private set; }
        public Office Office { get; private set; }
        public List<Worker> HiredScientists { get; }
        public List<Worker> HiredEngineers { get; }
        public List<Worker> FreeScientists { get; }
        public List<Worker> FreeEngineers { get; }

        private int _lastSalaryMonth;
        private DateTime _lastFreeWorkersGenerationTime;
        private readonly TimeService _timeService;
        private readonly CurrencyService _currencyService;
        private readonly TeamSettings _teamSettings;
        
        public TeamService(CoreSettings coreSettings, TimeService timeService, CurrencyService currencyService)
        {
            _timeService = timeService;
            _currencyService = currencyService;
            _teamSettings = coreSettings.TeamSettings;
            _lastSalaryMonth = _timeService.CurrentDate.Month;

            OfficeLevel = 1;
            Office = InstantiateOffice(_teamSettings.Offices[0]);
            HiredScientists = new List<Worker>();
            HiredEngineers = new List<Worker>();
            FreeScientists = new List<Worker>();
            FreeEngineers = new List<Worker>();
            
            _timeService.Tick += OnTick;
            
            GenerateFreeWorkers();
        }

        public void UpgradeOffice()
        {
            if(!CanUpgradeOffice()) throw new Exception("Can't upgrade office");
            var officeIndex = Array.IndexOf(_teamSettings.Offices, Office);
            var office = _teamSettings.Offices[officeIndex + 1];
            Object.Destroy(Office.gameObject);
            Office = InstantiateOffice(office);
            OfficeLevel++;
            foreach(var scientist in HiredScientists)
                Office.AddScientist(scientist);
            foreach(var programmer in HiredEngineers)
                Office.AddProgrammer(programmer);
        }
        public void HireScientist(Worker worker)
        {
            if(!CanHireScientist()) throw new Exception("Can't hire scientist");
            HiredScientists.Add(worker);
            Office.AddScientist(worker);
        }
        public void HireEngineer(Worker worker)
        {
            if(!CanHireProgrammer()) throw new Exception("Can't hire programmer");
            HiredEngineers.Add(worker);
            Office.AddProgrammer(worker);
        }
        public void FireScientist(Worker worker)
        {
            if (!HiredScientists.Contains(worker)) throw new Exception("Can't fire scientist");
            HiredScientists.Remove(worker);
            Office.RemoveScientist(worker);
        }
        public void FireEngineer(Worker worker)
        {
            if (!HiredEngineers.Contains(worker)) throw new Exception("Can't fire programmer");
            HiredEngineers.Remove(worker);
            Office.RemoveProgrammer(worker);
        }

        public bool CanUpgradeOffice()
        {
            var officeIndex = Array.IndexOf(_teamSettings.Offices, Office);
            return officeIndex < _teamSettings.Offices.Length - 1;
        }
        public bool CanHireScientist()
        {
            return Office.ScientistsPlaces.Length > HiredScientists.Count;
        }
        public bool CanHireProgrammer()
        {
            return Office.ProgrammersPlaces.Length > HiredEngineers.Count;
        }

        #region Callbacks
        private void OnTick()
        {
            if(CanGenerateFreeWorkers())
                GenerateFreeWorkers();
            if (CanPaySalaries())
                PaySalaries();
            EarnPoints();
        }
        #endregion

        #region Other
        private void EarnPoints()
        {
            var scientistsPointsGeneration = HiredScientists.Select(worker => worker.PointsGeneration).Sum();
            var programmersPointsGeneration = HiredEngineers.Select(worker => worker.PointsGeneration).Sum();
            _currencyService.GetCurrency("RP").Value += scientistsPointsGeneration;
            _currencyService.GetCurrency("DP").Value += programmersPointsGeneration;
            Debug.Log($"Earned {scientistsPointsGeneration} RP and {programmersPointsGeneration} DP");
        }
        private bool CanPaySalaries()
        {
            return _lastSalaryMonth != _timeService.CurrentDate.Month;
        }
        private void PaySalaries()
        {
            var scientistsSalary = HiredScientists.Select(worker => worker.Salary).Sum();
            var programmersSalary = HiredEngineers.Select(worker => worker.Salary).Sum();
            _currencyService.GetCurrency("Money").Value -= scientistsSalary + programmersSalary;
            _lastSalaryMonth = _timeService.CurrentDate.Month;
        }
        private bool CanGenerateFreeWorkers()
        {
            var timePassed = _timeService.CurrentDate - _lastFreeWorkersGenerationTime;
            return timePassed.Days >= _teamSettings.FreeWorkersGenerateDelay;
        }
        private void GenerateFreeWorkers()
        {
            FreeEngineers.Clear();
            FreeScientists.Clear();
            for(var i = 0; i < _teamSettings.FreeWorkersCount * 2; i++)
            {
                var name = _teamSettings.Names[UnityEngine.Random.Range(0, _teamSettings.Names.Length)];
                var surname = _teamSettings.Surnames[UnityEngine.Random.Range(0, _teamSettings.Surnames.Length)];
                var age = UnityEngine.Random.Range(_teamSettings.WorkersMinAge, _teamSettings.WorkersMaxAge);
                var salary = UnityEngine.Random.Range(_teamSettings.WorkersMinSalary, _teamSettings.WorkersMaxSalary);
                var pointsGeneration = UnityEngine.Random.Range(_teamSettings.WorkersMinPointsGeneration,
                    _teamSettings.WorkersMaxPointsGeneration);
                var workerIndex = UnityEngine.Random.Range(0, _teamSettings.WorkerPairs.Length);
                var workerView = _teamSettings.WorkerPairs[workerIndex].WorkerView;
                var icon = _teamSettings.WorkerPairs[workerIndex].Icon;
                var worker = new Worker(name, surname, age, salary, pointsGeneration, icon, workerView);
                if(i < _teamSettings.FreeWorkersCount)
                    FreeEngineers.Add(worker);
                else
                    FreeScientists.Add(worker);
            }
            _lastFreeWorkersGenerationTime = _timeService.CurrentDate;
        }
        private Office InstantiateOffice(Office office)
        {
            return Object.Instantiate(office, Vector3.zero, Quaternion.identity);
        }
        #endregion
    }
}