using UnityEngine;

public static class ShowGUI
{
    public static bool shouldShow = false; // Control visibility of the GUI

    private static bool toggleSpeed = false;
    private static bool toggleHeadlight = false;
    private static bool toggleProtection = false;

    private static GUIStyle guiStyleBox;
    private static GUIStyle guiStyleButton;
    private static Texture2D buttonNormal;
    private static Texture2D buttonHover;

    static ShowGUI()
    {
        InitializeStyles();
    }

    private static void InitializeStyles()
    {
        // Create textures
        buttonNormal = new Texture2D(1, 1);
        buttonNormal.SetPixel(0, 0, HexToColor("#5B4136"));
        buttonNormal.Apply();

        buttonHover = new Texture2D(1, 1);
        buttonHover.SetPixel(0, 0, HexToColor("#947022"));
        buttonHover.Apply();

        // Box style
        guiStyleBox = new GUIStyle(GUI.skin.box);
        guiStyleBox.normal.background = buttonNormal;
        guiStyleBox.normal.textColor = HexToColor("#F4B819");
        guiStyleBox.fontSize = 16;
        guiStyleBox.alignment = TextAnchor.UpperCenter;

        // Button style
        guiStyleButton = new GUIStyle(GUI.skin.button);
        guiStyleButton.normal.textColor = HexToColor("#F4B819");
        guiStyleButton.normal.background = buttonNormal;
        guiStyleButton.hover.textColor = HexToColor("#FFFF16");
        guiStyleButton.hover.background = buttonHover;
        guiStyleButton.fontSize = 14;
        guiStyleButton.alignment = TextAnchor.MiddleCenter;
    }

    public static void DrawGUI()
    {
        if (!shouldShow) return;

        float xPos = Screen.width / 2 - 100;
        float yPos = Screen.height / 2 - 75;

        GUI.Box(new Rect(xPos, yPos, 200, 150), "Casper's QOL Menu", guiStyleBox);

        toggleSpeed = GUI.Toggle(new Rect(xPos + 10, yPos + 40, 180, 30), toggleSpeed, "Speed", guiStyleButton);
        toggleHeadlight = GUI.Toggle(new Rect(xPos + 10, yPos + 75, 180, 30), toggleHeadlight, "Headlight", guiStyleButton);
        toggleProtection = GUI.Toggle(new Rect(xPos + 10, yPos + 110, 180, 30), toggleProtection, "Protection", guiStyleButton);
    }

    private static Color HexToColor(string hex)
    {
        hex = hex.Replace("#", "");
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        return new Color32(r, g, b, 255);
    }
}
