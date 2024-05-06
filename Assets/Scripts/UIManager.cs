// UIManager.cs

using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text livesText;

    // Update the lives count displayed on the UI
    public void UpdateLivesCount(int remainingLives)
    {
        livesText.text = "Lives: " + remainingLives.ToString();
    }
}
