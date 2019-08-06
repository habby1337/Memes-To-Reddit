using Newtonsoft.Json;
using System;

using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static System.Net.Mime.MediaTypeNames;

namespace r_memes_to_telegram
{
    class Program
    {


        class post
        {
            public static string title { get; set; }
            public static string author { get; set; }
            public static string img_url { get; set; }
            public static string post_url { get; set; }
            public static string id { get; set; }

        }

        class setting
        {
            public static int time = 60000;//600000;
            public static int conver = 60000;
        }
        static async Task Main(string[] args)
        {



            Console.WriteLine("Starting foo...");
            Thread.Sleep(1000); // hold console for a second on the screen


            TimerCallback callback = new TimerCallback(TickAsync);

            Console.WriteLine("[INFO] Creating timer: {0}\n",
                               DateTime.Now.ToString("h:mm:ss"));

            // create a one second timer tick
            Timer stateTimer = new Timer(callback, null, 0, setting.time);


            // loop here forever
            for (; ; )
            {
                // add a sleep for 100 mSec to reduce CPU usage
                Thread.Sleep(100);
            }
            Console.ReadKey();



        }
        static public async void TickAsync(Object stateInfo)
        {
            Console.WriteLine("----");
            //Console.WriteLine("Time: {0} Prossimo: {1}", DateTime.Now.ToString("h:mm:ss"), (DateTime.Now.ToString("h:mm:ss") + (setting.time + 60000)));
            Console.WriteLine("[INFO] Time: " + DateTime.Now.ToString("h:mm:ss") + " Prossimo alle: " + (((DateTime.Now.Hour + 11) % 12) + 1) + ":" +
                (DateTime.Now.Minute + (setting.time / setting.conver)) + ":" + DateTime.Now.Second);

            getJson();


        }


        public static void getJson()
        {
            string html;

            using (var client = new WebClient())
            {
                html = client.DownloadString("https://www.reddit.com/r/memes/new.json?sort=new&limit=1"); //Limit == qantità di post nel json
            }
            // html = System.IO.File.ReadAllText("test.json");

            parsJsonAsync(html);
        }

        public static async void parsJsonAsync(string json)
        {
            dynamic din_json = JsonConvert.DeserializeObject(json);

            if (din_json.data.children[0].data.id != post.id)
            {
                post.title = din_json.data.children[0].data.title;
                post.author = din_json.data.children[0].data.author;
                post.img_url = din_json.data.children[0].data.url;
                post.post_url = din_json.data.children[0].data.permalink;
                post.id = din_json.data.children[0].data.id;


                var botClient = new TelegramBotClient(token.key);
                var me = botClient.GetMeAsync().Result;

                //Console.WriteLine($"Bot: {me.Id} - Nome: {me.FirstName}");



                //var t = await botClient.SendTextMessageAsync("@MemeToTelegram", "text message");



                Message message = await botClient.SendPhotoAsync(
                 chatId: "@MemeToTelegram",
                 photo: post.img_url,
                caption: "<b>" + post.title + "</b> - <i>Posted by: " + post.author + "</i> ▪️ <a href=\"https://www.reddit.com" + post.post_url + "\">Reddit</a>",
                parseMode: ParseMode.Html
                );


            }
            else
            {
                Console.WriteLine("[ERROR] Same post, skipping..");
            }




            /*
            Console.WriteLine(din_json);

            Console.WriteLine("\n---------------------------------------\n");
            Console.WriteLine("TITLE: " + post.title);
            Console.WriteLine("AUTHOR: " + post.author);
            Console.WriteLine("IMG URL: " + post.img_url);
            Console.WriteLine("URL: " + post.post_url);
            Console.WriteLine("\n---------------------------------------\n");
            */


        }


    }
}
