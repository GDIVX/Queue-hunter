using System;

namespace Tzipory.Tools.Enums
{
    [Flags]
    public enum UIGroup
    {
        None = 0,
        MetaUI = 1,
        GameUI = 2,
        LoadingScreenUI = 4,
        MainMenuUI = 8,
        PopupUI = 16,
        PauseUI = 32,
        GameOverlayUI = 64,
        EndGameUI = 128,
        SettingsUI = 256,
        CampFireUI = 512,
        MainCampUI = 1024,
    }
}