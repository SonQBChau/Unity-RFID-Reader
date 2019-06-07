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
public class FirebaseAPI : MonoBehaviour
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
                //reset current screen on Start
                updateScreenPage(0);
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
        Debug.Log("Current Page: " + sceneFlow.currentPage);
        Debug.Log(args.Snapshot.GetRawJsonValue());

        // if (sceneFlow.currentPage == 30 || 
        //     sceneFlow.currentPage == 31 || 
        //     sceneFlow.currentPage == 1 ||
        //     sceneFlow.currentPage == 18 ||
        //     sceneFlow.currentPage == 17
        //     ) //only check if user at page 1.1, 2.1, 2.2, 3.2, 3.3
        if (sceneFlow.currentPage == 0 ) 
        {
            if (args.Snapshot != null && args.Snapshot.ChildrenCount > 0)
            {
                foreach (var childSnapshot in args.Snapshot.Children)
                {
                    if (childSnapshot.Child("Read") == null
                    || childSnapshot.Child("Read").Value == null)
                    {
                        Debug.LogError("Bad data in sample.  Did you forget to call SetEditorDatabaseUrl with your project id?");
                        break;
                    }
                    else
                    {   // If read value = 1, a new RFID just scanned from the RPI
                        if (childSnapshot.Child("Read").Value.ToString() == "1")
                        {
                            // 165068935866: CHILD CHECKIN
                            // 853040429192: CHILD CHECKOUT
                            // 225111446012: EMPLOYEE
                            // 225094668797: PARENT     

                        
                            //Debug.Log(childSnapshot.Child("Parent").Value.ToString());
                            //Debug.Log(childSnapshot.Child("Child").Value.ToString());
                            //Debug.Log(childSnapshot.Child("Read").Value.ToString());
                            //do something here, for example set active for gameobject


                            //Figure out how to launch different scenes based on values, one for child, one for parent. The methods are below


                 

/* 
                            // if this is child card, from 3.2 navigate to 3.1.1 screen
                            if (childSnapshot.Key == "165068935866" && sceneFlow.currentPage == 30)
                            {
                                Debug.Log("switch to page 28");
                                RunOnMainThread<int>(() => {
                                    sceneFlow.FadeSceneOut(30);
                                    sceneFlow.FadeSceneIn(28);
                                    updateScreenPage(28);
                                    return 0;
                                });
                     
                            }
                            // if this is child card, from 3.3 navigate to 3.1.2 screen
                            if (childSnapshot.Key == "853040429192" && sceneFlow.currentPage == 31)
                            {
                                Debug.Log("switch to page 29");
                                RunOnMainThread<int>(() => {
                                    sceneFlow.FadeSceneOut(31);
                                    sceneFlow.FadeSceneIn(29);
                                    updateScreenPage(29);
                                    return 0;
                                });
                                
                            }


                            // if this is parent card, from 1.1 navigate to 1.3 screen
                            if (childSnapshot.Key == "225094668797" && sceneFlow.currentPage == 1)
                            {
                                Debug.Log("Switch to page 2");
                                RunOnMainThread<int>(() => {
                                    sceneFlow.FadeSceneIn(2);
                                    updateScreenPage(2);
                                    return 0;
                                });
                                
                            }
                            // if this is parent card, from 2.2 navigate to 3.1 screen
                            if (childSnapshot.Key == "225094668797" && sceneFlow.currentPage == 18)
                            {
                                Debug.Log("Switch to page 27");
                                RunOnMainThread<int>(() => {
                                    sceneFlow.FadeSceneIn(27);
                                    updateScreenPage(27);
                                    return 0;
                                });
                                
                            }

                            // if this is employee card, from 2.1 navigate to 2.2 screen
                            if (childSnapshot.Key == "225111446012" && sceneFlow.currentPage == 17)
                            {
                                Debug.Log("Switch to page 18");
                                RunOnMainThread<int>(() => {
                                    sceneFlow.FadeSceneIn(18);
                                    updateScreenPage(18);
                                    return 0;
                                });
                                
                            }
*/


                            // if this is parent card, navigate from Scene 0 to 3
                            if (childSnapshot.Key == "225094668797" && sceneFlow.currentPage == 0)
                            {
                                Debug.Log("Switch to page 3");
                                RunOnMainThread<int>(() => {
                                    sceneFlow.FadeSceneIn(3);
                                    updateScreenPage(3);
                                    return 0;
                                });
                                
                            }

                            //update child read back to 0 after the app use it, also set correctCard true or false
                            updateChild(childSnapshot.Key);
                        }

                    }
                }
            }
        }// end if checking currentPage
    }

    private void updateChild(string tagId)
    {
        // after checking the card, set Read value back to 0 and set CorrectCard 0 or 1
        FirebaseDatabase.DefaultInstance
            .GetReference("KidsClubLog")
            .Child(tagId)
            .Child("Read")
            .SetValueAsync(0);

    }
    public void updateScreenPage(int screenPage)
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("CurrentScreen")
            .SetValueAsync(screenPage);
        Debug.Log("====================");
        Debug.Log(screenPage);
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