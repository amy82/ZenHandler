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

        public static void RaiseLanguageChanged()
        {
            Console.WriteLine("EventManager - RaiseLanguageChanged");

            LanguageChanged?.Invoke(null, EventArgs.Empty);  // 모든 구독자 호출
        }
    }
}
