using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooEmulator.Animals {
    internal static class ErrorCaptions {
        public const string DeadAnimalLiveError = "Animal is dead, it can not live";
        public const string UnknownAnimalState = "Unknown animal state: {0}";
        public const string SickAnimalEatError = "Sick animal can not eat.";
        public const string DeadAnimalEatError = "Animal is dead, it can not eat.";
        public const string DeadAnimalHealError = "Animal is dead, it can not eat.";
        public const string AddAnimalWithExistingNameError = "Animal with name '{0}' already exists. You can not add new animal with the name '{0}'.";
        public const string RemoveAliveAnimalError = "Animal with name '{0}' is still alive. You can not remove this animal.";
        public const string RemoveFreeAnimalError = "Animal with name '{0}' is not live in this zoo. You can not remove this animal.";
    }
}
