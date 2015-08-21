using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        static async void SendTweetWithSinglePicture()
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

            var uploadedMedia = await context.UploadMediaAsync(File.ReadAllBytes(@"c:\path\to\image.jpg"));
            var mediaIds = new List<ulong> { uploadedMedia.MediaID };

            await context.TweetAsync(
                "Hello World! I am testing @dougvdotcom's #LinqToTwitter demo, at " +
                "https://www.dougv.com/2015/08/posting-twitter-status-updates-tweets-with-linqtotwitter-and-net-part-3-media-tweets",
                mediaIds
            );
        }

        static async void SendTweetWithMultiplePictures()
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

            var imageUploadTasks =
                new List<Task<Media>>
                {
                    context.UploadMediaAsync(File.ReadAllBytes(@"c:\path\to\image1.jpg")),
                    context.UploadMediaAsync(File.ReadAllBytes(@"c:\path\to\image2.png")),
                    context.UploadMediaAsync(File.ReadAllBytes(@"c:\path\to\image3.jpg"))
                };
            await Task.WhenAll(imageUploadTasks);
            
            var mediaIds =
                (from tsk in imageUploadTasks
                 select tsk.Result.MediaID)
                .ToList();

            await context.TweetAsync(
                "Photos of Acadia National Park by Kim Seng https://www.flickr.com/photos/captainkimo/ #LinqToTwitter",
                mediaIds
            );
        }
    }
}
