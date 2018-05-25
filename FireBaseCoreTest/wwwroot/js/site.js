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

    var usersListRef = firebase.database().ref('users_list');
    var conversationsRef = firebase.database().ref('conversations');
    

    var usersInitIsDone = false;
    var chartInitIsDone = false;

    var newUserRow = $(".newUserRow");
    var usersList = $(".users");
    var welcomeText = $(".welcomeText");
    var chat = $(".fullChat");

    $(".addUser").click(function () {
        newUserRow.toggle();
        $(this).html($(".newUserRow").is(":visible") ? "Cancel" : "Add User");
    });

    $(".saveUser").click(function () {
        var name = $(".userNaem").val();
        var id = usersList.find("li").length + 1;
        usersListRef.push({ "id": id, "name": name });
        var path = newUserRef.toString();
    });

    $(document).on("click", ".userItem", function () {
        var id = $(this).attr("data-id");
        var name = $(this).attr("data-name");
        welcomeText.html(name).attr("data-user-id", id).attr("data-name", name);
    });

    usersListRef.once('value', function (snapshot) {
        //read all of the users on page load
        var val = snapshot.exportVal();
        if (!val) return;

        var i = 0;
        for (var prop in val) {
            var user = val[prop];
            if (i === 0) {
                welcomeText.html(user.name).attr("data-user-id", user.id).attr("data-name", user.name);
                i++;
            }
            usersList.append(createUserLi(user.id, user.name));
        }
        usersInitIsDone = true;
    });

    usersListRef.on('child_added', function (snapshot) {
        if (!usersInitIsDone) return;//get all the new users, as they are added
        var val = snapshot.val();
        usersList.append(createUserLi(val.id, val.name));
        $(".addUser").click();
    });

    function createUserLi(id, name) {
        return "<li class='userItem pointer' data-id='{0}' data-name='{1}'> {2}</li>".format(id, name, name);
    }

    conversationsRef.once('value', function (snapshot) {
        //read all of the conversations on page load
        var val = snapshot.exportVal();
        if (!val) {
            return;
        }

        var i = 0;
        for (var prop in val) {
            var message = val[prop];
            createChatMessage(message.username, message.text, message.date);
        }
        chartInitIsDone = true;
        $(".postText,.addUser").removeAttr('disabled');
    });

    $(".postText").click(function () {
        var text = $(".text").val();
        var name = welcomeText.attr("data-name");
        var date = (new Date()).format("dd/MM/yyyy HH:mm");
        conversationsRef.push({ text: text, username: name, date: date });
        var path = conversationsRef.toString();

        $(".text").val('');
    });

    conversationsRef.on('child_added', function (snapshot) {
        if (!chartInitIsDone) return;//get all the new conversations, as they are added
        var message = snapshot.val();
        createChatMessage(message.username, message.text, message.date);
    });

    function createChatMessage(userName, text, date) {
        chat.append("<div>{0}: {1} [{2}] </div>".format(userName, text, date));
    }


});
