using UnityEngine;

// �̱��� ����
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("#BGM")]
    public AudioClip bgmClip;           // �����
    public float bgmVolume;             // ����� ����
    private AudioSource bgmPlayer;      // ����� �÷��̾�

    [Header("#SFX")]
    public AudioClip[] sfxClips;        // ȿ������
    public float sfxVolume;             // ȿ���� ����
    public int channels;                // ä�� ����
    private AudioSource[] sfxPlayers;   // ȿ���� �÷��̾��
    int channelIndex;                   // ���� ä�� ��ȣ

    // ȿ���� ����
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
    
    // ����� �÷��̾� �ʱ�ȭ
    private void Init()
    {
        // ����� �÷��̾� �ʱ�ȭ
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform; // AudioManager�� �ڽ�����
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;

        // ȿ���� �÷��̾� �ʱ�ȭ
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform; // AudioManager�� �ڽ�����
        sfxPlayers = new AudioSource[channels];

        for(int i = 0; i < channels; i++)
        {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].volume = sfxVolume;
        }
    }

    // ����� ���
    public void PlayBgm(bool isPlay)
    {
        if (isPlay)
            bgmPlayer.Play();
        else
            bgmPlayer.Stop();
    }

    // ȿ���� ���
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

    // ����� ��� (���� ����)
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
