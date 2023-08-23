using System;
using System.Data.Common;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

using Microsoft.EntityFrameworkCore;
using fwmonitor.DB;
using Microsoft.AspNetCore.Mvc;

namespace fwmonitor
{

    public partial class Program
    {

        static string help = @"Варианты использования             
            import <filename> <comment> - прямой импорт из файла
            download - скачать и импортировать с cdn.fwupd.org 
            www - запустить сервер отчетности
            setup - установить (сбросить) базу данных 
            ";

        //Настройки
        public static Settings S;

        public static void RunWeb()
        {
            string[] args = new string[] { };

            var host = new WebHostBuilder()
            .UseKestrel()
            .UseStartup<Startup>()
            .UseUrls(S.ListenURL)
            .Build();

            host.Run();
        }

        public static void LoadSettings(string filepath)
        {
            try
            {
                System.IO.StreamReader r = new System.IO.StreamReader(filepath);
                string s = r.ReadToEnd();
                r.Close();
                S = Newtonsoft.Json.JsonConvert.DeserializeObject<Settings>(s);
                if (S==null || S.ConnectionString==null)
                    throw new Exception("");
            }
            catch (Exception e)
            {

                Console.WriteLine("Ошибка загрузки настроек " + e.Message + " , используем значения по умолчанию");
                S= new Settings();
                S.ListenURL="http://localhost:7001/";
                S.ConnectionString="Host=localhost;Database=dbfwmon;Username=dbfwmon;Password=1";
                S.Show2Console=true;
                S.LogFile="fwmonitor_log.txt";

                string s1 = Newtonsoft.Json.JsonConvert.SerializeObject(S);
                System.IO.File.WriteAllText(filepath, s1, System.Text.Encoding.UTF8);
            }
        }

        public static void Main(string[] args)
        {
            S= new Settings();
            //Загрузить настройки
            if (!Directory.Exists("config"))
                Directory.CreateDirectory("config");
            String hn = Dns.GetHostName();
            Console.WriteLine("Запускаем на "+hn+", загрузка настроек из "+"config/"+hn);
            LoadSettings("config/"+hn+".json");

            if (args.Length>0)
            {

                if (args[0]=="setup")
                {
                    var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                    var a = assembly.GetManifestResourceNames();
                    foreach (var a2 in a)
                    {
                        Console.WriteLine(a2);
                    }

                    Stream resource = assembly.GetManifestResourceStream("fwmonitor.fwmDB.db.sql");
                    string r = new StreamReader(resource).ReadToEnd();
                    try
                    {
                        var db1 = DB;
                        db1.Database.ExecuteSqlRaw(r);
                        Console.WriteLine("Структура импортирована");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Ошибка импорта структуры:"+ e.Message);
                    }
                    return;
                }

                if (args[0]=="www")
                {
                    Log.L("Запущен сервер отчетности");
                    RunWeb();
                    return;
                }

                Uhistory u = new Uhistory();
                if (args[0]=="import")
                {
                    if (args.Length<3)
                    {
                        Console.WriteLine(help);
                        return;
                    }
                    else
                    {
                        if (!File.Exists(args[1]))
                        {
                            Console.WriteLine(args[1]+" не существует");
                            return;
                        }
                        u.Ipavailable=new string[] { "local file" };
                        Parser.ImportFile(args[1], args[2], u);
                    }

                }

                if (args[0]=="download")
                {

                    var dni = Dns.GetHostAddresses("cdn.fwupd.org");
                    //int i=1;
                    string dt = DateTime.Now.ToString("yyyyMMdd");
                    string fn = "Data/"+dt+".xml.gz";
                    string fnu = "Data/"+dt+".xml";
                    List<string> ips = new List<string>();
                    foreach (var dni2 in dni)
                    {
                        //Console.WriteLine(i+"\t"+dni2.ToString());
                        ips.Add(dni2.ToString());
                    }
                    using (var client = new WebClient())
                    {
                        if (!Directory.Exists("Data"))
                        {
                            Directory.CreateDirectory("Data");
                        }

                        if (!File.Exists(fn))
                        {
                            Log.L("Загружаем");
                            client.DownloadFile("https://cdn.fwupd.org/downloads/firmware.xml.gz", fn);

                        }

                        Log.L("Распаковываем "+fn);
                        if (!File.Exists(fnu))
                        {
                            //unzip
                            byte[] file = File.ReadAllBytes(fn);
                            byte[] decompressed = Utils.Decompress(file);
                            File.WriteAllBytes(fnu, decompressed);

                        }
                        Log.L("Импортируем "+fnu);
                        // Импортируем                        
                        u.Ipavailable=ips.ToArray();
                        string r = Parser.ImportFile(fnu, "from www", u);

                    } // webclient
                    Log.L("Работа завершена");
                    return;
                }

            }
            else
            {
                Console.WriteLine(help);
                return;
            }

        }
        public static fwmonitor.DB.uiContext DB
        {
            get
            {
                DbContextOptionsBuilder<fwmonitor.DB.uiContext> options2 = new DbContextOptionsBuilder<fwmonitor.DB.uiContext>();
                options2.UseNpgsql(S.ConnectionString);//"Host="+S.Config.PGHost+";Database="+S.Config.PGDB+";Username="+S.Config.PGUsername+";Password="+S.Config.PGPassword);                                                                                                                     
                return new fwmonitor.DB.uiContext(options2.Options);
            }
        }
    }

    public static class Utils
    {


        public static double DateTimeToUnixTimestamp(DateTime dateTime)
        {
            DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            long unixTimeStampInTicks = (dateTime.ToUniversalTime() - unixStart).Ticks;
            return (double)unixTimeStampInTicks / TimeSpan.TicksPerSecond;
        }
        public static DateTime UnixTimestampToDateTime(double unixTime)
        {
            //DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Unspecified);
            long unixTimeStampInTicks = (long)(unixTime * TimeSpan.TicksPerSecond);
            return new DateTime(unixStart.Ticks + unixTimeStampInTicks, System.DateTimeKind.Unspecified);
        }

        public static string GetFileNameFromUrl(string url)
        {
            Uri uri;
            if (!Uri.TryCreate(url, UriKind.Absolute, out uri))
                uri = new Uri(url);

            return Path.GetFileName(uri.LocalPath);
        }

        public static byte[] Decompress(byte[] gzip)
        {
            using (GZipStream stream = new GZipStream(new MemoryStream(gzip), CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
        }
    }
}

