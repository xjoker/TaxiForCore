using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaxiForCore.StringHelper;
using TaxiForCore.SystemHelper;

namespace TaxiForCore.Network
{
    public class PingResponseType
    {
        private static int send = 0;
        private static int received = 0;
        private static int lost = send - received;
        public long Minimum { get; set; }
        public long Maximum { get; set; }
        public long Average { get; set; }
        public int Sent { get; set; }
        public int Received { get; set; }
        public int Lost { get; }
    }

    public class PortListType
    {

        public List<int> TcpPorts { get; set; }
        public List<int> UdpPorts { get; set; }
    }

    public class NetworkTools
    {
        /// <summary>
        /// PING方法
        /// </summary>
        /// <param name="host">主机</param>
        /// <param name="timeout">超时设定 默认3000</param>
        /// <param name="bufferSize">包尺寸 默认32</param>
        /// <returns></returns>
        public static async Task<PingReply> Ping(string host, int timeout = 3000, int bufferSize = 32)
        {
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions() { DontFragment = true };
            byte[] buffer = Encoding.ASCII.GetBytes(new string('a', bufferSize));
            var reply = await pingSender.SendPingAsync(host, timeout, buffer, options);
            return reply;
        }


        /// <summary>
        /// PING 检测主机是否连通
        /// </summary>
        /// <param name="host">主机</param>
        /// <returns></returns>
        public static bool PingCheck(string host)
        {
            var temp = Ping(host).Result;
            if (temp.Status == IPStatus.Success)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// PING 检测主机延时
        /// 如主机无法连通会返回NULL
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static long? PingDelay(string host)
        {
            var temp = Ping(host).Result;
            if (temp.Status == IPStatus.Success)
            {
                return temp.RoundtripTime;
            }
            return null;
        }

        /// <summary>
        /// PING 检测，可定义次数和间隔
        /// </summary>
        /// <param name="host"></param>
        /// <param name="count"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static PingResponseType PingCheckDetailed(string host, int count = 4, int interval = 1000)
        {
            PingResponseType prt = new PingResponseType();
            long delaySum = 0;
            for (int i = 0; i < count; i++)
            {
                prt.Sent++;
                var temp = Ping(host).Result;
                if (temp.Status == IPStatus.Success)
                {
                    prt.Received++;
                    if (prt.Minimum == 0)
                    {
                        prt.Minimum = temp.RoundtripTime;
                    }
                    if (temp.RoundtripTime > prt.Maximum) prt.Maximum = temp.RoundtripTime;
                    if (temp.RoundtripTime < prt.Minimum) prt.Minimum = temp.RoundtripTime;

                    delaySum = delaySum + temp.RoundtripTime;
                }
                Thread.Sleep(interval);
            }
            prt.Average = delaySum / 4;

            return prt;
        }

        /// <summary>
        /// 域名解析为IP地址
        /// </summary>
        /// <param name="host">主机或IP</param>
        /// <returns>返回IP，如果未查询到则返回NULL</returns>
        public static List<string> DomainToIP(string host)
        {
            var p = Dns.GetHostAddressesAsync(host).Result;
            var ipList = new List<string>();
            if (p.Length > 0)
            {
                foreach (var item in p)
                {
                    ipList.Add(item.ToString());
                }
            }
            return ipList;
        }

        /// <summary>
        /// 获取本机所有网卡的IP地址
        /// </summary>
        /// <returns></returns>
        public static List<string> GetLocalIPs()
        {
            return DomainToIP(Dns.GetHostName());
        }

        /// <summary>
        /// 获取本机所有网卡的MAC地址
        /// </summary>
        /// <returns></returns>
        public static List<string> GetLocalMacs()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            List<string> sMacAddress = new List<string>();
            foreach (NetworkInterface adapter in nics)
            {
                IPInterfaceProperties properties = adapter.GetIPProperties();
                if (!string.IsNullOrWhiteSpace(adapter.GetPhysicalAddress().ToString()))
                {
                    sMacAddress.Add(adapter.GetPhysicalAddress().ToString());
                }
            }
            return sMacAddress;
        }

        /// <summary>
        /// 获取所有在用状态的TCP/UDP端口
        /// </summary>
        /// <returns></returns>
        public static PortListType GetAllUsePort()
        {
            List<int> tcpPorts = new List<int>();
            List<int> udpPorts = new List<int>();

            //获取本地计算机的网络连接和通信统计数据的信息 
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();

            //返回本地计算机上的所有Tcp监听程序 
            IPEndPoint[] ipsTcp = ipGlobalProperties.GetActiveTcpListeners();

            //返回本地计算机上的所有UDP监听程序 
            IPEndPoint[] ipsUdp = ipGlobalProperties.GetActiveUdpListeners();

            foreach (IPEndPoint ep in ipsTcp) tcpPorts.Add(ep.Port);
            foreach (IPEndPoint ep in ipsUdp) udpPorts.Add(ep.Port);

            return new PortListType() { TcpPorts = tcpPorts, UdpPorts = udpPorts };
        }

        /// <summary>
        /// 检测TCP端口是否已被使用
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool CheckTCPPortIsUse(int port)
        {
            if (port >= 65535 && 0>port)
            {
                throw new ArgumentException("Port must in 1~65535");
            }
            var portUsed = GetAllUsePort().TcpPorts;
            return portUsed.Contains(port);
        }


        /// <summary>
        /// 检测UDP端口是否已被使用
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool CheckUDPPortIsUse(int port)
        {
            if (port >= 65535 && 0 > port)
            {
                throw new ArgumentException("Port must in 1~65535");
            }
            var portUsed = GetAllUsePort().UdpPorts;
            return portUsed.Contains(port);
        }
    }
}
