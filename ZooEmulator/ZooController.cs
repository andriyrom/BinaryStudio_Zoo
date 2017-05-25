using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooEmulator.Animals;
using ZooEmulator.Zoo;

namespace ZooEmulator {
    public class ZooController : IZooController {
        private object LockObject = new object();
        private AnimalsFactory Ferm;
        private IZoo Zoo;

        public ZooController() {
            Ferm = new AnimalsFactory();
            Zoo = new SimpleZoo();
        }

        public ZooController(IZoo zoo, AnimalsFactory ferm) {
            Ferm = ferm;
            Zoo = zoo;
        }

        public List<string> GetSpecies() {
            return Ferm.GetSpecies();
        }

        public List<string> GetAnimalNames() {
            lock (LockObject) {
                return Zoo.GetAnimals().Select(animal => animal.Name).ToList();
            }
        }

        public IAnimal GetAnimal(string name) {
            lock (LockObject) {
                IAnimal animal;
                CheckAnimalLiveInZoo(name, out animal);
                return animal;
            }
        }

        public List<IAnimal> QueryAnimal(IQuery<IAnimal> query) {
            lock (LockObject) {
                return Zoo.QueryAnimals(query);
            }
        }

        private ExecutionStatus ExecuteActionSafe(string name, Func<IAnimal, ExecutionStatus> function) {
            lock (LockObject) {
                IAnimal animal;
                ExecutionStatus enimalExistsCheck = CheckAnimalLiveInZoo(name, out animal);
                if (!enimalExistsCheck.Success) {
                    return enimalExistsCheck;
                }
                return function(animal);
            }
        }

        private ExecutionStatus CheckAnimalLiveInZoo(string name, out IAnimal animal) {
            animal = Zoo.GetAnimalByName(name);
            if (animal == null) {
                return new ExecutionStatus(false, string.Format(ErrorCaptions.RemoveFreeAnimalError, name));
            }
            return new ExecutionStatus(true, "");
        }

        public ExecutionStatus AddAnimal(string name, int typeNumber) {
            lock (LockObject) {
                string error;
                IAnimal animal = Ferm.CreateAnimal(name, typeNumber, out error);
                if (!string.IsNullOrEmpty(error)) {
                    return new ExecutionStatus(false, error);
                }
                return Zoo.AddAnimal(animal);
            }           
        }

        public ExecutionStatus RemoveAnimal(string name) {
            return ExecuteActionSafe(name, animal => Zoo.RemoveAnimal(animal));            
        }

        public ExecutionStatus HealAnimal(string name) {
            return ExecuteActionSafe(name, animal => animal.Heal());            
        }

        public ExecutionStatus FeedAnimal(string name) {
            return ExecuteActionSafe(name, animal => animal.Eat());            
        }

        public ExecutionStatus MakeRandomAnimalLive(out IAnimal chousenAnimal) {
            lock(LockObject) {
                var random = new Random();
                IEnumerable<IAnimal> aliveAnimals = Zoo.GetAnimals().Where(animal => animal.State != AnimalStates.Dead);
                if (!aliveAnimals.Any()) {
                    chousenAnimal = null;
                    return new ExecutionStatus(false, ErrorCaptions.AllAnimalsDeadMessage);
                }
                int animalNumer = random.Next() % aliveAnimals.Count();
                chousenAnimal = aliveAnimals.ElementAt(animalNumer);
                chousenAnimal.Live();
                return aliveAnimals.Any()
                    ? new ExecutionStatus(true, "")
                    : new ExecutionStatus(false, ErrorCaptions.AllAnimalsDeadMessage);                
            }
        }
    }
}
