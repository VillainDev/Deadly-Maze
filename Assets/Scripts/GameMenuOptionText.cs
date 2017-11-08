using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameMenu
{
    public class GameMenuOptionText : MonoBehaviour
    {
        #region VariableAndArray
        Player.Player CurrentPlayer;
        //float indicating animation time
        float over1, over2, over3;
        //bools for animation branching
        bool isChosen; //check whether setting is chosen
        bool FadeOut = false; //Fade out for new game
        bool isSettingFadeIn; //Fade in for setting
        bool isSettingFinished; //check whether options in setting is chosen
        //List of options
        public enum Options : int { NewGame = 1, LoadGame = 2, Settings, Quit};
        Options userOption;
        
        //List of setting options
        public enum SettingOptions : int { Sound = 1, BGM = 2, Fullscreen, ConsoleConfiguration, Credit, OK, Cancel};
        SettingOptions userSettingOptions;
        
        
        /*****************************/
        //GameTitle and Menu
        Text GameTitle;
        Text[] OptionArr = new Text[4];
        /*****************************/
        bool isFade; //bool indicating for intro fade in
        Light AreaLight, DirectionalLight, PathLight; //Reference to light
        const float ALInitialIntensity = 0.36f, DLInitialIntensity = 0.6f, PLInitialIntensity = 0.4f; //Inital value of lights
        //Objects for Load game
        static string[] Name = Player.SaveDataProcessing.GetSaveDataName(); //Get file names
        Player.Player[] SavedPlayer = new Player.Player[Name.Length]; //Get Players
        GameObject[] DataButton = new GameObject[20]; //Get button
        GameObject ScrollView;
        bool LoadFadeIn = true;
        bool IsDataChosen = false;
        bool IsLoadFinished = false;
        Renderer LoadBlack;
        int IDX = 0; //current index of saved data
        ScrollRect Slider;
        float SlidingRange = 1f - (float)(Name.Length - 3) / 20; //standarlize  value in slider (from 1 to SlidingRange)
        
        
        /***************************/
        //GameObject for settings
        Text[] SettingsOptions = new Text[7];
        Image SettingsPanel;
        const float SPanelInitialA = 1f / 3; // initial value of setting panel
        GameObject[] SettingsPattern;
        const float SPatternInitialA = 1;
        //GameObject for Setting Information
        GameObject SoundInfo;
        GameObject BGMInfo;
        GameObject FullScreenInfo;
        bool IsChecked = true;
        bool IsAllowed = false;
        float InitialSound;
        float InitialBGM;
        bool InitialFS;
        Image[] SettingGUI;
        Image BackgroundCheckMark;
        //Animation for new game
        GameObject MainCamera;
        bool NextScene = false;
        float fadeOutPosition; //only for z
        Renderer BlackBackground;
        /****************************/
        /************************/
        #endregion
        // Use this for initialization
        void Start()
        {
            //float LoadContentHeight = GameObject.Find("LoadContent").transform.bounds.size.y;
            //Debug.Log(LoadContentHeight);
            isChosen = false; //to decide whether user chooses.
            isFade = true; //to decide whether a part of game is fading
            isSettingFinished = false;
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
            //For the load game part
            for (int i = 0; i < Name.Length; i++)
                SavedPlayer[i] = Player.SaveDataProcessing.Load(Name[i].Substring(0, Name[i].Length - 4));
            ScrollView = GameObject.Find("ScrollView");
            LoadBlack = GameObject.Find("LoadBlack").GetComponent<Renderer>();
            //get button in load panel
            for (int i = 0; i < 20; i++)
            {
                string DataName = "Data (" + i.ToString() + ")";
                DataButton[i] = GameObject.Find(DataName);
                //cập nhật thông tin từ 0 tới 5
                
                if (i < Name.Length)
                {
                    //0. Player's name
                    Text tmpText = DataButton[i].transform.Find("Text (0)").GetComponent<Text>();
                    tmpText.text = "Player's name: " + SavedPlayer[i].PlayerName;
                    //1. Scene's name
                    tmpText = DataButton[i].transform.Find("Text (1)").GetComponent<Text>();
                    tmpText.text = "Current scene: " + SavedPlayer[i].GetScene();
                    //2. Health
                    tmpText = DataButton[i].transform.Find("Text (2)").GetComponent<Text>();
                    tmpText.text = "Player's health: " + SavedPlayer[i].ShowStatus();
                    //3. BulletPack
                    tmpText = DataButton[i].transform.Find("Text (3)").GetComponent<Text>();
                    tmpText.text = "Bullet pack: S x " + SavedPlayer[i].SmallBulletPack.GetBulletPackSize() + "  M x " + SavedPlayer[i].MediumBulletPack.GetBulletPackSize() + "  L x " + SavedPlayer[i].LargeBulletPack.GetBulletPackSize();
                    //4.Medical Kit
                    tmpText = DataButton[i].transform.Find("Text (4)").GetComponent<Text>();
                    tmpText.text = "Medical Kit: M x " + SavedPlayer[i].MediumFAK.GetFirstAidKitSize() + "  L x " + SavedPlayer[i].BigFAK.GetFirstAidKitSize();
                    //5.Bullet
                    tmpText = DataButton[i].transform.Find("Text (5)").GetComponent<Text>();
                    tmpText.text = "Bullet: HG x " + SavedPlayer[i].PlayerGun.GetHGBullet() + "  SG x " + SavedPlayer[i].PlayerGun.GetSGBullet() + "  AK x " + SavedPlayer[i].PlayerGun.GetAKBullet();            
                }
                else
                {
                    
                }
            }
            Slider = GameObject.Find("ScrollView").GetComponent<ScrollRect>();
            /***************************/
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
            //SettingsInfo
            SoundInfo = GameObject.Find("SoundInfo");
            BGMInfo = GameObject.Find("BGMInfo");
            FullScreenInfo = GameObject.Find("FullscreenInfo");
            //GUI Component: Slider, checkbox,...
            BackgroundCheckMark= GameObject.Find("Box").GetComponent<Image>();
            GameObject[] tmp = GameObject.FindGameObjectsWithTag("GUI");
            SettingGUI = new Image[tmp.Length];
            for (int i = 0; i < tmp.Length; i++)
                SettingGUI[i] = tmp[i].GetComponent<Image>();   
            /************************/

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
                over1 += Time.deltaTime / 5; //last 5 seconds
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
                else //final gán
                {
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
                //Selecting options
                if (Input.GetKeyDown(ConsoleSetting.Up) && userOption != Options.NewGame)
                {
                    OptionArr[(int)userOption - 1].color = Color.white;
                    userOption = (Options)((int)userOption - 1);
                }
                if (Input.GetKeyDown(ConsoleSetting.Down) && userOption != Options.Quit)
                {
                    OptionArr[(int)userOption - 1].color = Color.white;
                    userOption = (Options)((int)userOption + 1);
                }
                /******************/
                //Animation for each options
                BlackWhiteTransition(ref OptionArr[(int)userOption - 1]);
                //If chosen through enter button, change to new scene
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    isChosen = true;
                    //over1 will restart to begin fade in process
                    over1 = 0.0f; 
                    over2 = 0.0f;
                    over3 = 0.0f;
                }
            }
            #endregion
            #region NewGame
            else if (userOption == Options.NewGame)
            {
                //this is where fading process "for new game" will happen
                over1 += Time.deltaTime / 2; //last 2 seconds
                if (over1 <= 1)
                {
                    //fading for object
                    GameTitle.color = new Color(GameTitle.color.r, GameTitle.color.g, GameTitle.color.b, Mathf.Lerp(1,0,over1));
                    for (int i = 0; i < 4; i++)
                        OptionArr[i].color = new Color(OptionArr[i].color.r, OptionArr[i].color.g, OptionArr[i].color.b, Mathf.Lerp(1,0,over1));                 
                }
                else
                {
                    if (!FadeOut) //assigned final value
                    {
                        GameTitle.color = new Color(GameTitle.color.r, GameTitle.color.g, GameTitle.color.b, 0);
                        for (int i = 0; i < 4; i++)
                            OptionArr[i].color = new Color(OptionArr[i].color.r, OptionArr[i].color.g, OptionArr[i].color.b, 0);
                        FadeOut = true;
                    }
                    over2 += Time.deltaTime / 10; //last 10 seconds
                    if(FadeOut && !NextScene) //turning to camera animation
                    {
                        MainCamera.GetComponent<Animator>().SetTrigger("CameraIntro");
                        if (MainCamera.transform.position.z >= fadeOutPosition)
                        {
                            //slowly turn off the light after 5 seconds 
                            over3 += Time.deltaTime / 5; //last 5 seconds
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
                if(SavedPlayer.Length > 0)
                {
                    if (LoadFadeIn)
                    {
                        over1 += Time.deltaTime / (1f/2); //test
                        if (over1 <= 1)
                        {
                            //fade out for main option texts
                            for (int i = 0; i < 4; i++)
                                OptionArr[i].color = new Color(OptionArr[i].color.r, OptionArr[i].color.g, OptionArr[i].color.b, Mathf.Lerp(1, 0, over1));
                            Color tmp = ScrollView.GetComponent<Image>().color;
                            tmp.a = Mathf.Lerp(0, 0.2f, over1);
                            ScrollView.GetComponent<Image>().color = tmp;
                            for (int i = 0; i < 20; i++)
                            {
                                if (i < Name.Length)
                                {
                                    tmp = DataButton[i].GetComponent<Image>().color;
                                    tmp.a = Mathf.Lerp(0, 0.36f, over1);
                                    DataButton[i].GetComponent<Image>().color = tmp;
                                }
                            }
                            for (int i = 0; i < 20; i++)
                            {
                                if (i < Name.Length)
                                {
                                    //0. Player's name
                                    Text tmpText = DataButton[i].transform.Find("Text (0)").GetComponent<Text>();
                                    tmpText.color = new Color(tmpText.color.r, tmpText.color.g, tmpText.color.b, Mathf.Lerp(0, 1, over1));
                                    //1. Scene's name
                                    tmpText = DataButton[i].transform.Find("Text (1)").GetComponent<Text>();
                                    tmpText.color = new Color(tmpText.color.r, tmpText.color.g, tmpText.color.b, Mathf.Lerp(0, 1, over1));
                                    //2. Health
                                    tmpText = DataButton[i].transform.Find("Text (2)").GetComponent<Text>();
                                    tmpText.color = new Color(tmpText.color.r, tmpText.color.g, tmpText.color.b, Mathf.Lerp(0, 1, over1));
                                    //3. BulletPack
                                    tmpText = DataButton[i].transform.Find("Text (3)").GetComponent<Text>();
                                    tmpText.color = new Color(tmpText.color.r, tmpText.color.g, tmpText.color.b, Mathf.Lerp(0, 1, over1));
                                    //4.Medical Kit
                                    tmpText = DataButton[i].transform.Find("Text (4)").GetComponent<Text>();
                                    tmpText.color = new Color(tmpText.color.r, tmpText.color.g, tmpText.color.b, Mathf.Lerp(0, 1, over1));
                                    //5.Bullet
                                    tmpText = DataButton[i].transform.Find("Text (5)").GetComponent<Text>();
                                    tmpText.color = new Color(tmpText.color.r, tmpText.color.g, tmpText.color.b, Mathf.Lerp(0, 1, over1));
                                }
                            }
                        }
                        else //last result
                        {
                            for (int i = 0; i < 4; i++)
                                OptionArr[i].color = new Color(OptionArr[i].color.r, OptionArr[i].color.g, OptionArr[i].color.b, 0);
                            Color tmp = ScrollView.GetComponent<Image>().color;
                            tmp.a = 0.2f;
                            ScrollView.GetComponent<Image>().color = tmp;
                            for (int i = 0; i < 20; i++)
                            {
                                if (i < Name.Length)
                                {
                                    tmp = DataButton[i].GetComponent<Image>().color;
                                    tmp.a = 0.36f;
                                    DataButton[i].GetComponent<Image>().color = tmp;
                                }
                            }
                            LoadFadeIn = false;
                        }
                    }
                    else if(!IsLoadFinished) //begin choosing
                    {
                        if (Input.GetKeyDown(ConsoleSetting.Up))
                        {
                            DataButton[IDX].GetComponent<Image>().color = new Color(0, 0, 0, 0.36f);
                            if (IDX > 0)
                                --IDX;
                            if(IDX % 3 == 0)
                            {

                            }
                        }
                        if (Input.GetKeyDown(ConsoleSetting.Down))
                        {
                            DataButton[IDX].GetComponent<Image>().color = new Color(0, 0, 0, 0.36f);
                            if (IDX < Name.Length - 1)
                               ++IDX;
                            if(IDX % 3 == 0 && IDX != 0)
                            {
                                float y0 = DataButton[IDX - 3].transform.position.y;
                                float y = DataButton[IDX].transform.position.y;
                                float unitVal = (y - y0);
                                if (Slider.verticalNormalizedPosition > SlidingRange)
                                    Slider.verticalNormalizedPosition -= 0;
                            }
                        }
                        Image currentData = DataButton[IDX].GetComponent<Image>();
                        BlackWhiteTransition(ref currentData);
                        if (Input.GetKeyDown(KeyCode.Return))
                        {
                            IsLoadFinished = true;
                            //get Player's data
                            CurrentPlayer = SavedPlayer[IDX];
                            IsDataChosen = true;
                            over1 = 0;
                        }
                        if (Input.GetKeyDown(KeyCode.Escape))
                        {
                            IsLoadFinished = true;
                            over1 = 0;
                        }
                    }
                    else
                    {
                        //Fade out
                        if (!IsDataChosen) //return to main screen
                        {
                            over1 += Time.deltaTime / (1f / 2);
                            if (over1 <= 1)
                            {
                                //fade in for main option texts
                                for (int i = 0; i < 4; i++)
                                    OptionArr[i].color = new Color(OptionArr[i].color.r, OptionArr[i].color.g, OptionArr[i].color.b, Mathf.Lerp(0, 1, over1));
                                Color tmp = ScrollView.GetComponent<Image>().color;
                                tmp.a = Mathf.Lerp(0.2f, 0, over1);
                                ScrollView.GetComponent<Image>().color = tmp;
                                for (int i = 0; i < 20; i++)
                                {
                                    if (i < Name.Length)
                                    {
                                        tmp = DataButton[i].GetComponent<Image>().color;
                                        tmp.a = Mathf.Lerp(0.36f, 0, over1);
                                        DataButton[i].GetComponent<Image>().color = tmp;
                                    }
                                }
                                for (int i = 0; i < 20; i++)
                                {
                                    if (i < Name.Length)
                                    {
                                        //0. Player's name
                                        Text tmpText = DataButton[i].transform.Find("Text (0)").GetComponent<Text>();
                                        tmpText.color = new Color(tmpText.color.r, tmpText.color.g, tmpText.color.b, Mathf.Lerp(1, 0, over1));
                                        //1. Scene's name
                                        tmpText = DataButton[i].transform.Find("Text (1)").GetComponent<Text>();
                                        tmpText.color = new Color(tmpText.color.r, tmpText.color.g, tmpText.color.b, Mathf.Lerp(1, 0, over1));
                                        //2. Health
                                        tmpText = DataButton[i].transform.Find("Text (2)").GetComponent<Text>();
                                        tmpText.color = new Color(tmpText.color.r, tmpText.color.g, tmpText.color.b, Mathf.Lerp(1, 0, over1));
                                        //3. BulletPack
                                        tmpText = DataButton[i].transform.Find("Text (3)").GetComponent<Text>();
                                        tmpText.color = new Color(tmpText.color.r, tmpText.color.g, tmpText.color.b, Mathf.Lerp(1, 0, over1));
                                        //4.Medical Kit
                                        tmpText = DataButton[i].transform.Find("Text (4)").GetComponent<Text>();
                                        tmpText.color = new Color(tmpText.color.r, tmpText.color.g, tmpText.color.b, Mathf.Lerp(1, 0, over1));
                                        //5.Bullet
                                        tmpText = DataButton[i].transform.Find("Text (5)").GetComponent<Text>();
                                        tmpText.color = new Color(tmpText.color.r, tmpText.color.g, tmpText.color.b, Mathf.Lerp(1, 0, over1));
                                    }
                                }
                            }
                            else //last result
                            {
                                for (int i = 0; i < 4; i++)
                                    OptionArr[i].color = new Color(OptionArr[i].color.r, OptionArr[i].color.g, OptionArr[i].color.b, 1);
                                Color tmp = ScrollView.GetComponent<Image>().color;
                                tmp.a = 0;
                                ScrollView.GetComponent<Image>().color = tmp;
                                for (int i = 0; i < 20; i++)
                                {
                                    if (i < Name.Length)
                                    {
                                        tmp = DataButton[i].GetComponent<Image>().color;
                                        tmp.a = 0;
                                        DataButton[i].GetComponent<Image>().color = tmp;
                                    }
                                }
                                for (int i = 0; i < 20; i++)
                                {
                                    if (i < Name.Length)
                                    {
                                        //0. Player's name
                                        Text tmpText = DataButton[i].transform.Find("Text (0)").GetComponent<Text>();
                                        tmpText.color = new Color(tmpText.color.r, tmpText.color.g, tmpText.color.b, 0);
                                        //1. Scene's name
                                        tmpText = DataButton[i].transform.Find("Text (1)").GetComponent<Text>();
                                        tmpText.color = new Color(tmpText.color.r, tmpText.color.g, tmpText.color.b, 0);
                                        //2. Health
                                        tmpText = DataButton[i].transform.Find("Text (2)").GetComponent<Text>();
                                        tmpText.color = new Color(tmpText.color.r, tmpText.color.g, tmpText.color.b, 0);
                                        //3. BulletPack
                                        tmpText = DataButton[i].transform.Find("Text (3)").GetComponent<Text>();
                                        tmpText.color = new Color(tmpText.color.r, tmpText.color.g, tmpText.color.b, 0);
                                        //4.Medical Kit
                                        tmpText = DataButton[i].transform.Find("Text (4)").GetComponent<Text>();
                                        tmpText.color = new Color(tmpText.color.r, tmpText.color.g, tmpText.color.b, 0);
                                        //5.Bullet
                                        tmpText = DataButton[i].transform.Find("Text (5)").GetComponent<Text>();
                                        tmpText.color = new Color(tmpText.color.r, tmpText.color.g, tmpText.color.b, 0);
                                    }
                                }
                                IsLoadFinished = false;
                                LoadFadeIn = true;
                                isChosen = false;
                            }
                        }
                        else // load game
                        {
                            over1 += Time.deltaTime / 5f;
                            Debug.Log(LoadBlack.material.color);
                            if (over1 <= 1)
                                LoadBlack.material.color = new Color(LoadBlack.material.color.r, LoadBlack.material.color.g, LoadBlack.material.color.b, Mathf.Lerp(0, 1, over1));
                            else
                                LoadBlack.material.color = new Color(LoadBlack.material.color.r, LoadBlack.material.color.g, LoadBlack.material.color.b, 1);
                                //Load to selected game file
                        }
                    }
                }
                else
                {

                }
            }
            #endregion
            #region Settings
            else if (userOption == Options.Settings)
            {
                if (!isSettingFadeIn)
                {
                    over1 += Time.deltaTime / (1f/2); //last half second
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
                        //fade in for setting GUI
                        foreach (Image mem in SettingGUI)
                            mem.color = new Color(mem.color.r, mem.color.g, mem.color.b,Mathf.Lerp(0, 1, over1));
                        BackgroundCheckMark.color = new Color(BackgroundCheckMark.color.r, BackgroundCheckMark.color.g, BackgroundCheckMark.color.b, Mathf.Lerp(0, 1, over1));
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
                        foreach (Image mem in SettingGUI)
                            mem.color = new Color(mem.color.r, mem.color.g, mem.color.b, 1);
                        BackgroundCheckMark.color = new Color(BackgroundCheckMark.color.r, BackgroundCheckMark.color.g, BackgroundCheckMark.color.b, 1);
                        //get initial value
                        InitialSound = SoundInfo.GetComponent<Slider>().value;
                        InitialBGM = BGMInfo.GetComponent<Slider>().value;
                        InitialFS = FullScreenInfo.GetComponent<Toggle>().isOn;
                        isSettingFadeIn = true;

                    }
                }
                else
                {
                    if (!isSettingFinished)
                    {
                        if (Input.GetKeyDown(ConsoleSetting.Up) && userSettingOptions != SettingOptions.Sound)
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
                        if (Input.GetKeyDown(ConsoleSetting.Down) && userSettingOptions != SettingOptions.OK && userSettingOptions != SettingOptions.Cancel)
                        {

                            SettingsOptions[(int)userSettingOptions - 1].color = Color.white;
                            userSettingOptions = (SettingOptions)((int)userSettingOptions + 1);
                        }
                        if (Input.GetKeyDown(ConsoleSetting.Right) && userSettingOptions == SettingOptions.OK)
                        {
                            SettingsOptions[(int)userSettingOptions - 1].color = Color.white;
                            userSettingOptions = (SettingOptions)((int)userSettingOptions + 1);
                        }
                        if (Input.GetKeyDown(ConsoleSetting.Left) && userSettingOptions == SettingOptions.Cancel)
                        {
                            SettingsOptions[(int)userSettingOptions - 1].color = Color.white;
                            userSettingOptions = (SettingOptions)((int)userSettingOptions - 1);
                        }
                        BlackWhiteTransition(ref SettingsOptions[(int)userSettingOptions - 1]);
                        //Settings not finished implementing
                        if (userSettingOptions == SettingOptions.Sound)
                        {
                            if (Input.GetKey(ConsoleSetting.Left))
                                SoundInfo.GetComponent<Slider>().value -= 1;
                            else if (Input.GetKey(ConsoleSetting.Right))
                                SoundInfo.GetComponent<Slider>().value += 1;

                        }
                        if (userSettingOptions == SettingOptions.BGM)
                        {
                            if (Input.GetKey(ConsoleSetting.Left))
                                BGMInfo.GetComponent<Slider>().value -= 1;
                            else if (Input.GetKey(ConsoleSetting.Right))
                                BGMInfo.GetComponent<Slider>().value += 1;
                        }
                        if (userSettingOptions == SettingOptions.Fullscreen) {
                            if (Input.GetKeyDown(KeyCode.LeftArrow))
                            {
                                FullScreenInfo.GetComponent<Toggle>().isOn = false;
                                IsChecked = false;
                            }
                            else if (Input.GetKeyDown(KeyCode.RightArrow))
                            {
                                FullScreenInfo.GetComponent<Toggle>().isOn = true;
                            }
                        }
                        if(userSettingOptions == SettingOptions.ConsoleConfiguration)
                        {
                            //TODO
                        }
                        if (userSettingOptions == SettingOptions.Credit) { }
                        if (Input.GetKeyDown(KeyCode.Return) && userSettingOptions != SettingOptions.Fullscreen) //because of the same return key
                        {
                            if (userSettingOptions == SettingOptions.OK) //save changes for the setting
                            {
                                isSettingFinished = true;
                                over1 = 0;
                                InitialSound = SoundInfo.GetComponent<Slider>().value;
                                InitialBGM = BGMInfo.GetComponent<Slider>().value;
                                InitialFS = FullScreenInfo.GetComponent<Toggle>().isOn;
                                SettingsOptions[5].color = new Color(1, 1, 1, 1);
                            }
                            if (userSettingOptions == SettingOptions.Cancel)//nothing happened
                            {
                                isSettingFinished = true;
                                over1 = 0;
                                SoundInfo.GetComponent<Slider>().value = InitialSound;
                                BGMInfo.GetComponent<Slider>().value = InitialBGM;
                                FullScreenInfo.GetComponent<Toggle>().isOn = InitialFS;
                                SettingsOptions[6].color = new Color(1, 1, 1, 1);
                            }
                            //reset OK and Cancel button to white for fading out from setting                        
                        }
                        /**************************************/
                    }
                    else
                    {
                        over1 += Time.deltaTime / (1f/2); //half a second
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
                            //fade in for setting GUI
                            foreach (Image mem in SettingGUI)
                                mem.color = new Color(mem.color.r, mem.color.g, mem.color.b, Mathf.Lerp(1, 0, over1));
                            BackgroundCheckMark.color = new Color(BackgroundCheckMark.color.r, BackgroundCheckMark.color.g, BackgroundCheckMark.color.b, Mathf.Lerp(1, 0, over1));
                        }
                        else
                        {
                            for (int i = 0; i < 4; i++)
                                OptionArr[i].color = new Color(OptionArr[i].color.r, OptionArr[i].color.g, OptionArr[i].color.b, 1);
                            for (int i = 0; i < 7; i++)
                                SettingsOptions[i].color = new Color(SettingsOptions[i].color.r, SettingsOptions[i].color.g, SettingsOptions[i].color.b, 0);
                            SettingsPanel.color = new Color(SettingsPanel.color.r, SettingsPanel.color.g, SettingsPanel.color.b, 0);
                            //fade in for setting GUI
                            foreach (Image mem in SettingGUI)
                                mem.color = new Color(mem.color.r, mem.color.g, mem.color.b, 0);
                            BackgroundCheckMark.color = new Color(BackgroundCheckMark.color.r, BackgroundCheckMark.color.g, BackgroundCheckMark.color.b, 0);
                            isChosen = false;
                            isSettingFinished = false;
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
        #region Function
        void BlackWhiteTransition(ref Text option)
            {
                option.color = Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time, 1));
            }
        void BlackWhiteTransition(ref Image option)
        {
            option.color = Color.Lerp(new Color(0.4f,0.4f,0.4f,0.36f), new Color(0.2f,0.2f,0.2f,0.36f), Mathf.PingPong(Time.time, 1));
        }
        #endregion
    }
}
