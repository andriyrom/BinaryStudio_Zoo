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
    }
}
