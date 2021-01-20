using System;
using System.Threading;

namespace ConsoleTest
{
    class MathTask
    {
        private Thread _CalculationThread;
        private int _Result;
        private bool _IsComplited;

        public bool IsComplited => _IsComplited;

        public int Result
        {
            get
            {
                if (!_IsComplited)
                    _CalculationThread.Join();
                return _Result;
            }
        }

        public MathTask(Func<int> Calculation)
        {
            _CalculationThread = new Thread(
                    () =>
                    {
                        _Result = Calculation();
                        _IsComplited = true;
                    })
                { IsBackground = true };
        }

        public void Start() => _CalculationThread.Start();
    }
}