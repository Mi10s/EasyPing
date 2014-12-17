using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Threading;
using System.IO;
using System.Net;

namespace Internet_Tools
{
    public partial class iTools : Form
    {
        private bool check;
        private PingClass ping;

        public iTools()
        {
            
            ping = new PingClass();
            InitializeComponent();
            
            
        }

        public static bool HasConnection()
        {
            try
            {
                System.Net.IPHostEntry i = System.Net.Dns.GetHostEntry("www.google.com");
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 box = new AboutBox1();
            box.Text = "About EasyPING";

            box.ShowDialog();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            check = false;
            ping.setCheck(check);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool isEmpty;
            if (txtBoxPrint.Text == "")
            {
                MessageBox.Show("No data to save!");
                isEmpty = false;
            }
            else isEmpty = true;

            if (isEmpty)
            {
                saveFileDialog1.Title = "Save EasyPING log as text file";
                saveFileDialog1.Filter = ".txt |*";
                saveFileDialog1.ShowDialog();


                if (saveFileDialog1.FileName != "")
                {
                    if (Path.GetExtension(saveFileDialog1.FileName) != ".txt")
                    {
                        File.WriteAllText(saveFileDialog1.FileName + ".txt", txtBoxPrint.Text);
                    }
                    else File.WriteAllText(saveFileDialog1.FileName, txtBoxPrint.Text);

                }
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {

                numericUpDownPING.Enabled = false;
            }
            else
            {
                numericUpDownPING.Enabled = true;

            }
        }

        private void roundButton1_Click(object sender, EventArgs e)
        {
            initPingState();

            startPing();
        }

        private void roundButton2_Click(object sender, EventArgs e)
        {
            check = false;
            ping.setCheck(check);
        }


        private void clearTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtBoxPrint.Text = "";
        }

        private void txtBoxIP_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                e.Handled = true;
                initPingState();
                startPing();

               
            }
        }
        private void initPingState() {

            ping.resetPacketsLost();
            ping.resetPacketRecived();
            ping.resetPrint();
            txtBoxPrint.Clear();


            check = true;
            bool repeat = checkBox1.Checked;


            ping.setTimes(Convert.ToInt32(numericUpDownPING.Value));
            ping.setTtl(Convert.ToInt32(numericUpDownTTL.Value));
            ping.setRepeat(repeat);
            ping.setTxtBoxIP(txtBoxIP);
            ping.setTxtBoxPrint(txtBoxPrint);
            ping.setTimeout(Convert.ToInt32(numericUpDown1.Value));
            check = true;
            ping.setCheck(check);
        }

        private void startPing() {
            if (HasConnection())
            {

                if (txtBoxIP.Text != "")
                {

                    if (checkBox1.Checked)
                    {

                        do
                        {

                            ping.startPingLoop();


                        } while (check);
                    }

                    else
                    {

                        ping.startPing();

                    }
                }
                else txtBoxPrint.Text = "Please enter IP/Domain address!";
            }
            else txtBoxPrint.Text = "No internet connection.";
        }



       





    }
}
