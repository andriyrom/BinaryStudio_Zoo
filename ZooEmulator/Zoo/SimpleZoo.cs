using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooEmulator.Animals;

namespace ZooEmulator.Zoo {
    public class SimpleZoo : IZoo {
        private Dictionary<string, IAnimal> AnimalsInZoo = new Dictionary<string, IAnimal>();

        public bool AreAllAnimalsDead { get { return false; } }

        public List<IAnimal> GetAnimals() {
            return AnimalsInZoo.Values.ToList();
        }

        public ExecutionStatus AddAnimal(IAnimal animal) {
            if (AnimalsInZoo.ContainsKey(animal.Name)) {
                string errorMessage = string.Format(ErrorCaptions.AddAnimalWithExistingNameError, animal.Name);
                return new ExecutionStatus(false, errorMessage);
            }
            AnimalsInZoo.Add(animal.Name, animal);
            return new ExecutionStatus(true, "");
        }

        public ExecutionStatus RemoveAnimal(IAnimal animal) {
            if (!IsAnimalExistsInZoo(animal)) {
                return new ExecutionStatus(false, string.Format(ErrorCaptions.RemoveFreeAnimalError, animal.Name));
            }
            if (animal.State == AnimalStates.Dead) {
                AnimalsInZoo.Remove(animal.Name);
                return new ExecutionStatus(true, "");
            }
            return new ExecutionStatus(false, string.Format(ErrorCaptions.RemoveAliveAnimalError, animal.Name));
        }

        private bool IsAnimalExistsInZoo(IAnimal animal) {
            return AnimalsInZoo.ContainsKey(animal.Name);
        }

        public Animals.IAnimal GetAnimalByName(string name) {
            throw new NotImplementedException();
        }

        public List<IAnimal> QueryAnimals(IQuery<IAnimal> query) {
            throw new NotImplementedException();
        }
    }
}
