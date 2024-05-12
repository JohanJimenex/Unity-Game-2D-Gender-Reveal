using System;

public class User {

    public string firebaseId;
    public Data data;

    public User(string firebaseId, Data leaderboard) {
        this.firebaseId = firebaseId;
        this.data = leaderboard;
    }

}