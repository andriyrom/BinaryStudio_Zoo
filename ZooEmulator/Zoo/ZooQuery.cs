using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooEmulator.Animals;

namespace ZooEmulator.Zoo {
    /// <summary>
    /// Represent query to IZoo implementors.
    /// <param name="AnimalType">Animal type to search. If null or empty - search all types.</param>
    /// <param name="State">Animal state to search. If null search all states.</param>
    /// </summary>
    public class ZooQuery : IQuery<IAnimal> {
        public string AnimalType { get; private set; }

        public Nullable<AnimalStates> State { get; private set; }

        private ZooQuery(string type, Nullable<AnimalStates> state) {
            AnimalType = type;
            State = state;
        }

        public static ZooQuery SearchAll() {
            return new ZooQuery("", null);
        }

        public static ZooQuery SearchByType(string type) {
            return new ZooQuery(type, null); ;
        }

        public static ZooQuery SearchByState(AnimalStates state) {
            return new ZooQuery("", state); ;
        }

        public static ZooQuery Search(string type, AnimalStates state) {
            return new ZooQuery(type, state);
        }

        public bool Match(IAnimal element) {
            bool shouldFilterState = State.HasValue;
            bool shouldFilterType = !string.IsNullOrEmpty(AnimalType);
            bool stateFit = shouldFilterState ? element.State.Equals(State.Value) : true;
            bool typeFit = shouldFilterType ? element.Type.Equals(AnimalType) : true;
            return stateFit && typeFit;
        }
    }
}
