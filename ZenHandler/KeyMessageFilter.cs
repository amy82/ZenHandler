using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZenHandler
{
    // 메시지 필터 클래스
    public class KeyMessageFilter : IMessageFilter
    {
        //public delegate void KeyEventHandler(Keys key);

        public delegate void KeyEventHandler(Keys key, string eventId);  // 이벤트에 ID 추가
        public event KeyEventHandler KeyEvent;  // 키 이벤트를 다른 폼으로 전달할 수 있는 이벤트

        private readonly Dictionary<string, KeyEventHandler> keyEvents = new Dictionary<string, KeyEventHandler>();


        public void RegisterKeyHandler(string eventId, KeyEventHandler handler)
        {
            if (!keyEvents.ContainsKey(eventId))
            {
                keyEvents[eventId] = handler;
            }
        }

        public void UnregisterKeyHandler(string eventId)
        {
            if (keyEvents.ContainsKey(eventId))
            {
                keyEvents.Remove(eventId);
            }
        }
        public bool InvokeSpecificKeyEvent(Keys key, string eventId)
        {
            if (keyEvents.TryGetValue(eventId, out var handler))
            {
                handler.Invoke(key, eventId);
                return true;  // 성공적으로 실행됨
            }
            return false; // 해당 eventId가 없음
        }
        public bool PreFilterMessage(ref Message m)
        {
            // 키 입력 메시지 확인 (WM_KEYDOWN)
            if (m.Msg == 0x0100) // WM_KEYDOWN
            {
                Keys key = (Keys)(int)m.WParam;

                // 여기에서 막고 싶은 키를 체크 (예: Space 키)

                if (key == Keys.Enter)
                {
                    if (keyEvents.Count > 0)
                    {
                        InvokeSpecificKeyEvent(key, "InputForm");
                    }
                }
                if (key == Keys.Space)
                {
                    // KeyEvent 이벤트를 발생시켜 다른 폼으로 전달 

                    if (keyEvents.Count > 0)
                    {
                        InvokeSpecificKeyEvent(key, "MessagePopUpForm");
                    }
                    

                    //if (KeyEvent != null && KeyEvent.GetInvocationList().Length > 0)
                    //{
                    //    //KeyEvent.Invoke(key);
                        
                    //}
                    /////KeyEvent?.Invoke(key);

                    //if (KeyEvent != null)
                    //{
                    //    foreach (var d in KeyEvent.GetInvocationList())
                    //    {
                    //        KeyEvent -= (KeyEventHandler)d;       //이벤트 삭제하는 코드인데 지워버리면 이벤트 삭제돼서 주석처리함
                    //    }
                    //}


                    // 모든 등록된 이벤트 핸들러 호출
                    //foreach (var kvp in keyEvents)
                    //{
                    //    kvp.Value.Invoke(key, kvp.Key);  // 이벤트 ID와 함께 호출
                    //}
                    return true; // true 반환하면 키 입력을 막을 수 있음
                }
                
                
                if (key == Keys.Escape|| key == Keys.Alt)
                {
                    // Space 키 입력 막기
                    return true; // true 반환하면 키 입력을 막을 수 있음
                }
            }
            return false; // 다른 키는 그대로 처리
        }

    }
}
