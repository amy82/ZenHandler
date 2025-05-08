using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenHandler.Process
{
    public class ProcessManager
    {
        public TransferFlow transferFlow;
        public LiftFlow liftFlow;
        public MagazineFlow magazineFlow;

        public FwSocketFlow fwSocketFlow;
        public AoiSocketFlow aoiSocketFlow;
        public EEpromSocketFlow eepromSocketFlow;


        public ProcessManager()
        {
            transferFlow = new TransferFlow();
            liftFlow = new LiftFlow();
            magazineFlow = new MagazineFlow();
            //
            fwSocketFlow = new FwSocketFlow();
            aoiSocketFlow = new AoiSocketFlow();
            eepromSocketFlow = new EEpromSocketFlow();

        }



    }
}
