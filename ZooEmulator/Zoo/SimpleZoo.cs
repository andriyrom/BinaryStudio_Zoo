using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooEmulator.Animals;

namespace ZooEmulator.Zoo {
    public class SimpleZoo : IZoo {
        private Dictionary<string, IAnimal> AnimalsInZoo = new Dictionary<string, IAnimal>();

        public bool AreAllEnimalsDead { get { return false; } }

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
            throw new NotImplementedException();
        }

        public Animals.IAnimal GetAnimalByName(string name) {
            throw new NotImplementedException();
        }

        public List<IAnimal> QueryAnimals(IQuery<IAnimal> query) {
            throw new NotImplementedException();
        }
    }
}
