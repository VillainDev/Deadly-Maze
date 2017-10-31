using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameMenu
{
    public class GameMenuOptionText : MonoBehaviour
    {
        #region VariableAndArray
        //List of options
        public enum Options : int { NewGame = 1, LoadGame = 2, Settings, Quit};
        Options userOption;
        bool isChosen;
        //List of setting options
        public enum SettingOptions : int { Sound = 1, BGM = 2, Fullscreen, ConsoleConfiguration, Credit, OK, Cancel};
        SettingOptions userSettingOptions;
        bool isSettingChosen;
        bool isSettingFadeIn;
        /*****************************/
        //GameTitle and Menu
        Text GameTitle;
        Text[] OptionArr = new Text[4];
        /*****************************/
        bool isFade; //check whether an option is chosen / check whether fade process is completed
        const float FadeInDuration = 6; //6 seconds
        const float IntroFadeIn = 5; //5 seconds
        const float fastFadingDuration = 1f / 2;
        const float NewGameFadeIn = 2; //2 seconds
        float over1 = 0; //interpolants between 0 and 1
        float over2 = 0;
        Light AreaLight, DirectionalLight, PathLight; //Reference to light
        const float ALInitialIntensity = 0.36f, DLInitialIntensity = 0.6f, PLInitialIntensity = 0.4f; //Inital value of lights
        //GameObject for settings
        Text[] SettingsOptions = new Text[7];
        Image SettingsPanel;
        const float SPanelInitialA = 1f / 3; // initial value of setting panel
        GameObject[] SettingsPattern;
        const float SPatternInitialA = 1;
        //Animation for new game
        GameObject MainCamera;
        bool FinalValueAssigned = false;
        bool NextScene = false;
        float fadeOutPosition; //only for z
        float over3;
        Renderer BlackBackground;
        /************************/
        #endregion
        // Use this for initialization
        void Start()
        {
            isChosen = false; //to decide whether user chooses.
            isFade = true; //to decide whether a part of game is fading
            isSettingChosen = false;
            isSettingFadeIn = false;
            NextScene = false;
            userOption = Options.NewGame; //first options of user
            userSettingOptions = SettingOptions.Sound;
            //Main screen
            // Main Title
            GameTitle = GameObject.Find("GameTitle").GetComponent<Text>();
            // Main option
            OptionArr[0] = GameObject.Find("NewGame").GetComponent<Text>();
            OptionArr[1] = GameObject.Find("LoadGame").GetComponent<Text>();
            OptionArr[2] = GameObject.Find("Settings").GetComponent<Text>();
            OptionArr[3] = GameObject.Find("Quit").GetComponent<Text>();
            /********************************/
            //Lights for the main scene
            AreaLight = GameObject.Find("Area Light").GetComponent<Light>();
            DirectionalLight = GameObject.Find("Directional Light").GetComponent<Light>();
            PathLight= GameObject.Find("Path Light").GetComponent<Light>();
            /*******************************/
            //For the settings part
            //Settings UI
            for(int i = 0; i < 7; i++)
            {
                char[] tag = new char[3]; tag[0] = 'S'; tag[1] = 'O'; tag[2] = (char)(48 + i);
                string tagName = new string(tag);
                SettingsOptions[i] = GameObject.FindGameObjectWithTag(tagName).GetComponent<Text>();
            }
            SettingsPanel = GameObject.Find("SettingsPanel").GetComponent<Image>();
            SettingsPattern = GameObject.FindGameObjectsWithTag("SettingsPattern");
            /************************/
            //SettingsInfo, remember to create playerClass

            /*****************************/
            //For the new game animation
            MainCamera = GameObject.Find("Main Camera");
            fadeOutPosition = 236;
            over3 = 0;
            BlackBackground = GameObject.Find("BlackBackground").GetComponent<Renderer>();

            /*****************************/
        }
        // Update is called once per frame
        void Update()
        {
            //First Fade In
            #region FadeInProcessForIntro
            if (isFade)
            {
                over1 += Time.deltaTime / IntroFadeIn;
                if (over1 <= 1)
                {
                    //fading for texts
                    GameTitle.color = new Color(GameTitle.color.r, GameTitle.color.g, GameTitle.color.b, Mathf.Lerp(0,1,over1));
                    for (int i = 0; i < 4; i++)
                        OptionArr[i].color = new Color(OptionArr[i].color.r, OptionArr[i].color.g, OptionArr[i].color.b, Mathf.Lerp(0,1,over1));
                    //lowering intensity for lights
                    AreaLight.intensity = Mathf.Lerp(0, ALInitialIntensity, over1);
                    DirectionalLight.intensity = Mathf.Lerp(0, DLInitialIntensity, over1);
                }
                else
                {
                    //final result
                    GameTitle.color = new Color(GameTitle.color.r, GameTitle.color.g, GameTitle.color.b, 1);
                    for (int i = 0; i < 4; i++)
                        OptionArr[i].color = new Color(OptionArr[i].color.r, OptionArr[i].color.g, OptionArr[i].color.b, 1);
                    AreaLight.intensity = ALInitialIntensity;
                    DirectionalLight.intensity = DLInitialIntensity;
                    isFade = false;
                }
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
                    over1 = 0.0f; //over1 will restart to begin fade in process
                    isFade = false;
                }
            }
            #endregion
            #region NewGame
            else if (userOption == Options.NewGame)
            {
                //this is where fading process "for new game" will happen
                over1 += Time.deltaTime / NewGameFadeIn;
                if (over1 <= 1)
                {
                    //fading for object
                    GameTitle.color = new Color(GameTitle.color.r, GameTitle.color.g, GameTitle.color.b, Mathf.Lerp(1,0,over1));
                    for (int i = 0; i < 4; i++)
                        OptionArr[i].color = new Color(OptionArr[i].color.r, OptionArr[i].color.g, OptionArr[i].color.b, Mathf.Lerp(1,0,over1));
                    //lowering intensity for lights
                    
                }
                else
                {
                    if (!FinalValueAssigned)
                    {
                        over2 = 0;
                        GameTitle.color = new Color(GameTitle.color.r, GameTitle.color.g, GameTitle.color.b, 0);
                        for (int i = 0; i < 4; i++)
                            OptionArr[i].color = new Color(OptionArr[i].color.r, OptionArr[i].color.g, OptionArr[i].color.b, 0);
                        FinalValueAssigned = true;
                    }
                    over2 += Time.deltaTime / 10;
                    if(FinalValueAssigned && !NextScene)
                    {
                        MainCamera.GetComponent<Animator>().SetTrigger("CameraIntro");
                        if (MainCamera.transform.position.z >= fadeOutPosition)
                        {
                            over3 += Time.deltaTime / 5;
                            if (over3 <= 1)
                            {
                                AreaLight.intensity = Mathf.Lerp(ALInitialIntensity, 0, over3);
                                DirectionalLight.intensity = Mathf.Lerp(DLInitialIntensity, 0, over3);
                                PathLight.intensity = Mathf.Lerp(PLInitialIntensity, 0, over3);
                                BlackBackground.material.color = new Color(BlackBackground.material.color.r, BlackBackground.material.color.g, BlackBackground.material.color.b, Mathf.Lerp(0, 1, over3));
                            }
                            else
                            {
                                AreaLight.intensity = 0;
                                DirectionalLight.intensity = 0;
                                PathLight.intensity = 0;
                                BlackBackground.material.color = new Color(BlackBackground.material.color.r, BlackBackground.material.color.g, BlackBackground.material.color.b, 1);
                                NextScene = true;
                            }
                        }
                    }
                    if(NextScene) // animation completed! Turning to next scene
                    {
                       // Application.LoadLevel
                    }
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
                if (!isSettingFadeIn)
                {
                    over1 += Time.deltaTime / fastFadingDuration;
                    if (over1 <= 1)
                    {

                        //fade out for main option texts
                        for (int i = 0; i < 4; i++)
                            OptionArr[i].color = new Color(OptionArr[i].color.r, OptionArr[i].color.g, OptionArr[i].color.b, Mathf.Lerp(1, 0, over1));
                        //fade in for settings UI
                        for (int i = 0; i < 7; i++)
                            SettingsOptions[i].color = new Color(SettingsOptions[i].color.r, SettingsOptions[i].color.g, SettingsOptions[i].color.b, Mathf.Lerp(0,1, over1));
                        SettingsPanel.color = new Color(SettingsPanel.color.r, SettingsPanel.color.g, SettingsPanel.color.b, Mathf.Lerp(0, SPanelInitialA, over1));
                        foreach (GameObject mem in SettingsPattern)
                        {
                            Color tmp = mem.GetComponent<Renderer>().material.color;
                            tmp.a = Mathf.Lerp(0, SPatternInitialA, over1);
                            mem.GetComponent<Renderer>().material.color = tmp;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 4; i++)
                            OptionArr[i].color = new Color(OptionArr[i].color.r, OptionArr[i].color.g, OptionArr[i].color.b, 0);
                        for (int i = 0; i < 7; i++)
                            SettingsOptions[i].color = new Color(SettingsOptions[i].color.r, SettingsOptions[i].color.g, SettingsOptions[i].color.b, 1);
                        SettingsPanel.color = new Color(SettingsPanel.color.r, SettingsPanel.color.g, SettingsPanel.color.b, SPanelInitialA);
                        foreach (GameObject mem in SettingsPattern)
                        {
                            Color tmp = mem.GetComponent<Renderer>().material.color;
                            tmp.a = SPatternInitialA;
                            mem.GetComponent<Renderer>().material.color = tmp;
                        }
                        isSettingFadeIn = true;
                    }
                }
                else
                {
                    if (!isSettingChosen)
                    {
                        if (Input.GetKeyDown(KeyCode.UpArrow) && userSettingOptions != SettingOptions.Sound)
                        {
                            if (userSettingOptions == SettingOptions.Cancel)
                            {
                                SettingsOptions[(int)userSettingOptions - 1].color = Color.white;
                                userSettingOptions = (SettingOptions)((int)userSettingOptions - 2);
                            }
                            else
                            {
                                SettingsOptions[(int)userSettingOptions - 1].color = Color.white;
                                userSettingOptions = (SettingOptions)((int)userSettingOptions - 1);
                            }

                        }
                        if (Input.GetKeyDown(KeyCode.DownArrow) && userSettingOptions != SettingOptions.OK && userSettingOptions != SettingOptions.Cancel)
                        {

                            SettingsOptions[(int)userSettingOptions - 1].color = Color.white;
                            userSettingOptions = (SettingOptions)((int)userSettingOptions + 1);
                        }
                        if (Input.GetKeyDown(KeyCode.RightArrow) && userSettingOptions == SettingOptions.OK)
                        {
                            SettingsOptions[(int)userSettingOptions - 1].color = Color.white;
                            userSettingOptions = (SettingOptions)((int)userSettingOptions + 1);
                        }
                        if (Input.GetKeyDown(KeyCode.LeftArrow) && userSettingOptions == SettingOptions.Cancel)
                        {
                            SettingsOptions[(int)userSettingOptions - 1].color = Color.white;
                            userSettingOptions = (SettingOptions)((int)userSettingOptions - 1);
                        }
                        BlackWhiteTransition(ref SettingsOptions[(int)userSettingOptions - 1]);
                        //Settings not finished implementing
                        if (userSettingOptions == SettingOptions.Sound) { }
                        if (userSettingOptions == SettingOptions.BGM) { }
                        if (userSettingOptions == SettingOptions.Fullscreen) { }
                        if (userSettingOptions == SettingOptions.Credit) { }
                        if (Input.GetKeyDown(KeyCode.Return))
                        {
                            if (userSettingOptions == SettingOptions.OK)
                            {
                                isSettingChosen = true;
                                over1 = 0;
                            }
                            if (userSettingOptions == SettingOptions.Cancel)
                            {
                                isSettingChosen = true;
                                over1 = 0;
                            }
                        }
                        /**************************************/
                    }
                    else
                    {
                        if (userSettingOptions == SettingOptions.OK)
                        {
                            //save changes
                        }
                        over1 += Time.deltaTime / fastFadingDuration;
                        if (over1 <= 1)
                        {
                            //fade in for main option texts
                            for (int i = 0; i < 4; i++)
                                OptionArr[i].color = new Color(OptionArr[i].color.r, OptionArr[i].color.g, OptionArr[i].color.b, Mathf.Lerp(0, 1, over1));
                            //fade out for settings UI
                            for (int i = 0; i < 7; i++)
                                SettingsOptions[i].color = new Color(SettingsOptions[i].color.r, SettingsOptions[i].color.g, SettingsOptions[i].color.b, Mathf.Lerp(1, 0, over1));
                            SettingsPanel.color = new Color(SettingsPanel.color.r, SettingsPanel.color.g, SettingsPanel.color.b, Mathf.Lerp(SPanelInitialA, 0, over1));
                            foreach (GameObject mem in SettingsPattern)
                            {
                                Color tmp = mem.GetComponent<Renderer>().material.color;
                                tmp.a = Mathf.Lerp(SPatternInitialA, 0, over1);
                                mem.GetComponent<Renderer>().material.color = tmp;
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 4; i++)
                                OptionArr[i].color = new Color(OptionArr[i].color.r, OptionArr[i].color.g, OptionArr[i].color.b, 1);
                            for (int i = 0; i < 7; i++)
                                SettingsOptions[i].color = new Color(SettingsOptions[i].color.r, SettingsOptions[i].color.g, SettingsOptions[i].color.b, 0);
                            SettingsPanel.color = new Color(SettingsPanel.color.r, SettingsPanel.color.g, SettingsPanel.color.b, 0);
                            isChosen = false;
                            isSettingChosen = false;
                            isSettingFadeIn = false;
                            userSettingOptions = SettingOptions.Sound;
                        }
                    }
                }
            }
            #endregion
            #region Quit
            else if (userOption == Options.Quit)
            {
                Application.Quit();
            }
            #endregion
        }
        #region EventFunction
        void BlackWhiteTransition(ref Text option)
            {
                option.color = Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time, 1));
            }
        #endregion
    }
}
