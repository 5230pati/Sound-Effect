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
        private AudioSource audioSource;
        private AudioClip[] audioClips = new AudioClip[3];

        private bool bFailed;

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

            if (PlayerPrefs.HasKey("SoundEffectType"))
            {
                if (_BMSpawnController != null)
                {
                    string savedSET = PlayerPrefs.GetString("SoundEffectType");
                    if (savedSET.Contains("Bomb"))
                        _BMSpawnController.noteWasCutEvent += _BMSpawnController_noteWasCutEvent;
                    if (savedSET.Contains("Miss"))
                        _BMSpawnController.noteDidStartJumpEvent += _BMSpawnController_noteDidStartJumpEvent;
                }
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
                string url1 = System.Windows.Forms.Application.StartupPath + @"\UserData\Miss.wav";
                string url2 = System.Windows.Forms.Application.StartupPath + @"\UserData\Fail.wav";
                StartCoroutine(LoadAudioFromFile(0, url0));
                StartCoroutine(LoadAudioFromFile(1, url1));
                StartCoroutine(LoadAudioFromFile(2, url2));
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

        private void Nm_noteDidPassMissedMarkerEvent()
        {
            if (!bFailed)
            {
                try
                {
                    audioSource.PlayOneShot(audioClips[1], 0.75f);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Sound Effect", MessageBoxButtons.OK);
                    throw;
                }
            }
        }

        private void _BMSpawnController_noteWasCutEvent(BeatmapObjectSpawnController arg1, NoteController arg2, NoteCutInfo arg3)
        {
            NoteData noteData = arg2.noteData;
            if (noteData.noteType == NoteType.Bomb)
            {
                try
                {
                    audioSource.PlayOneShot(audioClips[0], 0.75f);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Sound Effect", MessageBoxButtons.OK);
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
        }

        private void _energyCounter_gameEnergyDidReach0Event()
        {
            bFailed = true;
            try
            {
                audioSource.PlayOneShot(audioClips[2], 0.75f);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sound Effect", MessageBoxButtons.OK);
                throw;
            }
        }
    }
}