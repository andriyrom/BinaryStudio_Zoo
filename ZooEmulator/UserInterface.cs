using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZooEmulator {
    class UserInterface {
        AutoResetEvent readBlocker = new AutoResetEvent(false);
        AutoResetEvent writeBlocker = new AutoResetEvent(false);
        private object WriteLockObject = new object();
        private object ReaderLockObject = new object();
        private object ContinueWorkLockObject = new object();
        private bool ContinueWork;
        private char InputKey = '\0';
        private string Input = "";
        private List<string> writeDataQueue = new List<string>(); 

        public void Start() {
            ContinueWork = true;
            Thread readerThread = new Thread(DataReaderStart);
            Thread writerThread = new Thread(DataWriterStart);
            readerThread.IsBackground = false;
            readerThread.Start();
            writerThread.Start();
        }

        public void Stop() {
            ContinueWork = false;
        }

        public void WriteData(string data) {
            lock (WriteLockObject) {
                writeDataQueue.Add(data);
            }
        }    

        private void DataWriterStart() {
            bool continueWork = true;
            while (continueWork) {
                lock (ReaderLockObject)
                lock (WriteLockObject) {
                    if (writeDataQueue.Any()) {
                        writeDataQueue.ForEach(data => Console.WriteLine(data));
                        writeDataQueue.Clear();
                    }
                }
                lock (ContinueWorkLockObject) {
                    continueWork = ContinueWork;
                }
            }
        }

        private void DataReaderStart() {
            bool continueWork = true;
            while (continueWork) {
                Thread testInput = new Thread(InputTest);
                lock (ReaderLockObject) {
                    testInput.Start();
                    if (testInput.Join(3000)) {
                        if (InputKey == 'y' || InputKey == 'Y') {
                            ReadInput();
                        } else {
                            ClearInput();
                        }
                    } else {
                        testInput.Abort();
                        ClearInput();
                    }
                    InputKey = '\0';
                }
                lock (ContinueWorkLockObject) {
                    continueWork = ContinueWork;
                }
            }
        }

        private void ReadInput() {
            Console.WriteLine();
            Console.Write("Enter your command with parameters here: ");
            string input = Console.ReadLine();
            Thread inputEventRaise = new Thread(RaiseInputEvent);
            inputEventRaise.IsBackground = true;
            inputEventRaise.Start(input);
        }

        private void ClearInput() {
            int rowToClearIndex = Console.CursorTop;
            string cleanText = "                                                              ";
            Console.SetCursorPosition(0, rowToClearIndex);
            Console.Write(cleanText);
            Console.SetCursorPosition(0, rowToClearIndex);
        }

        private void RaiseInputEvent(object parameter) {
            string input = (string)parameter;
            EventHandler<InputEventArgs> handler = DataTyped;
            if (handler != null) {
                handler(this, new InputEventArgs(input));
            }
        }

        private void InputTest() {
            try {       
                Console.Write("If you want to input command, press 'y': ");
                InputKey = Console.ReadKey().KeyChar; 
            } catch (ThreadAbortException) { } 
        }
        
        public event EventHandler<InputEventArgs> DataTyped;
    }

    class InputEventArgs : EventArgs {
        public string Input { get; private set; }

        public InputEventArgs(string input) {
            Input = input;
        }        
    }
}
