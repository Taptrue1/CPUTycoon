using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Team
{
    public class Office : MonoBehaviour
    {
        [field: SerializeField] public double Price { get; private set; }
        [field: SerializeField] public Transform[] WorkersPlaces { get; private set; }
        
        private Dictionary<Worker, GameObject> _workers;

        private void OnDestroy()
        {
            foreach (var worker in _workers.Keys)
                Destroy(_workers[worker]);
        }

        public void AddWorker(Worker worker)
        {
            var workerPlace = WorkersPlaces.First(place => place.childCount == 0);
            var workerView = Instantiate(worker.ViewPrefab, workerPlace);
            _workers.Add(worker, workerView);
        }
        public void RemoveWorker(Worker worker)
        {
            Destroy(_workers[worker]);
            _workers.Remove(worker);
        }
        
        //TODO Remove all this methods
        public void AddScientist(Worker worker)
        {
        }
        public void AddProgrammer(Worker worker)
        {
        }
        public void RemoveScientist(Worker worker)
        {
        }
        public void RemoveProgrammer(Worker worker)
        {
        }
    }
}