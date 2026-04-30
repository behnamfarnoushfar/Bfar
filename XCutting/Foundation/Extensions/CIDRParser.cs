using Bfar.XCutting.Abstractions.Entities.Models;
using Bfar.XCutting.Foundation.Constants;
using System.Buffers;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace Bfar.XCutting.Foundation.Extensions
{
    public sealed class CIDRParser
    {
        public static (IPAddress startIp, IPAddress endIp) GetIpRangeFromCidr(string cidr)
        {
            try
            {
                if (!cidr.Contains("/"))
                    cidr = cidr + "/32";
                string[] parts = cidr.Split('/');
                IPAddress ip = IPAddress.Parse(parts[0]);
                int prefixLength = int.Parse(parts[1]);

                uint ipUint = IpToUint(ip);
                uint mask = ~((1u << (32 - prefixLength)) - 1);
                uint startIpUint = ipUint & mask;
                uint endIpUint = startIpUint | ~mask;

                IPAddress startIp = UintToIp(startIpUint);
                IPAddress endIp = UintToIp(endIpUint);

                return (startIp, endIp);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"inavlid address: {cidr} " + ex);
                return (null, null);
            }

        }

        private static uint IpToUint(IPAddress ip)
        {
            byte[] bytes = ip.GetAddressBytes();
            Array.Reverse(bytes); // Convert to big-endian
            return BitConverter.ToUInt32(bytes, 0);
        }

        private static IPAddress UintToIp(uint ipUint)
        {
            byte[] bytes = BitConverter.GetBytes(ipUint);
            Array.Reverse(bytes); // Convert to little-endian
            return new IPAddress(bytes);

        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IPRecord ParseRecord(string ipAddress, string subnetMask)
        {

            var cidr = string.Empty;

            IPAddress ip = IPAddress.Parse(ipAddress);
            IPAddress mask = IPAddress.Parse(subnetMask);

            byte[] ipBytes = ip.GetAddressBytes();
            byte[] maskBytes = mask.GetAddressBytes();

            int subnetBits = 0;
            for (int i = 0; i < 4; i++)
            {
                int xorResult = ipBytes[i] ^ maskBytes[i];
                while (xorResult > 0)
                {
                    xorResult >>= 1;
                    subnetBits++;
                }
            }
            cidr = $"{ipAddress}/{subnetBits}";
            if (subnetMask == "255.255.255.255")
            {
                return this.ParseRecord(cidr) with { LastAddress = ipAddress, FirstAddress = ipAddress };
            }
            uint network = IP2Int(ipAddress) & IP2Int(subnetMask);
            uint broadcast = network + ~IP2Int(subnetMask);
            var _first = Int2IP(network + 1);
            var _last = Int2IP(broadcast - 1);

            return this.ParseRecord(cidr) with { LastAddress = _last, FirstAddress = _first };

        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IPRecord ParseRangeRecord(string start, string end)
        {
            var a = new BitArray(IPAddress.Parse(start).GetAddressBytes());
            var b = new BitArray(IPAddress.Parse(end).GetAddressBytes());
            int bits = 0;
            for (int i = 0; i < a.Length; i++)
                if (a[i] == b[i])
                    bits++;


            return ParseRecord($"{start}/{bits}");
        }



        readonly AddressFamily addressFamily;
        readonly byte[] lowerBytes;
        readonly byte[] upperBytes;

        public List<byte[]> ParseManualRange(string start, string end)
        {
            var parsed = ParseRecord(start);
            var parsed2 = ParseRecord(end);
            var currentAddress = parsed.CurrentAddress;
            byte[] result = parsed.Result;

            List<byte[]> mdl = new List<byte[]>();
            while (true)
            {
                mdl.Add(result);
                if (string.Join(".", result) == end)
                    break;
                if (result.Length == 0)
                    continue;
                result = MemoryPool<byte>.Shared.Rent(4).Memory.Slice(0, 4).ToArray();
                currentAddress.Increment();
                currentAddress.CopyTo(result, 0);
            }
            return mdl;


        }







        static uint IP2Int(string ipAddress)
        {
            string[] elements = ipAddress.Split('.');

            uint ip = 0;
            for (int i = 0; i < 4; i++)
            {
                ip += Convert.ToUInt32(elements[i]) << (24 - 8 * i);
            }
            return ip;
        }

        static string Int2IP(uint ip)
        {
            return $"{(ip >> 24) & 0xFF}.{(ip >> 16) & 0xFF}.{(ip >> 8) & 0xFF}.{ip & 0xFF}";
        }
        public List<byte[]> CreateIPLocation(string rangeIP)
        {
            var parsed = ParseRecord(rangeIP);
            var currentAddress = parsed.CurrentAddress;
            byte[] result = parsed.Result;
            var max = Math.Pow(2, 32 - parsed.HostBits);
            List<byte[]> mdl = new List<byte[]>((int)max);
            for (int ii = 0; ii < max; ii++)
            {
                mdl.Add(result);
                if (result.Length == 0)
                    continue;
                result = MemoryPool<byte>.Shared.Rent(4).Memory.Slice(0, 4).ToArray();
                currentAddress.Increment();
                currentAddress.CopyTo(result, 0);
            }
            return mdl;
        }
        public static string? ToString(in byte[] ipbytes)
        {
            return ipbytes.Length == 0 ? null : $"{ipbytes[0]}.{ipbytes[1]}.{ipbytes[2]}.{ipbytes[3]}";
        }
        /// <summary>
        /// This methods intend to create proper to record for parsing with other methods and not working by itself
        /// </summary>
        /// <param name="cidr"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IPRecord ParseRecord(in string cidr)
        {
            try
            {
                if (!cidr.Contains(StringConstants.Slash))
                {
                    var bytes = IPAddress.Parse(cidr).GetAddressBytes();
                    return new IPRecord() { CurrentAddress = new BitArray(IPAddress.Parse(cidr).GetAddressBytes()), HostBits = 32, Result = bytes };
                }

                var vlmask = cidr.Split(SplitterConstants.SlashSplitter, StringSplitOptions.RemoveEmptyEntries);
                var ipbytes = IPAddress.Parse(vlmask[0]).GetAddressBytes();
                var start = new BitArray(ipbytes);
                byte[] arr = new byte[4];
                var hostBits = Convert.ToInt32(vlmask[1]);
                start.CopyTo(arr, 0);
                var parsedModel = new IPRecord() { CurrentAddress = start, HostBits = hostBits, Result = arr };

                return parsedModel;
            }
            catch (Exception)
            {
                return new IPRecord() { CurrentAddress = new BitArray(0), HostBits = 32, Result = new byte[] { } };
            }

        }
    }
}
