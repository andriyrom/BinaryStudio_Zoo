using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooEmulator.Animals {
    public static class SpeciesData {
        private static readonly Lazy<List<SpeciesInfo>> AnimalTypes = 
            new Lazy<List<SpeciesInfo>>(InitializeAnimalTypes, true);
        public static List<string> GetSpeciesNames() {
            return GetSpeciesInfo().Select(animal => animal.Type).ToList();
        }

        public static List<SpeciesInfo> GetSpeciesInfo() {
            return AnimalTypes.Value;
        }

        private static List<SpeciesInfo> InitializeAnimalTypes() {
            return new List<SpeciesInfo>() { 
                new SpeciesInfo("Lion", 5),
                new SpeciesInfo("Tiger", 4),
                new SpeciesInfo("Elephant", 7),
                new SpeciesInfo("Bear", 6),
                new SpeciesInfo("Wolf", 4),
                new SpeciesInfo("Fox", 3)
            };
        }
    }
}
