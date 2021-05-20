using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class SpriteSaveToPng
{
    const string selectedSpriteSavePngMeshPath = "Assets/Create/2D/선택한 이미지 저장(PNG)";
    [MenuItem(selectedSpriteSavePngMeshPath, true, 0)]
    private static bool ValidateMenuSavePng()
    {
        UnityEngine.Object[] selected = Selection.objects;
        foreach(var item in selected)
        {
            if (item.GetType() == typeof(Sprite))
                return true;
        }    

        return false;
    }

    [MenuItem(selectedSpriteSavePngMeshPath, false, 0)]
    static public void SavePng()
    {
        UnityEngine.Object[] selected = Selection.objects;

        HashSet<string> textureFilePaths = new HashSet<string>();
        HashSet<string> selectedSprites = new HashSet<string>();
        for (int i = 0; i < selected.Length; i++)
        {
            var item = selected[i];


            string textureFilePath = AssetDatabase.GetAssetPath(item);
            textureFilePaths.Add(textureFilePath);

            Sprite selectedSprite = item as Sprite;
            if (selectedSprite != null)
                selectedSprites.Add(selectedSprite.name);
        }

        foreach (var textureFilePath in textureFilePaths)
        {
            //// 선택된 텍스쳐가 압축되어 있다면 풀자.
            TextureImporterCompression originalCompress;

            TextureImporter importer = AssetImporter.GetAtPath(textureFilePath) as TextureImporter;
            if (importer == null)
            {
                Debug.LogWarning(string.Format("{0} texture == null", textureFilePath));
                continue;
            }
            originalCompress = importer.textureCompression;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.isReadable = true;

            AssetDatabase.ImportAsset(textureFilePath);
            EditorUtility.SetDirty(importer);

            Texture2D texture = AssetDatabase.LoadAssetAtPath(textureFilePath, typeof(Texture2D)) as Texture2D;

            string desPath = textureFilePath;
            SaveTextureToFile(texture, importer, textureFilePath, selectedSprites);
            Debug.Log(desPath);

            // 텍스쳐 압축 원상 복구
            importer.isReadable = false;
            importer.textureCompression = originalCompress;
            AssetDatabase.ImportAsset(textureFilePath);
            EditorUtility.SetDirty(importer);
        }
    }

    static void SaveTextureToFile(Texture2D texture, TextureImporter textureImporter, string filePath, HashSet<string> selectedSprites)
    {
        try
        {
            string fullPath = Path.GetFullPath(filePath);
            string saveDirPath = fullPath.Replace(Path.GetFileName(filePath), "");
            string fileNameWithoutExtionsion = Path.GetFileNameWithoutExtension(filePath);
            for (int i = 0; i < textureImporter.spritesheet.Length; i++)
            {
                var sprite = textureImporter.spritesheet[i];
                if(selectedSprites !=null && selectedSprites.Count > 0)
                {
                    if (selectedSprites.Contains(sprite.name) == false)
                        continue;
                }

                Texture2D newTexture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
                Color[] newColors = texture.GetPixels((int)sprite.rect.x,
                                                                (int)sprite.rect.y,
                                                                (int)sprite.rect.width,
                                                                (int)sprite.rect.height);

                newTexture.SetPixels(newColors);
                newTexture.Apply();


                var bytes = newTexture.EncodeToPNG();
                string savePath = saveDirPath + fileNameWithoutExtionsion + "_" + (i + 1) + ".png";
                var file = File.Open(savePath, FileMode.Create);
                var binary = new BinaryWriter(file);
                binary.Write(bytes);
                file.Close();
            }

            AssetDatabase.Refresh();
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning(string.Format("{0} {1}", filePath, ex));
        }
    }
}
