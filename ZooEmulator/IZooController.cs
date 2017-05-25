using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooEmulator.Animals;
using ZooEmulator.Zoo;

namespace ZooEmulator {
    public interface IZooController {
        List<string> GetSpecies();

        List<string> GetAnimalNames();

        IAnimal GetAnimal(string name);

        List<IAnimal> QueryAnimal(IQuery<IAnimal> query);

        ExecutionStatus AddAnimal(string name, int typeNumber);

        ExecutionStatus RemoveAnimal(string name);

        ExecutionStatus HealAnimal(string name);

        ExecutionStatus FeedAnimal(string name);

        ExecutionStatus MakeRandomAnimalLive();
    }
}
