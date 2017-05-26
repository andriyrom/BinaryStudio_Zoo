using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooEmulator.Animals;

namespace ZooEmulator {
    class CommandInterpretator : IDisposable {        
        private IZooController Zoo;
        private UserInterface CommandPromprt;
        private UserCommands CommandImplementations;
        public CommandInterpretator(IZooController zoo, UserInterface commandPromprt) {
            Zoo = zoo;
            CommandPromprt = commandPromprt;
            CommandPromprt.DataTyped += OnCommandTyped;
            CommandImplementations = new UserCommands(this);
        }

        public void Dispose() {
            CommandPromprt.DataTyped -= OnCommandTyped;
            Zoo = null;
            CommandImplementations = null;
        }

        private void OnCommandTyped(object sender, InputEventArgs inputArgs) {
            string[] commandAndParameters = inputArgs.Input.Split(' ');
            string command = commandAndParameters[0];
            List<string> parameters = commandAndParameters.Skip(1).ToList();
            CommandSearchResult result = CommandImplementations.FindCommand(command);
            if (result.Success) {
                result.Command(parameters);
            } else {
                string errorMessage = string.Format("Command '{0}' not found.", command);
                CommandPromprt.WriteData(errorMessage);
            }
        }

        public event EventHandler Exit;
        public event EventHandler Start;

        private class UserCommands {
            private Dictionary<string, Action<List<string>>> Commands;
            private CommandInterpretator Interpretator;
            public UserCommands(CommandInterpretator parent) {
                Interpretator = parent;
                InitializeCommands();
            }

            private void InitializeCommands() {
               Commands = new Dictionary<string, Action<List<string>>>{
                   { "Add", AddAnimal },
                   { "Exit", Exit },
                   { "Start", Start },
                   { "Species", ShowSpecies },
                   { "Feed", FeedAnimal },
                   { "Heal", HealAnimal },
               };
            }

            public CommandSearchResult FindCommand(string command) {
                Action<List<string>> commandAction;
                bool success = Commands.TryGetValue(command, out commandAction);
                return new CommandSearchResult(success, commandAction);
            }

            private void AddAnimal(List<string> parameters) {
                string typeNumberStr = parameters[0];
                string animalName = parameters[1];
                
                int typeNumer;
                if (!int.TryParse(typeNumberStr, out typeNumer)) {
                    string errorMessage = typeNumberStr + " is not a class number.";
                    Interpretator.CommandPromprt.WriteData(errorMessage);
                    return;
                }
                typeNumer--;
                ExecutionStatus status = Interpretator.Zoo.AddAnimal(animalName, typeNumer);
                string message = status.Success ? animalName + " added to the zoo." : status.ErrorMessage;
                Interpretator.CommandPromprt.WriteData(message);
            }

            private void Exit(List<string> parameters) {
                EventHandler handler = Interpretator.Exit;
                if (handler != null) {
                    handler(this, new EventArgs());
                }
            }

            private void Start(List<string> parameters) {
                EventHandler handler = Interpretator.Start;
                if (handler != null) {
                    handler(this, new EventArgs());
                }
            }

            private void ShowSpecies(List<string> parameters) {
                List<string> species = Interpretator.Zoo.GetSpecies();
                int index = 1;
                StringBuilder speciesOutput = new StringBuilder();
                species.ForEach( curSpecies => {
                    speciesOutput.AppendFormat("{0}. {1} \n", index, curSpecies);
                    index++;
                });
                Interpretator.CommandPromprt.WriteData(speciesOutput.ToString());
            }


            private void FeedAnimal(List<string> parameters) {
                string animalName = parameters[0];               
                ExecutionStatus status = Interpretator.Zoo.FeedAnimal(animalName);
                string message = status.Success
                    ? animalName + " feaded.\n" + GetAnimalInfo(animalName)
                    : status.ErrorMessage;
                Interpretator.CommandPromprt.WriteData(message);
            }

            private void HealAnimal(List<string> parameters) {
                string animalName = parameters[0];
                ExecutionStatus status = Interpretator.Zoo.HealAnimal(animalName);
                string message = status.Success
                    ? animalName + " healed. \n" + GetAnimalInfo(animalName) 
                    : status.ErrorMessage;                
                Interpretator.CommandPromprt.WriteData(message);
            }

            private string GetAnimalInfo(string name) {
                IAnimal animal = Interpretator.Zoo.GetAnimal(name);
                string state = Enum.GetName(typeof(AnimalStates), animal.State);
                return string.Format(ErrorCaptions.AnimalMessageTemplate, animal.Type, animal.Name, state, animal.Health);
            }
        }

        private class CommandSearchResult {
            public bool Success { get; private set; }

            public Action<List<string>> Command { get; private set; }

            public CommandSearchResult(bool success, Action<List<string>> command) {
                Success = success;
                Command = command;
            }
        }
    }
}
