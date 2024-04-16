using CasperQOL;
using UnityEngine;

public static class ShowGUI
{
    public static bool shouldShow = false;

    private static GUIStyle guiStyleBox;
    private static GUIStyle guiStyleButton;
    private static GUIStyle guiStyleButtonSelected;
    public static GUIStyle guiStyleLabel;
    private static GUIStyle guiStyleBorder;

    private static Texture2D lightestBackground;
    private static Texture2D lightBackground;
    private static Texture2D purpleBackground;
    private static Texture2D orangeBorder;

    static ShowGUI()
    {
        InitializeStyles();
    }

    private static void InitializeStyles()
    {
        // Create textures
        lightestBackground = CreateColorTexture("#3A3A58"); 
        lightBackground = CreateColorTexture("#303040");  
        purpleBackground = CreateColorTexture("#800080");
        orangeBorder = CreateColorTexture("#FFA500");

        guiStyleBorder = new GUIStyle(GUI.skin.box);
        guiStyleBorder.normal.background = orangeBorder;
        guiStyleBorder.border = new RectOffset(3, 3, 3, 3);

        // Box style
        guiStyleBox = new GUIStyle(GUI.skin.box);
        guiStyleBox.normal.background = lightestBackground;
        guiStyleBox.normal.textColor = HexToColor("#FFA500"); 
        guiStyleBox.fontSize = 16;
        guiStyleBox.alignment = TextAnchor.UpperCenter;

        // Button style for normal state
        guiStyleButton = new GUIStyle(GUI.skin.button);
        guiStyleButton.normal.background = lightBackground;
        guiStyleButton.normal.textColor = HexToColor("#FFA500"); 
        guiStyleButton.hover.background = lightBackground;
        guiStyleButton.hover.textColor = HexToColor("#E59400");   
        guiStyleButton.active.background = lightBackground;
        guiStyleButton.active.textColor = HexToColor("#E59400");
        guiStyleButton.focused.background = lightBackground;
        guiStyleButton.focused.textColor = HexToColor("#FFA500");
        guiStyleButton.fontSize = 14;
        guiStyleButton.alignment = TextAnchor.MiddleCenter;

        // Button style for selected state
        guiStyleButtonSelected = new GUIStyle(guiStyleButton);
        guiStyleButtonSelected.normal.background = purpleBackground;
        guiStyleButtonSelected.normal.textColor = HexToColor("#FFA500");
        guiStyleButtonSelected.hover.background = purpleBackground;
        guiStyleButtonSelected.hover.textColor = HexToColor("#FFA500");
        guiStyleButtonSelected.active.background = purpleBackground;
        guiStyleButtonSelected.active.textColor = HexToColor("#FFA500");
        guiStyleButtonSelected.focused.background = purpleBackground;
        guiStyleButtonSelected.focused.textColor = HexToColor("#FFA500");
        guiStyleButtonSelected.onNormal.background = purpleBackground;
        guiStyleButtonSelected.onNormal.textColor = HexToColor("#FFA500");
        guiStyleButtonSelected.onHover.background = purpleBackground;
        guiStyleButtonSelected.onHover.textColor = HexToColor("#FFA500");
        guiStyleButtonSelected.onActive.background = purpleBackground;
        guiStyleButtonSelected.onActive.textColor = HexToColor("#FFA500");
        guiStyleButtonSelected.onFocused.background = purpleBackground;
        guiStyleButtonSelected.onFocused.textColor = HexToColor("#FFA500");

        guiStyleLabel = new GUIStyle(GUI.skin.label);
        guiStyleLabel.normal.textColor = HexToColor("#FFA500");
        guiStyleLabel.fontSize = 16;
        guiStyleLabel.alignment = TextAnchor.MiddleCenter;
    }



    public static void DrawGUI()
    {
        if (!shouldShow) return;

        float xPos = Screen.width / 2 - 100;
        float yPos = Screen.height / 2 - 90; 
        int boxWidth = 200;
        int boxHeight = 240;
        int borderSize = 3;

        // Outer GUI Box for the border
        GUI.Box(new Rect(xPos - borderSize, yPos - borderSize, boxWidth + 2 * borderSize, boxHeight + 2 * borderSize), GUIContent.none, guiStyleBorder);


        // Main GUI Box
        GUI.Box(new Rect(xPos, yPos, boxWidth, boxHeight), "Casper's QOL Menu", guiStyleBox);

        // Concrete Speed Toggle
        SharedState.speedToggle = GUI.Toggle(new Rect(xPos + 10, yPos + 30, 180, 30), SharedState.speedToggle, "Concrete Speed", SharedState.speedToggle ? guiStyleButtonSelected : guiStyleButton);
        if (GUI.changed)
        {
            Debug.Log("Concrete Speed Toggle changed: " + SharedState.speedToggle);
        }

        // Headlight Toggle
        SharedState.lightToggle = GUI.Toggle(new Rect(xPos + 10, yPos + 70, 180, 30), SharedState.lightToggle, "Headlight Toggle", SharedState.lightToggle ? guiStyleButtonSelected : guiStyleButton);
        if (GUI.changed)
        {
            Debug.Log("Headlight Toggle changed: " + SharedState.lightToggle);
        }

        // Label for Cheats section
        GUI.Label(new Rect(xPos, yPos + 115, boxWidth, 20), "Cheats:", guiStyleLabel);

        // Infinite Ore Toggle
        SharedState.oreProtect = GUI.Toggle(new Rect(xPos + 10, yPos + 140, 180, 30), SharedState.oreProtect, "Infinite Ore", SharedState.oreProtect ? guiStyleButtonSelected : guiStyleButton);
        if (GUI.changed)
        {
            Debug.Log("Infinite Ore Toggle changed: " + SharedState.oreProtect);
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
}
