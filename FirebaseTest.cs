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

  // Handler for UI buttons on the scene.  Also performs some
  // necessary setup (initializing the firebase app, etc) on
  // startup.
  public class FirebaseTest : MonoBehaviour {

    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    protected bool isFirebaseInitialized = false;

    // When the app starts, check to make sure that we have
    // the required dependencies to use Firebase, and if not,
    // add them if possible.
    protected virtual void Start() {
      FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
        dependencyStatus = task.Result;
        if (dependencyStatus == DependencyStatus.Available) {
          InitializeFirebase();
        } else {
          Debug.LogError(
            "Could not resolve all Firebase dependencies: " + dependencyStatus);
        }
      });
    }

    // Initialize the Firebase database:
    protected virtual void InitializeFirebase() {
      FirebaseApp app = FirebaseApp.DefaultInstance;
      // NOTE: You'll need to replace this url with your Firebase App's database
      // path in order for the database connection to work correctly in editor.
      app.SetEditorDatabaseUrl("https://fir-unity-4d40b.firebaseio.com/");
      //app.SetEditorDatabaseUrl("https://findmykid-51edc.firebaseio.com/");
      if (app.Options.DatabaseUrl != null)
        app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);
      StartListener();
      isFirebaseInitialized = true;
    }

    protected void StartListener() {
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
        if (args.Snapshot != null && args.Snapshot.ChildrenCount > 0)
        {
            foreach (var childSnapshot in args.Snapshot.Children)
            {
                if (childSnapshot.Child("read") == null
                || childSnapshot.Child("read").Value == null)
                {
                    Debug.LogError("Bad data in sample.  Did you forget to call SetEditorDatabaseUrl with your project id?");
                    break;
                }
                else
                {   // If read value = 1, sign the kid in 
                    if (childSnapshot.Child("read").Value.ToString() == "1")
                    {
                        Debug.Log(childSnapshot.Key);
                        Debug.Log(childSnapshot.GetRawJsonValue());
                        Debug.Log(childSnapshot.Child("parent").Value.ToString());
                        Debug.Log(childSnapshot.Child("kid").Value.ToString());
                        Debug.Log(childSnapshot.Child("read").Value.ToString());
                        //do something here, for example set active for gameobject

                        //update child read to 0 if needed
                        //updateChild(childSnapshot.Key);
                    }

                }
            }
        }
    }

    private void updateChild(string tagId)
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("KidsClubLog")
            .Child(tagId)
            .Child("read")
            .SetValueAsync(0);
    }

    // Exit if escape (or back, on mobile) is pressed.
    protected virtual void Update() {
      //if (Input.GetKeyDown(KeyCode.Escape)) {
      //  Application.Quit();
      //}
    }

}
