using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

	public class NetworkUtilites
	{
        public static string GetBroadcastAddress(string ipAddress, string subnetMask)
        {
            //determines a broadcast address from an ip and subnet
            var ip = IPAddress.Parse(ipAddress);
            var mask = IPAddress.Parse(subnetMask);

            byte[] ipAdressBytes = ip.GetAddressBytes();
            byte[] subnetMaskBytes = mask.GetAddressBytes();

            if (ipAdressBytes.Length != subnetMaskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

            byte[] broadcastAddress = new byte[ipAdressBytes.Length];
            for (int i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte)(ipAdressBytes[i] | (subnetMaskBytes[i] ^ 255));
            }
            return new IPAddress(broadcastAddress).ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string getSubNetMask()
        {
            string SubNetMask = "";
            NetworkInterface[] Interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface Interface in Interfaces)
            {
                if (Interface.NetworkInterfaceType == NetworkInterfaceType.Loopback) continue;
                UnicastIPAddressInformationCollection UnicastIPInfoCol = Interface.GetIPProperties().UnicastAddresses;
                foreach (UnicastIPAddressInformation UnicatIPInfo in UnicastIPInfoCol)
                {
                    if (UnicatIPInfo.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        if (UnicatIPInfo.IPv4Mask != null)
                        { 
                            SubNetMask = UnicatIPInfo.IPv4Mask.ToString();
                        }
                    }                    
                }
            }
            return SubNetMask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string getLocalIPAddress()
        {
            IPHostEntry host;
            string localIP = "?";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                }
            }

            //THIS CODE DOESNT WORK
            //JUST HARD CODE FOR NOW
            //localIP = "192.168.1.186";
            //localIP = "164.111.196.131";
            //localIP = "192.168.1.108";
            localIP = "192.168.1.101";

            return localIP;
        }

	}

