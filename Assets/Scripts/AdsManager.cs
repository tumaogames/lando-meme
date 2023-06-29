using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class AdsManager : GenericSingletonClass<AdsManager>, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string _androidGameId;
    [SerializeField] bool _testMode = false;
    [SerializeField] Button _showAdButton;
    [SerializeField] string _androidAdUnitIdInterstitial;
    [SerializeField] string _androidAdUnitIdRewarded;
    private string _gameId;

    void Awake()
    {
        InitializeAds();
        _showAdButton.interactable = false;
    }

    public void InitializeAds()
    {
        Advertisement.Initialize(_androidGameId, _testMode, this);
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        LoadAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    // Load content to the Ad Unit:
    public void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _androidAdUnitIdRewarded);
        Advertisement.Load(_androidAdUnitIdRewarded, this);

    }

    // If the ad successfully loads, add a listener to the button and enable it:
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);
        if (adUnitId.Equals(_androidAdUnitIdRewarded))
        {
            // Configure the button to call the ShowAd() method when clicked:
            _showAdButton.onClick.AddListener(ShowAdRewarded);
            // Enable the button for users to click:
            _showAdButton.interactable = true;
        }
    }

    // Implement a method to execute when the user clicks the button:
    public void ShowAd()
    {
        // Disable the button: 
        _showAdButton.interactable = false;
        // Then show the ad:
        Advertisement.Show(_androidAdUnitIdInterstitial, this);
    }

    // Implement a method to execute when the user clicks the button:
    public void ShowAdRewarded()
    {
        // Disable the button: 
        _showAdButton.interactable = false;
        // Then show the ad:
        Advertisement.Show(_androidAdUnitIdRewarded, this);
    }

    // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {

        if (adUnitId.Equals(_androidAdUnitIdRewarded) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            // Grant a reward.
            GameManager.Instance.Coins += 5;
            // Load another ad:
            //Advertisement.Load(_androidAdUnitIdRewarded, this);
        }
    }

    // Implement Load and Show Listener error callbacks:
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }

    void OnDestroy()
    {
        // Clean up the button listeners:
        _showAdButton.onClick.RemoveAllListeners();
    }
}