using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameMenu
{
    public class GameMenuOptionText : MonoBehaviour
    {
        enum Options : int { NewGame = 1, LoadGame = 2, Settings = 3, Quit = 4 };
        static Options userOption;
        Text GameTitle;
        Text[] OptionArr = new Text[4];
        bool[] transitionOption = new bool[4];
        bool isChosen;
        int fadingDuration = 5; //5 giay
        float over1 = 0;
        GameObject BlackBackground;
        Light AreaLight, DirectionalLight;
        float ALInitialIntensity, DLInitialIntensity;
        // Use this for initialization
        void Start()
        {
            userOption = Options.NewGame;
            GameTitle = GameObject.Find("GameTitle").GetComponent<UnityEngine.UI.Text>();
            OptionArr[0] = GameObject.Find("NewGame").GetComponent<UnityEngine.UI.Text>();
            OptionArr[1] = GameObject.Find("LoadGame").GetComponent<UnityEngine.UI.Text>();
            OptionArr[2] = GameObject.Find("Settings").GetComponent<UnityEngine.UI.Text>();
            OptionArr[3] = GameObject.Find("Quit").GetComponent<UnityEngine.UI.Text>();
            isChosen = false;
            BlackBackground = GameObject.Find("BlackBackground");
            AreaLight = GameObject.Find("Area Light").GetComponent<Light>();
            ALInitialIntensity = AreaLight.intensity;
            DirectionalLight = GameObject.Find("Directional Light").GetComponent<Light>();
            DLInitialIntensity = DirectionalLight.intensity;
            Color tmp = BlackBackground.GetComponent<Renderer>().material.color;
            tmp.a = 0;
            BlackBackground.GetComponent<Renderer>().material.color = tmp;
        }

        // Update is called once per frame
        void Update()
        {
            if (!isChosen)
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
                BlackWhiteTransition(ref OptionArr[(int)userOption - 1], ref transitionOption[(int)userOption - 1]);
                //If chosen through enter button, change to new scene
                if (Input.GetKeyDown(KeyCode.Return))
                    isChosen = true;
            }
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
            else if (userOption == Options.LoadGame)
            {
                //TODO
            }
            else if (userOption == Options.Settings)
            {
                //TODO
            }
            else if (userOption == Options.Quit)
            {
                Application.Quit();
            }
        }

        void BlackWhiteTransition(ref Text option, ref bool state)
        {
            //Color.Lerp: bao gồm 2 màu + hệ số xác định vị trí màu giữa 2 khoảng đó-> cần phải thay đổi liên tục bằng PingPongs
            option.color = Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time, 1));
        }

    }
}
