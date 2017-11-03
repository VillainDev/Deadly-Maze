using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class FinishScreen : MonoBehaviour {
    public AudioMixerSnapshot inFinish;
    // finishing canvas
    // appearing when reach finishing point in the ground
    public Transform FinishCanvas;
    // HUD canvas which shows player health bar and gun
    public Transform HUD;
    // memory panel: shows slot to save progress
    public Transform MemoryFile;
    // AUDIO REFERENCE
    // bpm: variable holds the speed of sound on. Developer can verify outside script
    // m_TransitionIn: the spped sound on. Dev cannot modify this
    public float bpm = 128;
    private float m_TransitionIn;
    void Start () {
        m_TransitionIn = 60 / bpm;
    }
    /// <summary>
    ///  when player reach finish point (triggered zone)
    /// 1. Finish theme will be on
    /// 2. Finish panel appears
    /// 3. HUD (healthbar and gun) disappears
    /// </summary>
    /// <param name="other">
    /// our player
    /// </param>
    void OnTriggerEnter (Collider other) {
        // get in finish zone, compare to the player
        if (other.CompareTag ("FinishZone")) {
            // sound on
            inFinish.TransitionTo (m_TransitionIn);
            // screen freeze
            Time.timeScale = 0;
            // show finish canvas and hide HUD canvas
            if (!FinishCanvas.gameObject.activeInHierarchy) {
                FinishCanvas.gameObject.SetActive (true);
                HUD.gameObject.SetActive (false);
            }
        }
    }
    /// <summary>
    /// Finish canvas contains
    /// 1. Continue button
    /// 2. Save game button
    /// 3. Main menu button
    /// 4. Stage progress
    /// </summary>
    public void ContinueTask () {

    }
    /// <summary>
    /// Finish canvas contains
    /// 1. Continue button
    /// 2. Save game button
    /// 3. Main menu button
    /// 4. Stage progress
    /// </summary>
    public void SaveTask () {

    }
    /// <summary>
    /// Finish canvas contains
    /// 1. Continue button
    /// 2. Save game button
    /// 3. Main menu button
    /// 4. Stage progress
    /// </summary>
    public void MainMenuTask () {

    }
}