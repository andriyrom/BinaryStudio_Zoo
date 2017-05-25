using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooEmulator.Animals {
    public class SimpleAnimal : IAnimal {
        private int MaxHealth;
        public string Type { get; private set; }

        public string Name { get; private set; }

        public AnimalStates State { get; private set; }

        public int Health { get; private set; }

        public SimpleAnimal(string name, string type, int maxHealth) {
            Name = name;
            Type = type;
            MaxHealth = maxHealth;
            Health = MaxHealth;
            State = AnimalStates.Full;
        }

        public ExecutionStatus Eat() {
            throw new NotImplementedException();
        }

        public ExecutionStatus Live() {
            switch (State) {
                case AnimalStates.Full: {
                        State = AnimalStates.Hungry;
                        return new ExecutionStatus(true, "");
                    }
                case AnimalStates.Hungry: {
                        State = AnimalStates.Sick;
                        return new ExecutionStatus(true, "");
                    }
                case AnimalStates.Sick: {
                        Health--;
                        State = Health > 0 ? AnimalStates.Sick : AnimalStates.Dead;
                        return new ExecutionStatus(true, "");
                    }
                case AnimalStates.Dead: {                        
                        return new ExecutionStatus(false, ErrorCaptions.DeadAnimalLiveError);
                    }                
            }
            string unknownStateErrorMessage = string.Format(ErrorCaptions.UnknownAnimalState, State.ToString());
            return new ExecutionStatus(false, unknownStateErrorMessage);
        }

        public ExecutionStatus Heal() {
            throw new NotImplementedException();
        }
    }
}
