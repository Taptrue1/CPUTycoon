using System;
using System.Collections.Generic;
using Core.Team;

namespace Core.NEW
{
    public class Company
    {
        public event Action<Office> OfficeChanged;
        public event Action<List<Worker>> StaffChanged;
        
        public string Name { get; }
        public double Budget { get; private set; }
        public double Reputation { get; private set; }
        public Office Office { get; private set; }
        public List<Worker> Staff { get; }
        
        public Company(string name, double budget, double reputation, Office office, List<Worker> staff)
        {
            Name = name;
            Budget = budget;
            Reputation = reputation;
            Office = office;
            Staff = staff;
        }
        
        public void Hire(Worker worker)
        {
            if(Staff.Count >= Office.WorkersPlaces.Length) return;
            Staff.Add(worker);
            Office.AddWorker(worker);
            StaffChanged?.Invoke(Staff);
        }
        public void Fire(Worker worker)
        {
            if(!Staff.Contains(worker)) return;
            Staff.Remove(worker);
            Office.RemoveWorker(worker);
            StaffChanged?.Invoke(Staff);
        }
        public bool TryUpgradeOffice(Office office)
        {
            if (Budget < office.Price) return false;
            Budget -= office.Price;
            Office = office;
            foreach (var worker in Staff)
                Office.AddWorker(worker);
            OfficeChanged?.Invoke(Office);
            return true;
        }
    }
}