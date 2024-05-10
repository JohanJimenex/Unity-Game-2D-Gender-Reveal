using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase.Database;
using System.Threading.Tasks;

public class FirebaseConnection : MonoBehaviour {

    private DatabaseReference databaseReference;

    private void Awake() {
        //Obtiene la referencia de la base de datos que esta en el archivo google-services.json o google-services.plist
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public async void WriteNewLeaderOnDB(string name, int score) {

        Leaderboard user = new Leaderboard(name, score);

        string json = JsonUtility.ToJson(user);

        // reference.SetRawJsonValueAsync(json);
        // reference.Child("data").Child(userId).Child("username").SetValueAsync(name);

        // Genera una nueva clave única bajo "users/" y agrega el usuario.
        DatabaseReference leaders = databaseReference.Child("leaders");
        DatabaseReference newLeader = leaders.Push();

        await newLeader.SetRawJsonValueAsync(json);
    }

    public async Task<List<Leaderboard>> GetLeaderboardFromDB() {

        var dataSnapshot = await databaseReference.Child("leaders").OrderByChild("score").GetValueAsync();

        List<Leaderboard> leaders = new List<Leaderboard>();

        foreach (var child in dataSnapshot.Children) {
            leaders.Add(JsonUtility.FromJson<Leaderboard>(child.GetRawJsonValue()));
        }

        leaders.Reverse(); // Because Firebase returns in ascending order
        return leaders;
    }
}

