using CasperQOL;
using UnityEngine;

public static class ShowGUI
{
    public static bool shouldShow = false; // Control visibility of the GUI

    private static GUIStyle guiStyleBox;
    private static GUIStyle guiStyleButton;
    private static Texture2D darkBackground;
    private static Texture2D lightBackground;
    private static Texture2D lightestBackground;

    static ShowGUI()
    {
        InitializeStyles();
    }

    private static void InitializeStyles()
    {
        // Create textures
        darkBackground = CreateColorTexture("#161626");
        lightBackground = CreateColorTexture("#303040");
        lightestBackground = CreateColorTexture("#3A3A58");

        // Box style
        guiStyleBox = new GUIStyle(GUI.skin.box);
        guiStyleBox.normal.background = darkBackground;
        guiStyleBox.normal.textColor = HexToColor("#5B4136");  // OrangeMid
        guiStyleBox.fontSize = 16;
        guiStyleBox.alignment = TextAnchor.UpperCenter;

        // Button style
        guiStyleButton = new GUIStyle(GUI.skin.button);
        guiStyleButton.normal.textColor = HexToColor("#5B4136");  // OrangeMid
        guiStyleButton.normal.background = lightBackground;
        guiStyleButton.hover.textColor = HexToColor("#795933");  // OrangeBright
        guiStyleButton.hover.background = lightBackground;
        guiStyleButton.fontSize = 14;
        guiStyleButton.alignment = TextAnchor.MiddleCenter;
    }

    public static void DrawGUI()
    {
        if (!shouldShow) return;

        float xPos = Screen.width / 2 - 100;
        float yPos = Screen.height / 2 - 75;

        GUI.Box(new Rect(xPos, yPos, 200, 150), "Casper's QOL Menu", guiStyleBox);

        // Update the toggles using SharedState
        SharedState.speedToggle = GUI.Toggle(new Rect(xPos + 10, yPos + 40, 180, 30), SharedState.speedToggle, "Speed", guiStyleButton);
        SharedState.lightToggle = GUI.Toggle(new Rect(xPos + 10, yPos + 75, 180, 30), SharedState.lightToggle, "Headlight", guiStyleButton);
        SharedState.protectToggle = GUI.Toggle(new Rect(xPos + 10, yPos + 110, 180, 30), SharedState.protectToggle, "Protection", guiStyleButton);
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
}
