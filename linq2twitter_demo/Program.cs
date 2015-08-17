using System;
using System.Threading.Tasks;
using LinqToTwitter;

namespace linq2twitter_demo
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Program started.");

            try
            {
                Task.Run(() => SendTweet());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Program completed.");
            Console.Read();
        }

        static async void SendTweet()
        {
            var auth = new SingleUserAuthorizer
            {
                CredentialStore = new SingleUserInMemoryCredentialStore
                {
                    ConsumerKey = "your consumer key",
                    ConsumerSecret = "your consumer secret",
                    AccessToken = "your access token",
                    AccessTokenSecret = "your access token secret"
                }
            };

            var context = new TwitterContext(auth);

            await context.TweetAsync(
                "Hello World! I am testing @dougvdotcom's #LinqToTwitter demo, at " +
                "https://www.dougv.com/2015/08/posting-status-updates-to-twitter-via-linqtotwitter-part-2-plain-text-tweets"
            );
        }
    }
}
