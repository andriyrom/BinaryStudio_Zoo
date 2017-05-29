using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooEmulator.Zoo;
using ZooEmulator.Animals;
using System.Linq;
using System.Collections.Generic;
using ZooEmulator;

namespace ZooEmulatorTest {
    [TestClass]
    public class LINQZooStatisticsCalculatorTest {
        private static string AnimalTemplate = "{0}_{1}_{2}";
        private static AnimalsFactory Farm = new AnimalsFactory();
        private static ZooController Controller;

        [ClassInitialize()]
        public static void Initialize(TestContext context) {
            Controller = PrepareZooTestData();
        }

        private static ZooController PrepareZooTestData() {
            var zoo = new SimpleZoo();
            var allAnimals = CreateAnimalsByState("First", AnimalStates.Sick)
                .Concat(CreateAnimalsByState("Second", AnimalStates.Hungry))
                .Concat(CreateAnimalsByState("Third", AnimalStates.Dead))
                .ToList();
            allAnimals.ForEach(animal => zoo.AddAnimal(animal));
            return new ZooController(zoo, Farm); 
        }

        private static List<IAnimal> CreateAnimalsByState(string namePrefix, AnimalStates state) {
            string stateName = Enum.GetName(typeof(AnimalStates), state);
            List<string> speciesNames = Farm.GetSpecies();
            int index = 0;
            var animals = speciesNames
                .Select(speciesName => {
                    string error;
                    string fullName = string.Format(AnimalTemplate, namePrefix, speciesName, stateName);
                    return Farm.CreateAnimal(fullName, index++, out error);
                })
                .Where(animal => animal != null).ToList();
            animals.ForEach(animal => SetAnimalState(animal, state));
            return animals;
        }

        private static void SetAnimalState(IAnimal animal, AnimalStates state) {
            while (!(animal.State == state || animal.State == AnimalStates.Dead)) {
                animal.Live();
            }
        }

        [TestMethod]
        public void TestGroupByType() {
            IZooStatistics statistic = Controller.GetStatisticsCalculator();
            var typeGrouping = statistic.GroupByType();

            List<string> types = typeGrouping.Keys.ToList();
            int expectedCount = 3;
            bool IsCountRight = typeGrouping.All(pair => pair.Value.Count == expectedCount);
            Assert.IsTrue(IsCountRight);
        }

        [TestMethod]
        public void TestGetAverageHealth() {
            IZooStatistics statistic = Controller.GetStatisticsCalculator();

            double expectedAverageHealth = Controller.QueryAnimal(ZooQuery.SearchAll()).Average(animal => animal.Health);
            double averageHealth = statistic.GetAverageHealthPerZoo();
            Assert.AreEqual(expectedAverageHealth, averageHealth);
        }

        [TestMethod]
        public void TestGetByState() {
            IZooStatistics statistic = Controller.GetStatisticsCalculator();
                        
            List<IAnimal> sickAnimals = statistic.GetByState(AnimalStates.Sick);
            var healthyAnimals = Controller.QueryAnimal(ZooQuery.SearchAll());
            healthyAnimals.RemoveAll(animal => sickAnimals.Contains(animal));
            Assert.IsTrue(sickAnimals.All(animal => animal.State == AnimalStates.Sick));
            Assert.IsTrue(healthyAnimals.All(animal => animal.State != AnimalStates.Sick));
        }

        [TestMethod]
        public void TestGetByStateAndType() {
            IZooStatistics statistic = Controller.GetStatisticsCalculator();

            string species = Farm.GetSpecies()[1];
            List<IAnimal> sickAnimalOfType = statistic.GetByStateAndType(AnimalStates.Sick, species);
            var otherAnimals = Controller.QueryAnimal(ZooQuery.SearchAll());
            otherAnimals.RemoveAll(animal => sickAnimalOfType.Contains(animal));
            Assert.IsTrue(sickAnimalOfType.All(animal => animal.State == AnimalStates.Sick && animal.Type == species));
            Assert.IsTrue(otherAnimals.All(animal => animal.State != AnimalStates.Sick || animal.Type != species));
        }

        [TestMethod]
        public void TestGetByNameAndType() {
            IZooStatistics statistic = Controller.GetStatisticsCalculator();

            string species = Farm.GetSpecies()[1];
            IAnimal animal = statistic.GetByTypeAndName(species, "First_Tiger_Sick");
            Assert.AreEqual("First_Tiger_Sick", animal.Name);
        }

        [TestMethod]
        public void TestGetByNameAndTypeWithWrongType() {
            IZooStatistics statistic = Controller.GetStatisticsCalculator();

            string species = Farm.GetSpecies()[0];
            IAnimal animal = statistic.GetByTypeAndName(species, "First_Tiger_Sick");            
            Assert.AreEqual(null, animal);
        }

        [TestMethod]
        public void TestGetAnimalNamesByState() {
            IZooStatistics statistic = Controller.GetStatisticsCalculator();

            List<string> animalNames = statistic.GetAnimalNamesByState(AnimalStates.Hungry);
            List<string> expectedNames = statistic.GetByState(AnimalStates.Hungry)
                .Select(animal => animal.Name).ToList();
            CollectionAssert.AreEquivalent(expectedNames, animalNames);
        }

        [TestMethod]
        public void TestGetMostHealthyGroupByType() {
            IZooStatistics statistic = Controller.GetStatisticsCalculator();

            var result = statistic.GetMostHealtyAnimalsGroupByType();
            List<SpeciesInfo> allSpecies = SpeciesData.GetSpeciesInfo();
            bool IsCorrectData = result.All(pair => {
                SpeciesInfo species = allSpecies.Find(info => info.Type == pair.Key);
                return species.MaxHealth == pair.Value.Health;
            });
            Assert.IsTrue(IsCorrectData);
        }

        [TestMethod]
        public void TestGetDeadAnimalsGroupByType() {
            IZooStatistics statistic = Controller.GetStatisticsCalculator();

            var result = statistic.GetDeadAnimalsGroupByType();
            int expectedTypesCount = SpeciesData.GetSpeciesInfo().Count();
            Assert.AreEqual(expectedTypesCount, result.Count);
            Assert.IsTrue(result.Values.All(deadCount => deadCount == 1));
        }

        [TestMethod]
        public void TestGetByTypeAndHealth() {
            IZooStatistics statistic = Controller.GetStatisticsCalculator();

            List<string> species = Farm.GetSpecies();
            int healthLowerBound = 3;
            var result = statistic.GetByTypeAndHealth(new[] { species[0], species[1], species[4] }, healthLowerBound);
            int expectedTypesCount = 3;
            Assert.AreEqual(expectedTypesCount, result.Count);
            Assert.IsTrue(result.Values.All(animalsGroup => animalsGroup.All(animal => animal.Health > healthLowerBound)));
        }

        [TestMethod]
        public void TestGetAnimalsWithMaxAndMinHealth() {
            IZooStatistics statistic = Controller.GetStatisticsCalculator();

            var result = statistic.GetAnimalsWithMaxAndMinHealth();
            int expectedCount = 2;
            var allAnimals = Controller.QueryAnimal(ZooQuery.SearchAll());
            int maxHealth = allAnimals.Max(animal => animal.Health);
            int minHealth = allAnimals.Min(animal => animal.Health);
            Assert.AreEqual(expectedCount, result.Count);
            Assert.AreEqual(maxHealth, result.First().Health);
            Assert.AreEqual(minHealth, result.Last().Health);
        }
    }
}
