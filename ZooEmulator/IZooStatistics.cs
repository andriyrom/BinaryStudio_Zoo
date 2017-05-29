using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooEmulator.Animals;

namespace ZooEmulator {
    public interface IZooStatistics {
        Dictionary<string, List<IAnimal>> GroupByType();

        List<IAnimal> GetByState(AnimalStates state);

        List<IAnimal> GetByStateAndType(AnimalStates state, string type);

        IAnimal GetByTypeAndName(string type, string name);

        List<string> GetAnimalNamesByState(AnimalStates state);

        Dictionary<string, IAnimal> GetMostHealtyAnimalsGroupByType();

        Dictionary<string, int> GetDeadAnimalsGroupByType();

        Dictionary<string, List<IAnimal>> GetByTypeAndHealth(IEnumerable<string> types, int minHealthLevel);

        List<IAnimal> GetAnimalsWithMaxAndMinHealth();

        double GetAverageHealthPerZoo();
    }
}
