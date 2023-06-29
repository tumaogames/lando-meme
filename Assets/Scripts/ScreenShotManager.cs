using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;


public class ScreenShotManager : MonoBehaviour
{
    public TMP_Text timer;
    int i = 0;
    public int j = 0;
    protected const string MEDIA_STORE_IMAGE_MEDIA = "android.provider.MediaStore$Images$Media";
    protected static AndroidJavaObject m_Activity;

    public void Start()
    {
    }
    private void Update()
    {
    }

    public void SaveScreenShot(bool share)
    {
        GameManager.Instance.memeButton.SetActive(false);
        StartCoroutine(CaptureScreenShoot(share));
    }

    IEnumerator CaptureScreenShoot(bool share)
    {
        yield return new WaitForEndOfFrame();
        Texture2D texture = ScreenCapture.CaptureScreenshotAsTexture();
        var texture1 = Resize(texture, 2048, 3072);
        SaveImageToGallery(texture1, "Meme", "Lando meme image");
        if (share)
        {

            // Share on WhatsApp only, if installed (Android only)
            if (NativeShare.TargetExists("com.facebook.katana")) 
            {
                new NativeShare().AddFile(texture1).AddTarget("com.facebook.katana").Share();
            } else
            {
                ShareMeme(texture1);
            }
        }
        StartCoroutine(EnableMemeButton(1));
        Object.Destroy(texture1);
    }

    public void ShareMeme(Texture2D shareMeme)
    {
        new NativeShare().AddFile(shareMeme).Share();
    }

    public IEnumerator EnableMemeButton(int frameCount)
    {
        while (frameCount > 0)
        {
            frameCount--;
            yield return null;
        }
        GameManager.Instance.memeButton.SetActive(true);
    }

    public static Texture2D Resize(Texture2D source, int newWidth, int newHeight)
    {
        source.filterMode = FilterMode.Bilinear;
        RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight);
        rt.filterMode = FilterMode.Bilinear;
        RenderTexture.active = rt;
        Graphics.Blit(source, rt);
        Texture2D nTex = new Texture2D(newWidth, newHeight);
        nTex.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
        nTex.Apply();
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(rt);
        return nTex;
    }

    protected static string SaveImageToGallery(Texture2D a_Texture, string a_Title, string a_Description)
    {
        using (AndroidJavaClass mediaClass = new AndroidJavaClass(MEDIA_STORE_IMAGE_MEDIA))
        {
            using (AndroidJavaObject contentResolver = Activity.Call<AndroidJavaObject>("getContentResolver"))
            {
                AndroidJavaObject image = Texture2DToAndroidBitmap(a_Texture);
                return mediaClass.CallStatic<string>("insertImage", contentResolver, image, a_Title, a_Description);
            }
        }
    }

    protected static AndroidJavaObject Texture2DToAndroidBitmap(Texture2D a_Texture)
    {
        byte[] encodedTexture = a_Texture.EncodeToPNG();
        using (AndroidJavaClass bitmapFactory = new AndroidJavaClass("android.graphics.BitmapFactory"))
        {
            return bitmapFactory.CallStatic<AndroidJavaObject>("decodeByteArray", encodedTexture, 0, encodedTexture.Length);
        }
    }

    protected static AndroidJavaObject Activity
    {
        get
        {
            if (m_Activity == null)
            {
                AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                m_Activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            }
            return m_Activity;
        }
    }
}
