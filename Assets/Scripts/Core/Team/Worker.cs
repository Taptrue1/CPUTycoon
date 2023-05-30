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
        public GameObject WorkerView { get; }
        
        //TODO add bonuses
        public Worker(string name, string surname, int age, int salary, int pointsGeneration, GameObject workerView)
        {
            Name = name;
            Surname = surname;
            Age = age;
            Salary = salary;
            PointsGeneration = pointsGeneration;
            WorkerView = workerView;
        }
    }
}