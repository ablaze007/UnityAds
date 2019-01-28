using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour
{
    private int callerType;
    //0 - coins reward
    //1 - continue the game

    private void Start()
    {
    }
    
    public void ShowRewardedAd()
    {
        ShowRewardedAd(0);
    }

    //for extra lives on main menu
    public void ShowVideoAd()
    {
        if (Advertisement.IsReady("video"))
        {
            GameObject livesManager = GameObject.FindGameObjectWithTag("LivesManager");
            if (livesManager != null)
                livesManager.GetComponent<LivesManager>().SimpleIncrement();

            soundManagerScript.StopAudio();
            Advertisement.Show("video");            
        }
        else 
        {
            GameObject messageBox = GameObject.FindGameObjectWithTag("AdMessage");
            if (messageBox != null)
                messageBox.GetComponent<AutoOff>().Disable();
        }
    }

    //to revival
    public void ShowVideoAdRevival()
    {
        if (Advertisement.IsReady("video"))
        {
            soundManagerScript.StopAudio();

            var options = new ShowOptions
            {
                resultCallback = HandleShowResult
            };

            Advertisement.Show("video", options);
        }
        else
        {
            UIManager.Instance.AdFailedOnRevival();
        }
    }

    //to buy coins from the shop
    public void ShowRewardedAd(int type)
    {
        //check if the ad is ready - rewardedVideo
        //then show(rewardedVideo) 
        callerType = type;

        if (callerType == 1)
            ShowVideoAdRevival();
        else
        {
            if (Advertisement.IsReady("rewardedVideo"))
            {
                soundManagerScript.StopAudio();

                var options = new ShowOptions
                {
                    resultCallback = HandleShowResult
                };

                Advertisement.Show("rewardedVideo", options);
            }
            else
            {
                UIManager.Instance.AdFailed();
            }
        }
    }

    void HandleShowResult(ShowResult result)
    {
        switch(result)
        {
            case ShowResult.Finished:
                //Debug.Log("Ad Completed");
                if (callerType == 0)
                    UIManager.Instance.rewardCoins();
                else if (callerType == 1)
                    UIManager.Instance.RevivePlayer();
                break;
            case ShowResult.Skipped:
                //Debug.Log("Ad skipped");
                if(callerType == 1)
                    UIManager.Instance.RevivePlayer();
                break;
            case ShowResult.Failed:
                Debug.Log("Ad failed");
                if (callerType == 0)
                    UIManager.Instance.AdFailed();
                else
                    UIManager.Instance.RevivePlayer();
                break;
            default:
                break;
        }
    }
}
