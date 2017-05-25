using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooEmulator.Animals;

namespace ZooEmulator.Zoo {
    public interface IZoo {
        List<IAnimal> GetAnimals();

        ExecutionStatus AddAnimal(IAnimal animal);

        ExecutionStatus RemoveAnimal(IAnimal animal);

        IAnimal GetAnimalByName(string name);

        List<IAnimal> QueryAnimals(IQuery<IAnimal> query);

        bool AreAllEnimalsDead { get; }
    }
}
