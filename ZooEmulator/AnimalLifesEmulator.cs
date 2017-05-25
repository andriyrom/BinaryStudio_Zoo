using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZooEmulator.Animals;

namespace ZooEmulator {
    class AnimalLifesEmulator {
        AutoResetEvent autoReset = new AutoResetEvent(false);
        private UserInterface CommandPrompt;
        private ZooController Zoo;
        Timer InnerTimer;
        public AnimalLifesEmulator(ZooController zoo, UserInterface commandPrompt) {
            CommandPrompt = commandPrompt;
            Zoo = zoo;
            InnerTimer = null;
        }

        public void Start() {
            if (InnerTimer == null) {
                InnerTimer = new Timer(TimerEventHandler, null, 0, 5000);
                autoReset.WaitOne();
            }
        }

        private void TimerEventHandler(object state) {
            IAnimal chousenAnimal;
            ExecutionStatus status = Zoo.MakeRandomAnimalLive(out chousenAnimal);
            string message = GetMessage(status, chousenAnimal);
            CommandPrompt.WriteData(message);
        }

        private string GetMessage(ExecutionStatus status, IAnimal animal) {
            return status.Success ? GetMessageForAnimal(animal) : status.ErrorMessage;            
        }

        private string GetMessageForAnimal(IAnimal animal) {
            string animalStateName = Enum.GetName(typeof(AnimalStates), animal.State);
            return string.Format(ErrorCaptions.AnimalMessageTemplate, animal.Type, animal.Name, animalStateName, animal.Health);           
        }

        public void Stop() {
            if (InnerTimer != null) {
                InnerTimer.Dispose();
                InnerTimer = null;
                autoReset.Set();
            }
        }
    }
}
