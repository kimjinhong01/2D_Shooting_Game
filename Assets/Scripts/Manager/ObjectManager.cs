using UnityEngine;

// 오브젝트 풀링
public class ObjectManager : MonoBehaviour
{
    // 각 몬스터별 프리팹
    public GameObject bossPrefab;
    public GameObject enemyAPrefab;
    public GameObject enemyBPrefab;
    public GameObject enemyCPrefab;

    public GameObject coinItemPrefab;       // 코인 프리팹
    public GameObject powerItemPrefab;      // 파워 프리팹
    public GameObject boomItemPrefab;       // 폭탄 프리팹

    // 각 종류별 총알 프리팹
    public GameObject enemyBulletAPrefab;
    public GameObject enemyBulletBPrefab;
    public GameObject enemyBulletCPrefab;
    public GameObject enemyBulletDPrefab;
    public GameObject playerBulletAPrefab;
    public GameObject playerBulletBPrefab;
    public GameObject followerBulletPrefab;

    // 폭파 효과 프리팹
    public GameObject explosionPrefab;

    // 미리 만들어두는 게임 오브젝트들
    private GameObject[] boss;
    private GameObject[] enemyA;
    private GameObject[] enemyB;
    private GameObject[] enemyC;

    private GameObject[] coinItem;
    private GameObject[] powerItem;
    private GameObject[] boomItem;

    private GameObject[] enemyBulletA;
    private GameObject[] enemyBulletB;
    private GameObject[] enemyBulletC;
    private GameObject[] enemyBulletD;
    private GameObject[] playerBulletA;
    private GameObject[] playerBulletB;

    private GameObject[] followerBullet;
    private GameObject[] targetPool;
    private GameObject[] explosion;

    private void Awake()
    {
        // 게임 오브젝트 미리 만들어두기
        boss = new GameObject[5];
        enemyA = new GameObject[20];
        enemyB = new GameObject[20];
        enemyC = new GameObject[20];

        coinItem = new GameObject[20];
        powerItem = new GameObject[20];
        boomItem = new GameObject[20];

        enemyBulletA = new GameObject[100];
        enemyBulletB = new GameObject[500];
        enemyBulletC = new GameObject[100];
        enemyBulletD = new GameObject[100];
        playerBulletA = new GameObject[100];
        playerBulletB = new GameObject[100];
        followerBullet = new GameObject[100];
        explosion = new GameObject[50];

        Generate();
    }

    private void Generate()
    {
        // 보스 세팅
        for (int i = 0; i < boss.Length; i++)
        {
            boss[i] = Instantiate(bossPrefab);
            boss[i].SetActive(false);
        }

        // 몬스터 세팅
        for (int i = 0; i < enemyA.Length; i++)
        {
            enemyA[i] = Instantiate(enemyAPrefab);
            enemyA[i].SetActive(false);
        }

        for (int i = 0; i < enemyB.Length; i++)
        {
            enemyB[i] = Instantiate(enemyBPrefab);
            enemyB[i].SetActive(false);
        }

        for (int i = 0; i < enemyC.Length; i++)
        {
            enemyC[i] = Instantiate(enemyCPrefab);
            enemyC[i].SetActive(false);
        }

        // 아이템 세팅
        for (int i = 0; i < coinItem.Length; i++)
        {
            coinItem[i] = Instantiate(coinItemPrefab);
            coinItem[i].SetActive(false);
        }

        for (int i = 0; i < powerItem.Length; i++)
        {
            powerItem[i] = Instantiate(powerItemPrefab);
            powerItem[i].SetActive(false);
        }

        for (int i = 0; i < boomItem.Length; i++)
        {
            boomItem[i] = Instantiate(boomItemPrefab);
            boomItem[i].SetActive(false);
        }

        // 몬스터 총알 세팅
        for (int i = 0; i < enemyBulletA.Length; i++)
        {
            enemyBulletA[i] = Instantiate(enemyBulletAPrefab);
            enemyBulletA[i].SetActive(false);
        }

        for (int i = 0; i < enemyBulletB.Length; i++)
        {
            enemyBulletB[i] = Instantiate(enemyBulletBPrefab);
            enemyBulletB[i].SetActive(false);
        }

        for (int i = 0; i < enemyBulletC.Length; i++)
        {
            enemyBulletC[i] = Instantiate(enemyBulletCPrefab);
            enemyBulletC[i].SetActive(false);
        }

        for (int i = 0; i < enemyBulletD.Length; i++)
        {
            enemyBulletD[i] = Instantiate(enemyBulletDPrefab);
            enemyBulletD[i].SetActive(false);
        }

        // 플레이어 총알 세팅
        for (int i = 0; i < playerBulletA.Length; i++)
        {
            playerBulletA[i] = Instantiate(playerBulletAPrefab);
            playerBulletA[i].SetActive(false);
        }

        for (int i = 0; i < playerBulletB.Length; i++)
        {
            playerBulletB[i] = Instantiate(playerBulletBPrefab);
            playerBulletB[i].SetActive(false);
        }

        // 팔로워 총알 세팅
        for (int i = 0; i < followerBullet.Length; i++)
        {
            followerBullet[i] = Instantiate(followerBulletPrefab);
            followerBullet[i].SetActive(false);
        }

        // 폭발 효과 세팅
        for (int i = 0; i < explosion.Length; i++)
        {
            explosion[i] = Instantiate(explosionPrefab);
            explosion[i].SetActive(false);
        }
    }

    // 사용 중이지 않은 오브젝트 반환
    public GameObject MakeObj(string type)
    {
        switch (type)
        {
            case "boss":
                targetPool = boss;
                break;
            case "enemyA":
                targetPool = enemyA;
                break;
            case "enemyB":
                targetPool = enemyB;
                break;
            case "enemyC":
                targetPool = enemyC;
                break;
            case "coinItem":
                targetPool = coinItem;
                break;
            case "powerItem":
                targetPool = powerItem;
                break;
            case "boomItem":
                targetPool = boomItem;
                break;
            case "enemyBulletA":
                targetPool = enemyBulletA;
                break;
            case "enemyBulletB":
                targetPool = enemyBulletB;
                break;
            case "enemyBulletC":
                targetPool = enemyBulletC;
                break;
            case "enemyBulletD":
                targetPool = enemyBulletD;
                break;
            case "playerBulletA":
                targetPool = playerBulletA;
                break;
            case "playerBulletB":
                targetPool = playerBulletB;
                break;
            case "followerBullet":
                targetPool = followerBullet;
                break;
            case "explosion":
                targetPool = explosion;
                break;
        }

        for (int i = 0; i < targetPool.Length; i++)
        {
            if (!targetPool[i].activeSelf)
            {
                targetPool[i].SetActive(true);
                return targetPool[i];
            }
        }

        return null;
    }

    // 오브젝트 풀 반환
    public GameObject[] GetPool(string type)
    {
        switch (type)
        {
            case "boss":
                targetPool = boss;
                break;
            case "enemyA":
                targetPool = enemyA;
                break;
            case "enemyB":
                targetPool = enemyB;
                break;
            case "enemyC":
                targetPool = enemyC;
                break;
            case "coinItem":
                targetPool = coinItem;
                break;
            case "powerItem":
                targetPool = powerItem;
                break;
            case "boomItem":
                targetPool = boomItem;
                break;
            case "enemyBulletA":
                targetPool = enemyBulletA;
                break;
            case "enemyBulletB":
                targetPool = enemyBulletB;
                break;
            case "enemyBulletC":
                targetPool = enemyBulletC;
                break;
            case "enemyBulletD":
                targetPool = enemyBulletD;
                break;
            case "playerBulletA":
                targetPool = playerBulletA;
                break;
            case "playerBulletB":
                targetPool = playerBulletB;
                break;
            case "followerBullet":
                targetPool = followerBullet;
                break;
            case "explosion":
                targetPool = explosion;
                break;
        }

        return targetPool;
    }
}
