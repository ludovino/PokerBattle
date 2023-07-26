using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Capturer : MonoBehaviour
{
    public RenderTexture tex;
    public Camera importCamera;
    public List<Suit> suits;
    public List<Face> faces;
    public TweenerCanvasCardDisplay cardDisplay;
    public string renderedCardsPath = "Sprites/RenderedCards/";
    [Button]
    public void CaputreCards()
    {
        CreateFolders();
        foreach(var suit in suits)
        {
            for(int i = 0; i<=21; i++)
            {
                CaptureCard(suit, i, null);
            }

            foreach(var face in faces)
            {
                CaptureCard(suit, 0, face);
            }

        }
    }

    public void CreateFolders()
    {
        foreach(var suit in suits)
        {
            if (suit) Directory.CreateDirectory(Path.Combine(Application.dataPath, renderedCardsPath, suit.name));
            else Directory.CreateDirectory(Path.Combine(Application.dataPath, renderedCardsPath, "blank"));
        }
    }

    public void CaptureCard(Suit suit, int value, Face face)
    {
        string numeral = GetNumeralValueName(value, face);
        var suitName = suit != null ? suit.name : "blank";
        Debug.Log(numeral);
        cardDisplay.Set(value, suit, face, true);
        Debug.Log("Get Texture");
        Texture2D tex2d = GetTexture();
        var fileName = $"{numeral}Of{suitName}.png";
        var IOpath = Path.Combine(Application.dataPath, renderedCardsPath, suitName, fileName);
        var assetPath = "Assets/" + renderedCardsPath + "/" + suitName + "/" + fileName;
        Debug.Log("Create Image");
        CreateImageAtPath(tex2d, IOpath);
        Debug.Log("Import Asset");
        ImportAsset(assetPath);
    }


    private Texture2D GetTexture()
    {
        RenderTexture.active = tex;
        importCamera.targetTexture = tex;
        importCamera.Render();
        var tex2d = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, false, true);

        tex2d.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
        tex2d.Apply();

        return tex2d;
    }
    private string GetNumeralValueName(int value, Face face)
    {
        if (face != null) return face.numeral;
        return value.ToString("D2");
    }

    public void CreateImageAtPath(Texture2D texture, string path)
    {
        var bytes = texture.EncodeToPNG();
        using var file = File.OpenWrite(path); 
        file.Write(bytes);
        file.Close();
    }

    public void ImportAsset(string path)
    {
        AssetDatabase.Refresh();
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceSynchronousImport);
        Debug.Log(path);
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        importer.textureType = TextureImporterType.Sprite;
        importer.textureCompression = TextureImporterCompression.Uncompressed;
        importer.mipmapEnabled = false;
        importer.maxTextureSize = 128;
        var settings = new TextureImporterSettings();
        importer.ReadTextureSettings(settings);
        settings.spriteMeshType = SpriteMeshType.FullRect;
        importer.SetTextureSettings(settings);
        importer.filterMode = FilterMode.Point;
        AssetDatabase.WriteImportSettingsIfDirty(path);
    }
}
