using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooEmulator.Animals {
    public class SpeciesInfo {
        public string Type { get; private set; }

        public int MaxHealth { get; private set; }

        public SpeciesInfo(string type, int maxHealth) {
            Type = type;
            MaxHealth = maxHealth;
        }
    }
}
