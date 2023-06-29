using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Price { PriceFree = 0, Price100 = 100, Price200 = 200};
[System.Serializable]
public class MemeImage
{
    [SerializeField]
    public Price currentPrice;
    [SerializeField]
    public string name;
    [SerializeField]
    public bool isLocked;
    [SerializeField]
    public Sprite memeSprite;
    [SerializeField]
    public bool isSelected;

    public static MemeImage CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<MemeImage>(jsonString);
    }
}
