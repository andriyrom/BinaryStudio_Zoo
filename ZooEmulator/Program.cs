using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooEmulator {
    public class Program {
        private static UserInterface CommandPromprt;
        private static AnimalLifesEmulator LifeEmulator;
        private static CommandInterpretator Interpretator;
        static void Main(string[] args) {
            var zoo = new ZooController();
            CommandPromprt = new UserInterface();
            LifeEmulator = new AnimalLifesEmulator(zoo, CommandPromprt);
            Interpretator = new CommandInterpretator(zoo, CommandPromprt);
            Interpretator.Start += OnStart;
            Interpretator.Exit += OnExit;
            CommandPromprt.Start();            
        }

        private static void OnStart(object sender, EventArgs args) {
            LifeEmulator.Start();
        }

        private static void OnExit(object sender, EventArgs args) {
            Interpretator.Start -= OnStart;
            Interpretator.Exit -= OnExit;
            Interpretator.Dispose();
            CommandPromprt.Stop();
            LifeEmulator.Stop();            
        }
    }
}
