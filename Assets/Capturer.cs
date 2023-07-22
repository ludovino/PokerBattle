using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Capturer : MonoBehaviour
{
    public RenderTexture tex;
    [Button]
    public void CaputreCard()
    {
        var cam = Camera.main;

        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = tex;
        cam.targetTexture = tex;
        cam.Render();

        var tex2d = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, false, true);
        Debug.Log("texture initialised");
        Debug.Log("start copy");
        tex2d.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
        tex2d.Apply();
        Debug.Log("end copy");
        Debug.Log("start encode");
        byte[] bytes = tex2d.EncodeToPNG();
        Debug.Log("end encode");
        Debug.Log("start write");

        using var file = File.OpenWrite(Application.dataPath + "/Sprites/RenderedCards/10H.png");
        file.Write(bytes);
        file.Close();
        Debug.Log("end write");
        cam.targetTexture = null;
        RenderTexture.active = currentRT;
    }
}
