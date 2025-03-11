using UnityEngine;

// 싱글톤 패턴
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("#BGM")]
    public AudioClip bgmClip;           // 배경음
    public float bgmVolume;             // 배경음 볼륨
    private AudioSource bgmPlayer;      // 배경음 플레이어

    [Header("#SFX")]
    public AudioClip[] sfxClips;        // 효고음들
    public float sfxVolume;             // 효과음 볼륨
    public int channels;                // 채널 개수
    private AudioSource[] sfxPlayers;   // 효과음 플레이어들
    int channelIndex;                   // 현재 채널 번호

    // 효과음 종류
    public enum Sfx
    {
        Fire,
        Hit,
        Die,
        Item,
        Boom,
        Respawn,
        GameStart,
        GameOver,
        GameClear
    }

    private void Awake()
    {
        instance = this;

        Init();
    }
    
    // 오디오 플레이어 초기화
    private void Init()
    {
        // 배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform; // AudioManager의 자식으로
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;

        // 효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform; // AudioManager의 자식으로
        sfxPlayers = new AudioSource[channels];

        for(int i = 0; i < channels; i++)
        {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].volume = sfxVolume;
        }
    }

    // 배경음 재생
    public void PlayBgm(bool isPlay)
    {
        if (isPlay)
            bgmPlayer.Play();
        else
            bgmPlayer.Stop();
    }

    // 효과음 재생
    public void PlaySfx(Sfx sfx)
    {
        for (int i = 0; i < channels; i++)
        {
            int loopIndex = (i + channelIndex) % channels;

            if (sfxPlayers[loopIndex].isPlaying)
                continue;

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }

    // 배경음 재생 (볼륨 조절)
    public void PlaySfx(Sfx sfx, float volume)
    {
        for (int i = 0; i < channels; i++)
        {
            int loopIndex = (i + channelIndex) % channels;

            if (sfxPlayers[loopIndex].isPlaying)
                continue;

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].volume = volume;
            sfxPlayers[loopIndex].Play();
            sfxPlayers[loopIndex].volume = sfxVolume;
            break;
        }
    }
}
