
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// Handler for UI buttons on the scene.  Also performs some
// necessary setup (initializing the firebase app, etc) on
// startup.
public class FirebaseAPIRegistration : MonoBehaviour
{

    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    protected bool isFirebaseInitialized = false;

    public SceneFlow sceneFlow;
    public Image[] scenes;

    public int parentNum;
    public int childNum;

    private ThreadDispatcher MyDispatcher;

    // When the app starts, check to make sure that we have
    // the required dependencies to use Firebase, and if not,
    // add them if possible.
    protected virtual void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
                //when we start the app, reset all read value back to zero
                resetReadValue("1011973426216");
                //reset current screen on Start
                // Debug.Log("Reset Current Page to 0");
                // updateScreenPage(0);
            }
            else
            {
                Debug.LogError(
                  "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });

     
    }

    // Initialize the Firebase database:
    protected virtual void InitializeFirebase()
    {
        FirebaseApp app = FirebaseApp.DefaultInstance;
        // NOTE: You'll need to replace this url with your Firebase App's database
        // path in order for the database connection to work correctly in editor.
        app.SetEditorDatabaseUrl("https://findmykid-51edc.firebaseio.com/");
        if (app.Options.DatabaseUrl != null)
            app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);
        StartListener();
        isFirebaseInitialized = true;
    }

    protected void StartListener()
    {
        Debug.Log("Start Listening...");
        FirebaseDatabase.DefaultInstance
          .GetReference("KidsClubLog")
          .ValueChanged += HandleValueChanged;
    }

    void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
		
		// only check if user at page 0 in the app
        if (sceneFlow.currentPage == 0 ) 
        {
            if (args.Snapshot != null && args.Snapshot.ChildrenCount > 0)
            {
                foreach (var childSnapshot in args.Snapshot.Children)
                {
                    if (childSnapshot.Child("RegistrationRead") == null
                    || childSnapshot.Child("RegistrationRead").Value == null)
                    {
                        Debug.LogError("Bad data in sample.  Did you forget to call SetEditorDatabaseUrl with your project id?");
                        break;
                    }
                    else
                    {   // If read value = 1, a new RFID just scanned from the RPI
                        if (childSnapshot.Child("RegistrationRead").Value.ToString() == "1")
                        {
                            // REGISTRATION APP
                            // 1011973426216 


                            if (childSnapshot.Key == "1011973426216" && sceneFlow.currentPage == 0)
                            {
                                // Debug.Log("Switch to page 1");
                                RunOnMainThread<int>(() => {
                                    sceneFlow.FadeSceneIn(1);
                                    // updateScreenPage(1);
                                    return 0;
                                });
                                
                            }
                           
                            
                                
                            

                            //update child read back to 0 after the app use it
                            resetReadValue(childSnapshot.Key);
                        }

                    }
                }
            }
        }// end if checking currentPage
    }

    private void resetReadValue(string tagId)
    {
        // after checking the card, set Read value back to 0 
        FirebaseDatabase.DefaultInstance
            .GetReference("KidsClubLog")
            .Child(tagId)
            .Child("RegistrationRead")
            .SetValueAsync(0);

    }
    public void updateScreenPage(int screenPage)
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("CurrentScreen")
            .SetValueAsync(screenPage);
    }


    // Exit if escape (or back, on mobile) is pressed.
    //protected virtual void Update() {
    //if (Input.GetKeyDown(KeyCode.Escape)) {
    //  Application.Quit();
    //}
    //}

    public void Awake()
    {
        // Create the ThreadDispatcher on a call that is guaranteed to run on the main Unity thread.
        MyDispatcher = new ThreadDispatcher();
    }

    public void Update()
    {
        MyDispatcher.PollJobs();
    }

    public TResult RunOnMainThread<TResult>(System.Func<TResult> f)
    {
        return MyDispatcher.Run(f);
    }


}