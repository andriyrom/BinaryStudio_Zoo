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
    }
}
