mergeInto(LibraryManager.library, {

  insertUser: function (path,objectName,username, password,name,score1,score2,score3,callback) {
    var parsePath = Pointer_stringify(path);
    var parseObjectName = Pointer_stringify(objectName);
    var parseUsername = Pointer_stringify(username);
    var parsePassword = Pointer_stringify(password);
    var parseName = Pointer_stringify(name);
    var parseScore1 = (score1);
    var parseScore2 = (score2);
    var parseScore3 = (score3);
    var parseCallback = Pointer_stringify(callback);

    const usersRef = firebase.database().ref(parsePath);

    const userKey = 'user_' + Date.now();

    const newUser = {
        username: parseUsername,
        password: parsePassword,
        name:parseName,
        score1:parseScore1,
        score2:parseScore2,
        score3:parseScore3
    };

    usersRef.orderByChild('username').equalTo(parseUsername).once('value')
        .then((snapshot) => {
            // If there are any results, the username is already taken
            if(snapshot.exists())
            {
                //console.log('Username Taken');
                window.unityInstance.Module.SendMessage(parseObjectName,parseCallback,"Username already exist!");
            }else
            {
                usersRef.child(userKey).set(newUser)
                .then(() => {
                    //console.log('User added successfully');
                    window.unityInstance.Module.SendMessage(parseObjectName,parseCallback,"Registration Successfull");
                })
                .catch((error) => {
                    window.unityInstance.Module.SendMessage(parseObjectName,parseCallback,"Registration Failed");
                });
            }
        })
        .catch((error) => {
            console.error('Error checking username availability:', error);
            return false; // Handle the error appropriately in your code
        });


},

    loginUser: function (path,objectName,username, password, callback,getData) {

    var parsePath = Pointer_stringify(path);
    var parseObjectName = Pointer_stringify(objectName);
    var parseUsername = Pointer_stringify(username);
    var parsePassword = Pointer_stringify(password);
    var parseCallback = Pointer_stringify(callback);
    var parseGetdata = Pointer_stringify(getData);

    const usersRef = firebase.database().ref(parsePath);

    usersRef.orderByChild('username').equalTo(parseUsername).once('value')
        .then((snapshot) => {
            if (snapshot.exists()) {
                // User with the provided username exists
                const userData = snapshot.val();

                // Check if the password matches
                const userKey = Object.keys(userData)[0]; // Assuming unique usernames
                if (userData[userKey].password === parsePassword) {
                    //console.log('Login successful');
                    //console.log(userData[userKey].username);
                    window.unityInstance.Module.SendMessage(parseObjectName,parseCallback,"Login Successfull");
                    window.unityInstance.Module.SendMessage(parseObjectName,parseGetdata,userData[userKey].username);
                } else {
                    //console.log('Incorrect password');
                    window.unityInstance.Module.SendMessage(parseObjectName,parseCallback,"Incorrect password");
                }
            } else {
                //console.log('User not found');
                window.unityInstance.Module.SendMessage(parseObjectName,parseCallback,"User not found");
            }
        })
        .catch((error) => {
            console.error('Error logging in:', error);
        });
    },

    getUser: function (path,objectName,username,getData,getRank) {

    var parsePath = Pointer_stringify(path);
    var parseObjectName = Pointer_stringify(objectName);
    var parseUsername = Pointer_stringify(username);
    var parseGetdata = Pointer_stringify(getData);
    var parseGetrank = Pointer_stringify(getRank);
    const usersRef = firebase.database().ref(parsePath);

    usersRef.orderByChild('username').equalTo(parseUsername).once('value')
        .then((snapshot) => {
            if (snapshot.exists()) {
                // User with the provided username exists
                const userData = snapshot.val();

                // Check if the password matches
                const userKey = Object.keys(userData)[0]; // Assuming unique usernames
                //console.log(JSON.stringify(userData));
                const yourScore = userData[userKey].score;
                
                    usersRef.orderByChild('score')
                    .startAt(yourScore) // Replace with your actual score
                    .once('value')
                    .then((snapshot) => {
                        const rankedPlayers = snapshot.val();
                        // Ranked players contain players with scores equal to or higher than your score.
                        // You can determine your rank based on the length of this list.
                        const yourRank = Object.keys(rankedPlayers).length;
                        userData[userKey].rank = yourRank;
                        window.unityInstance.Module.SendMessage(parseObjectName,parseGetdata,JSON.stringify(userData[userKey]));
                    })
                    .catch((error) => {
                        console.error('Error fetching ranked players:', error);
                    });


            } else {
                //console.log('User not found');
                window.unityInstance.Module.SendMessage(parseObjectName,parseCallback,"User not found");
            }
        })
        .catch((error) => {
            console.error('Error logging in:', error);
        });
 
    },

    getRankings: function (path,objectName,mode,getRank) {
    var parsePath = Pointer_stringify(path);
    var parseObjectName = Pointer_stringify(objectName);
    var parseMode = mode;
    var parseGetrank = Pointer_stringify(getRank);

    const database = firebase.database();
    const playersRef = database.ref(parsePath);

    playersRef.orderByChild('score')
        .limitToLast(10) // Fetch the top 10 players with the highest scores
        .once('value')
        .then((snapshot) => {
            // Convert the snapshot to an array of players
            const players = [];
            snapshot.forEach((childSnapshot) => {
                const player = childSnapshot.val();
                players.push(player);
            });

            // Sort the players by score in descending order
            

            if(parseMode == 1)
            {
                players.sort((a, b) => b.score1 - a.score1);
            }else if(parseMode == 2)
            {
                players.sort((a, b) => b.score2 - a.score2);
            }else if(parseMode == 3)
            {
                players.sort((a, b) => b.score3 - a.score3);
            }

            // Now you have the top 10 players sorted by score
            //console.log(JSON.stringify(players));
            window.unityInstance.Module.SendMessage(parseObjectName,parseGetrank,JSON.stringify(players));
        })
        .catch((error) => {
            console.error('Error fetching top players:', error);
        });
    },


    listenForPlayerDataUpdates:function (path,objectName,calllback) {

    var parsePath = Pointer_stringify(path);
    var parseObjectName = Pointer_stringify(objectName);
    var parseCallback = Pointer_stringify(calllback);

    const playersRef = firebase.database().ref(parsePath);

    playersRef.on('value', (snapshot) => {
        window.unityInstance.Module.SendMessage(parseObjectName,parseCallback);
    });
},

    

    updateScoreByUsername: function (path,username,score,mode) {

    var parsePath = Pointer_stringify(path);
    var parseUsername = Pointer_stringify(username);
    var pointsToAdd = score;
    var parseMode = mode;

    const usersRef = firebase.database().ref(parsePath);

    usersRef.orderByChild('username').equalTo(parseUsername).once('value')
        .then((snapshot) => {
            if (snapshot.exists()) {
                const userData = snapshot.val();
                const userKey = Object.keys(userData)[0]; // Assuming unique usernames
                var score = 0;

                if(mode == 1)
                {
                    score = userData[userKey].score1;
                }else if(mode == 2)
                {
                    score = userData[userKey].score2;
                }else if(mode == 3)
                {
                    score = userData[userKey].score3;
                }

                const currentScore = score;
                const newScore = currentScore + pointsToAdd;

                snapshot.forEach((userSnapshot) => {
                    // Get the user's key (e.g., user_1694757930819)
                    const userKey = userSnapshot.key;
                    const userRef = usersRef.child(userKey);

                    // Update the user's score in the database
                    
                    if(parseMode == 1)
                    {
                    userRef.update({ score1: newScore })
                        .then(() => {
                            console.log(`Updated ${parseUsername}'s score to ${newScore}`);
                        })
                        .catch((error) => {
                            console.error('Error updating user score:', error);
                        });
                    }else if(parseMode == 2)
                    {
                    userRef.update({ score2: newScore })
                        .then(() => {
                            console.log(`Updated ${parseUsername}'s score to ${newScore}`);
                        })
                        .catch((error) => {
                            console.error('Error updating user score:', error);
                        });
                    }else if(parseMode == 3)
                    {
                    userRef.update({ score3: newScore })
                        .then(() => {
                            console.log(`Updated ${parseUsername}'s score to ${newScore}`);
                        })
                        .catch((error) => {
                            console.error('Error updating user score:', error);
                        });
                    }
                });
            } else {
                //console.log('User not found');
            }
        })
        .catch((error) => {
            console.error('User not found:', error);
        });
    },
 

  
});