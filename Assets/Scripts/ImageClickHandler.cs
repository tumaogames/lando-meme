using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ImageClickHandler : MonoBehaviour, IPointerClickHandler
{
    public GameObject memeObj;
    public SoundManager soundManager;
    // Start is called before the first frame update
    void Start()
    {
        memeObj = this.gameObject;
        soundManager = GameObject.FindObjectOfType<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData pointerEventData) 
    {
        GameManager.Instance.MemeSelected(memeObj);
        if (GameManager.Instance.images[int.Parse(memeObj.name) - 1].isLocked)
        {
            GameManager.Instance.useImageButtonText.text = "BUY IMAGE";
        } else
        {
            GameManager.Instance.useImageButtonText.text = "USE IMAGE";
        }
        soundManager.playClip();
    }
}
