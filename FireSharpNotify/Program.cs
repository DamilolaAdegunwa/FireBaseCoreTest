using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireSharpNotify
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var apiKey = "AIzaSyAf3zX8rG4hGfjLQK4Er61bTW7cQjTptgE"; // your app secret
                var firebaseAuthProvider = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
                var auth = await firebaseAuthProvider.SignInAnonymouslyAsync();

                var firebaseClient = new FirebaseClient(
                  "https://barontest-152eb.firebaseio.com/",
                  new FirebaseOptions
                  {
                      AuthTokenAsyncFactory = () => Task.FromResult(auth.FirebaseToken.ToString())
                  });

                var notifications = firebaseClient.Child("notifications");

                //delte old records
                await notifications.DeleteAsync();

                var list = new List<Notification> {
                    new Notification{IsError=false, Message="This is a test", Channel="Test_Channel",Id=1 },
                    new Notification{IsError=false, Message="Empty_Channel (will not show)", Channel="Empty_Channel",Id=2 },
                    new Notification{IsError=true, Message="Error: NULL stuff", Channel="Test_Channel",Id=3 },
                    new Notification{IsError=false, Message="more test", Channel="Test_Channel",Id=4 },
                };

                Console.WriteLine("Start");
                foreach (var item in list)
                {
                    await Task.Delay(5 * 1000);

                    item.CreatedOn = DateTime.Now;
                    var newNode = await notifications.PostAsync(item);
                    Console.WriteLine($"Posted {item.Message}");
                }

                Console.WriteLine("End");
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
