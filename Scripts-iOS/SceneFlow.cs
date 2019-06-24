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
using UnityEngine.UI;
using DG.Tweening;

public class SceneFlow : MonoBehaviour
{
    public Image[] scenes;

    public int currentPage;

    public void SlideSceneIn(int num, bool vertical)
    {
        if (vertical)
        {
            scenes[num].gameObject.SetActive(true);
            scenes[num].rectTransform.DOAnchorPosY(0, 0.5f);
        }

        else
        {
            scenes[num].gameObject.SetActive(true);
            scenes[num].rectTransform.DOAnchorPosX(0, 0.5f);
        }
        currentPage = num;
    }

    public void SlideSceneOut(int num, bool vertical)
    {
        if (vertical)
        {
#if UNITY_IOS
            scenes[num].rectTransform.DOAnchorPosY(-2224, 0.5f).OnComplete(() => KillCard(num));
#else
            scenes[num].rectTransform.DOAnchorPosY(-1920, 0.5f).OnComplete(() => KillCard(num));
#endif
        }

        else
        {
#if UNITY_IOS
            scenes[num].rectTransform.DOAnchorPosX(1668, 0.5f).OnComplete(() => KillCard(num));
#else
            scenes[num].rectTransform.DOAnchorPosX(-1200, 0.5f).OnComplete(() => KillCard(num));
#endif
        }
    }

    public void FadeSceneIn(int num)
    {
        scenes[num].gameObject.SetActive(true);
        scenes[num].DOFade(1, 0.5f);
        currentPage = num;
    }

    public void FadeSceneOut(int num)
    {
        scenes[num].DOFade(0, 0.1f).OnComplete(() => KillCard(num));
    }

    public void KillCard(int num)
    {
        scenes[num].gameObject.SetActive(false);
    }
}