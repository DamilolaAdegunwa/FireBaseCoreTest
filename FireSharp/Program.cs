using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Linq;

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

                    var apiKey = "AIzaSyAf3zX8rG4hGfjLQK4Er61bTW7cQjTptgE"; // your app secret
                    var firebaseAuthProvider = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
                    var auth = await firebaseAuthProvider.SignInAnonymouslyAsync();

                    var firebaseClient = new FirebaseClient(
                      "https://barontest-152eb.firebaseio.com/",
                      new FirebaseOptions
                      {
                          AuthTokenAsyncFactory = () => Task.FromResult(auth.FirebaseToken.ToString())
                      });

                    var users = firebaseClient.Child("users_list");
                    var usersCollection = await users
                    .OrderByKey()
                    .LimitToFirst(1)
                    .OnceAsync<UserNode>();
                    var user = usersCollection.ToList().First();


                    var conversations = firebaseClient.Child("conversations");

                    var conversationsHistory = new Dictionary<string, ConverstionNode>();

                    var conversationsList = await conversations
                      .OrderByKey()
                      .OnceAsync<ConverstionNode>();

                    foreach (var conversation in conversationsList)
                    {
                        Console.WriteLine($"{conversation.Key} is {conversation.Object.ToString()}");
                        conversationsHistory.Add(conversation.Key, conversation.Object);
                    }


                    // add new item to list of data and let the client generate new key for you (done offline)
                    var newNode = await conversations.PostAsync(new ConverstionNode
                    {
                        date = DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                        username = user.Object.name,
                        text = $"hello to the first userP {user.Object.name} [{DateTime.Now.Ticks}]"

                    });

                    var obs = conversations.AsObservable<ConverstionNode>();
                    obs.Where(c => !conversationsHistory.ContainsKey(c.Key))
                    .Subscribe(f => Console.WriteLine($"after insert: {f.Key}: {f.Object.ToString()}"));

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

    public class UserNode
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}
