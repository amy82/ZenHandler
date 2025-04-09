using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ActiveAligApp
{
    abstract class BaseThread
    {
        private Thread _thread;

        protected BaseThread()
        {
            _thread = new Thread(new ThreadStart(this.RunThread));
        }

        // Thread methods / properties
        public void Start() => _thread.Start();
        public void Join() => _thread.Join();
        public bool IsAlive => _thread.IsAlive;
        public void Interrupt() => _thread.Interrupt();
        //public bool IsBackground => _thread.IsBackground;
        public bool IsBackground
        {
            get
            {
                return _thread.IsBackground;
            }
            set
            {
                _thread.IsBackground = value;
            }
        }
        // Override in base class
        public abstract void RunThread();
    }
}
