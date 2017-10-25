using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameMenu
{
    public class GameMenuOptionText : MonoBehaviour
    {
        //List of options
        public enum Options : int { NewGame = 1, LoadGame = 2, Settings = 3, Quit = 4 };
        static public Options userOption;
        /*****************************/
        //GameTitle and Menu
        Text GameTitle;
        Text[] OptionArr = new Text[4];
        /*****************************/
        bool isChosen, isFade; //check whether an option is chosen / check whether fade process is completed
        const int FadeInDuration = 6; //6 seconds
        const int fadingDuration = 5; //5 seconds
        const int fastFadingDuration = 1; //1 seconds
        float over1 = 0; //interpolants between 0 and 1
        GameObject BlackBackground; //Black background gameObject
        Light AreaLight, DirectionalLight; //Reference to light
        float ALInitialIntensity, DLInitialIntensity; //Inital value of lights
        //GameObject for settings, size not defined!
        Text ConfigurationOptionText;
        // Use this for initialization
        void Start()
        {
            userOption = Options.NewGame;
            // Deadly Maze
            GameTitle = GameObject.Find("GameTitle").GetComponent<UnityEngine.UI.Text>();
            // start new story
            OptionArr[0] = GameObject.Find("NewGame").GetComponent<UnityEngine.UI.Text>();
            OptionArr[1] = GameObject.Find("LoadGame").GetComponent<UnityEngine.UI.Text>();
            OptionArr[2] = GameObject.Find("Settings").GetComponent<UnityEngine.UI.Text>();
            OptionArr[3] = GameObject.Find("Quit").GetComponent<UnityEngine.UI.Text>();
            // not chose anything yet
            isChosen = false;
            isFade = true;
            BlackBackground = GameObject.Find("BlackBackground");
            AreaLight = GameObject.Find("Area Light").GetComponent<Light>();
            ALInitialIntensity = 0.36f;
            DirectionalLight = GameObject.Find("Directional Light").GetComponent<Light>();
            DLInitialIntensity = 0.6f;
            Color tmp = BlackBackground.GetComponent<Renderer>().material.color;
            tmp.a = 0;
            BlackBackground.GetComponent<Renderer>().material.color = tmp;
            ConfigurationOptionText = GameObject.Find("ConfigurationOptionText").GetComponent<UnityEngine.UI.Text>();
            tmp = ConfigurationOptionText.color;
            tmp.a = 0;
            ConfigurationOptionText.color = tmp;
        }
        // Update is called once per frame
        void Update()
        {
            //First Fade In
            #region FadeInProcess
            if (isFade)
            {
                over1 += Time.deltaTime / fadingDuration;
                if (over1 <= 1)
                {
                    //fading for object
                    Color tmp = GameTitle.color;
                    tmp.a = Mathf.Lerp(0, 1, over1);
                    GameTitle.color = tmp;
                    tmp = BlackBackground.GetComponent<Renderer>().material.color;
                    tmp.a = Mathf.Lerp(1, 0, over1);
                    BlackBackground.GetComponent<Renderer>().material.color = tmp;
                    for (int i = 0; i < 4; i++)
                    {
                        tmp = OptionArr[i].color;
                        tmp.a = Mathf.Lerp(0, 1, over1);
                        OptionArr[i].color = tmp;
                    }
                    //lowering intensity for lights
                    float intensity = Mathf.Lerp(0, ALInitialIntensity, over1);
                    AreaLight.intensity = intensity;
                    intensity = Mathf.Lerp(0, DLInitialIntensity, over1);
                    DirectionalLight.intensity = intensity;
                }
                else
                    isFade = false;
            }
            #endregion
            #region SettingsMenu
            else if (!isChosen)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) && userOption != Options.NewGame)
                {
                    OptionArr[(int)userOption - 1].color = Color.white;
                    userOption = (Options)((int)userOption - 1);
                }
                if (Input.GetKeyDown(KeyCode.DownArrow) && userOption != Options.Quit)
                {
                    OptionArr[(int)userOption - 1].color = Color.white;
                    userOption = (Options)((int)userOption + 1);
                }
                BlackWhiteTransition(ref OptionArr[(int)userOption - 1]);
                //If chosen through enter button, change to new scene
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    isChosen = true;
                    over1 = 0.0f;
                    isFade = false;
                }
            }
            #endregion
            #region NewGame
            else if (userOption == Options.NewGame)
            {
                //this is where fading process "for new game" will happen
                over1 += Time.deltaTime / fadingDuration;
                if (over1 <= 1)
                {
                    //fading for object
                    Color tmp = GameTitle.color;
                    tmp.a = Mathf.Lerp(1, 0, over1);
                    GameTitle.color = tmp;
                    tmp = BlackBackground.GetComponent<Renderer>().material.color;
                    tmp.a = Mathf.Lerp(0, 1, over1);
                    BlackBackground.GetComponent<Renderer>().material.color = tmp;
                    for (int i = 0; i < 4; i++)
                    {
                        tmp = OptionArr[i].color;
                        tmp.a = Mathf.Lerp(1, 0, over1);
                        OptionArr[i].color = tmp;
                    }
                    //lowering intensity for lights
                    float intensity = Mathf.Lerp(ALInitialIntensity, 0, over1);
                    AreaLight.intensity = intensity;
                    intensity = Mathf.Lerp(DLInitialIntensity, 0, over1);
                    DirectionalLight.intensity = intensity;
                }
            }
            #endregion
            #region LoadGame
            else if (userOption == Options.LoadGame)
            {
                //TODO
            }
            #endregion
            #region Settings
            else if (userOption == Options.Settings)
            {
               
            }
            #endregion
            #region Quit
            else if (userOption == Options.Quit)
            {
                Application.Quit();
            }
            #endregion
        }
        void BlackWhiteTransition(ref Text option)
            {
                //Color.Lerp: bao gồm 2 màu + hệ số xác định vị trí màu giữa 2 khoảng đó-> cần phải thay đổi liên tục bằng PingPongs
                option.color = Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time, 1));
            }
    }
}
