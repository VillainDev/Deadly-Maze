using UnityEngine;
using System.Collections;

public class SwitchScene : MonoBehaviour {
    public void GoToMainMenu (string SceneName) {
        Application.LoadLevel (SceneName);
    }
}