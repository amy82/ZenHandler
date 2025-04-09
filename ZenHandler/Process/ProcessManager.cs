using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenHandler.Process
{
    public class ProcessManager
    {
        public PcbProcess pcbProcess;
        public TransferFlow transferFlow;
        public LiftFlow liftFlow;

        public ProcessManager()
        {
            pcbProcess = new PcbProcess();
            transferFlow = new TransferFlow();
            liftFlow = new LiftFlow();
        }



    }
}
