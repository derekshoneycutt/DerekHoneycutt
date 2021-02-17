using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
/*
namespace SearchIcd10.Outputs
{
    /// <summary>
    /// Output Handler that places text in the DMNE/DMPE Dictation Box
    /// </summary>
    public class DragonLegacyOutputHandler : IOutputHandler
    {
        private bool m_WaitingForRecog;

        /// <summary>
        /// Places the output into Dragon's Dictation Box
        /// </summary>
        /// <param name="outputText">Text to place in the dictation box</param>
        /// <returns>True if successful--False if no Dragon Client SDK, no user opened, or failure at opening the Dictation Box</returns>
        public bool PutOutputText(string outputText)
        {    
            object o_engineControl = null;
            try
            {
                o_engineControl = new DNSTools.DgnEngineControl();
            }
            catch (COMException)
            {
                return false;
            }

            var engineControl = (DNSTools.DgnEngineControl)o_engineControl;

            if (String.IsNullOrWhiteSpace(engineControl.Speaker))
            {
                return false;
            }

            bool ret = false;
            engineControl.Register();

            try
            {
                engineControl.MimicDone += engineControl_MimicDone;

                m_WaitingForRecog = true;
                engineControl.RecognitionMimic("open dictation box");
                var waiter = Task.Factory.StartNew(() =>
                {
                    while (m_WaitingForRecog)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                    System.Threading.Thread.Sleep(1500);

                    var hWnd = Win32.Windows.FindByClassName(s => s.StartsWith("DgnDictationBoxCls", StringComparison.InvariantCultureIgnoreCase));
                    if (hWnd != IntPtr.Zero)
                    {
                        var rtfChild = Win32.Windows.FindChild(hWnd, childWnd =>
                            Win32.Windows.GetClassName(childWnd).StartsWith("RICHEDIT20", StringComparison.InvariantCultureIgnoreCase));
                        if (rtfChild != IntPtr.Zero)
                        {
                            Win32.Windows.SendReplaceSelection(rtfChild, outputText);
                            ret = true;
                        }
                    }
                });

                waiter.Wait();
            }
            finally
            {
                engineControl.MimicDone -= engineControl_MimicDone;
                engineControl.UnRegister(false);
            }
            return ret;
        }

        private void engineControl_MimicDone(ref int Parameter)
        {
            m_WaitingForRecog = false;
        }

        /// <summary>
        /// Switch to the existing window, if applicable to the output
        /// </summary>
        public void SwitchToExisting() { }
    }
}
*/