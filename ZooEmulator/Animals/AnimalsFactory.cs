using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooEmulator.Animals {
     public class AnimalsFactory {
        public List<string> GetSpecies() {
            return SpeciesData.GetSpeciesNames();
        }

        public IAnimal CreateAnimal(string name, int speciesNumber, out string error) {
            List<SpeciesInfo> allSpecies = SpeciesData.GetSpeciesInfo();
            error = Validate(name, speciesNumber, allSpecies.Count);
            if (string.IsNullOrEmpty(error)) {
                return null;
            }            
            SpeciesInfo species = allSpecies[speciesNumber];
            return new SimpleAnimal(name, species.Type, species.MaxHealth);
        }

        private string Validate(string name, int speciesNumber, int allSpeciesCount) {
            if (string.IsNullOrWhiteSpace(name) || speciesNumber >= allSpeciesCount || speciesNumber < 0) {
                return ErrorCaptions.WrongAnimalCreationParameters;
            }
            return "";
        }
    }
}
