// Copyright 2016 Google Inc. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.


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
public class FirebaseAPIEmployee : MonoBehaviour
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
                resetReadValue("225094668797");
                resetReadValue("225111446012");
                resetReadValue("165068935866");
                resetReadValue("584189768852");
                resetReadValue("584196215187");
                //reset current screen on Start
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
        // Debug.Log("Current Page: " + sceneFlow.currentPage);
        // Debug.Log(args.Snapshot.GetRawJsonValue());

        // only check if user at page 0, 8, 9 in the app
        if (sceneFlow.currentPage == 0 || sceneFlow.currentPage == 8 || sceneFlow.currentPage == 9) 
        {
            if (args.Snapshot != null && args.Snapshot.ChildrenCount > 0)
            {
                foreach (var childSnapshot in args.Snapshot.Children)
                {
                    if (childSnapshot.Child("EmployeeRead") == null
                    || childSnapshot.Child("EmployeeRead").Value == null)
                    {
                        Debug.LogError("Bad data in sample.  Did you forget to call SetEditorDatabaseUrl with your project id?");
                        break;
                    }
                    else
                    {   // If read value = 1, a new RFID just scanned from the RPI
                        if (childSnapshot.Child("EmployeeRead").Value.ToString() == "1")
                        {
                            // EMPLOYEE APP
                            // 225094668797 -> Parent Card
                            // 225111446012 -> Employee Card
                            // 165068935866 -> Child Card (Check In)
                            // 584189768852 -> Wildband (Check In AND Check Out)
                            // 584196215187 -> Wildband 2 (Check In AND Check Out)

                            if (childSnapshot.Key == "225094668797" || childSnapshot.Key == "225111446012" || childSnapshot.Key == "165068935866" || childSnapshot.Key == "584189768852" || childSnapshot.Key == "584196215187" ){                
                                if(sceneFlow.currentPage == 0){
                                    Debug.Log("Switch to page 5");
                                    RunOnMainThread<int>(() => {
                                    // sceneFlow.FadeSceneIn(5);
                                    // sceneFlow.FadeSceneOut(0);
                                    sceneFlow.SlideSceneIn(5, false);
                                    // updateScreenPage(5);
                                    return 0;
                                    });
                                }
                                else if (sceneFlow.currentPage == 8){
                                    Debug.Log("Switch to page 6");
                                    RunOnMainThread<int>(() => {
                                    sceneFlow.FadeSceneIn(6);
                                    sceneFlow.FadeSceneOut(8);
                                    // updateScreenPage(6);
                                    return 0;
                                    });
                                }
                                else if (sceneFlow.currentPage == 9){
                                    Debug.Log("Switch to page 7");
                                    RunOnMainThread<int>(() => {
                                    sceneFlow.FadeSceneIn(7);
                                    sceneFlow.FadeSceneOut(9);
                                    // updateScreenPage(7);
                                    return 0;
                                    });
                                }

                            }
                           
                            
                                
                            

                            //update child read back to 0 after the app use it, also set correctCard true or false
                            resetReadValue(childSnapshot.Key);
                        }

                    }
                }
            }
        }// end if checking currentPage
    }

    private void resetReadValue(string tagId)
    {
        // after checking the card, set Read value back to 0 and set CorrectCard 0 or 1
        FirebaseDatabase.DefaultInstance
            .GetReference("KidsClubLog")
            .Child(tagId)
            .Child("EmployeeRead")
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