using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace ipScanner
{
    class IPScanner
    {
        public static void Main(string[] args) {

            string ipAddressBase = GetIPAddress();
            string[] ipAddressNums = ipAddressBase.Split('.');

            ipAddressBase = CreateIPTemplate(ipAddressNums);
            int locations = 255;

            for (int i = 1; i <= locations; i++) {
                string ipToCheck = ipAddressBase + (i.ToString());

                Ping ping = new Ping();
                ping.PingCompleted += new PingCompletedEventHandler(ValidateIP);
                ping.SendAsync(ipToCheck, 100, ipToCheck);
            }
        }

        static void ValidateIP(object s, PingCompletedEventArgs arg) {

            string ipToValidate = (string)arg.UserState;
            Console.WriteLine("{0} is up: ({1} ms)", ipToValidate, arg.Reply.RoundtripTime);
        }

        static string CreateIPTemplate(string[] parts) {
            string ip = "";
            for (int i = 0; i < parts.Length-1; i++) {
                ip += parts[i] + ".";
            }
            return ip;
        }

        static string GetIPAddress() {
            IPHostEntry entry;
            string localIP = null;

            entry = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress address in entry.AddressList)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork) localIP = address.ToString();
            }
            return localIP;
        }
    }
}