using IllusionPlugin;
using System;
using System.Linq;
using System.Media;
using System.Collections;
using UnityEngine;
using System.Windows.Forms;

namespace Sound_Effect
{
    class SoundEffect : MonoBehaviour
    {
        private GameEnergyCounter _energyCounter;
        private BeatmapObjectSpawnController _BMSpawnController;
        private MainAudioEffects _mainAudioEffects;
        private AudioSource audioSource;
        private AudioClip[] audioClips = new AudioClip[5];

        private bool bFailed;

        public static bool _dist = false;
        public static float _distLen = 0.15f;

        public static bool _miss = false;
        public static float _missVol = 0.75f;

        public static bool _bomb = false;
        public static float _bombVol = 0.75f;

        public static bool _hit = false;
        public static float _hitVol = 0.75f;

        public static bool _bad = false;
        public static float _badVol = 0.75f;

        public static bool _fail = false;
        public static float _failVol = 0.75f;

        /////////////////////////////////////////////////////
        private IEnumerator GetEnergyCounter()
        {
            bool loaded = false;
            while (!loaded)
            {
                _energyCounter = Resources.FindObjectsOfTypeAll<GameEnergyCounter>().FirstOrDefault();
                if (_energyCounter == null)
                    yield return new WaitForSeconds(0.1f);
                else
                    loaded = true;
            }

            if (_energyCounter != null)
            {
                if (_fail)
                    _energyCounter.gameEnergyDidReach0Event += _energyCounter_gameEnergyDidReach0Event;
            }
        }

        private IEnumerator GetBeatmapObjectSpawnController()
        {
            bool loaded = false;
            while (!loaded)
            {
                _BMSpawnController = Resources.FindObjectsOfTypeAll<BeatmapObjectSpawnController>().FirstOrDefault();
                if (_BMSpawnController == null)
                    yield return new WaitForSeconds(0.1f);
                else
                    loaded = true;
            }

            if (_BMSpawnController != null)
            {
                if (_bomb || _hit)
                    _BMSpawnController.noteWasCutEvent += _BMSpawnController_noteWasCutEvent;
                if (_miss)
                    _BMSpawnController.noteDidStartJumpEvent += _BMSpawnController_noteDidStartJumpEvent;
            }
        }

        private IEnumerator GetMainAudioEffects()
        {
            bool loaded = false;
            while (!loaded)
            {
                _mainAudioEffects = Resources.FindObjectsOfTypeAll<MainAudioEffects>().FirstOrDefault();
                if (_mainAudioEffects == null)
                    yield return new WaitForSeconds(0.1f);
                else
                    loaded = true;
            }
        }

        private IEnumerator LoadAudioFromFile(byte val, string audioPath)
        {
            using (var www = new WWW(audioPath))
            {
                yield return www;

                
                audioClips[val] = www.GetAudioClip(true, true, AudioType.UNKNOWN);

                while (audioClips[val].length == 0)
                {
                    yield return null;
                }
            }
        }

        private void LoadSoundEffects()
        {
            try
            {
                string url0 = System.Windows.Forms.Application.StartupPath + @"\UserData\Bomb.wav";
                StartCoroutine(LoadAudioFromFile(0, url0));

                string url1 = System.Windows.Forms.Application.StartupPath + @"\UserData\Miss.wav";
                StartCoroutine(LoadAudioFromFile(1, url1));

                string url2 = System.Windows.Forms.Application.StartupPath + @"\UserData\Fail.wav";
                StartCoroutine(LoadAudioFromFile(2, url2));

                string url3 = System.Windows.Forms.Application.StartupPath + @"\UserData\Hit.wav";
                StartCoroutine(LoadAudioFromFile(3, url3));

                string url4 = System.Windows.Forms.Application.StartupPath + @"\UserData\BadHit.wav";
                StartCoroutine(LoadAudioFromFile(4, url4));

                var newGameObject = new GameObject("My Audio Source");
                audioSource = newGameObject.AddComponent<AudioSource>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sound Effect", MessageBoxButtons.OK);
                throw;
            }
        }

        private void _BMSpawnController_noteDidStartJumpEvent(BeatmapObjectSpawnController arg1, NoteController arg2)
        {
            NoteMovement nm = arg2.GetComponent<NoteMovement>();
            if (nm != null && (arg2.noteData.noteType == NoteType.NoteA || arg2.noteData.noteType == NoteType.NoteB))
            {
                nm.noteDidPassMissedMarkerEvent += Nm_noteDidPassMissedMarkerEvent;
            }
        }

        private IEnumerator LowPass(float time)
        {
            for (float i = 0; i <= time; i += Time.deltaTime)
            {
                _mainAudioEffects.TriggerLowPass();
                yield return null;
            }
        }

        private void Nm_noteDidPassMissedMarkerEvent()
        {
            if (!bFailed)
            {
                try
                {
                    if (_dist)
                        StartCoroutine(LowPass(_distLen));
                    else
                        audioSource.PlayOneShot(audioClips[1], _missVol);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Miss: " + ex.Message, "Sound Effect", MessageBoxButtons.OK);
                    throw;
                }
            }
        }

        private void _BMSpawnController_noteWasCutEvent(BeatmapObjectSpawnController arg1, NoteController arg2, NoteCutInfo arg3)
        {
            NoteData noteData = arg2.noteData;
            if (noteData.noteType == NoteType.Bomb)
            {
                if (_bomb)
                {
                    try
                    {
                        audioSource.PlayOneShot(audioClips[0], _bombVol);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Bomb: " + ex.Message, "Sound Effect", MessageBoxButtons.OK);
                        throw;
                    }
                }
            }
            else
            {
                try
                {
                    if (_hit && arg3.saberTypeOK && arg3.allIsOK)
                        audioSource.PlayOneShot(audioClips[3], _hitVol);
                    //else if (_bad && !arg3.saberTypeOK)
                    else if (_bad && !arg3.allIsOK)
                        audioSource.PlayOneShot(audioClips[4], _badVol);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hit: " + ex.Message, "Sound Effect", MessageBoxButtons.OK);
                    throw;
                }
            }
        }

        void Awake()
        {
            _energyCounter = null;
            _BMSpawnController = null;
            bFailed = false;
            LoadSoundEffects();
            StartCoroutine(GetEnergyCounter());
            StartCoroutine(GetBeatmapObjectSpawnController());
            StartCoroutine(GetMainAudioEffects());
        }

        private void _energyCounter_gameEnergyDidReach0Event()
        {
            bFailed = true;
            try
            {
                audioSource.PlayOneShot(audioClips[2], _failVol);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fail: " + ex.Message, "Sound Effect", MessageBoxButtons.OK);
                throw;
            }
        }
    }
}