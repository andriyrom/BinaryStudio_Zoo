using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooEmulator.Animals {
    public interface IAnimal {
        string Type { get; }

        string Name { get; }

        AnimalStates State { get; }

        int Health { get; }

        ExecutionStatus Eat();

        ExecutionStatus Live();

        ExecutionStatus Heal();
    }
}
