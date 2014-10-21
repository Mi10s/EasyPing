using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using System.Threading;
using System.Net;



namespace Internet_Tools
{
    class PingClass
    {
        private PingReply reply;
        private Ping pingSender;
        private StringBuilder print;
        private int packetLost = 0;
        private int packetRecieved = 0;
        private PingOptions options;
        private string data;
        private byte[] buffer;
        private int times = 0;
        private TextBox txtBoxIP;
        private TextBox txtBoxPrint;
        private bool repeat;
        private String printSeparator = "==============================================" + Environment.NewLine + Environment.NewLine;
        private int ttl = 128;
        private int timeout = 1000;
        private bool check = true;

        public PingClass()
        {

            print = new StringBuilder();
            

          
            pingSender = new Ping();
            options = new PingOptions();
            

            // Use the default Ttl value which is 128, 
            // but change the fragmentation behavior.
            options.DontFragment = false;

            // Create a buffer of 32 bytes of data to be transmitted. 
            data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            buffer = Encoding.ASCII.GetBytes(data);
            options.Ttl = ttl;

            

        }

        public void startPing()
        {

            options.Ttl = ttl;
            for (int i = 0; i < times; i++)
            {
                if (!check)
                    break;

                resetPrint();

                try
                {
                    reply = pingSender.Send(txtBoxIP.Text, timeout, buffer, options);

                    switch (reply.Status)
                    {
                        case IPStatus.Success:

                            packetRecieved++;
                            print.Append("  Address: " + reply.Address.ToString() + Environment.NewLine);
                            print.Append("  RoundTrip time: " + reply.RoundtripTime + Environment.NewLine);
                            print.Append("  Time to live: " + this.getTtl() + Environment.NewLine);
                            print.Append("  Don't fragment: false" + Environment.NewLine);
                            print.Append("  Buffer size: " + reply.Buffer.Length + Environment.NewLine);
                            print.Append("  Itearation: " + (i + 1) + Environment.NewLine);
                            print.Append("  Lost packets: " + packetLost + Environment.NewLine +
                                         "  Recived packets: " + packetRecieved + Environment.NewLine);
                            print.Append(printSeparator);

                            txtBoxPrint.AppendText(print.ToString());

                            txtBoxPrint.SelectionStart = txtBoxPrint.TextLength;
                            txtBoxPrint.ScrollToCaret();
                            txtBoxPrint.Refresh();
                            int y = 5;
                            while (y > 0)
                            {
                                Application.DoEvents();


                                Thread.Sleep(20);
                                y--;
                            }

                            continue;




                        case IPStatus.TimedOut:
                            packetLost++;
                            txtBoxPrint.Text += "   Connection timed out." + Environment.NewLine;
                            txtBoxPrint.Text += "   Lost packets: " + packetLost + Environment.NewLine +
                                                "   Recived packets: " + packetRecieved + Environment.NewLine
                                                 + printSeparator ;
                            txtBoxPrint.SelectionStart = txtBoxPrint.TextLength;
                            txtBoxPrint.ScrollToCaret();
                            txtBoxPrint.Refresh();
                            int x = 25;
            while (x > 0)
            {
                Application.DoEvents();
                

                Thread.Sleep(20);
                x--;
            }
                            continue;

                        case IPStatus.DestinationHostUnreachable:
                            packetLost++;
                            IPAddress[] addresslist = Dns.GetHostAddresses(txtBoxIP.Text);
                            txtBoxPrint.Text += "   IP address: " + addresslist[0].ToString() + Environment.NewLine +
                                "   Request timed out." + Environment.NewLine +
                                "   Lost packets: " + packetLost + Environment.NewLine +
                                "   Recived packets: " + packetRecieved + Environment.NewLine +
                                printSeparator+Environment.NewLine;
                            txtBoxPrint.SelectionStart = txtBoxPrint.TextLength;
                            txtBoxPrint.ScrollToCaret();
                            txtBoxPrint.Refresh();
                            continue;



                        default:
                            packetLost++;
                            txtBoxPrint.Text += "Ping failed. " + Environment.NewLine + printSeparator + Environment.NewLine;
                            txtBoxPrint.Text += "   Lost packets: " + packetLost + Environment.NewLine +
                                                "   Recived packets: " + packetRecieved + Environment.NewLine + printSeparator + Environment.NewLine;
                            txtBoxPrint.SelectionStart = txtBoxPrint.TextLength;
                            txtBoxPrint.ScrollToCaret();
                            txtBoxPrint.Refresh();
                            break;

                    }



                }
                catch (PingException e)
                {
                    
                    txtBoxPrint.Text = "Host unreachable or no valid address.";
                    break;
                }
                catch (ArgumentNullException ex)
                {
                    ex = null;
                }

            }
        }



        public void startPingLoop()
        {
            
            options.Ttl = ttl;


            resetPrint();


            int x = 25;
            while (x > 0)
            {
                Application.DoEvents();
                Thread.Sleep(10);
                x--;
            }
                try
                {

                    reply = pingSender.Send(txtBoxIP.Text, timeout, buffer, options);
                
                    switch (reply.Status)
                    {
                        case IPStatus.Success:

                            packetRecieved++;
                            print.Append("  Address: " + reply.Address.ToString() + Environment.NewLine);
                            print.Append("  RoundTrip time: " + reply.RoundtripTime + Environment.NewLine);
                            print.Append("  Time to live: " + this.getTtl() + Environment.NewLine);
                            print.Append("  Don't fragment: false" + Environment.NewLine);
                            print.Append("  Buffer size: " + reply.Buffer.Length + Environment.NewLine);
                            print.Append("  Itearation: " + (packetRecieved+packetLost) + Environment.NewLine);
                            print.Append("  Lost packets: " + packetLost + Environment.NewLine +
                                         "  Recived packets: " + packetRecieved + Environment.NewLine);
                            print.Append(printSeparator);

                            txtBoxPrint.AppendText(print.ToString());

                            txtBoxPrint.SelectionStart = txtBoxPrint.TextLength;
                            txtBoxPrint.ScrollToCaret();
                            txtBoxPrint.Refresh();

                            break;




                        case IPStatus.TimedOut:
                            packetLost++;
                            txtBoxPrint.Text += "   Connection timed out." + Environment.NewLine;
                            txtBoxPrint.Text += "   Lost packets: " + packetLost + Environment.NewLine +
                                                "   Recived packets: " + packetRecieved + Environment.NewLine+printSeparator;
                            txtBoxPrint.SelectionStart = txtBoxPrint.TextLength;
                            txtBoxPrint.ScrollToCaret();
                            txtBoxPrint.Refresh();
                            break;

                        case IPStatus.DestinationHostUnreachable:
                            packetLost++;
                            IPAddress[] addresslist = Dns.GetHostAddresses(txtBoxIP.Text);
                            txtBoxPrint.Text += "   IP address: " + addresslist[0].ToString() + Environment.NewLine +
                                "   Request timed out." + Environment.NewLine +
                                "   Lost packets: " + packetLost + Environment.NewLine +
                                "   Recived packets: " + packetRecieved + Environment.NewLine +
                                printSeparator + Environment.NewLine;
                            txtBoxPrint.SelectionStart = txtBoxPrint.TextLength;
                            txtBoxPrint.ScrollToCaret();
                            txtBoxPrint.Refresh();
                            break;



                        default:
                            packetLost++;
                            txtBoxPrint.Text += "Ping failed. " + Environment.NewLine + printSeparator + Environment.NewLine;
                            txtBoxPrint.Text += "   Lost packets: " + packetLost + Environment.NewLine +
                                                "   Recived packets: " + packetRecieved + Environment.NewLine + printSeparator + Environment.NewLine;
                            txtBoxPrint.SelectionStart = txtBoxPrint.TextLength;
                            txtBoxPrint.ScrollToCaret();
                            txtBoxPrint.Refresh();
                            break;

                    }



                }
                catch (PingException e)
                {
                    txtBoxPrint.Text = "Host unreachable or no valid address.";
                    
                }

            
        }



        public void setRepeat(bool repeat)
        {
            this.repeat = repeat;

        }

        public bool getRepeat()
        {

           return this.repeat;

        }

        public void setTxtBoxIP(TextBox txtBoxIP)
        {
            this.txtBoxIP = txtBoxIP;
        }

        public TextBox getTxtBoxIP()
        {
            return this.txtBoxIP;
        }


        public void setTxtBoxPrint(TextBox txtBoxPrint)
        {
            this.txtBoxPrint = txtBoxPrint;
        }

        public TextBox getTxtBoxPrint()
        {
            return this.txtBoxPrint;
        }

        public void setTimes(int times)
        {
            this.times = times;
        }

        public int getTimes()
        {
            return this.times;
        }

        public void setTtl(int ttl)
        {
            this.ttl = ttl;
        }

        public int getTtl()
        {
            return this.ttl;
        }

        public void resetPrint()
        {
            print.Clear();
        }

        public void resetPacketRecived()
        {
            packetRecieved = 0;
        }

        public void resetPacketsLost()
        {
            packetLost = 0;
        }


        public void setTimeout(int timeout)
        {
            this.timeout = timeout;
        }

        public int getTimeout()
        {
            return this.timeout;
        }

        public void setCheck(bool check)
        {
            this.check = check;

        }

        public bool getCheck()
        {
            return this.check;
        }
    }
}
