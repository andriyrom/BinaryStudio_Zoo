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
    }
}
