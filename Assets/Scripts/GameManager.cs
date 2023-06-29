using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<MemeImage> images;
    public RectTransform imagesGroup;
    public RectTransform _prefabImage;
    public Canvas menuCanvas;
    public Canvas memeCanvas;
    public Canvas popupCanvas;
    public Canvas unlockCanvas;
    public Canvas iapCanvas;
    public GameObject memeButton;
    public Image selectedBorder;
    public RectTransform price100ImagePrefab;
    public RectTransform price200ImagePrefab;
    RectTransform priceImage;
    RectTransform objImage;
    public Image Selector;
    public MemeImage selectedMeme;
    public string SelectedName;
    public TMP_Text useImageButtonText;
    public TMP_Text unlockPopupText;
    public Button closeButton;
    public Button watchAds;
    public TMP_Text coinsButtonText;
    public int AdShowCounter;
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
    public int coins;
    public int Coins
    {
        get { return coins; }
        set
        {
            coins = value;
            coinsButtonText.text = "Coins: " + coins;
            PlayerPrefs.SetInt("Coins", coins);
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        if (PlayerPrefs.HasKey("Coins"))
        {
            Coins = PlayerPrefs.GetInt("Coins");
        }
        SelectedName = "1";
        selectedMeme = images[int.Parse(SelectedName) - 1];
    }

    private void Start()
    {
        Invoke("GenerateImages", 1f);
        
    }

    public void GenerateImages()
    {
        images = GetComponent<GameManager>().images;
        for (int i = 0; i < images.Count; i++)
        {
            objImage = Instantiate(_prefabImage) as RectTransform;
            objImage.name = (i + 1).ToString();
            objImage.gameObject.GetComponent<Image>().sprite = images[i].memeSprite;
            objImage = LockImage(objImage, i);
            objImage.transform.SetParent(imagesGroup.transform, false);
        }
        Selector = Instantiate(selectedBorder);
        Selector.transform.SetParent(imagesGroup.GetChild(int.Parse(SelectedName) - 1).transform, false);
    }

    public void MemeSelected(GameObject Obj)
    {
        foreach (MemeImage img in images)
        {
            if (img.name == "image_" + (Obj.name))
            {
                img.isSelected = true;
                Selector.transform.SetParent(Obj.transform, false);
                SelectedName = Obj.name;
                selectedMeme = images[int.Parse(SelectedName) - 1];
            } else
            {
                img.isSelected = false;
            }

        }
    }

    RectTransform LockImage(RectTransform objLocked, int id)
    {
        if (images[id].isLocked)
        {
            priceImage = (images[id].currentPrice == Price.Price100) ? Instantiate(price100ImagePrefab) : Instantiate(price200ImagePrefab);
            priceImage.SetParent(objLocked.transform, false);
        }
        return objLocked;
    }
}
