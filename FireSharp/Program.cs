using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Threading.Tasks;

namespace FireSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                try
                {
                    //        var config = {
                    //    apiKey: "AIzaSyAf3zX8rG4hGfjLQK4Er61bTW7cQjTptgE",
                    //    authDomain: "barontest-152eb.firebaseapp.com",
                    //    databaseURL: "https://barontest-152eb.firebaseio.com",
                    //    projectId: "barontest-152eb",
                    //    storageBucket: "barontest-152eb.appspot.com",
                    //    messagingSenderId: "592512603881"
                    //};
                    //"conversations"

                    var apiKey = "AIzaSyAf3zX8rG4hGfjLQK4Er61bTW7cQjTptgE"; // your app secret

                    var firebaseAuthProvider = new FirebaseAuthProvider(new FirebaseConfig(apiKey));

                    var auth = await firebaseAuthProvider.SignInAnonymouslyAsync();

                    var firebaseClient = new FirebaseClient(
                      "https://barontest-152eb.firebaseio.com/",
                      new FirebaseOptions
                      {
                          AuthTokenAsyncFactory = () => Task.FromResult(auth.FirebaseToken.ToString())
                      });

                    var conversations = firebaseClient.Child("conversations");

                    var conversationsList = await conversations
                      .OrderByKey()
                      .LimitToFirst(2)
                      .OnceAsync<ConverstionNode>();

                    foreach (var conversation in conversationsList)
                    {
                        Console.WriteLine($"{conversation.Key} is {conversation.Object.ToString()}");
                    }


                    //// add new item to list of data and let the client generate new key for you (done offline)
                    //var dino = await firebase
                    //  .Child("dinosaurs")
                    //  .PostAsync(new Dinosaur());

                    //// note that there is another overload for the PostAsync method which delegates the new key generation to the firebase server

                    //Console.WriteLine($"Key for the new dinosaur: {dino.Key}");

                    //// add new item directly to the specified location (this will overwrite whatever data already exists at that location)
                    //await firebase
                    //  .Child("dinosaurs")
                    //  .Child("t-rex")
                    //  .PutAsync(new Dinosaur());

                    //// delete given child node
                    //await firebase
                    //  .Child("dinosaurs")
                    //  .Child("t-rex")
                    //  .DeleteAsync();

                }
                catch (Exception ex)
                {
                    throw;
                }
            }).Wait();

            Console.ReadLine();
        }
    }

    public class ConverstionNode
    {
        public string text { get; set; }
        public string username { get; set; }
        public string date { get; set; }

        public override string ToString() => $"{username}:{text} [{date}]";
    }
}
