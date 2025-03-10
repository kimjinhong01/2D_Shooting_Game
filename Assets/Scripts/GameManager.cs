using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    public string[] enemies;
    public Player player;
    public GameObject boss;

    public float realCoolTime;
    private float coolTime;
    private float curTime;
    private bool isInvoke;

    public Text scoreText;
    public Image[] lifeIcons;
    public Image[] boomIcons;
    public GameObject gameOverUI;

    public ObjectManager objectManager;

    public List<SpawnPattern> spawnList;
    public int spawnIndex;
    public bool spawnEnd;

    public Transform[] spawnPoints;

    private float curTime2;
    private float curTime3;
    public float stageTime;
    public int stage;
    public int maxStage;
    public int curRepeat;
    public int maxRepeat;

    public GameObject stageStart;
    public GameObject stageClear;

    public GameObject fade;

    private bool gameEnd;
    public GameObject gameClearUI;
    public Text scoreBoard;
    public Text gameOverScoreBoard;

    public GameObject LoadingUI;
    public GameObject InvinsibleUI;

    private void Awake()
    {
        gameEnd = true;

        spawnList = new List<SpawnPattern>();
        enemies = new string[] { "enemyA", "enemyB", "enemyC", "boss" };

        Invoke("StageStart", 3);
    }

    private void Start()
    {
        AudioManager.instance.PlayBgm(true);
    }

    private void StageStart()
    {
        LoadingUI.GetComponent<Animator>().SetTrigger("OnStart");

        Invoke("GameStart", 1);
    }

    private void GameStart()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.GameStart);
        stageStart.SetActive(true);
        stageStart.GetComponent<Animator>().SetTrigger("showText");
        fade.GetComponent<Animator>().SetTrigger("FadeOut");

        Invoke("activeSet", 2);
    }

    private void activeSet()
    {
        LoadingUI.SetActive(false);
        gameEnd = false;
        ReadSpawnFile("Stage 0");

        stageStart.SetActive(false);
        fade.SetActive(false);
    }

    private void StageClear()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.GameClear);

        stageClear.SetActive(true);
        stageClear.GetComponent<Animator>().SetTrigger("showText");
        fade.SetActive(true);
        fade.GetComponent<Animator>().SetTrigger("FadeIn");

        Invoke("EndGame", 2);
    }

    private void EndGame()
    {
        stageClear.SetActive(false);
        gameClearUI.SetActive(true);
        scoreBoard.text = "Your score\n" + string.Format("{0:n0}", player.score);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void ReadSpawnFile(string stageName)
    {
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;

        // TextAsset : 텍스트 파일 및 에셋 클래스
        // Resources.Load() : Resources 폴더 내 파일 불러오기
        // as : 검증하는 키워드
        TextAsset textFile = Resources.Load(stageName) as TextAsset;
        // StringReader : 파일 내의 문자열 데이터 읽기 클래스
        StringReader stringReader = new StringReader(textFile.text);

        while (stringReader != null)
        {
            // ReadLine() : 텍스트 데이터를 한 줄씩 반환(자동 줄바꿈)
            string line = stringReader.ReadLine();

            if (line == null)
                break;

            SpawnPattern spawnPattern = new SpawnPattern();
            spawnPattern.coolTime = float.Parse(line.Split(',')[0]);
            spawnPattern.type = line.Split(',')[1];
            spawnPattern.point = int.Parse(line.Split(',')[2]);
            spawnList.Add(spawnPattern);

        }

        // StringReader로 열어둔 파일은 작업이 끝난 후 꼭 닫기
        stringReader.Close();

        coolTime = spawnList[0].coolTime;
    }

    private void Update()
    {
        if (gameEnd) return;

        // string.Format() : 지정된 양식으로 문자열을 변환해주는 함수
        // {0:n0} : 세자리마다 쉼표로 나눠주는 숫자 양식
        scoreText.text = string.Format("{0:n0}", player.score);
        if (!player.gameObject.activeSelf && !isInvoke)
        {
            UpdateLifeIcon(player.maxHealth, player.health);
            isInvoke = true;
            Invoke("RespawnPlayer", 1);
        }
        UpdateBoomIcon(player.maxBoom, player.boom);

        if (boss != null && boss.activeSelf)
            return;

        curTime += Time.deltaTime;
        if (curTime >= coolTime && !spawnEnd)
        {
            SpawnPattern();
            curTime = 0;
        }
        curTime2 += Time.deltaTime;
        if (curTime2 >= realCoolTime)
        {
            SpawnEnemy();
            curTime2 = 0;
        }
        curTime3 += Time.deltaTime;
        if (curTime3 >= stageTime)
        {
            stage++;
            if (stage < maxStage - 1)
                ReadSpawnFile("Stage " + stage.ToString());
            else if(stage < maxStage)
            {
                curRepeat++;
                if (curRepeat < maxRepeat)
                    stage = -1;
                else
                    ReadSpawnFile("Stage " + stage.ToString());
            }

            curTime3 = 0;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            InvinsibleUI.SetActive(true);
            player.isInvinsible = !player.isInvinsible;
            InvinsibleUI.GetComponent<Text>().text = "Invinsible Mode " + (player.isInvinsible ? "ON" : "OFF");
            Invoke("InvinsibleUIOff", 1f);
        }
    }

    void InvinsibleUIOff()
    {
        InvinsibleUI.SetActive(false);
    }

    public void CallExplosion(Vector3 pos, string type)
    {
        if (gameEnd) return;

        GameObject explosion = objectManager.MakeObj("explosion");
        Explosion explosionLogic = explosion.GetComponent<Explosion>();

        explosion.transform.position = pos;
        explosionLogic.StartExplosion(type);

        if (type == "boss")
        {
            gameEnd = true;
            Invoke("StageClear", 1);
        }
    }

    private void SpawnPattern()
    {
        int enemyIndex = 0;
        switch (spawnList[spawnIndex].type)
        {
            case "A":
                enemyIndex = 0;
                break;
            case "B":
                enemyIndex = 1;
                break;
            case "C":
                enemyIndex = 2;
                break;
            case "boss":
                enemyIndex = 3;
                break;
        }
        int enemyPoint = spawnList[spawnIndex].point - 1;
        GameObject enemyObj = objectManager.MakeObj(enemies[enemyIndex]);
        if (spawnList[spawnIndex].type == "boss")
        {
            boss = enemyObj;
            enemyObj.transform.position = spawnPoints[enemyPoint].position + new Vector3(0.5f, 0, 0);
        }
        else
            enemyObj.transform.position = spawnPoints[enemyPoint].position;

        Rigidbody2D enemyRigid = enemyObj.GetComponent<Rigidbody2D>();
        Enemy enemy = enemyObj.GetComponent<Enemy>();
        enemy.player = player;
        enemy.objectManager = objectManager;
        enemy.gameManager = this;
        enemy.bulletDir = Spawn.Up;
        enemyRigid.AddForce(Vector2.down * enemy.speed, ForceMode2D.Impulse);
        enemyObj.transform.Rotate(Vector3.forward, 180);

        if (enemyPoint == 7 || enemyPoint == 9)
        {
            enemy.bulletDir = Spawn.Left;
            enemyObj.transform.Rotate(Vector3.forward, 270);
            enemyRigid.AddForce(new Vector2(enemy.speed, -1), ForceMode2D.Impulse);
        }
        else if(enemyPoint == 8 || enemyPoint == 10)
        {
            enemy.bulletDir = Spawn.Right;
            enemyObj.transform.Rotate(Vector3.forward, 90);
            enemyRigid.AddForce(new Vector2(-enemy.speed, -1), ForceMode2D.Impulse);
        }

        spawnIndex++;
        if (spawnIndex >= spawnList.Count)
        {
            spawnEnd = true;
            return;
        }

        coolTime = spawnList[spawnIndex].coolTime;
    }

    private void SpawnEnemy()
    {
        int spawnIndex = Random.Range(0, 2);
        switch (spawnIndex)
        {
            case 0:
                float xRange = Random.Range(-player.xRange + 0.5f, player.xRange - 0.5f);
                Vector3 spawnPos = new Vector3(xRange, 8, 0);
                int enemyIndex = Random.Range(0, enemies.Length - 1);
                GameObject enemyObj = objectManager.MakeObj(enemies[enemyIndex]);
                enemyObj.transform.position = spawnPos;
                enemyObj.transform.Rotate(Vector3.forward, 180);

                Rigidbody2D enemyRigid = enemyObj.GetComponent<Rigidbody2D>();
                Enemy enemy = enemyObj.GetComponent<Enemy>();
                enemy.player = player;
                enemy.objectManager = objectManager;
                enemy.gameManager = this;
                enemy.bulletDir = Spawn.Up;
                enemyRigid.AddForce(Vector2.down * enemy.speed, ForceMode2D.Impulse);
                break;
            case 1:
                float yRange = Random.Range(-2f, player.yRange);
                float ranPosX = Random.Range(0, 2) == 0 ? player.xRange + 2 : -player.xRange - 2;
                spawnPos = new Vector3(ranPosX, yRange, 0);
                enemyIndex = Random.Range(0, enemies.Length - 1);
                enemyObj = objectManager.MakeObj(enemies[enemyIndex]);
                enemyObj.transform.position = spawnPos;
                enemyObj.transform.Rotate(Vector3.forward, 180);

                enemyRigid = enemyObj.GetComponent<Rigidbody2D>();
                enemy = enemyObj.GetComponent<Enemy>();
                enemy.player = player;
                enemy.objectManager = objectManager;
                enemy.gameManager = this;
                if (ranPosX < 0)
                {
                    enemy.bulletDir = Spawn.Left;
                    enemyObj.transform.Rotate(Vector3.forward, 90);
                    enemyRigid.AddForce(new Vector2(enemy.speed, -1), ForceMode2D.Impulse);
                }
                else
                {
                    enemy.bulletDir = Spawn.Right;
                    enemyObj.transform.Rotate(Vector3.forward, 270);
                    enemyRigid.AddForce(new Vector2(-enemy.speed, -1), ForceMode2D.Impulse);
                }
                break;
        }
    }

    private void RespawnPlayer()
    {
        if (gameEnd) return;

        if (player.health <= 0)
            return;

        isInvoke = false;
        player.gameObject.SetActive(true);
        player.transform.position = Vector2.down * 4;

        player.Respawn();
    }

    private void UpdateLifeIcon(int maxHealth, int health)
    {
        // All hide
        for (int i = 0; i < maxHealth; i++)
        {
            lifeIcons[i].color = new Color(1, 1, 1, 0);
        }

        // Show
        for (int i = 0; i < health; i++)
        {
            lifeIcons[i].color = new Color(1, 1, 1, 1);
        }

        if (health <= 0)
            GameOver();
    }

    private void UpdateBoomIcon(int maxBoom, int boom)
    {
        // All hide
        for (int i = 0; i < maxBoom; i++)
        {
            boomIcons[i].color = new Color(1, 1, 1, 0);
        }

        // Show
        for (int i = 0; i < boom; i++)
        {
            boomIcons[i].color = new Color(1, 1, 1, 1);
        }
    }

    public void GameOver()
    {
        if (gameOverUI.activeSelf)
            return;

        AudioManager.instance.PlaySfx(AudioManager.Sfx.GameOver);
        AudioManager.instance.PlayBgm(false);

        gameEnd = true;
        fade.SetActive(true);
        fade.GetComponent<Animator>().SetTrigger("FadeIn");
        gameOverUI.SetActive(true);
        gameOverScoreBoard.text = "Your score\n" + string.Format("{0:n0}", player.score);

        Invoke("DisableAudio", 1);
    }

    private void DisableAudio()
    {
        AudioManager.instance.gameObject.SetActive(false);
    }

    public void Retry()
    {
        SceneManager.LoadScene("MainScene");
    }
}
