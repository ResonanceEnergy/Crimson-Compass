using UnityEngine;
using UnityEditor;
using System.IO;

public class PlaceholderAssetGenerator : EditorWindow
{
    [MenuItem("Tools/Generate Placeholder Assets")]
    public static void GenerateAssets()
    {
        GenerateUISprites();
        GenerateAudio();
        AssetDatabase.Refresh();
        Debug.Log("Placeholder assets generated!");
    }

    static void GenerateUISprites()
    {
        string spritesPath = "Assets/Sprites/UI";

        // Button Normal
        CreateColoredTexture(200, 50, new Color(0.3f, 0.3f, 0.3f), Path.Combine(spritesPath, "ButtonNormal.png"));
        CreateColoredTexture(200, 50, new Color(0.4f, 0.4f, 0.4f), Path.Combine(spritesPath, "ButtonHover.png"));
        CreateColoredTexture(200, 50, new Color(0.2f, 0.2f, 0.2f), Path.Combine(spritesPath, "ButtonPressed.png"));

        // Panel Background
        CreateColoredTexture(400, 300, new Color(0.1f, 0.1f, 0.15f, 0.9f), Path.Combine(spritesPath, "PanelBackground.png"));

        // UI Elements
        CreateColoredTexture(32, 32, new Color(0.8f, 0.8f, 0.8f), Path.Combine(spritesPath, "IconPlaceholder.png"));
    }

    static void CreateColoredTexture(int width, int height, Color color, string path)
    {
        Texture2D texture = new Texture2D(width, height);
        Color[] colors = new Color[width * height];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = color;
        }
        texture.SetPixels(colors);
        texture.Apply();

        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(path, bytes);

        // Set texture import settings
        AssetDatabase.ImportAsset(path);
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer != null)
        {
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.mipmapEnabled = false;
            importer.filterMode = FilterMode.Point;
            importer.SaveAndReimport();
        }

        Object.DestroyImmediate(texture);
    }

    static void GenerateAudio()
    {
        string audioPath = "Assets/Audio";

        if (!Directory.Exists(audioPath))
        {
            Directory.CreateDirectory(audioPath);
        }

        // Generate simple beep sound
        CreateBeepAudio(Path.Combine(audioPath, "UIButton.wav"), 0.5f, 440f, 0.3f);
        CreateBeepAudio(Path.Combine(audioPath, "UIBackgroundMusic.wav"), 30f, 220f, 0.1f);
    }

    static void CreateBeepAudio(string path, float duration, float frequency, float volume)
    {
        int sampleRate = 44100;
        int sampleCount = (int)(sampleRate * duration);
        float[] samples = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            float t = (float)i / sampleRate;
            samples[i] = Mathf.Sin(2f * Mathf.PI * frequency * t) * volume * Mathf.Exp(-t * 2f); // Fading envelope
        }

        AudioClip clip = AudioClip.Create(Path.GetFileNameWithoutExtension(path), sampleCount, 1, sampleRate, false);
        clip.SetData(samples, 0);

        // Save as WAV
        SaveAudioClipToWAV(clip, path);

        AssetDatabase.ImportAsset(path);
    }

    static void SaveAudioClipToWAV(AudioClip clip, string path)
    {
        // Simple WAV writer for mono 16-bit
        using (FileStream fileStream = new FileStream(path, FileMode.Create))
        using (BinaryWriter writer = new BinaryWriter(fileStream))
        {
            int sampleCount = clip.samples;
            int sampleRate = clip.frequency;

            // WAV header
            writer.Write("RIFF".ToCharArray());
            writer.Write(36 + sampleCount * 2); // File size
            writer.Write("WAVE".ToCharArray());
            writer.Write("fmt ".ToCharArray());
            writer.Write(16); // Subchunk1Size
            writer.Write((short)1); // AudioFormat (PCM)
            writer.Write((short)1); // NumChannels
            writer.Write(sampleRate); // SampleRate
            writer.Write(sampleRate * 2); // ByteRate
            writer.Write((short)2); // BlockAlign
            writer.Write((short)16); // BitsPerSample
            writer.Write("data".ToCharArray());
            writer.Write(sampleCount * 2); // Subchunk2Size

            // Audio data
            float[] samples = new float[sampleCount];
            clip.GetData(samples, 0);
            for (int i = 0; i < sampleCount; i++)
            {
                short sample = (short)(samples[i] * short.MaxValue);
                writer.Write(sample);
            }
        }
    }
}