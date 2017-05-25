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
            switch (State) {
                case AnimalStates.Hungry:
                case AnimalStates.Full: {
                        State = AnimalStates.Full;
                        return new ExecutionStatus(true, "");
                    }
                case AnimalStates.Sick: {
                        return new ExecutionStatus(false, ErrorCaptions.SickAnimalEatError);
                    }
                case AnimalStates.Dead: {
                        return new ExecutionStatus(false, ErrorCaptions.DeadAnimalEatError);
                    }
            }
            return GetUnknownErrorStatus();
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
            return GetUnknownErrorStatus();
        }

        public ExecutionStatus Heal() {
            if (State == AnimalStates.Dead) {
                return new ExecutionStatus(false, ErrorCaptions.DeadAnimalHealError);
            }
            if (State == AnimalStates.Sick) { State = AnimalStates.Hungry; }
            if (Health < MaxHealth) { Health++; }
            return new ExecutionStatus(true, "");
        }

        private ExecutionStatus GetUnknownErrorStatus() {
            string unknownStateErrorMessage = string.Format(ErrorCaptions.UnknownAnimalState, State.ToString());
            return new ExecutionStatus(false, unknownStateErrorMessage);
        }
    }
}
