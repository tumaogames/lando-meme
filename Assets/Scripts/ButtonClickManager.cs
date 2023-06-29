using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClickManager : MonoBehaviour
{
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickExitButton()
    {
        gameManager.menuCanvas.gameObject.SetActive(true);
        gameManager.memeCanvas.gameObject.SetActive(false);
        //AdsManager.Instance.ShowAd();
        if (gameManager.AdShowCounter == 3)
        {
            gameManager.AdShowCounter = 0;
            AdsManager.Instance.ShowAd();
        }
        foreach (RectTransform meme in gameManager.memeCanvas.GetComponentsInChildren<RectTransform>(true))
        {
            if(meme.tag == "Meme") meme.gameObject.SetActive(false);
        }
    }

    public void OnClickUseImageButton()
    {
        if (!gameManager.selectedMeme.isLocked) {
            gameManager.menuCanvas.gameObject.SetActive(false);
            gameManager.memeCanvas.gameObject.SetActive(true);
            gameManager.memeButton.gameObject.SetActive(true);
            // AdsManager.Instance.bannerView.Hide();
            //AdsManager.Instance.ShowInterstitialAd();
            gameManager.AdShowCounter++;
            foreach (RectTransform meme in gameManager.memeCanvas.GetComponentsInChildren<Transform>(true))
            {
                if (meme.name == "Meme" + gameManager.SelectedName)
                {
                    meme.gameObject.SetActive(true);
                }
            }
        }
        else if (gameManager.coins >= (int)gameManager.selectedMeme.currentPrice)
        {
            UnlockCanvasActivate();
        }
        else
        {
            MenuCanvasActivate(false, false, true);
        }
    }

    public void Unlock()
    {
        gameManager.Coins -= (int)gameManager.selectedMeme.currentPrice;
        gameManager.selectedMeme.isLocked = false;
        gameManager.selectedMeme.currentPrice = Price.PriceFree;
        DataManager.Instance.Save();
        Object.Destroy(gameManager.imagesGroup.GetChild(int.Parse(gameManager.SelectedName) - 1).GetChild(0).gameObject);
        UnlockCanvasActivate(true, true, false);
        gameManager.useImageButtonText.text = "USE IMAGE";
    }

    public void CancelUnlock()
    {
        UnlockCanvasActivate(true, true, false);
    }

    public void OnClickOkButton()
    {
        MenuCanvasActivate();
    }

    public void OnClickCloseButton()
    {
        Application.Quit();
    }


    public void OnClickCoinsButton()
    {
        IAPCanvasActivate();
    }

    public void OnClickIAPCanvasCloseButton()
    {
        IAPCanvasActivate(true, true, false);
    }

    void MenuCanvasActivate(bool interactible = true, bool blocksRaycasts = true, bool popupCanvas = false)
    {
        gameManager.menuCanvas.gameObject.GetComponent<CanvasGroup>().interactable = interactible;
        gameManager.menuCanvas.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = blocksRaycasts;
        gameManager.popupCanvas.gameObject.SetActive(popupCanvas);
    }

    void UnlockCanvasActivate(bool interactible = false, bool blocksRaycasts = false, bool popupCanvas = true)
    {
        gameManager.menuCanvas.gameObject.GetComponent<CanvasGroup>().interactable = interactible;
        gameManager.menuCanvas.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = blocksRaycasts;
        gameManager.unlockCanvas.gameObject.SetActive(popupCanvas);
        gameManager.unlockPopupText.text = "BUY THIS IMAGE FOR " + ((int)gameManager.selectedMeme.currentPrice).ToString() + " COINS?";
    }

    public void IAPCanvasActivate(bool interactible = false, bool blocksRaycasts = false, bool popupCanvas = true)
    {
        gameManager.menuCanvas.gameObject.GetComponent<CanvasGroup>().interactable = interactible;
        gameManager.menuCanvas.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = blocksRaycasts;
        gameManager.iapCanvas.gameObject.SetActive(popupCanvas);
    }

}
