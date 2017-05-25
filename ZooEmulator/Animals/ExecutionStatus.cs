using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooEmulator.Animals {
    public class ExecutionStatus {
        public bool Success { get; private set; }

        public string ErrorMessage { get; private set; }

        public ExecutionStatus(bool success, string errorMessage) {
            Success = success;
            ErrorMessage = errorMessage;
        }
    }
}
