using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase.Database;
using System.Threading.Tasks;
using System;

public class FirebaseConnection : MonoBehaviour {

    private DatabaseReference databaseReference;

    private void Awake() {
        //Obtiene la referencia de la base de datos que esta en el archivo google-services.json o google-services.plist
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public async void WriteNewLeaderOnDB(string name, int score) {

        Data user = new Data(name, score);

        string json = JsonUtility.ToJson(user);

        // reference.SetRawJsonValueAsync(json);
        // reference.Child("data").Child(userId).Child("username").SetValueAsync(name);

        // Genera una nueva clave única bajo "users/" y agrega el usuario.
        DatabaseReference leaders = databaseReference.Child("leaders");
        DatabaseReference newLeader = leaders.Push();

        await newLeader.SetRawJsonValueAsync(json);
    }

    public async Task<List<User>> GetLeaderboardFromDB() {

        var dataSnapshot = await databaseReference.Child("leaders").OrderByChild("score").GetValueAsync();

        List<User> users = new List<User>();

        foreach (var child in dataSnapshot.Children) {
            Data leaderboard = JsonUtility.FromJson<Data>(child.GetRawJsonValue());
            users.Add(new User(child.Key, leaderboard));
        }
        // Because Firebase returns in ascending order
        users.Reverse();
        return users;
    }

    public async Task DeleteRecord(string recordKey) {
        Debug.Log(recordKey);
        try {
            await databaseReference.Child("leaders").Child(recordKey).RemoveValueAsync();
        }
        catch (Exception ex) {
            Debug.LogError("Error al eliminar registro: " + ex.Message);
        }
    }
}

