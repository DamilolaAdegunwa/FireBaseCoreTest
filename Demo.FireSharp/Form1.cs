using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo.FireSharp
{
    public partial class Form1 : Form
    {
        FirebaseClient firebaseClient;
        ChildQuery notifications;
        public Form1()
        {
            InitializeComponent();
        }

        private async Task Init()
        {
            var apiKey = "AIzaSyAf3zX8rG4hGfjLQK4Er61bTW7cQjTptgE"; // your app secret
            var firebaseAuthProvider = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
            var auth = await firebaseAuthProvider.SignInAnonymouslyAsync();

            firebaseClient = new FirebaseClient(
            "https://barontest-152eb.firebaseio.com/",
            new FirebaseOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(auth.FirebaseToken.ToString())
            });

            notifications = firebaseClient.Child("notifications");
            await notifications.DeleteAsync();
        }

        private async void btnSend_Click(object sender, System.EventArgs e)
        {
            btnSend.Enabled = false;
            await Send();
            btnSend.Enabled = true;
        }

        private async void txtText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSend.Enabled = false;
                await Send();

                btnSend.Enabled = true;
            }
        }

        private async Task Send()
        {
            await Init();
            var item = new Notification { IsError = false, Message = txtText.Text, Channel = "Test_Channel", Id = 1 };
            item.CreatedOn = DateTime.Now;
            var newNode = await notifications.PostAsync(item);

            txtText.Text = "";
        }
    }
}
