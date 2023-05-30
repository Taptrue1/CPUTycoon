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
        public Office Office { get; private set; }
        public List<Worker> HiredScientists { get; }
        public List<Worker> HiredProgrammers { get; }
        public List<Worker> FreeScientists { get; }
        public List<Worker> FreeProgrammers { get; }

        private int _lastSalaryMonth;
        private DateTime _lastFreeWorkersGenerationTime;
        private readonly TimeService _timeService;
        private readonly CurrencyService _currencyService;
        private readonly TeamSettings _coreSettings;
        
        public TeamService(CoreSettings coreSettings, TimeService timeService, CurrencyService currencyService)
        {
            _timeService = timeService;
            _currencyService = currencyService;
            _coreSettings = coreSettings.TeamSettings;
            _lastSalaryMonth = _timeService.CurrentDate.Month;

            Office = InstantiateOffice(_coreSettings.Offices[0]);
            HiredScientists = new List<Worker>();
            HiredProgrammers = new List<Worker>();
            FreeScientists = new List<Worker>();
            FreeProgrammers = new List<Worker>();
            
            _timeService.Tick += OnTick;
            
            GenerateFreeWorkers();
        }

        public void UpgradeOffice()
        {
            if(!CanUpgradeOffice()) throw new Exception("Can't upgrade office");
            var officeIndex = Array.IndexOf(_coreSettings.Offices, Office);
            var office = _coreSettings.Offices[officeIndex + 1];
            Object.Destroy(Office.gameObject);
            Office = InstantiateOffice(office);
            foreach(var scientist in HiredScientists)
                Office.AddScientist(scientist);
            foreach(var programmer in HiredProgrammers)
                Office.AddProgrammer(programmer);
        }
        public void HireScientist(Worker worker)
        {
            if(!CanHireScientist()) throw new Exception("Can't hire scientist");
            HiredScientists.Add(worker);
            Office.AddScientist(worker);
        }
        public void HireProgrammer(Worker worker)
        {
            if(!CanHireProgrammer()) throw new Exception("Can't hire programmer");
            HiredProgrammers.Add(worker);
            Office.AddProgrammer(worker);
        }
        public void FireScientist(Worker worker)
        {
            if (!HiredScientists.Contains(worker)) throw new Exception("Can't fire scientist");
            HiredScientists.Remove(worker);
            Office.RemoveScientist(worker);
        }
        public void FireProgrammer(Worker worker)
        {
            if (!HiredProgrammers.Contains(worker)) throw new Exception("Can't fire programmer");
            HiredProgrammers.Remove(worker);
            Office.RemoveProgrammer(worker);
        }

        public bool CanUpgradeOffice()
        {
            var officeIndex = Array.IndexOf(_coreSettings.Offices, Office);
            return officeIndex < _coreSettings.Offices.Length - 1;
        }
        public bool CanHireScientist()
        {
            return Office.ScientistsPlaces.Length > HiredScientists.Count;
        }
        public bool CanHireProgrammer()
        {
            return Office.ProgrammersPlaces.Length > HiredProgrammers.Count;
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
            var programmersPointsGeneration = HiredProgrammers.Select(worker => worker.PointsGeneration).Sum();
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
            var programmersSalary = HiredProgrammers.Select(worker => worker.Salary).Sum();
            _currencyService.GetCurrency("Money").Value -= scientistsSalary + programmersSalary;
            _lastSalaryMonth = _timeService.CurrentDate.Month;
        }
        private bool CanGenerateFreeWorkers()
        {
            var timePassed = _timeService.CurrentDate - _lastFreeWorkersGenerationTime;
            return timePassed.Days >= _coreSettings.FreeWorkersGenerateDelay;
        }
        private void GenerateFreeWorkers()
        {
            FreeProgrammers.Clear();
            FreeScientists.Clear();
            for(var i = 0; i < _coreSettings.FreeWorkersCount * 2; i++)
            {
                var name = _coreSettings.Names[UnityEngine.Random.Range(0, _coreSettings.Names.Length)];
                var surname = _coreSettings.Surnames[UnityEngine.Random.Range(0, _coreSettings.Surnames.Length)];
                var age = UnityEngine.Random.Range(18, 60);
                var salary = UnityEngine.Random.Range(1000, 10000);
                var pointsGeneration = UnityEngine.Random.Range(1, 10);
                var workerView = _coreSettings.WorkerViews[UnityEngine.Random.Range(0, _coreSettings.WorkerViews.Length)];
                var worker = new Worker(name, surname, age, salary, pointsGeneration, workerView);
                if(i < _coreSettings.FreeWorkersCount)
                    FreeProgrammers.Add(worker);
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