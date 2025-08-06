using UnityEngine;
using UnityEditor;
using System.IO;

public class ExportSlicedSpritesPOT {
    [MenuItem("Tools/Export Sliced Sprites (POT)")]
    static void ExportSpritesPOT() {
        Texture2D texture = Selection.activeObject as Texture2D;

        if (texture == null) {
            Debug.LogError("No texture selected!");
            return;
        }

        string path = AssetDatabase.GetAssetPath(texture);
        Object[] sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(path);

        if (sprites == null || sprites.Length == 0) {
            Debug.LogWarning("No sliced sprites found in the selected asset.");
            return;
        }

        string folder = Path.GetDirectoryName(path);
        string exportFolder = Path.Combine(folder, texture.name + "_ExtractedPOT");
        Directory.CreateDirectory(exportFolder);

        foreach (var obj in sprites) {
            Sprite sprite = obj as Sprite;
            if (sprite == null) continue;

            Rect rect = sprite.rect;
            int width = (int)rect.width;
            int height = (int)rect.height;

            // Get original pixels
            Color[] pixels = sprite.texture.GetPixels(
                (int)rect.x,
                (int)rect.y,
                width,
                height
            );

            // Create original-sized texture
            Texture2D original = new Texture2D(width, height, TextureFormat.RGBA32, false);
            original.SetPixels(pixels);
            original.Apply();

            // Calculate POT size
            int potWidth = Mathf.NextPowerOfTwo(width);
            int potHeight = Mathf.NextPowerOfTwo(height);

            // Create POT texture and center the original inside it
            Texture2D potTex = new Texture2D(potWidth, potHeight, TextureFormat.RGBA32, false);
            Color[] empty = new Color[potWidth * potHeight];
            for (int i = 0; i < empty.Length; i++) empty[i] = Color.clear;
            potTex.SetPixels(empty);

            // Copy pixels to center
            int offsetX = (potWidth - width) / 2;
            int offsetY = (potHeight - height) / 2;
            potTex.SetPixels(offsetX, offsetY, width, height, original.GetPixels());
            potTex.Apply();

            // Save as PNG
            byte[] png = potTex.EncodeToPNG();
            string filePath = Path.Combine(exportFolder, sprite.name + ".png");
            File.WriteAllBytes(filePath, png);
        }

        AssetDatabase.Refresh();
        Debug.Log("Exported sliced sprites as POT to: " + exportFolder);
    }
}
