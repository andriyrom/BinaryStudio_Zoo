using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using ZooEmulator.Animals;

namespace ZooEmulatorTest.Animals {
    [TestClass]
    public class SimpleAnimalTest {

        [TestMethod]
        public void TestAnimalInitialState() {
            string name = "Barsik";
            SpeciesInfo species = SpeciesData.GetSpeciesInfo().First();
            IAnimal animal = new SimpleAnimal(name, species.Type, species.MaxHealth);
            Assert.AreEqual(AnimalStates.Full, animal.State);
            Assert.AreEqual(name, animal.Name);
            Assert.AreEqual(species.Type, animal.Type);
            Assert.AreEqual(species.MaxHealth, animal.Health);
        }

        [TestMethod]
        public void TestAnimalLiveWithFullState() {
            string name = "Barsik";
            SpeciesInfo species = SpeciesData.GetSpeciesInfo().First();
            IAnimal animal = new SimpleAnimal(name, species.Type, species.MaxHealth);
            Assert.AreEqual(AnimalStates.Full, animal.State);
            ExecutionStatus status = animal.Live();
            Assert.IsTrue(status.Success);
            Assert.AreEqual(AnimalStates.Hungry, animal.State);
            Assert.AreEqual(species.MaxHealth, animal.Health);
        }

        [TestMethod]
        public void TestAnimalLiveWithHungryState() {
            string name = "Barsik";
            SpeciesInfo species = SpeciesData.GetSpeciesInfo().First();
            IAnimal animal = GetHungryAnimal(name, species);                
            Assert.AreEqual(AnimalStates.Hungry, animal.State);
            ExecutionStatus status = animal.Live();
            Assert.IsTrue(status.Success);
            Assert.AreEqual(AnimalStates.Sick, animal.State);
            Assert.AreEqual(species.MaxHealth, animal.Health);
        }

        [TestMethod]
        public void TestAnimalLiveWithSickState() {
            string name = "Barsik";
            SpeciesInfo species = SpeciesData.GetSpeciesInfo().First();
            IAnimal animal = GetSickAnimal(name, species);
            Assert.AreEqual(AnimalStates.Sick, animal.State);
            ExecutionStatus status = animal.Live();
            Assert.IsTrue(status.Success);
            Assert.AreEqual(AnimalStates.Sick, animal.State);
            Assert.AreEqual(species.MaxHealth - 1, animal.Health);
        }

        [TestMethod]
        public void TestAnimalDie() {
            string name = "Barsik";
            SpeciesInfo species = SpeciesData.GetSpeciesInfo().First();
            IAnimal animal = GetSickAnimal(name, species);
            Assert.AreEqual(AnimalStates.Sick, animal.State);
            ExecutionStatus status = null;
            for (int i = 0; i < species.MaxHealth; i++) {
                status = animal.Live();
            }
            Assert.IsTrue(status.Success);
            Assert.AreEqual(AnimalStates.Dead, animal.State);
            Assert.AreEqual(0, animal.Health);
        }

        [TestMethod]
        public void TestAnimalLiveWithDeadState() {
            string name = "Barsik";
            SpeciesInfo species = SpeciesData.GetSpeciesInfo().First();
            IAnimal animal = GetDeadAnimal(name, species);
            Assert.AreEqual(AnimalStates.Dead, animal.State);
            ExecutionStatus status = animal.Live();
            Assert.IsFalse(status.Success);
            Assert.AreEqual(ErrorCaptions.DeadAnimalLiveError, status.ErrorMessage);
            Assert.AreEqual(AnimalStates.Dead, animal.State);
        }   

        private IAnimal GetHungryAnimal(string name, SpeciesInfo species) {
            IAnimal animal = new SimpleAnimal(name, species.Type, species.MaxHealth);
            animal.Live();
            return animal;
        }

        private IAnimal GetSickAnimal(string name, SpeciesInfo species) {
            IAnimal animal = GetHungryAnimal(name, species);
            animal.Live();
            return animal;
        }

        private IAnimal GetDeadAnimal(string name, SpeciesInfo species) {
            IAnimal animal = GetSickAnimal(name, species);
            for (int i = 0; i < species.MaxHealth; i++) {
                animal.Live();
            }
            return animal;
        }
    }
}
