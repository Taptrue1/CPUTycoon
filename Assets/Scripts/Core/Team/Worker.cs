using UnityEngine;

namespace Core.Team
{
    public class Worker
    {
        public string Name { get; }
        public string Surname { get; }
        public int Age { get; }
        public int Salary { get; }
        public int PointsGeneration { get; }
        public Sprite Icon { get; }
        public GameObject WorkerViewPrefab { get; }
        
        //TODO add bonuses
        public Worker(string name, string surname, int age, int salary, int pointsGeneration, Sprite icon, GameObject workerViewPrefab)
        {
            Age = age;
            Icon = icon;
            Name = name;
            Salary = salary;
            Surname = surname;
            PointsGeneration = pointsGeneration;
            WorkerViewPrefab = workerViewPrefab;
        }
    }
}