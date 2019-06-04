using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Reflection;
using HookWinForm.Methods;

namespace HookWinForm
{
    public partial class Form1 : Form
    {
        HookProc MouseHookProcedure;
        int hHook = 0;

        public Form1()
        {
            InitializeComponent();
        }

        public int MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            //Marshall the data from the callback.
            MouseHookStruct MyMouseHookStruct = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));

            if (nCode < 0)
            {
                return DllImport.CallNextHookEx(hHook, nCode, wParam, lParam);
            }
            else
            {
                //Create a string variable that shows the current mouse coordinates.
                String strCaption = "x = " +
                MyMouseHookStruct.pt.x.ToString("d") +
                "  y = " +
                MyMouseHookStruct.pt.y.ToString("d");
                //You must get the active form because it is a static function.
         
                label1.Text = strCaption;
                return DllImport.CallNextHookEx(hHook, nCode, wParam, lParam);
            }
        }
         
        private void button1_Click(object sender, EventArgs e)
        {
            if (hHook != 0)
                return;
            MouseHookProcedure = new HookProc(MouseHookProc);

            hHook = DllImport.SetWindowsHookEx(Consts.WH_MOUSE_LL, MouseHookProcedure,
                Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (hHook == 0)
            {
                MessageBox.Show("SetWindowsHookEx Failed");
                return;
            }
            
            if (DllImport.UnhookWindowsHookEx(hHook) == false)
                 return;

            hHook = 0;
        }
    }

    
}
