using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZenHandler
{
    static class Program
    {
        public const string VERSION_INFO = "1.0.0.1";
        public const string BUILD_DATE = "25-04-16";        //16:00

        public static bool NORINDA_MODE = true;    //TODO: 배포전 변경해야된다.
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MainForm());


            // 뮤텍스를 생성하여 애플리케이션이 이미 실행 중인지 확인
            using (Mutex mutex = new Mutex(true, "{Assembly.GetExecutingAssembly().GetName().Name}", out bool isAppAlreadyRunning))
            {
                if (isAppAlreadyRunning)
                {
                    // 애플리케이션이 처음 실행될 때
                    Application.Run(new MainForm());
                }
                else
                {
                    // 이미 실행 중이면 사용자에게 메시지 표시
                    MessageBox.Show("이 애플리케이션은 이미 실행 중입니다.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            
        }
    }
}
