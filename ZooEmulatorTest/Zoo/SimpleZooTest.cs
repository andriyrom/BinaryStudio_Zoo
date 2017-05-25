using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using ZooEmulator.Zoo;
using ZooEmulator.Animals;
using System.Collections.Generic;
using ZooEmulator;

namespace ZooEmulatorTest.Zoo {
    [TestClass]
    public class SimpleZooTest {
        [TestMethod]
        public void TestAddAnimal() {
            string name = "Barsik";
            IAnimal animal = CreateTestAnimal(name, 0);
            IZoo zoo = new SimpleZoo();
            ExecutionStatus status = zoo.AddAnimal(animal);
            List<IAnimal> result = zoo.GetAnimals();
            Assert.IsTrue(status.Success);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(animal, result.First());
        }

        private IAnimal CreateTestAnimal(string name, int typeNumber) {
            SpeciesInfo species = SpeciesData.GetSpeciesInfo()[typeNumber];
            return new SimpleAnimal(name, species.Type, species.MaxHealth);
        }

        [TestMethod]
        public void TestAdd2AnimalsWithSameNames() {
            string name = "Barsik";
            IAnimal animal1 = CreateTestAnimal(name, 0);
            IAnimal animal2 = CreateTestAnimal(name, 1);
            IZoo zoo = new SimpleZoo();

            ExecutionStatus status = zoo.AddAnimal(animal1);
            Assert.IsTrue(status.Success);

            status = zoo.AddAnimal(animal2);
            string expectedErrorText = string.Format(ErrorCaptions.AddAnimalWithExistingNameError, name);
            Assert.IsFalse(status.Success);
            Assert.AreEqual(expectedErrorText, status.ErrorMessage);

            List<IAnimal> result = zoo.GetAnimals();            
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(animal1, result.First());
        }

        [TestMethod]
        public void TestAdd2AnimalsWithDifferentNames() {
            string name1 = "Barsik";
            string name2 = "Myrzik";
            IAnimal animal1 = CreateTestAnimal(name1, 0);
            IAnimal animal2 = CreateTestAnimal(name2, 1);
            IZoo zoo = new SimpleZoo();

            ExecutionStatus status = zoo.AddAnimal(animal1);
            Assert.IsTrue(status.Success);

            status = zoo.AddAnimal(animal2);
            Assert.IsTrue(status.Success);

            List<IAnimal> result = zoo.GetAnimals();
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Contains(animal1));
            Assert.IsTrue(result.Contains(animal2));
        }

        [TestMethod]
        public void TestRemoveAliveAnimal() {
            string name1 = "Barsik";
            string name2 = "Myrzik";
            IAnimal animal1 = CreateTestAnimal(name1, 0);
            IAnimal animal2 = CreateTestAnimal(name2, 1);
            IZoo zoo = new SimpleZoo();

            ExecutionStatus status = zoo.AddAnimal(animal1);
            Assert.IsTrue(status.Success);
            status = zoo.AddAnimal(animal2);
            Assert.IsTrue(status.Success);

            status = zoo.RemoveAnimal(animal1);
            Assert.IsFalse(status.Success);
            string expectedErrorText = string.Format(ErrorCaptions.RemoveAliveAnimalError, animal1.Name);
            Assert.AreEqual(expectedErrorText, status.ErrorMessage);

            List<IAnimal> result = zoo.GetAnimals();
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Contains(animal1));
            Assert.IsTrue(result.Contains(animal2));
        }

        [TestMethod]
        public void TestRemoveDeadAnimal() {
            string name1 = "Barsik";
            string name2 = "Myrzik";
            IAnimal animal1 = CreateTestAnimal(name1, 0);
            IAnimal animal2 = CreateTestAnimal(name2, 1);
            IZoo zoo = new SimpleZoo();

            ExecutionStatus status = zoo.AddAnimal(animal1);
            Assert.IsTrue(status.Success);
            status = zoo.AddAnimal(animal2);
            Assert.IsTrue(status.Success);
            KillAnimal(animal2);

            status = zoo.RemoveAnimal(animal2);
            Assert.IsTrue(status.Success);
            
            List<IAnimal> result = zoo.GetAnimals();
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.Contains(animal1));
        }

        private void KillAnimal(IAnimal animal) {
            while (animal.State != AnimalStates.Dead) {
                animal.Live();
            }
        }

        [TestMethod]
        public void TestRemoveFreeAnimal() {
            string name1 = "Barsik";
            string name2 = "Myrzik";
            IAnimal animal1 = CreateTestAnimal(name1, 0);
            IAnimal animal2 = CreateTestAnimal(name2, 1);
            IZoo zoo = new SimpleZoo();

            ExecutionStatus status = zoo.AddAnimal(animal1);
            Assert.IsTrue(status.Success);            

            status = zoo.RemoveAnimal(animal2);
            Assert.IsFalse(status.Success);
            string expectedErrorText = string.Format(ErrorCaptions.RemoveFreeAnimalError, animal2.Name);
            Assert.AreEqual(expectedErrorText, status.ErrorMessage);

            List<IAnimal> result = zoo.GetAnimals();
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.Contains(animal1));
        }

        [TestMethod]
        public void TestGetAnimalByName() {
            string name1 = "Barsik";
            string name2 = "Myrzik";
            IAnimal animal1 = CreateTestAnimal(name1, 0);
            IAnimal animal2 = CreateTestAnimal(name2, 1);
            IZoo zoo = new SimpleZoo();

            zoo.AddAnimal(animal1);
            zoo.AddAnimal(animal2);
            IAnimal result = zoo.GetAnimalByName(name1);

            Assert.AreEqual(animal1, result);
        }

        [TestMethod]
        public void TestNonExistingAnimalByName() {
            string name1 = "Barsik";
            string name2 = "Myrzik";
            IAnimal animal1 = CreateTestAnimal(name1, 0);
            IAnimal animal2 = CreateTestAnimal(name2, 1);
            IZoo zoo = new SimpleZoo();

            zoo.AddAnimal(animal1);
            zoo.AddAnimal(animal2);
            IAnimal result = zoo.GetAnimalByName("DummyName");

            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestAreAllAnimalsDeadWhenDead() {
            string name1 = "Barsik";
            string name2 = "Myrzik";
            IAnimal animal1 = CreateTestAnimal(name1, 0);
            IAnimal animal2 = CreateTestAnimal(name2, 1);
            IZoo zoo = new SimpleZoo();

            zoo.AddAnimal(animal1);
            zoo.AddAnimal(animal2);
            KillAnimal(animal1);
            KillAnimal(animal2);

            Assert.IsTrue(zoo.AreAllAnimalsDead);
        }

        [TestMethod]
        public void TestAreAllAnimalsDeadWhenSomeAlive() {
            string name1 = "Barsik";
            string name2 = "Myrzik";
            IAnimal animal1 = CreateTestAnimal(name1, 0);
            IAnimal animal2 = CreateTestAnimal(name2, 1);
            IZoo zoo = new SimpleZoo();

            zoo.AddAnimal(animal1);
            zoo.AddAnimal(animal2);
            KillAnimal(animal1);

            Assert.IsFalse(zoo.AreAllAnimalsDead);
        }

        [TestMethod]
        public void TestQueryAnimal() {
            List<string> names1 = new List<string> { "Murzik", "Murzik2" };
            List<string> names2 = new List<string> { "Barsik", "Barsik2" };
            List<IAnimal> animals1 = names1.Select(name => CreateTestAnimal(name, 0)).ToList();
            List<IAnimal> animals2 = names2.Select(name => CreateTestAnimal(name, 1)).ToList();
            List<IAnimal> allAnimals = animals1.Concat(animals2).ToList();
            IZoo zoo = new SimpleZoo();
            allAnimals.ForEach(animal => zoo.AddAnimal(animal));

            List<IAnimal> resultAll = zoo.QueryAnimals(ZooQuery.SearchAll());
            Assert.AreEqual(animals1.Count + animals2.Count, resultAll.Count);

            string type = SpeciesData.GetSpeciesNames()[0];
            List<IAnimal> species1 = zoo.QueryAnimals(ZooQuery.Search(type, AnimalStates.Full));
            CollectionAssert.AreEquivalent(names1, species1);

            List<IAnimal> dead = zoo.QueryAnimals(ZooQuery.SearchByState(AnimalStates.Dead));
            Assert.AreEqual(0, dead.Count);
        }
    }
}
