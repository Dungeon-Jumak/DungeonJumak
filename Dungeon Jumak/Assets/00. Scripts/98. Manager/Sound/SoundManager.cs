using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Linq;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;

public class SoundManager : MonoBehaviour
{
    private AudioClip[] m_bgmClips;
    private AudioSource[] m_audioSources;

    private List<AudioSource> m_audioSourcePool = new List<AudioSource>();

    private Dictionary<string, AudioClip> m_audioClips = new Dictionary<string, AudioClip>();
    private Dictionary<AudioSource, bool> m_audioSourceStates = new Dictionary<AudioSource, bool>();
    private Dictionary<Define.SFX_Label, AudioClip[]> m_effectClips = new Dictionary<Define.SFX_Label, AudioClip[]>();

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
    /// <param name="_type"> 사운드 종류 (Bgm | Effect) </param>
    /// <param name="_index"> 재생하려는 Bgm 이 'BGM' 폴더에서 몇 번째 순서인지 _ 0번부터 시작) </param>
    /// <param name="_loop"> 반복 유무 </param>
    public void Play(Define.Sound _type, int _index, bool _loop = false)
    {
        if (_type == Define.Sound.Bgm)
        {
            PlayBgm(_index, _loop);
        }
    }

    /// <summary>
    /// Sfx 재생.
    /// </summary>
    /// <param name="_labelEnum"> 라벨 종류 (Main | Dungeon | ETC) </param>
    /// <param name="_index"> 재생하려는 Sfx 가 'SFX' 폴더에서 몇 번째 순서인지 _ 0번부터 시작) </param>
    /// <param name="_loop"> 반복 유무 </param>
    public void Play(Define.SFX_Label _labelEnum, int _index, bool _loop = false)
    {
        PlaySfx(_labelEnum, _index, _loop);
    }

    /// <summary>
    /// 재생 중인 모든 사운드를 일시 정지.
    /// </summary>
    public void Pause()
    {
        if (m_isPaused) return;

        PauseAudioSource(m_audioSources[(int)Define.Sound.Bgm]);

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

        ResumeAudioSource(m_audioSources[(int)Define.Sound.Bgm]);

        foreach (var audioSource in m_audioSourcePool)
        {
            ResumeAudioSource(audioSource);
        }

        m_isPaused = false;
    }

    /// <summary>
    /// 특정 Bgm 소리 중지 및 제거.
    /// </summary>
    /// <param name="_type"> 사운드 종류 (Bgm | Effect) </param>
    /// <param name="_index"> 중지하려는 Bgm 이 'BGM' 폴더에서 몇 번째 순서인지 _ 0번부터 시작) </param>
    public void Clear(Define.Sound _type, int _index)
    {
        if (_type == Define.Sound.Bgm)
        {
            ClearBgm(_index);
        }
        else
        {
            return;
        }
    }

    /// <summary>
    /// 특정 Sfx 소리 중지 및 제거.
    /// </summary>
    /// <param name="_label"> 라벨 종류 (Main | Dungeon | ETC) </param>
    /// <param name="_index"> 중지하려는 Sfx 가 'SFX' 폴더에서 몇 번째 순서인지 _ 0번부터 시작) </param>
    public void Clear(Define.SFX_Label _label, int _index)
    {
        ClearSfx(_label, _index);
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
        string[] soundNames = System.Enum.GetNames(typeof(Define.Sound));
        m_audioSources = new AudioSource[soundNames.Length - 1];

        for (int i = 0; i < m_audioSources.Length; i++)
        {
            GameObject go = new GameObject { name = soundNames[i] };
            m_audioSources[i] = go.AddComponent<AudioSource>();
            go.transform.parent = _root.transform;
        }

        m_audioSources[(int)Define.Sound.Bgm].loop = true;
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
            m_bgmClips = handle.Result.ToArray();
        }
    }

    private async UniTask LoadEffectClipsAsync()
    {
        foreach (Define.SFX_Label labelEnum in System.Enum.GetValues(typeof(Define.SFX_Label)))
        {
            string label = labelEnum.ToString();
            var handle = Addressables.LoadAssetsAsync<AudioClip>(label, null);
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                m_effectClips[labelEnum] = handle.Result.ToArray();
            }
        }
    }
    #endregion

    #region Sound Play Manager
    private void PlayBgm(int _index, bool _loop)
    {
        if (m_bgmClips != null && m_bgmClips.Length > _index)
        {
            AudioClip selectedClip = m_bgmClips[_index];
            AudioSource audioSource = m_audioSources[(int)Define.Sound.Bgm];
            audioSource.clip = selectedClip;
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

    private void PlaySfx(Define.SFX_Label _labelEnum, int _index, bool _loop)
    {
        if (m_effectClips.TryGetValue(_labelEnum, out AudioClip[] clips) && clips.Length > _index)
        {
            AudioClip selectedClip = clips[_index];
            AudioSource audioSource = GetPooledAudioSource();
            audioSource.clip = selectedClip;

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
                        DisableAudioSourceAsync(audioSource, selectedClip.length).Forget();
                    }
                }
            });
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
    private void ClearBgm(int _index)
    {
        if (m_bgmClips != null && m_bgmClips.Length > _index)
        {
            AudioSource bgmSource = m_audioSources[(int)Define.Sound.Bgm];

            if (bgmSource.clip == m_bgmClips[_index])
            {
                bgmSource.Stop();
                bgmSource.clip = null;
            }
        }
    }

    private void ClearSfx(Define.SFX_Label _label, int _index)
    {
        if (m_effectClips.TryGetValue(_label, out AudioClip[] clips) && clips.Length > _index)
        {
            AudioClip targetClip = clips[_index];

            foreach (var audioSource in m_audioSourcePool)
            {
                if (audioSource.clip == targetClip)
                {
                    audioSource.Stop();
                    audioSource.gameObject.SetActive(false);
                }
            }
        }
    }

    private void ClearBgm()
    {
        AudioSource bgmSource = m_audioSources[(int)Define.Sound.Bgm];
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
