using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using ZooEmulator;
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

        [TestMethod]
        public void TestAnimalEatWithHungryState() {
            string name = "Barsik";
            SpeciesInfo species = SpeciesData.GetSpeciesInfo().First();
            IAnimal animal = GetHungryAnimal(name, species);
            int initialHealth = animal.Health;
            Assert.AreEqual(AnimalStates.Hungry, animal.State);
            ExecutionStatus status = animal.Eat();
            Assert.IsTrue(status.Success);
            Assert.AreEqual(AnimalStates.Full, animal.State);
            Assert.AreEqual(initialHealth, animal.Health);
        }

        [TestMethod]
        public void TestAnimalEatWithFullState() {
            string name = "Barsik";
            SpeciesInfo species = SpeciesData.GetSpeciesInfo().First();
            IAnimal animal = new SimpleAnimal(name, species.Type, species.MaxHealth);
            int initialHealth = animal.Health;
            Assert.AreEqual(AnimalStates.Full, animal.State);
            ExecutionStatus status = animal.Eat();
            Assert.IsTrue(status.Success);
            Assert.AreEqual(AnimalStates.Full, animal.State);
            Assert.AreEqual(initialHealth, animal.Health);
        }

        [TestMethod]
        public void TestAnimalEatWithSickState() {
            string name = "Barsik";
            SpeciesInfo species = SpeciesData.GetSpeciesInfo().First();
            IAnimal animal = GetSickAnimal(name, species);
            int initialHealth = animal.Health;
            Assert.AreEqual(AnimalStates.Sick, animal.State);
            ExecutionStatus status = animal.Eat();
            Assert.IsFalse(status.Success);
            Assert.AreEqual(ErrorCaptions.SickAnimalEatError, status.ErrorMessage);
            Assert.AreEqual(AnimalStates.Sick, animal.State);
            Assert.AreEqual(initialHealth, animal.Health);
        }

        [TestMethod]
        public void TestAnimalEatWithDeadState() {
            string name = "Barsik";
            SpeciesInfo species = SpeciesData.GetSpeciesInfo().First();
            IAnimal animal = GetDeadAnimal(name, species);
            int initialHealth = animal.Health;
            Assert.AreEqual(AnimalStates.Dead, animal.State);
            ExecutionStatus status = animal.Eat();
            Assert.IsFalse(status.Success);
            Assert.AreEqual(ErrorCaptions.DeadAnimalEatError, status.ErrorMessage);
            Assert.AreEqual(AnimalStates.Dead, animal.State);
            Assert.AreEqual(0, animal.Health);
        }

        private IAnimal GetSickAnimalWithLostHealth(string name, SpeciesInfo species, int healthLose) {
            IAnimal animal = GetSickAnimal(name, species);
            for (int i = 0; i < healthLose; i++) {
                animal.Live();
            }
            return animal;
        }

        [TestMethod]
        public void TestAnimalHealWithSickStateAndHealthLose() {
            string name = "Barsik";
            int healthLose = 1;
            SpeciesInfo species = SpeciesData.GetSpeciesInfo().First();

            IAnimal animal = GetSickAnimalWithLostHealth(name, species, healthLose);
            int initialHealth = animal.Health;
            Assert.AreEqual(AnimalStates.Sick, animal.State);
            ExecutionStatus status = animal.Heal();
            Assert.IsTrue(status.Success);
            Assert.AreEqual(AnimalStates.Hungry, animal.State);
            Assert.AreEqual(initialHealth + 1, animal.Health);
        }

        [TestMethod]
        public void TestAnimalHealWithSickStateAndFullHealth() {
            string name = "Barsik";
            SpeciesInfo species = SpeciesData.GetSpeciesInfo().First();
            IAnimal animal = GetSickAnimal(name, species);
            Assert.AreEqual(AnimalStates.Sick, animal.State);
            Assert.AreEqual(species.MaxHealth, animal.Health);
            ExecutionStatus status = animal.Heal();
            Assert.IsTrue(status.Success);
            Assert.AreEqual(AnimalStates.Hungry, animal.State);
            Assert.AreEqual(species.MaxHealth, animal.Health);
        }

        [TestMethod]
        public void TestAnimalHealWithFullState() {
            string name = "Barsik";
            SpeciesInfo species = SpeciesData.GetSpeciesInfo().First();
            IAnimal animal = new SimpleAnimal(name, species.Type, species.MaxHealth);
            ExecutionStatus status = animal.Heal();
            Assert.IsTrue(status.Success);
            Assert.AreEqual(AnimalStates.Full, animal.State);
            Assert.AreEqual(species.MaxHealth, animal.Health);
        }

        [TestMethod]
        public void TestAnimalHealWithFullStateAndHealthLose() {
            string name = "Barsik";
            int healthLose = 2;
            SpeciesInfo species = SpeciesData.GetSpeciesInfo().First();
            IAnimal animal = GetSickAnimalWithLostHealth(name, species, healthLose);
            animal.Heal();
            animal.Eat();
            int initialHealth = animal.Health;
            Assert.AreEqual(AnimalStates.Full, animal.State);
            ExecutionStatus status = animal.Heal();
            Assert.IsTrue(status.Success);
            Assert.AreEqual(AnimalStates.Full, animal.State);
            Assert.AreEqual(initialHealth + 1, animal.Health);
        }

        [TestMethod]
        public void TestAnimalHealWithHungryStateAndHealthLose() {
            string name = "Barsik";
            int healthLose = 2;
            SpeciesInfo species = SpeciesData.GetSpeciesInfo().First();
            IAnimal animal = GetSickAnimalWithLostHealth(name, species, healthLose);
            animal.Heal();
            int initialHealth = animal.Health;
            Assert.AreEqual(AnimalStates.Hungry, animal.State);
            ExecutionStatus status = animal.Heal();
            Assert.IsTrue(status.Success);
            Assert.AreEqual(AnimalStates.Hungry, animal.State);
            Assert.AreEqual(initialHealth + 1, animal.Health);
        }

        [TestMethod]
        public void TestAnimalHealWithHungryStateAndFullHealth() {
            string name = "Barsik";
            int healthLose = 1;
            SpeciesInfo species = SpeciesData.GetSpeciesInfo().First();
            IAnimal animal = GetSickAnimalWithLostHealth(name, species, healthLose);
            animal.Heal();
            Assert.AreEqual(AnimalStates.Hungry, animal.State);
            Assert.AreEqual(species.MaxHealth, animal.Health);
            ExecutionStatus status = animal.Heal();
            Assert.IsTrue(status.Success);
            Assert.AreEqual(AnimalStates.Hungry, animal.State);
            Assert.AreEqual(species.MaxHealth, animal.Health);
        }

        [TestMethod]
        public void TestAnimalHealWithDeadState() {
            string name = "Barsik";
            SpeciesInfo species = SpeciesData.GetSpeciesInfo().First();
            IAnimal animal = GetDeadAnimal(name, species);
            int initialHealth = animal.Health;
            Assert.AreEqual(AnimalStates.Dead, animal.State);
            ExecutionStatus status = animal.Heal();
            Assert.IsFalse(status.Success);
            Assert.AreEqual(ErrorCaptions.DeadAnimalHealError, status.ErrorMessage);
            Assert.AreEqual(AnimalStates.Dead, animal.State);
            Assert.AreEqual(0, animal.Health);
        }
    }
}
