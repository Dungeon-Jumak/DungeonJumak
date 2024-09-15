using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

using System.Collections.Generic;
using System.Linq;

using Cysharp.Threading.Tasks;
using Unity.VisualScripting;

namespace Utils.EnumTypes
{
    public class SoundManager : MonoBehaviour
    {
        private AudioSource[] m_audioSources;

        private List<AudioSource> m_audioSourcePool = new List<AudioSource>();
        private Dictionary<string, AudioClip> m_bgmClips = new Dictionary<string, AudioClip>();
        private Dictionary<string, AudioClip[]> m_effectClips = new Dictionary<string, AudioClip[]>();
        private Dictionary<AudioSource, bool> m_audioSourceStates = new Dictionary<AudioSource, bool>();

        private int m_poolSize = 10;
        private bool m_isPaused = false;

        // ==================================== //

        /// <summary>
        /// SoundManager 초기화 및 초기 세팅.
        /// </summary>
        public async UniTask InitializeAsync()
        {
            GameObject root = GameObject.Find("@Sound");
            DontDestroyOnLoad(root);

            SetAudioSources(root);
            SetAudioSourcePool(root);

            await LoadBgmClipsAsync();
            await LoadEffectClipsAsync();
        }

        /// <summary>
        /// Bgm 재생.
        /// </summary>
        /// <param name="_name"> Bgm 이름 </param>
        /// <param name="_loop"> 반복 유무 </param>
        public void Play(string _name, bool _loop = false)
        {
            PlayBgm(_name, _loop);
        }

        /// <summary>
        /// Sfx 재생.
        /// </summary>
        /// <param name="_labelEnum"> 라벨 종류 (Main_SFX | Dungeon_SFX | ETC_SFX) </param>
        /// <param name="_name"> Sfx 이름 </param>
        /// <param name="_loop"> 반복 유무 </param>
        public void Play(SFX_Label _labelEnum, string _name, bool _loop = false)
        {
            PlaySfx(_labelEnum, _name, _loop);
        }

        /// <summary>
        /// 재생 중인 모든 사운드를 일시 정지.
        /// </summary>
        public void Pause()
        {
            if (m_isPaused) return;

            PauseAudioSource(m_audioSources[(int)SoundType.Bgm]);

            foreach (var audioSource in m_audioSourcePool)
            {
                PauseAudioSource(audioSource);
            }

            m_isPaused = true;
        }

        /// <summary>
        /// 일시 정지된 모든 사운드를 다시 시작.
        /// </summary>
        public void Resume()
        {
            if (!m_isPaused) return;

            ResumeAudioSource(m_audioSources[(int)SoundType.Bgm]);

            foreach (var audioSource in m_audioSourcePool)
            {
                ResumeAudioSource(audioSource);
            }

            m_isPaused = false;
        }

        /// <summary>
        /// 특정 Bgm 소리 중지 및 제거.
        /// </summary>
        /// <param name="_name"> Bgm 이름 </param>
        public void Clear(string _name)
        {
            ClearBgm(_name);
        }

        /// <summary>
        /// 특정 Sfx 소리 중지 및 제거.
        /// </summary>
        /// <param name="_label"> 라벨 종류 (Main_SFX | Dungeon_SFX | ETC_SFX) </param>
        /// <param name="_name"> Sfx 이름 </param>
        public void Clear(SFX_Label _label, string _name)
        {
            ClearSfx(_label, _name);
        }

        /// <summary>
        /// 모든 소리 제거.
        /// </summary>
        public void Clear()
        {
            ClearBgm();

            foreach (var audioSource in m_audioSourcePool)
            {
                audioSource.Stop();
                audioSource.gameObject.SetActive(false);
            }
        }

        // ==================================== //

        #region Setting
        private void SetAudioSources(GameObject _root)
        {
            string[] soundNames = System.Enum.GetNames(typeof(SoundType));
            m_audioSources = new AudioSource[soundNames.Length - 1];

            for (int i = 0; i < m_audioSources.Length; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] };
                m_audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = _root.transform;
            }

            m_audioSources[(int)SoundType.Bgm].loop = true;
        }

        private void SetAudioSourcePool(GameObject _root)
        {
            for (int i = 0; i < m_poolSize; i++)
            {
                AudioSource audioSource = new GameObject("PooledSFX").AddComponent<AudioSource>();
                audioSource.transform.parent = _root.transform;
                audioSource.gameObject.SetActive(false);
                m_audioSourcePool.Add(audioSource);
            }
        }

        private async UniTask LoadBgmClipsAsync()
        {
            var handle = Addressables.LoadAssetsAsync<AudioClip>("BGM", null);
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                m_bgmClips = handle.Result.ToDictionary(clip => clip.name, clip => clip);
            }
        }

        private async UniTask LoadEffectClipsAsync()
        {
            foreach (SFX_Label labelEnum in System.Enum.GetValues(typeof(SFX_Label)))
            {
                string label = labelEnum.ToString(); // 열거형을 문자열로 변환
                var handle = Addressables.LoadAssetsAsync<AudioClip>(label, null);
                await handle.Task;

                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    m_effectClips[label] = handle.Result.ToArray();
                }
            }
        }
        #endregion

        #region Sound Play Manager
        private void PlayBgm(string _name, bool _loop)
        {
            if (m_bgmClips.TryGetValue(_name, out AudioClip clip))
            {
                AudioSource audioSource = m_audioSources[(int)SoundType.Bgm];
                audioSource.clip = clip;
                audioSource.loop = _loop;

                GetAudioMixerByLabel("BGM_Mixer", audioMixer =>
                {
                    if (audioMixer != null)
                    {
                        AudioMixerGroup[] mixerGroups = audioMixer.FindMatchingGroups("BGM_Mixer");
                        audioSource.outputAudioMixerGroup = mixerGroups.FirstOrDefault();
                        audioSource.Play();
                    }
                });
            }
        }

        private void PlaySfx(SFX_Label _labelEnum, string _name, bool _loop)
        {
            string label = _labelEnum.ToString();
            if (m_effectClips.TryGetValue(label, out AudioClip[] clips))
            {
                AudioClip clip = clips.FirstOrDefault(c => c.name == _name);
                if (clip != null)
                {
                    AudioSource audioSource = GetPooledAudioSource();
                    audioSource.clip = clip;

                    GetAudioMixerByLabel("SFX_Mixer", audioMixer =>
                    {
                        if (audioMixer != null)
                        {
                            AudioMixerGroup[] mixerGroups = audioMixer.FindMatchingGroups("SFX_Mixer");
                            audioSource.outputAudioMixerGroup = mixerGroups.FirstOrDefault();
                            audioSource.loop = _loop;
                            audioSource.Play();
                            if (!_loop)
                            {
                                DisableAudioSourceAsync(audioSource, clip.length).Forget();
                            }
                        }
                    });
                }
            }
        }
        #endregion

        #region Audio Source Pool Manager
        private AudioSource GetPooledAudioSource()
        {
            var availableSource = m_audioSourcePool.FirstOrDefault(source => !source.gameObject.activeInHierarchy);

            if (availableSource != null)
            {
                availableSource.gameObject.SetActive(true);
                return availableSource;
            }

            AudioSource newSource = new GameObject("PooledSFX").AddComponent<AudioSource>();
            newSource.transform.parent = GameObject.Find("@Sound").transform;
            m_audioSourcePool.Add(newSource);
            return newSource;
        }

        private async UniTaskVoid DisableAudioSourceAsync(AudioSource _audioSource, float _length)
        {
            await UniTask.Delay((int)(_length * 1000));

            if (_audioSource != null)
            {
                _audioSource.Stop();
                _audioSource.gameObject.SetActive(false);
            }
        }
        #endregion

        #region Pause & Resume Manager
        private void PauseAudioSource(AudioSource _audioSource)
        {
            if (_audioSource.isPlaying)
            {
                m_audioSourceStates[_audioSource] = true;
                _audioSource.Pause();
            }
            else
            {
                m_audioSourceStates[_audioSource] = false;
            }
        }

        private void ResumeAudioSource(AudioSource _audioSource)
        {
            if (m_audioSourceStates.TryGetValue(_audioSource, out bool wasPlaying) && wasPlaying)
            {
                _audioSource.UnPause();
            }
        }
        #endregion

        #region Clear Manager
        private void ClearBgm(string _name)
        {
            if (m_bgmClips.TryGetValue(_name, out AudioClip clip))
            {
                AudioSource bgmSource = m_audioSources[(int)SoundType.Bgm];

                if (bgmSource.clip == clip)
                {
                    bgmSource.Stop();
                    bgmSource.clip = null;
                }
            }
        }

        private void ClearSfx(SFX_Label _label, string _name)
        {
            string label = _label.ToString();

            if (m_effectClips.TryGetValue(label, out AudioClip[] clips))
            {
                AudioClip clip = clips.FirstOrDefault(c => c.name == _name);

                if (clip != null)
                {
                    foreach (var audioSource in m_audioSourcePool)
                    {
                        if (audioSource.clip == clip)
                        {
                            audioSource.Stop();
                            audioSource.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }

        private void ClearBgm()
        {
            AudioSource bgmSource = m_audioSources[(int)SoundType.Bgm];
            bgmSource.Stop();
            bgmSource.clip = null;
        }
        #endregion

        #region Audio Mixer Manager
        private void GetAudioMixerByLabel(string _mixerGroup, System.Action<AudioMixer> _onComplete)
        {
            Addressables.LoadAssetsAsync<AudioMixer>("Mixer", null).Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    AudioMixer audioMixer = handle.Result.FirstOrDefault(mixer => mixer.FindMatchingGroups(_mixerGroup).Length > 0);
                    if (audioMixer != null)
                    {
                        _onComplete?.Invoke(audioMixer);
                    }
                }
            };
        }
        #endregion
    }
}