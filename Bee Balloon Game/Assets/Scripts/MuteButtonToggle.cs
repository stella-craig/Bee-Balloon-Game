using UnityEngine;

public class MuteButtonToggle : MonoBehaviour
{
    public GameObject muteIcon;    // Reference to the MuteIcon GameObject
    public GameObject unmuteIcon;  // Reference to the UnmuteIcon GameObject

    private bool isMuted = false;  // Tracks the mute state

    private void Start()
    {
        // Ensure the initial state is correct
        if (muteIcon != null) muteIcon.SetActive(false);  // Mute icon starts inactive
        if (unmuteIcon != null) unmuteIcon.SetActive(true);  // Unmute icon starts active
        AudioListener.volume = 1; // Ensure audio is unmuted initially
    }

    public void ToggleMute()
    {
        // Toggle the mute state
        isMuted = !isMuted;

        // Toggle the visibility of the icons
        if (muteIcon != null) muteIcon.SetActive(isMuted);
        if (unmuteIcon != null) unmuteIcon.SetActive(!isMuted);

        // Mute or unmute the audio
        AudioListener.volume = isMuted ? 0 : 1;

        // Debug logs to confirm state
        Debug.Log(isMuted ? "Audio Muted" : "Audio Unmuted");
    }
}
