/*
 * © 900lbs of Creative
 * Creation Date: DATE HERE
 * Date last Modified: MOST RECENT MODIFICATION DATE HERE
 * Name: AUTHOR NAME HERE
 * 
 * Description: DESCRIPTION HERE
 * 
 * Scripts referenced: LIST REFERENCED SCRIPTS HERE
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Restart : MonoBehaviour 
{
    public Button restart;

    private void Start()
    {
        restart.onClick.AddListener(restartScene);
    }

    void restartScene()
    {
        SceneManager.LoadScene(0);
    }
}
