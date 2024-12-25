using DG.Tweening;
using System.Collections;
using UnityEngine;

/// <summary>
/// 音源管理クラス
/// </summary>
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    // BGM管理
    public enum BGM_Type
    {
        // BGM用の列挙子をゲームに合わせて登録

        testBGM_1 = 0,
        testBGM_2 = 1,
        testBGM_3 = 2,
        testBGM_4 = 3,
        testBGM_5 = 4,

        //SILENCE = 999,        // 無音状態をBGMとして作成したい場合には追加しておく。それ以外は不要
    }

    // SE管理
    public enum SE_Type
    {
        // SE用の列挙子をゲームに合わせて登録

        attack_01 = 0,
        attack_02 = 1,
        attack_03 = 2,
        attack_04 = 3,
        attack_05 = 4,
        defense_01 = 5,
        run_01 = 6,
        run_02 = 7,
        door_open = 8,
        door_close = 9,
        water_drop = 10,
        load = 11,
        alert = 12,
        push_01 = 13,
        push_02 = 14,
        push_03 = 15,
        push_04 = 16,
        push_05 = 17,
        push_06 = 18,
        cancel_01 = 19,
        cancel_02 = 20,
        menu_open_01 = 21,
        menu_open_02 = 22,
        menu_open_03 = 23,
        beep = 24,
        tap_01 = 25,
        tap_02 = 26
    }

    // VOICE管理
    public enum VOICE_Type
    {
        // VOICE用の列挙子をゲームに合わせて登録

        boar_01 = 0,
        boar_02 = 1,
        boar_03 = 2,
        cat_01 = 3,
        cat_02 = 4,
        cat_03 = 5,
        cat_04 = 6,
        cattle_01 = 7,
        dog_01 = 8,
        dog_02 = 9,
        dog_03 = 10,
        dog_04 = 11,
        dog_05 = 12,
        dog_06 = 13,
        elephant_01 = 14,
        elephant_02 = 15,
        elephant_03 = 16,
        goat_01 = 17,
        horse_01 = 18,
        horse_02 = 19,
        horse_03 = 20,
        horse_04 = 21,
        horse_05 = 22,
        horse_06 = 23,
        horse_07 = 24,
        horse_08 = 25,
        lion_01 = 26,
        lion_02 = 27,
        lion_03 = 28,
        pig_01 = 29,
        pig_02 = 30,
        sheep_01 = 31,
        wolf_01 = 32,
    }

    // クロスフェード時間
    public const float CrossFadeTime = 1.0f;

    // ボリューム関連
    public float bgmVolume = 0.1f;
    public float seVolume = 0.2f;
    public float voiceVolume = 0.2f;
    public bool Mute = false;

    // AudioClip
    public AudioClip[] bgmClips;
    public AudioClip[] seClips;
    public AudioClip[] voiceClips;

    // AudioSource
    private AudioSource[] _bgmSources = new AudioSource[2];
    private AudioSource[] _seSources = new AudioSource[16];
    private AudioSource[] _voiceSources = new AudioSource[16];

    private bool _isCrossFading;
    private int _currentBgmIndex = 999;

    void Awake()
    {
        // シングルトンかつ、シーン遷移しても破棄されないようにする
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // BGM用 AudioSource追加
        _bgmSources[0] = gameObject.AddComponent<AudioSource>();
        _bgmSources[1] = gameObject.AddComponent<AudioSource>();

        // SE用 AudioSource追加
        for (int i = 0; i < _seSources.Length; i++)
        {
            _seSources[i] = gameObject.AddComponent<AudioSource>();
        }

        // VOICE用 AudioSource追加
        for (int i = 0; i < _voiceSources.Length; i++)
        {
            _voiceSources[i] = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        // ボリューム設定
        if (!_isCrossFading)
        {
            _bgmSources[0].volume = bgmVolume;
            _bgmSources[1].volume = bgmVolume;
        }

        foreach (AudioSource source in _seSources)
        {
            source.volume = seVolume;
        }

        foreach (AudioSource source in _voiceSources)
        {
            source.volume = voiceVolume;
        }
    }

    /// <summary>
    /// BGM再生
    /// </summary>
    /// <param name="bgmType"></param>
    /// <param name="loopFlg"></param>
    public void PlayBGM(BGM_Type bgmType, bool loopFlg = true)
    {
        // BGMなしの状態にする場合            
        if ((int)bgmType == 999)
        {
            StopBGM();
            return;
        }

        int index = (int)bgmType;
        _currentBgmIndex = index;

        if (index < 0 || bgmClips.Length <= index)
        {
            return;
        }

        // 同じBGMの場合は何もしない
        if (_bgmSources[0].clip != null && _bgmSources[0].clip == bgmClips[index])
        {
            return;
        }
        else if (_bgmSources[1].clip != null && _bgmSources[1].clip == bgmClips[index])
        {
            return;
        }

        // フェードでBGM開始
        if (_bgmSources[0].clip == null && _bgmSources[1].clip == null)
        {
            _bgmSources[0].loop = loopFlg;
            _bgmSources[0].clip = bgmClips[index];
            _bgmSources[0].Play();
        }
        else
        {
            // クロスフェード処理
            StartCoroutine(CrossFadeChangeBMG(index, loopFlg));
        }
    }

    /// <summary>
    /// BGMのクロスフェード処理
    /// </summary>
    /// <param name="index">AudioClipの番号</param>
    /// <param name="loopFlg">ループ設定。ループしない場合だけfalse指定</param>
    /// <returns></returns>
    private IEnumerator CrossFadeChangeBMG(int index, bool loopFlg)
    {
        _isCrossFading = true;
        if (_bgmSources[0].clip != null)
        {
            // [0]が再生されている場合、[0]の音量を徐々に下げて、[1]を新しい曲として再生
            _bgmSources[1].volume = 0;
            _bgmSources[1].clip = bgmClips[index];
            _bgmSources[1].loop = loopFlg;
            _bgmSources[1].Play();
            _bgmSources[1].DOFade(1.0f, CrossFadeTime).SetEase(Ease.Linear);
            _bgmSources[0].DOFade(0, CrossFadeTime).SetEase(Ease.Linear);

            yield return new WaitForSeconds(CrossFadeTime);
            _bgmSources[0].Stop();
            _bgmSources[0].clip = null;
        }
        else
        {
            // [1]が再生されている場合、[1]の音量を徐々に下げて、[0]を新しい曲として再生
            _bgmSources[0].volume = 0;
            _bgmSources[0].clip = bgmClips[index];
            _bgmSources[0].loop = loopFlg;
            _bgmSources[0].Play();
            _bgmSources[0].DOFade(1.0f, CrossFadeTime).SetEase(Ease.Linear);
            _bgmSources[1].DOFade(0, CrossFadeTime).SetEase(Ease.Linear);

            yield return new WaitForSeconds(CrossFadeTime);
            _bgmSources[1].Stop();
            _bgmSources[1].clip = null;
        }
        _isCrossFading = false;
    }

    /// <summary>
    /// BGM完全停止
    /// </summary>
    public void StopBGM()
    {
        _bgmSources[0].Stop();
        _bgmSources[1].Stop();
        _bgmSources[0].clip = null;
        _bgmSources[1].clip = null;
    }

    /// <summary>
    /// SE再生
    /// </summary>
    /// <param name="seType"></param>
    public void PlaySE(SE_Type seType)
    {
        int index = (int)seType;
        if (index < 0 || seClips.Length <= index)
        {
            return;
        }

        // 再生中ではないAudioSourceをつかってSEを鳴らす
        foreach (AudioSource source in _seSources)
        {

            // 再生中の AudioSource の場合には次のループ処理へ移る
            if (source.isPlaying)
            {
                continue;
            }

            // 再生中でない AudioSource に Clip をセットして SE を鳴らす
            source.clip = seClips[index];
            source.Play();
            break;
        }
    }

    /// <summary>
    /// SE停止
    /// </summary>
    public void StopSE()
    {
        // 全てのSE用のAudioSourceを停止する
        foreach (AudioSource source in _seSources)
        {
            source.Stop();
            source.clip = null;
        }
    }

    /// <summary>
    /// VOICE再生
    /// </summary>
    /// <param name="voiceType"></param>
    public void PlayVOICE(VOICE_Type voiceType)
    {
        int index = (int)voiceType;
        if (index < 0 || voiceClips.Length <= index)
        {
            return;
        }

        // 再生中ではないAudioSourceをつかってVOICEを鳴らす
        foreach (AudioSource source in _voiceSources)
        {

            // 再生中の AudioSource の場合には次のループ処理へ移る
            if (source.isPlaying)
            {
                continue;
            }

            // 再生中でない AudioSource に Clip をセットして VOICE を鳴らす
            source.clip = voiceClips[index];
            source.Play();
            break;
        }
    }

    /// <summary>
    /// VOICE停止
    /// </summary>
    public void StopVOICE()
    {
        // 全てのVOICE用のAudioSourceを停止する
        foreach (AudioSource source in _voiceSources)
        {
            source.Stop();
            source.clip = null;
        }
    }

    /// <summary>
    /// BGM一時停止
    /// </summary>
    public void MuteBGM()
    {
        _bgmSources[0].Stop();
        _bgmSources[1].Stop();
    }

    /// <summary>
    /// 一時停止した同じBGMを再生(再開)
    /// </summary>
    public void ResumeBGM()
    {
        _bgmSources[0].Play();
        _bgmSources[1].Play();
    }
}