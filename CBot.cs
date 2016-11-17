using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using DiscordSharp;
using Discord;
using Discord.Audio;
using Discord.Commands;
using System.Text.RegularExpressions;
using System.Threading;

namespace C
{
    
    class C
    {
        public static Discord.DiscordClient bot;
        public static List<String> deletedmessages;
        public static List<MessageEventArgs> storedmessages;
        public static bool animate = false;
        public static Thread animator;
        public static MessageEventArgs animatedmessage;
        public static int messagelength;
        public static bool add = true;

        public static void Battleship(MessageEventArgs e)
        {
            if (BattleShip.battleship != null && BattleShip.battleship.playing)
            {
                if (BattleShip.battleship.awaiting)
                {
                    if (e.User.Name.StartsWith("CandyBot"))
                    {
                        BattleShip.battleship.previous = e;
                        BattleShip.battleship.awaiting = false;
                    }
                }
                else if (BattleShip.battleship.awaitingx1)
                {
                    BattleShip.battleship.previousx1 = e;
                    BattleShip.battleship.awaitingx1 = false;
                }
                else if (BattleShip.battleship.awaitingx2)
                {
                    BattleShip.battleship.previousx2 = e;
                    BattleShip.battleship.awaitingx2 = false;
                }
                if (e.Message.Text.StartsWith("/teams"))
                {
                    BattleShip.MakeTeams(e);
                }
                if (e.Message.Text.StartsWith("/test "))
                {
                    BattleShip.Main(e, 0);
                }
                else if (e.Message.Text.StartsWith("/ready"))
                {
                    BattleShip.GetReady(e);
                }
                else if (e.Message.Text.StartsWith("/attack "))
                {
                    BattleShip.Main(e, 1);
                }
                else if (e.Message.Text.StartsWith("/join"))
                {
                    BattleShip.AddPlayer(e);
                }
                else if (e.Message.Text.StartsWith("/start"))
                {
                    BattleShip.StartGame(e);
                }
                else if (e.Message.Text.StartsWith("/leave"))
                {
                    BattleShip.LeaveList(e);
                }
            }
            else if (e.Message.Text.StartsWith("/test"))
            {
                BattleShip.First(e, bot);
            }
        }

        public static void Urban(MessageEventArgs e)
        {
            string[] searchedWords = e.Message.Text.Replace("/urban ", "").Split(';');
            using (WebClient client = new WebClient())
            {
                string htmlCode = client.DownloadString("http://www.urbandictionary.com/define.php?term=" + searchedWords[0].Replace(" ", "+"));  //Raw html code
                int z = 0;
                int max;
                e.Channel.SendMessage("http://www.urbandictionary.com/define.php?term=" + searchedWords[0].Replace(" ", "+"));
            }
        }

        public static void Youtube(MessageEventArgs e)
        {
            string[] searchedWords = e.Message.Text.Replace("/youtube ", "").Split(';');
            using (WebClient client = new WebClient())
            {
                string htmlCode = client.DownloadString("https://www.youtube.com/results?search_query=" + searchedWords[0].Replace(" ", "+"));  //Raw html code
                string[] htmlCodeLines = htmlCode.Split(new string[1] { "\n" }, StringSplitOptions.None);   //Array of every lines of the html code
                List<string> availableGifs = new List<string>();
                //Retrieve every availabe gifs urls in htmlCodeLines
                int z = 0;
                int max;
                if (searchedWords.Length == 2)
                    max = Convert.ToInt32(searchedWords[1]);
                else
                    max = 1;
                foreach (string line in htmlCodeLines)
                {
                    if (line.Contains("data-context-item-id="))
                    {
                        string temp = line.Substring(line.IndexOf("data-context-item-id="), 34).Remove(0, 21).Replace("\"", "");
                        if (!temp.Contains("vid"))
                        {
                            e.Channel.SendMessage("https://www.youtube.com/watch?v=" + temp);
                            z++;
                        }
                    }
                    if (z >= max)
                        break;
                }
            }
        }

        public static void NadekoReplies(MessageEventArgs e)
        {
            if (e.Message.Text.Equals("/o/") && e.User.Name != "Nadeko" && e.User.Name != "CandyBot")
            {
                e.Channel.SendMessage("\\o\\");
            }
            else if (e.Message.Text.Equals("\\o\\") && e.User.Name != "Nadeko" && e.User.Name != "CandyBot")
            {
                e.Channel.SendMessage("/o/");
            }
            else if (e.Message.Text.Equals("e"))
            {
                e.Channel.SendMessage(e.User.Mention + " did it 😒 🔫");
            }
            else if (e.Message.Text.StartsWith("e "))
            {
                bool b = false;
                foreach (User a in e.Server.FindUsers(e.Message.Text.Remove(0, 2)))
                {
                    if (a != null && a.Name != null && !a.Name.Equals(string.Empty) && a.Name.Length >= 1)
                    {
                        e.Channel.SendMessage(a.Mention + " did it 😒 🔫");
                        b = true;
                    }
                }
                if (!b)
                {
                    e.Channel.SendMessage(e.Message.Text.Remove(0, 2) + " did it 😒 🔫");
                }
            }
            else if (e.Message.Text.StartsWith("/quote "))
            {
                String tempm = e.Message.Text.Replace("/quote ", "");
                String[] temps = tempm.Split(' ');
                String tempn = temps[0];
                String message = tempm.Replace(temps[0] + " ", "");
                Console.WriteLine("Name: " + tempn + ", Message: " + message);
                foreach (User a in e.Server.FindUsers(tempn))
                {
                    if (a != null && a.Name != null && !a.Name.Equals(string.Empty) && a.Name.Length >= 1)
                    {
                        e.Message.Delete();
                        e.Channel.SendMessage("`" + a.Name + " - Today\n" + message + "`");
                    }
                }
            }
        }

        public static void Tumblr(MessageEventArgs e)
        {
            string searchedWords = e.Message.Text.Replace("/tumblr ", "").Replace(' ', '+');
            using (WebClient client = new WebClient())
            {
                string htmlCode = client.DownloadString("https://www.tumblr.com/search/" + searchedWords);  //Raw html code
                string[] htmlCodeLines = htmlCode.Split(new string[1] { "\n" }, StringSplitOptions.None);   //Array of every lines of the html code
                List<string> availableGifs = new List<string>();
                //Retrieve every availabe gifs urls in htmlCodeLines
                foreach (string line in htmlCodeLines)
                {
                    if (line.Contains(".gif") && line.Contains("src"))
                    {
                        int start = line.IndexOf('"') + 1;
                        int end = line.IndexOf('"', start);
                        string gifUrl = line.Substring(start, end - start);
                        if (gifUrl.StartsWith("http"))    //Make sure our url is really an url
                        {
                            availableGifs.Add(gifUrl);
                        }
                    }
                }
                //Get a random gif
                if (availableGifs.Count > 0)
                {
                    Random r = new Random();
                    e.Channel.SendMessage(availableGifs[r.Next(0, availableGifs.Count)]);
                    e.Message.Delete();	//Delete the command message (may be needed to make this more global instead of adding it in every functions)
                }
            }
        }

        public static async void Osu(MessageEventArgs e)
        {
            if (e.Message.Text.StartsWith("/osu"))
            {
                string[] searchedWords = e.Message.Text.Replace("/osu ", "").Split(';');
                using (WebClient client = new WebClient())
                {
                    string htmlCode = await client.DownloadStringTaskAsync("https://osu.ppy.sh/api/get_user?u=" + searchedWords[0].Replace(" ", "+") + "&k=08a1c6b90e680de2bd2609a210bbc758e4d3d0da");  //Raw html code
                    while (client.IsBusy)
                    {
                        Console.WriteLine("wait");
                    }
                    string[] htmlCodeLines = htmlCode.Split(new string[1] { "\n" }, StringSplitOptions.None);   //Array of every lines of the html code
                    List<string> availableGifs = new List<string>();
                    //Retrieve every availabe gifs urls in htmlCodeLines
                    int z = 0;
                    if (htmlCode.Length < 20)
                    {
                        e.Channel.SendMessage("User doesn't exist");
                    }
                    else
                        foreach (string line in htmlCodeLines)
                    {
                        e.Channel.SendMessage("https://osu.ppy.sh/u/" + searchedWords[0].Replace(" ", "+"));
                        //Console.WriteLine(line);
                        String temp = line.Substring(line.IndexOf("pp_raw"), line.IndexOf("accuracy") - line.IndexOf("pp_raw")).Replace("pp_raw", "").Remove(0, 3);
                            e.Channel.SendMessage("pp: " + temp.Remove(temp.Length - 5));
                            temp = line.Substring(line.IndexOf("level"), line.IndexOf("pp_raw") - line.IndexOf("level")).Replace("level", "").Remove(0, 3);
                            e.Channel.SendMessage("level: " + temp.Remove(temp.Length - 4));
                    }
                }
            }
        }

        public static async void Animation()
        {
            while (animate)
            {
                if (animatedmessage != null)
                {
                    Console.WriteLine(animatedmessage.Message.Text);
                    Random r = new Random();
                    if (animatedmessage.Message.Text.Length > messagelength + 50)
                    {
                        add = false;
                    }
                    if (animatedmessage.Message.Text.Length < messagelength + 10)
                    {
                        add = true;
                    }
                    if (add)
                    {
                        int z = r.Next(0, 16);
                        if (z < 3)
                        {
                            await animatedmessage.Message.Edit(animatedmessage.Message.Text);
                        }
                        else if (z < 8)
                        {
                            await animatedmessage.Message.Edit("*" + animatedmessage.Message.Text + "*");
                        }
                        else if (z < 13)
                        {
                            await animatedmessage.Message.Edit("_" + animatedmessage.Message.Text + "_");
                        }
                        else if (z < 14)
                        {
                            await animatedmessage.Message.Edit("`" + animatedmessage.Message.Text + "`");
                        }
                        else if (z < 15)
                        {
                            await animatedmessage.Message.Edit(" " + animatedmessage.Message.Text + " ");
                        }
                        else
                        {
                            await animatedmessage.Message.Edit("~~" + animatedmessage.Message.Text + "~~");
                        }
                    }
                    else
                    {
                        int z = r.Next(2, 5);
                        while (z % 2 != 0)
                        {
                            z = r.Next(0, 8);
                        }
                        string temp = animatedmessage.Message.Text.Substring(z);
                        try
                        {
                            await animatedmessage.Message.Edit(temp.Remove(temp.Length - z));
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("z: " + z, ", " + e.Message);
                        }
                    }
                    /*
                    if (animatedmessage.Message.Text.Length == messagelength + 4)
                    {
                        add = false;
                    }
                    if (animatedmessage.Message.Text.Length == messagelength)
                    {
                        add = true;
                    }
                    if (add)
                    {
                        await animatedmessage.Message.Edit(animatedmessage.Message.Text + " ~");
                    }
                    else
                    {
                        await animatedmessage.Message.Edit(animatedmessage.Message.Text.Remove(animatedmessage.Message.Text.Length - 2, 2));
                    }*/
                }
                Thread.Sleep(1000);
            }
        }
        

        static void Main(string[] args)
        {
            bot = new Discord.DiscordClient();
            deletedmessages = new List<String>();
            storedmessages = new List<MessageEventArgs>();

            bot.MessageReceived += bot_MessageReceived;
            bot.MessageDeleted += bot_MessageDeleted;
            bot.UserJoined += bot_UserJoined;
            bot.LoggedIn += bot_LoggedIn;
            //bot.Connect("");
            bot.Wait(); 
        }

        static void bot_LoggedIn(object sender, EventArgs e)
        {/*
            foreach (Server a in bot.Servers)
            {
                a.DefaultChannel.SendMessage("Welcome Back lel-chan!");
            }*/
        }

        static void bot_MessageReceived(object sender, MessageEventArgs e)
        {
            //Console.WriteLine(e.User.Name);
            NadekoReplies(e);
            Osu(e);
            //Battleship(e);
            if (e.Message.IsMentioningMe())
            {
                if(e.Message.Text.Replace("@CandyBot ", "").ToLower().StartsWith("type"))
                {
                    e.Channel.SendMessage(e.Message.Text.Replace("@CandyBot ", "").Remove(0, 5));
                }
            }
            if (e.Message.Text.ToLower().StartsWith("/animate"))
            {
                if (!animate)
                {
                    animator = new Thread(new ThreadStart(Animation));
                    animator.Start();
                    animate = true;
                }
                else
                {
                    animator.Abort();
                    animate = false;
                }
                e.Message.Delete();
            }
            if (animate && animator.IsAlive)
            {
                if (e.User.Id == bot.CurrentUser.Id)
                if (!e.Message.Text.ToLower().StartsWith("/animate"))
                {
                        Console.WriteLine(e.Message.Text);
                        if (animatedmessage != null)
                            animatedmessage.Message.Delete();
                        animatedmessage = e;
                        messagelength = animatedmessage.Message.Text.Length;
                }
            }
            if (e.Message.Text.ToLower().StartsWith("/youtube"))
            {
                Youtube(e);
            }
            else if (e.Message.Text.ToLower().StartsWith("/urban"))
            {
                Urban(e);
            }
            else if (e.Message.Text.StartsWith("Candybot can you help "))
            {
                String temp = e.Message.Text.Replace("Candybot can you help ", "");
                String[] texts = temp.Split(' ');
                if (temp.Replace(texts[0], "").ToLower() == " with something")
                {
                    e.Channel.SendMessage("Ok, what does " + texts[0] + " need help with?");
                }
            }
            else if (e.Message.Text.StartsWith("/pm "))
            {
                String temp = e.Message.Text.Replace("/pm ", "");
                String[] texts = temp.Split(' ');
                foreach (User u in e.Channel.Users)
                {
                    if (u.Name.StartsWith(texts[0]))
                    {
                        u.SendMessage("hehe");
                    }
                }
            }
            if (e.Message.Text.Equals("/help"))
            {
                e.Channel.SendMessage("Made by lel-chan \nWith code from various others, including Kraby and Plat");
            }
            else if (e.Message.Text.StartsWith("/roles "))
            {
                foreach (User u in e.Channel.Users)
                {
                    if (u.Name.StartsWith(e.Message.Text.Replace("/roles ", "")))
                    {
                        String temp = "`";
                        foreach (Role r in u.Roles)
                        {
                            temp += r.Name + "\n";
                        }
                        e.Channel.SendMessage(temp + "`");
                        break;
                    }
                }
            }
            if (e.Message.Text.StartsWith("/tumblr"))
            {
                Tumblr(e);
            }
        }

        static void bot_UserJoined(object sender, UserEventArgs e)
        {
            //e.Server.DefaultChannel.SendMessage("Welcome, " + e.User.Name + " To My Server");
        }

        static void bot_MessageDeleted(object sender, MessageEventArgs e)
        {
        }

        
    }




    //=================================BATTLESHIP=================================BATTLESHIP=================================
    //=================================BATTLESHIP=================================BATTLESHIP=================================
    //=================================BATTLESHIP=================================BATTLESHIP=================================
    //=================================BATTLESHIP=================================BATTLESHIP=================================
    //=================================BATTLESHIP=================================BATTLESHIP=================================
    //=================================BATTLESHIP=================================BATTLESHIP=================================
    //=================================BATTLESHIP=================================BATTLESHIP=================================
    //=================================BATTLESHIP=================================BATTLESHIP=================================
    //=================================BATTLESHIP=================================BATTLESHIP=================================
    //=================================BATTLESHIP=================================BATTLESHIP=================================




    public class BattleShip
    {
        public static Discord.DiscordClient bot;
        public static BattleShip battleship;
        public int[,] team1;
        public int[,] team2;
        public string[] letters;
        public bool playing;
        public MessageEventArgs previous;
        public MessageEventArgs previousx1;
        public MessageEventArgs previousx2;
        public bool awaiting;
        public bool awaitingx1;
        public bool awaitingx2;
        public static Channel x1 = null;
        public static Channel x2 = null;
        public static Channel x3 = null;
        public bool start = false;
        public List<User> users;
        public List<User> teams1;
        public List<User> teams2;
        public bool madeteams = false;
        public bool attackready = false;
        public int leaderattacksready = 0;

        public BattleShip()
        {
            awaiting = false;
            awaitingx1 = false;
            awaitingx2 = false;
            team1 = new int[16, 16];
            team2 = new int[16, 16];
            letters = new string[16] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P" };

            for (int z = 0; z < 16; z++)
            {
                for (int zed = 0; zed < 16; zed++)
                {
                    team1[zed, z] = 0;
                    team2[zed, z] = 0;
                }
            }
        }

        public static void LeaveList(MessageEventArgs e)
        {
            battleship.users.Remove(e.User);
        }

        public static void First(MessageEventArgs e, Discord.DiscordClient b)
        {
            bot = b;
            battleship = new BattleShip();
            battleship.playing = true;
            battleship.users = new List<User>();
            battleship.teams1 = new List<User>();
            battleship.teams2 = new List<User>();
            e.Channel.SendMessage("Type /join if you want to play");
        }

        public static async void AddPlayer(MessageEventArgs e)
        {
            foreach (Role r in e.User.Roles)
            {
                if (r.Name == "Team 1" || r.Name == "Team 2" || r.Name.ToLower() == "leader")
                {
                    await e.User.RemoveRoles(r);
                    await e.User.RemoveRoles(r);
                    await e.User.RemoveRoles(r);
                }
            }
            if (!battleship.users.Contains(e.User))
            {
                battleship.users.Add(e.User);
                e.Channel.SendMessage("Added " + e.User.Name);
            }
            battleship.madeteams = false;
        }

        public static void GetReady(MessageEventArgs e)
        {
            int temp = WhichUser(e);
            if (temp < 3)
            {
                e.Channel.SendMessage("Only the leaders can use this command.");
            }
            else if (e.Channel.Name.ToLower() != "attacks")
            {
                e.Channel.SendMessage("Use this command in the attacks text channel.");
            }
            else if (battleship.attackready == false && (temp == 3 || temp == 4))
            {
                if (battleship.leaderattacksready == 0)
                {
                    battleship.leaderattacksready = temp;
                }
                else if (battleship.leaderattacksready == temp)
                {
                    e.Channel.SendMessage("Waiting for the other team.");
                }
                else
                {
                    battleship.leaderattacksready = 0;
                    battleship.attackready = true;
                }
            }
            else if (battleship.attackready)
                e.Channel.SendMessage("hehe");
        }

        public static void StartGame(MessageEventArgs e)
        {
            if (battleship.madeteams)
            {
                Display(x1);
                Display(x2);
                Display(x3);
            }
            else
            {
                e.Channel.SendMessage("Make teams first.");
            }
        }

        public static async void MakeTeams(MessageEventArgs e)
        {
            Role r1 = null;
            Role r2 = null;
            Role r3 = null;
            roles(e);
            Console.WriteLine(battleship.users.Count);
            foreach (Role rl in e.Server.Roles)
            {
                if (rl.Name == "Team 1")
                {
                    r1 = rl;
                }
                else if (rl.Name == "Team 2")
                {
                    r2 = rl;
                }
                else if (rl.Name == "Leader")
                {
                    r3 = rl;
                }
            }
            if (battleship.users.Count < 2)
            {
                e.Channel.SendMessage("Not enough players.");
            }
            else if (battleship.users.Count % 2 != 0)
            {
                e.Channel.SendMessage("Teams are odd, waiting for 1 person to join or leave.");
            }
            else
            {
                String team1temp = "";
                String team2temp = "";
                String leader1 = "";
                String leader2 = "";
                int tempn = battleship.users.Count;
                bool half = false;
                if (tempn > 2)
                    e.Channel.SendMessage("Calculating teams..\nThis might take a while depending on the size of the team");
                for (int z = 0; z < tempn; z++)
                {
                    if (!half && z > tempn / 2)
                    {
                        e.Channel.SendMessage("Almost 50% Done..");
                        half = true;
                    }
                    Console.WriteLine("Users: " + Convert.ToString(z) + ", " + Convert.ToString(battleship.users.Count));
                    Random random = new Random();
                    int temp = random.Next(0, battleship.users.Count);
                    User tempuser;
                    tempuser = battleship.users[temp];
                    battleship.users.RemoveAt(temp);
                    if (z % 2 == 0)
                    {
                        await tempuser.AddRoles(r1);
                        await tempuser.AddRoles(r1);
                        await tempuser.AddRoles(r1);
                        await tempuser.AddRoles(r1);
                        battleship.teams1.Add(tempuser);
                        Console.WriteLine("Added " + tempuser.Name + " to team 1");
                        team1temp += tempuser.Mention + " ";
                    }
                    else if (z % 2 == 1)
                    {
                        await tempuser.AddRoles(r2);
                        await tempuser.AddRoles(r2);
                        await tempuser.AddRoles(r2);
                        await tempuser.AddRoles(r2);
                        battleship.teams2.Add(tempuser);
                        Console.WriteLine("Added " + tempuser.Name + " to team 2");
                        team2temp += tempuser.Mention + " ";
                    }
                    Console.WriteLine("\n");

                    if (z == 0 || z == 1)
                    {
                        await tempuser.AddRoles(r3);
                        await tempuser.AddRoles(r3);
                        Console.WriteLine("Added " + tempuser.Name + " to leader");
                        Console.WriteLine("\n");
                        if (z % 2 == 0)
                            leader1 = tempuser.Mention;
                        else
                            leader2 = tempuser.Mention;
                    }
                }
                e.Channel.SendMessage("Team 1: " + team1temp + "\nWith " + leader1 + " as the leader.");
                e.Channel.SendMessage("Team 2: " + team2temp + "\nWith " + leader2 + " as the leader.");
                channels(e);
                battleship.madeteams = true;
                e.Channel.SendMessage("\nNew text channels have been created.\nIn your team chats, place your ships to get ready to attack.\nType /help in your team channels for more info.");
            }
        }
        public static async void roles(MessageEventArgs e)
        {
            Role A;
            Role B;
            Role C;
            bool a = false;
            bool b = false;
            bool c = false;
            bool a1 = false;
            bool a2 = false;
            bool a3 = false;
            foreach (Role r in e.Server.Roles) //Check if roles exist and set channel permissions
            {
                if (r != null && r.Name.ToLower().StartsWith("team"))
                {
                    if (r.Name == "Team 1")
                    {
                        A = r;
                        a = true;
                    }
                    if (r.Name == "Team 2")
                    {
                        B = r;
                        b = true;
                    }
                }
                else if (r != null && r.Name.ToLower() == "leader")
                {
                    C = r;
                    c = true;
                }
            }
            if (a == false) //Create roles if they don't exist and set channel permissions
            {
                A = await e.Server.CreateRole("Team 1");
            }
            if (b == false)
            {
                B = await e.Server.CreateRole("Team 2");
            }
            if (c == false)
            {
                C = await e.Server.CreateRole("Leader");
            }
        }

        public static async void channels(MessageEventArgs e)
        {
            Role A;
            Role B;
            Role C;
            Role E;
            bool a = false;
            bool b = false;
            bool c = false;
            bool a1 = false;
            bool a2 = false;
            bool a3 = false;

            foreach (Channel ch in e.Server.AllChannels) //Check if the 3 channels exist (Team A, Team B, Attacks)
            {
                if (ch != null && ch.Type != null && ch.Type == ChannelType.Text && ch.Name != null && ch.Name == "t1")
                {
                    await ch.Delete();
                }
                if (ch != null && ch.Type != null && ch.Type == ChannelType.Text && ch.Name != null && ch.Name == "t2")
                {
                    await ch.Delete();
                }
                if (ch != null && ch.Type != null && ch.Type == ChannelType.Text && ch.Name != null && ch.Name == "attacks")
                {
                    await ch.Delete();
                }
            }
            if (a1 == false) //Create the 3 channels if they don't exist
                await e.Server.CreateChannel("t1", ChannelType.Text);
            if (a2 == false)
                await e.Server.CreateChannel("t2", ChannelType.Text);
            if (a3 == false)
                await e.Server.CreateChannel("attacks", ChannelType.Text);

            while (a1 == false || a2 == false || a3 == false) //Wait til the channel variables are initialized
            {
                foreach (Channel ch in e.Server.AllChannels)
                {
                    if (ch != null && ch.Type == ChannelType.Text && ch.Name == "t1")
                    {
                        a1 = true;
                        x1 = ch;
                    }
                    if (ch != null && ch.Type == ChannelType.Text && ch.Name == "t2")
                    {
                        a2 = true;
                        x2 = ch;
                    }
                    if (ch != null && ch.Type == ChannelType.Text && ch.Name == "attacks")
                    {
                        a3 = true;
                        x3 = ch;
                    }
                }
            }

            if (x1 != null && x2 != null)
            {
                foreach (Role r in e.Server.Roles) //Check if roles exist and set channel permissions
                {
                    if (r != null && r.Name.ToLower().StartsWith("team"))
                    {
                        if (r.Name == "Team 1")
                        {
                            A = r;
                            x1.AddPermissionsRule(A, ChannelPermissions.TextOnly, ChannelPermissions.None);
                            x2.AddPermissionsRule(A, ChannelPermissions.None, ChannelPermissions.TextOnly);
                            x3.AddPermissionsRule(A, ChannelPermissions.None, ChannelPermissions.TextOnly);
                            a = true;
                        }
                        if (r.Name == "Team 2")
                        {
                            B = r;
                            x2.AddPermissionsRule(B, ChannelPermissions.TextOnly, ChannelPermissions.None);
                            x1.AddPermissionsRule(B, ChannelPermissions.None, ChannelPermissions.TextOnly);
                            x3.AddPermissionsRule(B, ChannelPermissions.None, ChannelPermissions.TextOnly);
                            b = true;
                        }
                    }
                    else if (r != null && r.Name.ToLower() == "leader")
                    {
                        C = r;
                        x3.AddPermissionsRule(C, ChannelPermissions.TextOnly, ChannelPermissions.None);
                        c = true;
                    }
                    else if (r != null && r.Name.Contains("everyone"))
                    {
                        E = r;
                        x1.AddPermissionsRule(E, ChannelPermissions.None, ChannelPermissions.TextOnly);
                        x2.AddPermissionsRule(E, ChannelPermissions.None, ChannelPermissions.TextOnly);
                        x3.AddPermissionsRule(E, ChannelPermissions.None, ChannelPermissions.TextOnly);
                    }
                }
                if (a == false) //Create roles if they don't exist and set channel permissions
                {
                    A = await e.Server.CreateRole("Team 1");
                    x1.AddPermissionsRule(A, ChannelPermissionOverrides.InheritAll);
                    x2.AddPermissionsRule(A, ChannelPermissions.None, ChannelPermissions.TextOnly);
                }
                if (b == false)
                {
                    B = await e.Server.CreateRole("Team 2");
                    x2.AddPermissionsRule(B, ChannelPermissionOverrides.InheritAll);
                    x1.AddPermissionsRule(B, ChannelPermissions.None, ChannelPermissions.TextOnly);
                }
                if (c == false)
                {
                    C = await e.Server.CreateRole("Leader");
                    x3.AddPermissionsRule(C, ChannelPermissions.TextOnly, ChannelPermissions.None);
                }
            }
        }

        public static void Main(MessageEventArgs e, int num)
        {
            if (num == 0)
            {
                if (e.Channel.Name.ToLower() != "t1" && e.Channel.Name.ToLower() != "t2")
                {
                    e.Channel.SendMessage("Use your team text channels");
                }
                else
                    Place(e);
            }
            else if (num == 1)
            {
                bool has = false;
                foreach (Role r in e.User.Roles)
                {
                    if (r.Name == "Leader")
                    {
                        has = true;
                    }
                }
                if (battleship.attackready && has && e.Channel.Name == "attacks")
                {
                    Attack(e);
                    Display(x1);
                    Display(x2);
                    Display(x3);
                }
                else if (!battleship.attackready)
                {
                    e.Channel.SendMessage("Wait til both teams are ready to attack.\nBoth leaders must type /ready in the attacks channel once ready to attack.");
                }
                else if (e.Channel.Name != "attacks")
                {
                    e.Channel.SendMessage("Attacks can only be done in the Attacks channel");
                }
                else if (!has)
                {
                    e.Message.Delete();
                }
            }
        }

        public static int WhichUser(MessageEventArgs e)
        {
            int whichuser = 0;
            bool hasleader = false;
            foreach (Role r in e.User.Roles)
            {
                if (r.Name == "Team 1")
                {
                    whichuser = 1;
                }
                else if (r.Name == "Team 2")
                {
                    whichuser = 2;
                }
                else if (r.Name.ToLower() == "leader")
                {
                    hasleader = true;
                }
            }
            if (hasleader)
            {
                if (whichuser == 1)
                    return 3;
                else if (whichuser == 2)
                    return 4;
            }
            return whichuser;
        }

        public static void Place(MessageEventArgs e)
        {
            string message = e.Message.Text.Replace("/test ", "");
            int whichuser = WhichUser(e);
            int temp = Convert.ToInt32(message.Substring(1));
            for (int z = 0; z < 16; z++)
            {
                if (battleship.letters[z] == message.Substring(0, 1).ToUpper())
                {
                    if (whichuser == 1 || whichuser == 3)
                    {
                        if (battleship.team1[z, temp] == 0)
                        {
                            battleship.team1[z, temp]++;
                            Display(x1);
                        }
                    }
                    else if (whichuser == 2 || whichuser == 4)
                    {
                        if (battleship.team2[z, temp] == 0)
                        {
                            battleship.team2[z, temp]++;
                            Display(x2);
                        }
                    }
                    break;
                }
            }
        }

        public static void Attack(MessageEventArgs e)
        {
            int whichuser = WhichUser(e);
            string message = e.Message.Text.Replace("/attack ", "");
            int temp = Convert.ToInt32(message.Substring(1));
            for (int z = 0; z < 16; z++)
            {
                if (battleship.letters[z] == message.Substring(0, 1).ToUpper())
                {
                    if (whichuser == 3)
                    {

                        if (battleship.team2[z, temp] == 0)
                        {
                            battleship.team2[z, temp] = 2;

                        }
                        else if (battleship.team2[z, temp] == 1)
                        {
                            battleship.team2[z, temp] = 3;
                        }
                    }
                    else if (whichuser == 4)
                    {
                        if (battleship.team1[z, temp] == 0)
                            battleship.team1[z, temp] = 2;
                        else if (battleship.team1[z, temp] == 1)
                        {
                            battleship.team1[z, temp] = 3;
                        }
                    }
                    break;
                }
            }
        }

        public static void Display(Channel e)
        {
            if (e == x3)
            {
                if (battleship.previous != null)
                    battleship.previous.Message.Delete();
                battleship.awaiting = true;
            }
            else if (e == x1)
            {
                if (battleship.previousx1 != null)
                    battleship.previousx1.Message.Delete();
                battleship.awaitingx1 = true;
            }
            else if (e == x2)
            {
                if (battleship.previousx2 != null)
                    battleship.previousx2.Message.Delete();
                battleship.awaitingx2 = true;
            }
            if (e != x3)
            {
                e.SendMessage(BoardOne(e) + "\n" + BoardTwo(e));
            }
            else
                BoardAttack(e);
        }

        public static String BoardOne(Channel e)
        {
            String temp = String.Empty;

            for (int z = 0; z < 16; z++)
            {
                temp += battleship.letters[z] + " ";
            }
            temp += "\n";
            for (int z = 0; z < 16; z++)
            {
                for (int zed = 0; zed < 16; zed++)
                {
                    if (zed == 0)
                    {
                        if (z < 10)
                        {
                            temp += " " + Convert.ToString(z);
                        }
                        else
                        {
                            temp += Convert.ToString(z);
                        }
                    }
                    if (e == x1)
                        temp += "|" + (battleship.team1[zed, z] == 0 ? " " : battleship.team1[zed, z] == 1 ? "O" : battleship.team1[zed, z] == 2 ? "X" : "Ø");
                    else if (e == x2)
                        temp += "|" + (battleship.team1[zed, z] == 2 ? "X" : battleship.team1[zed, z] == 3 ? "Ø" : " ");
                }
                temp += "|\n";
            }
            return "Team 1:\n      `" + temp + "`";
        }

        public static String BoardTwo(Channel e)
        {
            String temp = String.Empty;
            for (int z = 0; z < 16; z++)
            {
                temp += battleship.letters[z] + " ";
            }
            temp += "\n";
            for (int z = 0; z < 16; z++)
            {
                for (int zed = 0; zed < 16; zed++)
                {
                    if (zed == 0)
                    {
                        if (z < 10)
                        {
                            temp += " " + Convert.ToString(z);
                        }
                        else
                        {
                            temp += Convert.ToString(z);
                        }
                    }
                    if (e == x1)
                        temp += "|" + (battleship.team2[zed, z] == 2 ? "X" : battleship.team2[zed, z] == 3 ? "Ø" : " ");
                    else if (e == x2)
                        temp += "|" + (battleship.team2[zed, z] == 0 ? " " : battleship.team2[zed, z] == 1 ? "O" : battleship.team2[zed, z] == 2 ? "X" : "Ø");
                }
                temp += "|\n";
            }
            return "Team 2:\n      `" + temp + "`";
        }

        public static void BoardAttack(Channel e)
        {

            String temp = String.Empty;
            for (int z = 0; z < 16; z++)
            {
                temp += battleship.letters[z] + " ";
            }
            temp += "\n";
            for (int z = 0; z < 16; z++)
            {
                for (int zed = 0; zed < 16; zed++)
                {
                    if (zed == 0)
                    {
                        if (z < 10)
                        {
                            temp += " " + Convert.ToString(z);
                        }
                        else
                        {
                            temp += Convert.ToString(z);
                        }
                    }
                    if (e == x3)
                        temp += "|" + (battleship.team2[zed, z] == 2 || battleship.team1[zed, z] == 2 ? "X" : battleship.team2[zed, z] == 3 || battleship.team1[zed, z] == 3 ? "X" : " ");
                }
                temp += "|\n";
            }
            e.SendMessage("\n      `" + temp + "`");
        }
    }
}
