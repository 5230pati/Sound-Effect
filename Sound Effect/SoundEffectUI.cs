using HMUI;
using IllusionPlugin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VRUI;
using Image = UnityEngine.UI.Image;

namespace Sound_Effect
{
    class SoundEffectUI : MonoBehaviour
    {

        private RectTransform _mainMenuRectTransform;
        private MainMenuViewController _mainMenuViewController;

        private Button _buttonInstance;
        private Button _backButtonInstance;

        public static SoundEffectUI _instance;

        public static List<Sprite> icons = new List<Sprite>();

        internal static void OnLoad()
        {
            if (_instance != null)
            {
                return;
            }
            new GameObject("SoundEffectUI").AddComponent<SoundEffectUI>();

        }

        private void Awake()
        {
            _instance = this;
            foreach (Sprite sprite in Resources.FindObjectsOfTypeAll<Sprite>())
            {
                icons.Add(sprite);
            }
            try
            {
                _buttonInstance = Resources.FindObjectsOfTypeAll<Button>().First(x => (x.name == "QuitButton"));
                _backButtonInstance = Resources.FindObjectsOfTypeAll<Button>().First(x => (x.name == "BackArrowButton"));
                _mainMenuViewController = Resources.FindObjectsOfTypeAll<MainMenuViewController>().First();
                _mainMenuRectTransform = _buttonInstance.transform.parent as RectTransform;
            }
            catch (Exception e)
            {
                Console.WriteLine("EXCEPTION ON AWAKE(TRY FIND BUTTONS): " + e);
            }

            //CreateSoundEffectButton();

            CreateSoundEffectSettingsUI();
        }

        private static void CreateSoundEffectSettingsUI()
        {
            float[] volArray = new float[21];
            float[] distArray = new float[51];

            for (int i = 0; i <= 20; i++)
            {
                volArray[i] = (float)i * 0.05f;
            }
            for (int i = 0; i <= 50; i++)
            {
                distArray[i] = (float)i * 0.01f;
            }

            var subMenu = SettingsUI.CreateSubMenu("Sound Effects");
            var subMenuVol = SettingsUI.CreateSubMenu("Sound Effects Values");

            // Distortion
            // Enable/disable
            var distMenuB = subMenu.AddBool("Distortion");
            distMenuB.GetValue += delegate
            {
                return ModPrefs.GetBool(SoundEffectPlugin.PluginName, "Distortion", SoundEffect._dist);
            };
            distMenuB.SetValue += delegate (bool value)
            {
                SoundEffect._dist = value;
                ModPrefs.SetBool(SoundEffectPlugin.PluginName, "Distortion", SoundEffect._dist);
            };
            // Length
            var distMenuV = subMenuVol.AddList("Distortion Length", distArray);
            distMenuV.GetValue += delegate
            {
                return ModPrefs.GetFloat(SoundEffectPlugin.PluginName, "DistortionLength", SoundEffect._distLen);
            };
            distMenuV.SetValue += delegate (float value)
            {
                SoundEffect._distLen = value;
                ModPrefs.SetFloat(SoundEffectPlugin.PluginName, "DistortionLength", SoundEffect._distLen);
            };
            distMenuV.FormatValue += delegate (float value) { return value.ToString(); };



            // Miss
            // Enable/disable
            var missMenuB = subMenu.AddBool("Miss");
            missMenuB.GetValue += delegate
            {
                return ModPrefs.GetBool(SoundEffectPlugin.PluginName, "Miss", SoundEffect._miss);
            };
            missMenuB.SetValue += delegate (bool value)
            {
                SoundEffect._miss = value;
                ModPrefs.SetBool(SoundEffectPlugin.PluginName, "Miss", SoundEffect._miss);
            };
            // Value
            var missMenuV = subMenuVol.AddList("Miss Volume", volArray);
            missMenuV.GetValue += delegate
            {
                return ModPrefs.GetFloat(SoundEffectPlugin.PluginName, "MissVolume", SoundEffect._missVol);
            };
            missMenuV.SetValue += delegate (float value)
            {
                SoundEffect._missVol = value;
                ModPrefs.SetFloat(SoundEffectPlugin.PluginName, "MissVolume", SoundEffect._missVol);
            };
            missMenuV.FormatValue += delegate (float value) { return value.ToString(); };



            // Bomb
            // Enable/disable
            var bombMenuB = subMenu.AddBool("Bomb");
            bombMenuB.GetValue += delegate
            {
                return ModPrefs.GetBool(SoundEffectPlugin.PluginName, "Bomb", SoundEffect._bomb);
            };
            bombMenuB.SetValue += delegate (bool value)
            {
                SoundEffect._bomb = value;
                ModPrefs.SetBool(SoundEffectPlugin.PluginName, "Bomb", SoundEffect._bomb);
            };
            // Value
            var bombMenuV = subMenuVol.AddList("Bomb Volume", volArray);
            bombMenuV.GetValue += delegate
            {
                return ModPrefs.GetFloat(SoundEffectPlugin.PluginName, "BombVolume", SoundEffect._bombVol);
            };
            bombMenuV.SetValue += delegate (float value)
            {
                SoundEffect._bombVol = value;
                ModPrefs.SetFloat(SoundEffectPlugin.PluginName, "BombVolume", SoundEffect._bombVol);
            };
            bombMenuV.FormatValue += delegate (float value) { return value.ToString(); };



            // Hit
            // Enable/disable
            var hitMenuB = subMenu.AddBool("Hit");
            hitMenuB.GetValue += delegate
            {
                return ModPrefs.GetBool(SoundEffectPlugin.PluginName, "Hit", SoundEffect._hit);
            };
            hitMenuB.SetValue += delegate (bool value)
            {
                SoundEffect._hit = value;
                ModPrefs.SetBool(SoundEffectPlugin.PluginName, "Hit", SoundEffect._hit);
            };
            // Value
            var hitMenuV = subMenuVol.AddList("Hit Volume", volArray);
            hitMenuV.GetValue += delegate
            {
                return ModPrefs.GetFloat(SoundEffectPlugin.PluginName, "HitVolume", SoundEffect._hitVol);
            };
            hitMenuV.SetValue += delegate (float value)
            {
                SoundEffect._hitVol = value;
                ModPrefs.SetFloat(SoundEffectPlugin.PluginName, "HitVolume", SoundEffect._hitVol);
            };
            hitMenuV.FormatValue += delegate (float value) { return value.ToString(); };



            // Hit
            // Enable/disable
            var badMenuB = subMenu.AddBool("BadHit");
            badMenuB.GetValue += delegate
            {
                return ModPrefs.GetBool(SoundEffectPlugin.PluginName, "BadHit", SoundEffect._bad);
            };
            badMenuB.SetValue += delegate (bool value)
            {
                SoundEffect._bad = value;
                ModPrefs.SetBool(SoundEffectPlugin.PluginName, "BadHit", SoundEffect._bad);
            };
            // Value
            var badMenuV = subMenuVol.AddList("Bad Hit Volume", volArray);
            badMenuV.GetValue += delegate
            {
                return ModPrefs.GetFloat(SoundEffectPlugin.PluginName, "BadHitVolume", SoundEffect._badVol);
            };
            badMenuV.SetValue += delegate (float value)
            {
                SoundEffect._badVol = value;
                ModPrefs.SetFloat(SoundEffectPlugin.PluginName, "BadHitVolume", SoundEffect._badVol);
            };
            badMenuV.FormatValue += delegate (float value) { return value.ToString(); };



            // Fail
            // Enable/disable
            var failMenuB = subMenu.AddBool("Fail");
            failMenuB.GetValue += delegate
            {
                return ModPrefs.GetBool(SoundEffectPlugin.PluginName, "Fail", SoundEffect._fail);
            };
            failMenuB.SetValue += delegate (bool value)
            {
                SoundEffect._fail = value;
                ModPrefs.SetBool(SoundEffectPlugin.PluginName, "Fail", SoundEffect._fail);
            };
            // Value
            var failMenuV = subMenuVol.AddList("Fail Volume", volArray);
            failMenuV.GetValue += delegate
            {
                return ModPrefs.GetFloat(SoundEffectPlugin.PluginName, "FailVolume", SoundEffect._failVol);
            };
            failMenuV.SetValue += delegate (float value)
            {
                SoundEffect._failVol = value;
                ModPrefs.SetFloat(SoundEffectPlugin.PluginName, "FailVolume", SoundEffect._failVol);
            };
            failMenuV.FormatValue += delegate (float value) { return value.ToString(); };
        }

        private void CreateSoundEffectButton()
        {

            Button _soundEffectButton = CreateUIButton(_mainMenuRectTransform, "SettingsButton");

            try
            {
                (_soundEffectButton.transform as RectTransform).anchoredPosition += new Vector2(-29f, 0f);
                (_soundEffectButton.transform as RectTransform).sizeDelta = new Vector2(28f, 10f);

                SetButtonText(ref _soundEffectButton, PlayerPrefs.HasKey("SoundEffectType") ? PlayerPrefs.GetString("SoundEffectType") : "None");

                _soundEffectButton.onClick.AddListener(delegate () {

                    try
                    {
                        // change sound effect type
                        string newSET;
                        if (PlayerPrefs.HasKey("SoundEffectType"))
                        {
                            string savedSET = PlayerPrefs.GetString("SoundEffectType");

                            switch (savedSET)
                            {
                                case "None":
                                    newSET = "Miss";
                                    break;
                                case "Miss":
                                    newSET = "Bomb";
                                    break;
                                case "Bomb":
                                    newSET = "Miss+Bomb";
                                    break;
                                case "Miss+Bomb":
                                default:
                                    newSET = "None";
                                    break;
                            }
                        }
                        else
                        {
                            newSET = "None";
                        }

                        PlayerPrefs.SetString("SoundEffectType", newSET);
                        SetButtonText(ref _soundEffectButton, newSET);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("EXCETPION IN BUTTON: " + e.Message);
                    }

                });

            }
            catch (Exception e)
            {
                Console.WriteLine("EXCEPTION: " + e.Message);
            }

        }



        public Button CreateUIButton(RectTransform parent, string buttonTemplate)
        {
            if (_buttonInstance == null)
            {
                return null;
            }

            Button btn = Instantiate(Resources.FindObjectsOfTypeAll<Button>().First(x => (x.name == buttonTemplate)), parent, false);
            DestroyImmediate(btn.GetComponent<SignalOnUIButtonClick>());
            btn.onClick = new Button.ButtonClickedEvent();

            return btn;
        }

        public Button CreateBackButton(RectTransform parent)
        {
            if (_backButtonInstance == null)
            {
                return null;
            }

            Button _button = Instantiate(_backButtonInstance, parent, false);
            DestroyImmediate(_button.GetComponent<SignalOnUIButtonClick>());
            _button.onClick = new Button.ButtonClickedEvent();

            return _button;
        }

        public T CreateViewController<T>() where T : VRUIViewController
        {
            T vc = new GameObject("CreatedViewController").AddComponent<T>();

            vc.rectTransform.anchorMin = new Vector2(0f, 0f);
            vc.rectTransform.anchorMax = new Vector2(1f, 1f);
            vc.rectTransform.sizeDelta = new Vector2(0f, 0f);
            vc.rectTransform.anchoredPosition = new Vector2(0f, 0f);

            return vc;
        }

        public TextMeshProUGUI CreateText(RectTransform parent, string text, Vector2 position)
        {
            TextMeshProUGUI textMesh = new GameObject("TextMeshProUGUI_GO").AddComponent<TextMeshProUGUI>();
            textMesh.rectTransform.SetParent(parent, false);
            textMesh.text = text;
            textMesh.fontSize = 4;
            textMesh.color = Color.white;
            textMesh.font = Resources.Load<TMP_FontAsset>("Teko-Medium SDF No Glow");
            textMesh.rectTransform.anchorMin = new Vector2(0.5f, 1f);
            textMesh.rectTransform.anchorMax = new Vector2(0.5f, 1f);
            textMesh.rectTransform.sizeDelta = new Vector2(60f, 10f);
            textMesh.rectTransform.anchoredPosition = position;

            return textMesh;
        }

        public void SetButtonText(ref Button _button, string _text)
        {
            if (_button.GetComponentInChildren<TextMeshProUGUI>() != null)
            {

                _button.GetComponentInChildren<TextMeshProUGUI>().text = _text;
            }

        }

        public void SetButtonTextSize(ref Button _button, float _fontSize)
        {
            if (_button.GetComponentInChildren<TextMeshProUGUI>() != null)
            {
                _button.GetComponentInChildren<TextMeshProUGUI>().fontSize = _fontSize;
            }


        }

        public void SetButtonIcon(ref Button _button, Sprite _icon)
        {
            if (_button.GetComponentsInChildren<UnityEngine.UI.Image>().Count() > 1)
            {

                _button.GetComponentsInChildren<UnityEngine.UI.Image>()[1].sprite = _icon;
            }

        }

        public void SetButtonBackground(ref Button _button, Sprite _background)
        {
            if (_button.GetComponentsInChildren<Image>().Any())
            {

                _button.GetComponentsInChildren<UnityEngine.UI.Image>()[0].sprite = _background;
            }

        }


    }
}