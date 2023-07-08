using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Team
{
    public class Office : MonoBehaviour
    {
        [field: SerializeField] public Transform[] ScientistsPlaces { get; private set; }
        [field: SerializeField] public Transform[] ProgrammersPlaces { get; private set; }
        
        private Dictionary<Worker, GameObject> _scientists = new();
        private Dictionary<Worker, GameObject> _programmers = new();

        private void OnDestroy()
        {
            foreach(var scientist in _scientists)
                Destroy(scientist.Value);
            foreach(var programmer in _programmers)
                Destroy(programmer.Value);
            _scientists.Clear();
            _programmers.Clear();
        }

        public void AddScientist(Worker worker)
        {
            var workerPlace = ScientistsPlaces.First(place => place.childCount == 0);
            var workerView = Instantiate(worker.WorkerViewPrefab, workerPlace);
            _scientists.Add(worker, workerView);
        }
        public void AddProgrammer(Worker worker)
        {
            var workerPlace = ProgrammersPlaces.First(place => place.childCount == 0);
            var workerView = Instantiate(worker.WorkerViewPrefab, workerPlace);
            _programmers.Add(worker, workerView);
        }
        public void RemoveScientist(Worker worker)
        {
            Destroy(_scientists[worker]);
            _scientists.Remove(worker);
        }
        public void RemoveProgrammer(Worker worker)
        {
            Destroy(_programmers[worker]);
            _programmers.Remove(worker);
        }
    }
}