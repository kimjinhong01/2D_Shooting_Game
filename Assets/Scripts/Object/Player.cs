using UnityEngine;

public class Player : MonoBehaviour
{
    private float x;
    private float y;
    public float speed;             // 이동 속도
    private Rigidbody2D rigid;
    private Animator anim;
    public float xRange;            // 가로 제한
    public float yRange;            // 세로 제한

    public GameObject bulletA;
    public GameObject bulletB;
    public float bulletSpeed;
    public float coolTime;
    private float curTime;
    public int maxPower;            // 최대 파워
    public int power;               // 현재 파워
    public int maxHealth;           // 최대 체력
    public int health;              // 현재 체력
    public int score;               // 점수

    private SpriteRenderer sprite;
    private BoxCollider2D coll;
    private bool isHit;
    public GameObject boomEffect;

    public int maxBoom;
    public int boom;
    public bool isBoom;

    public GameManager gameManager;
    public ObjectManager objectManager;
    public GameObject[] followers;

    public bool[] joyControl;
    public bool isControl;
    public bool isButtonA;
    public bool isButtonB;

    public bool isInvinsible;

    // 조이스틱 UI를 9개로 나누어 조작 설정
    public void joyPanel(int type)
    {
        for (int i = 0; i < 9; i++)
        {
            joyControl[i] = i == type;
        }
    }

    public void joyDown()
    {
        isControl = true;
    }

    public void joyUp()
    {
        isControl = false;
    }

    public void ButtonADown()
    {
        isButtonA = true;
    }

    public void ButtonAUp()
    {
        isButtonA = false;
    }

    public void ButtonBDown()
    {
        isButtonB = true;
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        health = maxHealth;
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");

        // 조이스틱UI 범위을 9개로 나눠서 실제 조이스틱과 같은 조작
        if (!isControl) { }
        else if (joyControl[0]) { x = -1; y = 1; }
        else if (joyControl[1]) { x = 0; y = 1; }
        else if (joyControl[2]) { x = 1; y = 1; }
        else if (joyControl[3]) { x = -1; y = 0; }
        else if (joyControl[4]) { x = 0; y = 0; }
        else if (joyControl[5]) { x = 1; y = 0; }
        else if (joyControl[6]) { x = -1; y = -1; }
        else if (joyControl[7]) { x = 0; y = -1; }
        else if (joyControl[8]) { x = 1; y = -1; }

        anim.SetFloat("x_value", x);

        curTime += Time.deltaTime;

        Fire();
        Boom();
    }

    private void FixedUpdate()
    {
        BlockPlayer();

        rigid.AddForce(new Vector2(x, y) * speed);
    }

    // 플레이어가 범위 바깥으로 나가지 못하게 이동 제한
    private void BlockPlayer()
    {
        if (transform.position.x > xRange)
        {
            rigid.velocity = new Vector2(0.0f, rigid.velocity.y);
            transform.position = new Vector2(xRange, transform.position.y);
        }
        else if (transform.position.x < -xRange)
        {
            rigid.velocity = new Vector2(0.0f, rigid.velocity.y);
            transform.position = new Vector2(-xRange, transform.position.y);
        }
        if (transform.position.y > yRange)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, 0.0f);
            transform.position = new Vector2(transform.position.x, yRange);
        }
        else if (transform.position.y < -yRange)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, 0.0f);
            transform.position = new Vector2(transform.position.x, -yRange);
        }
    }

    public void Fire()
    {
        //if (!Input.GetButton("Fire1"))
        //    return;

        if (!isButtonA)
            return;

        if (curTime < coolTime)
            return;

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Fire);

        // 파워에 따라 다른 총알 발사
        switch (power)
        {
            case 0:
                Vector3 pos1 = transform.position + new Vector3(0, 0.5f, 0);
                Vector3 pos2, pos3;
                GameObject bullet1 = objectManager.MakeObj("playerBulletA");
                bullet1.transform.position = pos1;
                GameObject bullet2, bullet3;
                Rigidbody2D bulletRigid1 = bullet1.GetComponent<Rigidbody2D>();
                Rigidbody2D bulletRigid2, bulletRigid3;
                bulletRigid1.AddForce(Vector2.up * bulletSpeed, ForceMode2D.Impulse);
                break;
            case 1:
                pos1 = transform.position + new Vector3(-0.1f, 0.5f, 0);
                pos2 = transform.position + new Vector3(0.1f, 0.5f, 0);

                bullet1 = objectManager.MakeObj("playerBulletA");
                bullet1.transform.position = pos1;
                bullet2 = objectManager.MakeObj("playerBulletA");
                bullet2.transform.position = pos2;

                bulletRigid1 = bullet1.GetComponent<Rigidbody2D>();
                bulletRigid2 = bullet2.GetComponent<Rigidbody2D>();

                bulletRigid1.AddForce(Vector2.up * bulletSpeed, ForceMode2D.Impulse);
                bulletRigid2.AddForce(Vector2.up * bulletSpeed, ForceMode2D.Impulse);
                break;
            case 2:
                pos1 = transform.position + new Vector3(-0.2f, 0.5f, 0);
                pos2 = transform.position + new Vector3(0, 0.5f, 0);
                pos3 = transform.position + new Vector3(0.2f, 0.5f, 0);

                bullet1 = objectManager.MakeObj("playerBulletA");
                bullet1.transform.position = pos1;
                bullet2 = objectManager.MakeObj("playerBulletA");
                bullet2.transform.position = pos2;
                bullet3 = objectManager.MakeObj("playerBulletA");
                bullet3.transform.position = pos3;

                bulletRigid1 = bullet1.GetComponent<Rigidbody2D>();
                bulletRigid2 = bullet2.GetComponent<Rigidbody2D>();
                bulletRigid3 = bullet3.GetComponent<Rigidbody2D>();

                bulletRigid1.AddForce(Vector2.up * bulletSpeed, ForceMode2D.Impulse);
                bulletRigid2.AddForce(Vector2.up * bulletSpeed, ForceMode2D.Impulse);
                bulletRigid3.AddForce(Vector2.up * bulletSpeed, ForceMode2D.Impulse);
                break;
            case 3:
                pos1 = transform.position + new Vector3(-0.1f, 0.5f, 0);
                bullet1 = objectManager.MakeObj("playerBulletB");
                bullet1.transform.position = pos1;

                bulletRigid1 = bullet1.GetComponent<Rigidbody2D>();
                bulletRigid1.AddForce(Vector2.up * bulletSpeed, ForceMode2D.Impulse);
                break;
            default:
                pos1 = transform.position + new Vector3(-0.2f, 0.5f, 0);
                pos2 = transform.position + new Vector3(0, 0.5f, 0);
                pos3 = transform.position + new Vector3(0.2f, 0.5f, 0);

                bullet1 = objectManager.MakeObj("playerBulletA");
                bullet1.transform.position = pos1;
                bullet2 = objectManager.MakeObj("playerBulletB");
                bullet2.transform.position = pos2;
                bullet3 = objectManager.MakeObj("playerBulletA");
                bullet3.transform.position = pos3;

                bulletRigid1 = bullet1.GetComponent<Rigidbody2D>();
                bulletRigid2 = bullet2.GetComponent<Rigidbody2D>();
                bulletRigid3 = bullet3.GetComponent<Rigidbody2D>();

                bulletRigid1.AddForce(Vector2.up * bulletSpeed, ForceMode2D.Impulse);
                bulletRigid2.AddForce(Vector2.up * bulletSpeed, ForceMode2D.Impulse);
                bulletRigid3.AddForce(Vector2.up * bulletSpeed, ForceMode2D.Impulse);
                break;
        }

        curTime = 0f;
    }

    public void Boom()
    {
        //if (!Input.GetButton("Fire2") || isBoom || boom <= 0)
        //    return;

        if (!isButtonB || isBoom || boom <= 0)
            return;

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Boom, 1);

        isButtonB = false;
        isBoom = true;

        // Active
        boomEffect.SetActive(true);
        Invoke("OffBoomEffect", 2);

        // Enemy Boom
        GameObject[] enemyA = objectManager.GetPool("enemyA");
        GameObject[] enemyB = objectManager.GetPool("enemyB");
        GameObject[] enemyC = objectManager.GetPool("enemyC");
        for (int i = 0; i < enemyA.Length; i++)
        {
            if (enemyA[i].activeSelf)
            {
                Enemy enemy = enemyA[i].GetComponent<Enemy>();
                enemy.OnHit(100);
            }
        }
        for (int i = 0; i < enemyB.Length; i++)
        {
            if (enemyB[i].activeSelf)
            {
                Enemy enemy = enemyB[i].GetComponent<Enemy>();
                enemy.OnHit(100);
            }
        }
        for (int i = 0; i < enemyC.Length; i++)
        {
            if (enemyC[i].activeSelf)
            {
                Enemy enemy = enemyC[i].GetComponent<Enemy>();
                enemy.OnHit(100);
            }
        }

        // Bullet Boom
        GameObject[] enemyBulletA = objectManager.GetPool("enemyBulletA");
        GameObject[] enemyBulletB = objectManager.GetPool("enemyBulletB");
        GameObject[] enemyBulletC = objectManager.GetPool("enemyBulletC");
        GameObject[] enemyBulletD = objectManager.GetPool("enemyBulletD");
        for (int i = 0; i < enemyBulletA.Length; i++)
        {
            if (enemyBulletA[i].activeSelf)
            {
                enemyBulletA[i].SetActive(false);
                enemyBulletA[i].transform.rotation = Quaternion.identity;
            }
        }
        for (int i = 0; i < enemyBulletB.Length; i++)
        {
            if (enemyBulletB[i].activeSelf)
            {
                enemyBulletB[i].SetActive(false);
                enemyBulletB[i].transform.rotation = Quaternion.identity;
            }
        }
        for (int i = 0; i < enemyBulletC.Length; i++)
        {
            if (enemyBulletC[i].activeSelf)
            {
                enemyBulletC[i].SetActive(false);
                enemyBulletC[i].transform.rotation = Quaternion.identity;
            }
        }
        for (int i = 0; i < enemyBulletD.Length; i++)
        {
            if (enemyBulletD[i].activeSelf)
            {
                enemyBulletD[i].SetActive(false);
                enemyBulletD[i].transform.rotation = Quaternion.identity;
            }
        }

        boom--;
    }

    private void OffBoomEffect()
    {
        isBoom = false;

        boomEffect.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isHit)
            return;

        // EnemyBullet에 부딪히면 체력 감소
        if (collision.CompareTag("EnemyBullet"))
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Die, 1);

#if UNITY_ANDROID

Handheld.Vibrate();

#endif

            isHit = true;
            if (!isInvinsible)
                health--;
            gameObject.SetActive(false);
            gameManager.CallExplosion(transform.position, "player");
            collision.gameObject.SetActive(false);
            collision.transform.rotation = Quaternion.identity;
        }
        else if (collision.CompareTag("Enemy"))
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Die, 1);

#if UNITY_ANDROID

Handheld.Vibrate();

#endif

            isHit = true;
            if (!isInvinsible)
                health--;
            gameObject.SetActive(false);
            gameManager.CallExplosion(transform.position, "player");
            collision.gameObject.GetComponent<Enemy>().OnHit(100);
            collision.transform.rotation = Quaternion.identity;
            if (collision.gameObject.GetComponent<Enemy>().enemyName == "boss")
                collision.transform.Rotate(Vector3.forward * 180);
        }
        // 아이템 종류에 따라 효과 적용
        else if (collision.CompareTag("Item"))
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Item);

            Item item = collision.GetComponent<Item>();
            switch (item.type)
            {
                case "Boom":
                    if (boom >= maxBoom)
                        score += 50;
                    else
                        boom++;
                    break;
                case "Power":
                    if (power >= maxPower)
                        score += 50;
                    else
                    {
                        power++;
                        AddFollower();
                    }
                    break;
                case "Coin":
                    score += 100;
                    break;
            }
            collision.gameObject.SetActive(false);
            collision.transform.rotation = Quaternion.identity;
        }
    }

    // 보조 비행선 추가
    private void AddFollower()
    {
        if (power == 5)
            followers[0].GetComponent<Follower>().OnActive();
        else if (power == 6)
            followers[1].GetComponent<Follower>().OnActive();
        else if (power == 7)
            followers[2].GetComponent<Follower>().OnActive();
        else if (power == 8)
            followers[3].GetComponent<Follower>().OnActive();
        else if (power == 9)
            followers[0].GetComponent<Follower>().power++;
        else if (power == 10)
            followers[1].GetComponent<Follower>().power++;
        else if (power == 11)
            followers[2].GetComponent<Follower>().power++;
        else if (power == 12)
            followers[3].GetComponent<Follower>().power++;
        else if (power == 13)
            followers[0].GetComponent<Follower>().power++;
        else if (power == 14)
            followers[1].GetComponent<Follower>().power++;
        else if (power == 15)
            followers[2].GetComponent<Follower>().power++;
        else if (power == 16)
            followers[3].GetComponent<Follower>().power++;
    }

    public void Respawn()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Respawn);

        isHit = false;
        sprite.color = new Color(1, 1, 1, 0.3f);
        coll.enabled = false;
        Invoke("EnableCollider", 1);
    }

    private void EnableCollider()
    {
        sprite.color = new Color(1, 1, 1, 1);
        coll.enabled = true;
    }
}
