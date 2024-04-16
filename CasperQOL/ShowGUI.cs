using CasperQOL;
using System.IO;
using System.Reflection;
using UnityEngine;

public static class ShowGUI
{
    public static bool shouldShow = false;

    private static Texture2D lightestBackground;
    private static Texture2D lightBackground;
    private static Texture2D purpleBackground;

    // test

    private static Texture2D guiBoxNormal;

    public static void DrawGUI()
    {
        if (!shouldShow) return;

        lightestBackground = CreateColorTexture("#3A3A58");
        lightBackground = CreateColorTexture("#303040");
        purpleBackground = CreateColorTexture("#800080");

        float xPos = Screen.width / 2 - 100;
        float yPos = Screen.height / 2 - 90;
        int boxWidth = 208; // 200
        int boxHeight = 240;

        GUIStyle guiStyleBox = new GUIStyle()
        {
            fontSize = 16,
            normal = { textColor = Color.white, background = guiBoxNormal },
            alignment = TextAnchor.UpperCenter,
            padding = new RectOffset(0, 0, 10, 0) // Adjust the top padding here (10 in this example)
        };

        // Button style for normal state
        GUIStyle guiStyleButton = new GUIStyle()
        {
            normal = { background = lightBackground, textColor = HexToColor("#FFA500") },
            hover = { background = lightBackground, textColor = HexToColor("#E59400") },
            active = { background = lightBackground, textColor = HexToColor("#E59400") },
            focused = { background = lightBackground, textColor = HexToColor("#FFA500") },
            fontSize = 14,
            alignment = TextAnchor.MiddleCenter
        };

        // Button style for selected state
        GUIStyle guiStyleButtonSelected = new GUIStyle()
        {
            normal = { background = purpleBackground, textColor = HexToColor("#FFA500") },
            hover = { background = purpleBackground, textColor = HexToColor("#FFA500") },
            active = { background = purpleBackground, textColor = HexToColor("#FFA500") },
            focused = { background = purpleBackground, textColor = HexToColor("#FFA500") },
            onNormal = { background = purpleBackground, textColor = HexToColor("#FFA500") },
            onHover = { background = purpleBackground, textColor = HexToColor("#FFA500") },
            onActive = { background = purpleBackground, textColor = HexToColor("#FFA500") },
            onFocused = { background = purpleBackground, textColor = HexToColor("#FFA500") }
        };


        // Outer GUI Box for the border
        // GUI.Box(new Rect(xPos - borderSize, yPos - borderSize, boxWidth + 2 * borderSize, boxHeight + 2 * borderSize), GUIContent.none, guiStyleBorder);


        // Main GUI Box
        //GUI.Box(new Rect(xPos, yPos, boxWidth, boxHeight), "Casper's QOL Menu", guiStyleBox);
        //test
        GUI.Box(new Rect(xPos, yPos, boxWidth, boxHeight), "Casper's QOL Menu", guiStyleBox);

        // Concrete Speed Toggle
        SharedState.speedToggle = GUI.Toggle(new Rect(xPos + 10, yPos + 30, 180, 30), SharedState.speedToggle, "Concrete Speed", SharedState.speedToggle ? guiStyleButtonSelected : guiStyleButton);
        if (GUI.changed)
        {
            Debug.Log("Speed Toggle changed: " + SharedState.speedToggle);
        }

        SharedState.lightToggle = GUI.Toggle(new Rect(xPos + 10, yPos + 75, 180, 30), SharedState.lightToggle, "Headlight", SharedState.lightToggle ? guiStyleButtonSelected : guiStyleButton);
        if (GUI.changed)
        {
            Debug.Log("Light Toggle changed: " + SharedState.lightToggle);
        }

        SharedState.protectToggle = GUI.Toggle(new Rect(xPos + 10, yPos + 110, 180, 30), SharedState.protectToggle, "Protection", SharedState.protectToggle ? guiStyleButtonSelected : guiStyleButton);
        if (GUI.changed)
        {
            Debug.Log("Protection Toggle changed: " + SharedState.protectToggle);
        }

    }

    private static Color HexToColor(string hex)
    {
        hex = hex.Replace("#", "");
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        return new Color32(r, g, b, 255);
    }

    private static Texture2D CreateColorTexture(string hex)
    {
        Color color = HexToColor(hex);
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();
        return texture;
    }

    //test area
    public static void LoadImages()
    {
        LoadImage("CasperQOL.Images.Background208x250.png", ref guiBoxNormal);

    }
    private static void LoadImage(string path, ref Texture2D output)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();

        using (Stream stream = assembly.GetManifestResourceStream(path))
        {
            if (stream == null)
            {
                Debug.LogError($"Could not find image with path '{path}'");
                return;
            }

            using (MemoryStream memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                byte[] fileData = memoryStream.ToArray();

                output = new Texture2D(2, 2);
                output.LoadImage(fileData);
            }
        }
    }
}
