using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooEmulator {
    public class Program {
        private static UserInterface CommandPromprt;
        private static AnimalLifesEmulator LifeEmulator;
        static void Main(string[] args) {
            var zoo = new ZooController();
            CommandPromprt = new UserInterface();
            CommandPromprt.DataTyped += OnDataTyped;
            zoo.AddAnimal("Mur", 0);
            zoo.AddAnimal("Miau", 1);
            LifeEmulator = new AnimalLifesEmulator(zoo, CommandPromprt);
            CommandPromprt.Start();
            LifeEmulator.Start();
        }

        private static void OnDataTyped(object sender, InputEventArgs inputArgs) {
            CommandPromprt.DataTyped -= OnDataTyped;
            LifeEmulator.Stop();
            CommandPromprt.Stop();
        }
    }
}
