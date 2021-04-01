using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = LoadConfig();
            Random r = new Random();
            var client = new UdpClient();
            long count = 0;
            while (true)
            {
                var randomValue = r.Next(config.MinRange, config.MaxRange);
                byte[] data = BitConverter.GetBytes(randomValue);
                for (int i = 0; i < config.Adresses.Count; i++)
                {
                    client.Send(data, data.Length, config.Adresses[i], 8088);
                    count++;
                    Console.WriteLine($"Всего отправлено {count} значений");
                }
            }
        }

        private static Config LoadConfig()
        {
            Config result;
            XmlSerializer formatter = new XmlSerializer(typeof(Config));
            using (FileStream fs = new FileStream("Config.xml", FileMode.OpenOrCreate))
            {
                result = (Config)formatter.Deserialize(fs);
            }
            return result;
        }


        private static void WriteXmlFile()
        {
            var config = new Config
            {
                MinRange = 0,
                MaxRange = 10,
                Adresses = new List<string> { "192.168.10.1", "192.168.11.1", "192.168.12.1" }
            };

            XmlSerializer formatter = new XmlSerializer(typeof(Config));
            using (var fs = new FileStream("Config.xml", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, config);
            }
        }
    }
}
