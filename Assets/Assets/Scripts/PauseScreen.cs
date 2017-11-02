using System.Collections;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PauseScreen : MonoBehaviour {
    public Transform PauseCanvas;
    public Transform TaskList;
    public Transform MemoryFile;
    void Update () {
        if (Input.GetKeyDown (KeyCode.Escape)) {
            ResumeTask ();
        }
    }
    public void ResumeTask () {
        if (!PauseCanvas.gameObject.activeInHierarchy) {
            PauseCanvas.gameObject.SetActive (true);
            EnablePausePanel();
            Time.timeScale = 0;
        } else {
            PauseCanvas.gameObject.SetActive (false);
            Time.timeScale = 1;
        }
    }
    public void SaveTask () {
        if (!MemoryFile.gameObject.activeInHierarchy) {
            MemoryFile.gameObject.SetActive(true);
            TaskList.gameObject.SetActive(false);
        }
    }
    public void LoadTask () {
        if (!MemoryFile.gameObject.activeInHierarchy) {
            MemoryFile.gameObject.SetActive(true);
            TaskList.gameObject.SetActive(false);
        }
    }
    public void EnablePausePanel () {
        TaskList.gameObject.SetActive(true);
        MemoryFile.gameObject.SetActive(false);
    }
}