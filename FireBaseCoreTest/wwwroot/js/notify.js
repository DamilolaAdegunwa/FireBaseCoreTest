$(function () {
    // Initialize Firebase
    var config = {
        apiKey: "AIzaSyAf3zX8rG4hGfjLQK4Er61bTW7cQjTptgE",
        authDomain: "barontest-152eb.firebaseapp.com",
        databaseURL: "https://barontest-152eb.firebaseio.com",
        projectId: "barontest-152eb",
        storageBucket: "barontest-152eb.appspot.com",
        messagingSenderId: "592512603881"
    };
    firebase.initializeApp(config);
    firebase.auth().signInAnonymously();

    var notificationRef = firebase.database().ref('notifications');
    var initIsDone = false;

    var notificationList = $(".notificationList");

    var mapping = {
        Test_Channel: function (notification) {
            var message = "{0}: {1} [{2}]".format(notification.Id, notification.Message, notification.CreateOnText)
            if (notification.IsError) {
                error(message, notification.Channel);
            }
            else {
                show(message, notification.Channel);
            }
        }
    };

    notificationRef.on('child_added', function (snapshot) {
        if (!initIsDone) return;//get all the new users, as they are added
        var val = snapshot.val();

        var func = mapping[val.Channel];
        if (func) {
            func(val);
        }

        notificationList.append("<div>{0}: {1} [{2}] </div>".format(val.Channel, val.Message, val.CreateOnText));
    });

    notificationRef.once('value', function (snapshot) {
        //read all of the data on page load
        var val = snapshot.exportVal();
        if (!val) return;
        
        initIsDone = true;
    });

    function show(message, title) {
        toastr.info(message, title);
    }

    function error(message, title) {
        toastr.error(message, title);
    }

});
