using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatApp
{
    internal class Program
    {
        public static int RemotePort { get; set; } //qosulacagimiz port
        public static int LocalPort { get; set; } //bizim port
        public static IPAddress IP { get; set; }

        static void Main(string[] args)
        {

            Console.Title = "ChatApp";
            IP = IPAddress.Parse("192.168.0.131"); //oz ip-mizi yaziriq

            Console.Write("Ozunuzde olan portu daxil edin: ");
            LocalPort = int.Parse(Console.ReadLine());
            Console.Write("Qosulacaginiz portu daxil edin: ");
            RemotePort = int.Parse(Console.ReadLine());

            Task.Factory.StartNew(() =>
            {
                //mesajlari qebul etmek ucun olan method
                Listener();

            }, TaskCreationOptions.LongRunning);

            Console.ForegroundColor = ConsoleColor.Red;

            while (true)
            {
                //mesaj gondermek ucun
                Client(Console.ReadLine());
            }

            Console.ResetColor();


        }

        //asagidaki method udpClient vasitesile parametr olaraq qebul etdiyi datani gonderecek diger terefe
        private static void Client(string? data)
        {
            using(var client=new UdpClient())
            {
                try
                {
                    var ep = new IPEndPoint(IP, RemotePort); //qarsi terefin portu yazilir ve elaqe qurulur
                    var bytes = Encoding.UTF8.GetBytes(data);
                    client.Send(bytes, bytes.Length, ep);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }


        private static void Listener()
        {
            try
            {
                while (true)
                {
                    UdpClient client = new UdpClient(LocalPort);
                    IPEndPoint ep = new IPEndPoint(IP, RemotePort);
                    var response = client.Receive(ref ep);

                    var data = Encoding.UTF8.GetString(response);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Message: " + data);
                    client.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


    }
}
