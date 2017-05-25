using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using ZooEmulator.Zoo;
using ZooEmulator.Animals;
using System.Collections.Generic;

namespace ZooEmulatorTest {
    [TestClass]
    public class ZooQueryTest {
        private const string AnimalTemplate = "Animal{0}_{1}";

        [TestMethod]
        public void TestSearchAllQuery() {
            List<IAnimal> animals = GetTestAnimals();
            IQuery<IAnimal> query = ZooQuery.SearchAll();
            int resultQueryCount = animals.Where(animal => query.Match(animal)).Count();
            Assert.AreEqual(resultQueryCount, animals.Count);
        }

        private List<IAnimal> GetTestAnimals() {
            var allSpecies = SpeciesData.GetSpeciesInfo();
            List<IAnimal> firstSpeciesState = GetAnimalAllStates(allSpecies[0]);
            List<IAnimal> secondSpeciesState = GetAnimalAllStates(allSpecies[1]);
            return firstSpeciesState.Concat(secondSpeciesState).ToList();
        }

        private List<IAnimal> GetAnimalAllStates(SpeciesInfo species) {
            string[] states = Enum.GetNames(typeof(AnimalStates));
            return states.Select(state => { 
                string animalName = string.Format(AnimalTemplate, species.Type, state);
                return new SimpleAnimal(animalName, species.Type, species.MaxHealth) as IAnimal;
            }).ToList();
        }

        [TestMethod]
        public void TestSearchByTypeQuery() {
            List<IAnimal> animals = GetTestAnimals();
            string type = animals.First().Type;
            IQuery<IAnimal> query = ZooQuery.SearchByType(type);

            List<IAnimal> result = animals.Where(animal => query.Match(animal)).ToList();
            List<IAnimal> animalsLeft = animals.Where(animal => !query.Match(animal)).ToList();

            Assert.IsTrue(result.All(animal => animal.Type == type));
            Assert.IsTrue(animalsLeft.All(animal => animal.Type != type));
        }

        [TestMethod]
        public void TestSearchByStateQuery() {
            List<IAnimal> animals = GetTestAnimals();
            AnimalStates state = AnimalStates.Hungry;
            IQuery<IAnimal> query = ZooQuery.SearchByState(state);

            List<IAnimal> result = animals.Where(animal => query.Match(animal)).ToList();
            List<IAnimal> animalsLeft = animals.Where(animal => !query.Match(animal)).ToList();

            Assert.IsTrue(result.All(animal => animal.State == state));
            Assert.IsTrue(animalsLeft.All(animal => animal.State != state));
        }

        [TestMethod]
        public void TestSearchByTypeAndStateQuery() {
            List<IAnimal> animals = GetTestAnimals();
            string type = animals.First().Type;
            AnimalStates state = AnimalStates.Hungry;
            IQuery<IAnimal> query = ZooQuery.Search(type, state);

            List<IAnimal> result = animals.Where(animal => query.Match(animal)).ToList();
            List<IAnimal> animalsLeft = animals.Where(animal => !query.Match(animal)).ToList();

            Assert.IsTrue(result.All(animal => animal.Type == type && animal.State == state));
            Assert.IsTrue(animalsLeft.All(animal => animal.Type != type || animal.State != state));
        }
    }
}
