using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine.UI;


public class CheckRFIDReads : MonoBehaviour {

    //public RawImage parentProfilePic;
    //public RawImage parentProfilePic2;

    public SceneFlow sceneFlow;

    public int parentNum;
    public int childNum;

    DatabaseReference myDatabaseRef;


    private void Awake()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                //   app = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
                Debug.Log("Firebase is ready!");
                InitializeFirebase();
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }


    //void HandleValueChanged(object sender, ValueChangedEventArgs args)
    //{
    //    if (args.DatabaseError != null)
    //    {
    //        Debug.LogError(args.DatabaseError.Message);
    //        return;
    //    }
    //}



    /* Initialize the Firebase database */
    protected virtual void InitializeFirebase()
    {
        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://findmykid-51edc.firebaseio.com/");

        // Get the root reference location of the database.
        myDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;

        //FirebaseDatabase.DefaultInstance.GetReference("KidsClubLog").ValueChanged += HandleValueChanged;
    }


    /* Used for reading Eddystone format from DB */
    void ReadFromDB()
    {
        DataSnapshot snapshot;
        Debug.Log("------Start Read------");
        FirebaseDatabase.DefaultInstance.GetReference("KidsClubLog").Child("165068935866").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
                Debug.LogError("Error reading from db");
            }
            else if (task.IsCompleted)
            {
                snapshot = task.Result;
                Debug.Log("My Value: " + snapshot.GetRawJsonValue());
                var read = snapshot.Child("Read").Value.ToString();
                Debug.Log("*****" + read);
                if (read == "1")
                {
                    readCompleteDBWrite("165068935866");
                    sceneFlow.FadeSceneIn(childNum);
                    //parentProfilePic.gameObject.SetActive(true);
                }
                Debug.Log("Read Success!");
            }
            //Debug.Log("------End Read------");

        });
    }


    /* Used for reading Eddystone format from DB */
    void ReadFromDB2()
    {
        DataSnapshot snapshot;
        FirebaseDatabase.DefaultInstance.GetReference("KidsClubLog").Child("225111446012").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
                Debug.LogError("Error reading from db");
            }
            else if (task.IsCompleted)
            {
                snapshot = task.Result;
                var read = snapshot.Child("Read").Value.ToString();
                if (read == "1")
                {
                    readCompleteDBWrite("225111446012");
                    sceneFlow.FadeSceneIn(childNum);
                    //parentProfilePic2.gameObject.SetActive(true);
                }
            }
        });
    }


    private void readCompleteDBWrite(string tagID)
    {
        if (myDatabaseRef == null)
        {
            //Debug.Log("DB ref was null");
            myDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        }

        FirebaseDatabase.DefaultInstance.GetReference("KidsClubLog").Child(tagID).Child("Read").SetValueAsync(0);

        //EddystoneBeaconInfo beacon = new EddystoneBeaconInfo(beaconID, instance, bssid, lastSeen);
        //string json = JsonUtility.ToJson(beacon);

        //myDatabaseRef.Child("BeaconLocations").Child(instance).SetRawJsonValueAsync(json);

        Debug.Log("Wrote something!");
    }

    private void CheckForRFIDReads()
    {
        ReadFromDB();
        ReadFromDB2();
    }

    // Use this for initialization
    void Start () 
    {
        InvokeRepeating("CheckForRFIDReads", 0f, 1.0f);
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.anyKey)
        {
            //parentProfilePic.gameObject.SetActive(false);
            //parentProfilePic2.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {

    }
}
