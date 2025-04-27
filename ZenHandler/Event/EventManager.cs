using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenHandler.Event
{
    public static class EventManager
    {
        public static event EventHandler LanguageChanged;
        public static event EventHandler PgExitCall;        //TODO: 여기에 종료시 연결시키기

        public static void RaiseLanguageChanged()
        {
            Console.WriteLine("EventManager - RaiseLanguageChanged");

            LanguageChanged?.Invoke(null, EventArgs.Empty);  // 모든 구독자 호출
        }

        public static void RaisePgExit()
        {
            Console.WriteLine("EventManager - RaisePgExit");

            PgExitCall?.Invoke(null, EventArgs.Empty);  // 모든 구독자 호출
        }
    }
}
