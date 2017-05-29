using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooEmulator.Animals;
using ZooEmulator.Zoo;

namespace ZooEmulator {
    internal class LINQZooStatisticsCalculator : IZooStatistics{
        private object LockObject;
        private IZoo Zoo;
        private List<IAnimal> Animals {
            get {
                return Zoo.GetAnimals();
            }
        }

        internal LINQZooStatisticsCalculator(IZoo zoo, object lockSyncObject) {
            LockObject = lockSyncObject;
            Zoo = zoo;
        }

        public Dictionary<string, List<IAnimal>> GroupByType() {
            lock(LockObject) {
                return Animals.GroupBy(animal => animal.Type).ToDictionary(grouping => grouping.Key, grouping => grouping.ToList());
            }
        }

        public List<IAnimal> GetByState(AnimalStates state) {
            lock (LockObject) {
                return SelectAnimalsByState(state).ToList();
            }
        }

        private IEnumerable<IAnimal> SelectAnimalsByState(AnimalStates state) {
            foreach (var animal in Animals) {
                if (animal.State == state) {
                    yield return animal;
                }
            }
        }

        public List<IAnimal> GetByStateAndType(AnimalStates state, string type) {
            lock (LockObject) {
                return Animals.Where(animal => animal.State == state && animal.Type == type).ToList();
            }
        }

        public IAnimal GetByTypeAndName(string type, string name) {
            lock (LockObject) {
                return Animals.Where(animal => animal.Name == name && animal.Type == type).FirstOrDefault();
            }
        }

        public List<string> GetAnimalNamesByState(AnimalStates state) {
            lock (LockObject) {
                return Animals
                    .Where(animal => animal.State == state)
                    .Select(animal => animal.Name)
                    .ToList();
            }
        }

        public Dictionary<string, IAnimal> GetMostHealtyAnimalsGroupByType() {
            lock (LockObject) {
                return Animals.GroupBy(animal => animal.Type)
                    .ToDictionary(
                        grouping => grouping.Key,
                        grouping => {
                            int maxHealth = grouping.Max(animal => animal.Health);
                            return grouping.Where(animal => animal.Health == maxHealth).First();
                        });
            }
        }

        public Dictionary<string, int> GetDeadAnimalsGroupByType() {
            lock (LockObject) {
                return Animals.GroupBy(animal => animal.Type)
                    .ToDictionary(
                        grouping => grouping.Key,
                        grouping => grouping.Where(animal => animal.State == AnimalStates.Dead).Count());
            }
        }

        public Dictionary<string, List<IAnimal>> GetByTypeAndHealth(IEnumerable<string> types, int minHealthLevel) {
            lock (LockObject) {
                return Animals.Where(animal => animal.Health > minHealthLevel && types.Contains(animal.Type))
                    .GroupBy(animal => animal.Type)
                    .ToDictionary(
                        group => group.Key,
                        group => group.ToList()
                    );
            }
        }

        public List<IAnimal> GetAnimalsWithMaxAndMinHealth() {
            lock (LockObject) {
                return (from animalHealth in Animals
                        let maxHealth = Animals.Max(animal => animal.Health)
                        where animalHealth.Health == maxHealth
                        select animalHealth).Take(1)
                       .Concat(
                        (from animalHealth in Animals
                         let minHealth = Animals.Min(animal => animal.Health)
                         where animalHealth.Health == minHealth
                         select animalHealth).Take(1)
                         ).ToList();
            }
        }

        public double GetAverageHealthPerZoo() {
            lock (LockObject) {
                return Animals.Select(animal => animal.Health).Average();
            }
        }
    }
}
