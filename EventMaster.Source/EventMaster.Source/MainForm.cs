using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EventMaster.Source
{
    public partial class MainForm : Form
    {
        private SplashScreenForm splashScreen;
        public MainForm()
        {
            InitializeSplashScreen();
            InitializeComponent();

            InitializeApplication();
            ClearSplashScreen();

        }

        

        private void InitializeApplication()
        {
            Thread.Sleep(5000);
        }

        private void InitializeSplashScreen()
        {
            splashScreen = new SplashScreenForm();
            splashScreen.Show();
            Application.DoEvents();
        }
        private void ClearSplashScreen()
        {
            splashScreen.Close();
            splashScreen.Dispose();
            splashScreen = null;
        }

        private void splashScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SplashScreenForm form = new SplashScreenForm();
            form.Show();
            Application.DoEvents();
            Thread.Sleep(3000);
            form.Close();
        }
    }
}
