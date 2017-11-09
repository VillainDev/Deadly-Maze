using System.Collections;
using UnityEngine;

public class PauseScreen : MonoBehaviour {
    /// <summary>
    /// Pause canvas appears when user press "Esc"
    /// </summary>
    public Transform PauseCanvas;
    /// <summary>
    /// Finish canvas appears when user reach finish zone
    /// At this time, pause canvas deactivated
    /// </summary>
    public Transform FinishCanvas;
    /// <summary>
    /// Tasklist contains list of buttons
    /// 1. Resume button
    /// 2. Save button
    /// 3. Load button
    /// 4. Option button
    /// 5. Main menu button
    /// </summary>
    public Transform TaskList;
    /// <summary>
    /// Memory file contains memory slot to save or load progress
    /// </summary>
    public Transform MemoryFile;
    void Update () {
        if (!FinishCanvas.gameObject.activeInHierarchy) {
            if (Input.GetKeyDown (KeyCode.Escape))
                ResumeTask ();
        }
    }
    /// <summary>
    /// Resume task will back to game play
    /// </summary>
    public void ResumeTask () {
        if (!PauseCanvas.gameObject.activeInHierarchy) {
            PauseCanvas.gameObject.SetActive (true);
            EnablePausePanel ();
            Time.timeScale = 0;
        } else {
            PauseCanvas.gameObject.SetActive (false);
            Time.timeScale = 1;
        }
    }
    /// <summary>
    /// Save task will store your game progress in game slot.
    /// User can store multiple slot, or overwrite slot
    /// at any time in stage
    /// </summary>
    public void SaveTask () {
        if (!MemoryFile.gameObject.activeInHierarchy) {
            MemoryFile.gameObject.SetActive (true);
            TaskList.gameObject.SetActive (false);
        }
    }
    /// <summary>
    /// Load task will read your game progress in game slot.
    /// User can read in any slot, although that slot does not load currently
    /// at any time in stage
    /// </summary>
    public void LoadTask () {
        if (!MemoryFile.gameObject.activeInHierarchy) {
            MemoryFile.gameObject.SetActive (true);
            TaskList.gameObject.SetActive (false);
        }
    }
    /// <summary>
    /// When reaching the memory slot panel,
    /// Click "Back" button to return to pause panel
    /// </summary>
    public void EnablePausePanel () {
        TaskList.gameObject.SetActive (true);
        MemoryFile.gameObject.SetActive (false);
    }
}