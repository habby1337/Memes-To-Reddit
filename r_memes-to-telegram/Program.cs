using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        class Posts
        {
            //private data members
            private string title;
            private string id;
            private string author;
            private string permalink;
            private string url;
            private string is_video;

            //method to set  details
            public Posts(string title, string author, string url, string permalink, string id, string is_video)
            {
                this.title = title;
                this.id = id;
                this.author = author;
                this.permalink = permalink;
                this.url = url;
                this.is_video = is_video;
            }

            //method to print student details
            public async void printInfoAsync()
            {
               /* Console.WriteLine
                 (
                 "Title: " + title
                 + "\nAuthor: " + author
                 + "\nId: " + id
                 + "\nUrl: " + url
                 + "\nPermaLink: " + permalink
                 + "\nIs Video: " + is_video
                 + "\n---------------"
                 );*/

                var botClient = new TelegramBotClient(token.key);
                var me = botClient.GetMeAsync().Result;

                Message message = await botClient.SendPhotoAsync(
                 chatId: "@MemeToTelegram",
                 photo: url,
                caption: "<b>" + title + "</b> - <i>Posted by: " + author + "</i> ▪️ <a href=\"https://www.reddit.com" + permalink + "\">Reddit</a>",
                parseMode: ParseMode.Html
                );


            }

        }



        class setting
        {
            public static int time = 1; 
            public static int Npost = 3; //Numero di post
        }
        static async Task Main(string[] args)
        {



            Console.WriteLine("Starting foo...");
            Thread.Sleep(1000); // hold console for a second on the screen


            TimerCallback callback = new TimerCallback(TickAsync);

            Console.WriteLine("[INFO] Creating timer: {0}\n",
                               DateTime.Now.ToString("h:mm:ss"));

            // create a one second timer tick
            Timer stateTimer = new Timer(callback, null, 0, (int)TimeSpan.FromHours(setting.time).TotalMilliseconds);


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
            Console.WriteLine("[INFO] Time: " + DateTime.Now.ToString("h:mm:ss") 
                + " Prossimo alle: " + 
                (((DateTime.Now.Hour + 11) % 12) + 1 + setting.time) 
                + ":" +
                DateTime.Now.Minute 
                + ":" +
                DateTime.Now.Second);

            getJson();


        }


        public static void getJson()
        {
            string html;

             using (var client = new WebClient())
             {
                 html = client.DownloadString("https://www.reddit.com/r/memes/new.json?sort=new&limit=" + setting.Npost); //Limit == qantità di post nel json
             }
            //html = System.IO.File.ReadAllText("test.json");

            parsJsonAsync(html);
        }

        public static async void parsJsonAsync(string json)
        {
            dynamic din_json = JsonConvert.DeserializeObject(json);


           /* for (int i = 0; i < setting.Npost; i++)
            {

                Console.WriteLine(
                    "\n##############\n\nPost [" + i + "]: \n-----------"
                    + "\nTitle: " + din_json.data.children[i].data.title
                    + "\nAuthor: " + din_json.data.children[i].data.author
                    + "\nUrl: " + din_json.data.children[i].data.url
                    + "\nPermalink: " + din_json.data.children[i].data.permalink
                    + "\nId: " + din_json.data.children[i].data.id
                    + "\nIsVideo: " + din_json.data.children[i].data.is_video
                    );
            }*/

            List<Posts> post = new List<Posts>();

            for (int i = 0; i < setting.Npost; i++)
            {
                post.Add(
                    new Posts(
                    (string)din_json.data.children[i].data.title,
                    (string)din_json.data.children[i].data.author,
                    (string)din_json.data.children[i].data.url,
                    (string)din_json.data.children[i].data.permalink,
                    (string)din_json.data.children[i].data.id,
                    (string)din_json.data.children[i].data.is_video
                    )
                    );
            }


            Console.WriteLine("#-------------------------------------------------#");

            for (int i = 0; i < setting.Npost; i++)
            {
                post[i].printInfoAsync();
            }
        }
    }
}
