using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Holds references to in game UI to be passed from <see cref="GameManager"/> to <see cref="MainMenuState"/>
/// </summary>
public class MainMenuStateReferences : MonoBehaviour
{
    public Button BTN_NewGame;
    public Button BTN_Quit;
    public AudioSource AUDIO_MenuMusic;
}
