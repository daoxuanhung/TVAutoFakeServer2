using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace TVAutoFakeServer2
{
    public partial class Form1 : Form
    {
        Thread serverThread;
        SimpleHttpServer httpServer;
        TVServer tvServer;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.Name == "TVAuto")
                    btnInstallNic.Enabled = false;
            }
        }

        private void btnInstallNic_Click(object sender, EventArgs e)
        {
            btnInstallNic.Enabled = false;

            if (!Devcon.InstallDriver("C:\\Windows\\Inf\\netloop.inf", "*MSLOOP"))
            {
                MessageBox.Show("Không cài được card mạng loopback", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string nicName = Devcon.GetNICName();
                if (nicName == "")
                {
                    MessageBox.Show("Không lấy được tên card mạng loopback", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (!Devcon.SetIP(nicName))
                    {
                        MessageBox.Show("Không set được IP card mạng loopback", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("OK");
                    }
                }
            }

            btnInstallNic.Enabled = true;
        }

        private void btnHTTPServer_Click(object sender, EventArgs e)
        {
            btnHTTPServer.Enabled = false;
            httpServer = new SimpleHttpServer("http", 80);
        }

        private void btnServer_Click(object sender, EventArgs e)
        {
            btnServer.Enabled = false;
            tvServer = new TVServer();
            serverThread = new Thread(tvServer.Start);
            serverThread.IsBackground = true;
            serverThread.Start();
        }
    }
}
