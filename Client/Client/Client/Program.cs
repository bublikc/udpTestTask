using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static Calculator calculator;
        static CancellationTokenSource cts;
        static object locker = new object();
        static void Main(string[] args)
        {
            calculator = new Calculator();
            cts = new CancellationTokenSource();
            Task receiver = Task.Factory.StartNew(() => Receive(cts.Token), cts.Token);
            while (Console.ReadKey().Key != ConsoleKey.Escape)
            {
                lock (locker)
                    PrintCalc();
            }
            cts.Cancel();
            Console.WriteLine("Получатель остановлен");
        }

        static void PrintCalc()
        {
            Console.Clear();

            Stopwatch stp = new Stopwatch();
            stp.Start();

            Console.WriteLine($"Всего значений: {calculator.Count}");

            var average = Task.Factory.StartNew(() => { return calculator.CalcAverage(); });
            Console.WriteLine($"Среднее значение: {average.Result}");

            var mode = Task.Factory.StartNew(() => { return calculator.CalcMode(); });
            Console.WriteLine($"Мода / моды: {string.Join("; ", mode.Result)}");

            var median = Task.Factory.StartNew(() => { return calculator.CalcMedian(); });
            Console.WriteLine($"Медиана: {median.Result}");

            var squaredDeviation = Task.Factory.StartNew(() => { return calculator.SquaredDeviation(); });
            Console.WriteLine($"Среднее квадратичное отклонение: {squaredDeviation.Result}");

            stp.Stop();
            Console.WriteLine($"Подсчитано за {stp.Elapsed}");
        }

        static void Receive(CancellationToken cancellationToken)
        {
            var client = new UdpClient(8088);
            IPEndPoint remoteIp = null;
            while (!cancellationToken.IsCancellationRequested)
            {
                var data = client.Receive(ref remoteIp);
                var value = BitConverter.ToInt32(data, 0);
                Console.WriteLine($"Getting value {value}");
                lock (locker)
                    calculator.Add(value);
            }
        }
    }
}
