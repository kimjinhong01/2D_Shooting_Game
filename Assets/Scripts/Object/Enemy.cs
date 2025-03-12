using UnityEngine;

public enum Spawn
{
    Up,
    Left,
    Right
}

public class Enemy : MonoBehaviour
{
    public bool isBoss;             // 보스인지
    public string enemyName;        // 몬스터 이름

    public float speed;             // 이동 속도
    public int maxHealth;           // 최대 체력
    public int health;              // 현재 체력
    public Sprite[] sprites;        // 스프라이트

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigid;

    public string bulletObj;        // 총알 이름
    public GameObject[] itemObj;    // 드롭 아이템
    public float bulletSpeed;       // 총알 속도
    public float coolTime;
    private float curTime;

    public Spawn bulletSpawn;       // 총알 생성되는 방향
    public Player player;
    public int enemyScore;

    public GameManager gameManager;
    public ObjectManager objectManager;
    private Animator animator;

    public int patternIndex;
    public int curPatternCount;
    public int[] maxPatternCount;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        if (isBoss)
            animator = GetComponent<Animator>();
    }

    // 활성화되면 체력 세팅
    private void OnEnable()
    {
        health = maxHealth;
        if (isBoss)
            Invoke("Stop", 3.5f);
    }

    private void Update()
    {
        if (!isBoss)
        {
            curTime += Time.deltaTime;
            Fire();
        }
    }

    private void OnDisable()
    {
        CancelInvoke("Think");
        CancelInvoke("FireFoward");
        CancelInvoke("FireShot");
        CancelInvoke("FireArc");
        CancelInvoke("FireAround");
    }

    private void Stop()
    {
        rigid.velocity = Vector3.zero;
        Invoke("Think", 2);
    }

    // 보스 공격 패턴 결정
    private void Think()
    {
        patternIndex = patternIndex == 3 ? 0 : patternIndex + 1;
        curPatternCount = 0;

        switch (patternIndex)
        {
            case 0:
                FireFoward();
                break;
            case 1:
                FireShot();
                break;
            case 2:
                FireArc();
                break;
            case 3:
                FireAround();
                break;
        }
    }

    // 앞으로 4발을 발사
    private void FireFoward()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Fire);

        // 생성 위치
        Vector3 pos = transform.position + new Vector3(0.83f, -1.1f, 0);
        Vector3 pos1 = transform.position + new Vector3(0.625f, -1.1f, 0);
        Vector3 pos2 = transform.position + new Vector3(-0.83f, -1.1f, 0);
        Vector3 pos3 = transform.position + new Vector3(-0.625f, -1.1f, 0);

        // 오브젝트 가져오기
        GameObject bullet = objectManager.MakeObj(bulletObj);
        GameObject bullet1 = objectManager.MakeObj(bulletObj);
        GameObject bullet2 = objectManager.MakeObj(bulletObj);
        GameObject bullet3 = objectManager.MakeObj(bulletObj);

        // 위치 설정
        bullet.transform.position = pos;
        bullet1.transform.position = pos1;
        bullet2.transform.position = pos2;
        bullet3.transform.position = pos3;

        // 컴포넌트 가져오기
        Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();
        Rigidbody2D bulletRigid1 = bullet1.GetComponent<Rigidbody2D>();
        Rigidbody2D bulletRigid2 = bullet2.GetComponent<Rigidbody2D>();
        Rigidbody2D bulletRigid3 = bullet3.GetComponent<Rigidbody2D>();

        // 총알 발사
        bulletRigid.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        bulletRigid1.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        bulletRigid2.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        bulletRigid3.AddForce(Vector2.down * 8, ForceMode2D.Impulse);

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireFoward", 2);
        else
            Invoke("Think", 3);
    }
    
    // 5발을 산탄으로 발사
    private void FireShot()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Fire);

        // 플레이어 기준으로 산탄총 발사
        int a = -2;
        for(int i = 0; i < 5; i++)
        {
            Vector3 pos = transform.position + new Vector3(0, -1.1f, 0);
            GameObject bullet = objectManager.MakeObj(bulletObj);
            bullet.transform.position = pos;
            Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 bulletDir = player.transform.position - transform.position;
            bulletDir += new Vector2(a, 0); a++;
            bulletRigid.AddForce(bulletDir.normalized * 3, ForceMode2D.Impulse);
        }

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireShot", 3.5f);
        else
            Invoke("Think", 3);
    }

    // 각도에 맞춰 순차적으로 발사
    private void FireArc()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Fire);

        Vector3 pos = transform.position + new Vector3(0, -1.1f, 0);
        GameObject bullet = objectManager.MakeObj(bulletObj);
        bullet.transform.position = pos;
        bullet.transform.rotation = Quaternion.identity;

        // Mathf.Cos함수로 180도 각도로 순차적으로 발사
        Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();
        Vector2 bulletDir = new Vector2(Mathf.Cos(Mathf.PI * 10 * curPatternCount / maxPatternCount[patternIndex]), -1);
        bulletRigid.AddForce(bulletDir.normalized * 5, ForceMode2D.Impulse);

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireArc", 0.15f);
        else
            Invoke("Think", 3);
    }
    
    // 동시에 사방으로 발사
    private void FireAround()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Fire);

        int roundNumA = 40;
        int roundNumB = 30;
        int roundNum = curPatternCount % 2 == 0 ? roundNumA : roundNumB;

        for (int i = 0; i < roundNum; i++)
        {
            GameObject bullet = objectManager.MakeObj(bulletObj);
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.identity;

            // 360도 사방으로 총알 발사
            Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 bulletDir = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / roundNum),
                                            Mathf.Sin(Mathf.PI * 2 * i / roundNum));
            bulletRigid.AddForce(bulletDir.normalized * 2, ForceMode2D.Impulse);

            Vector3 rot = (Vector3.forward * 360 * i / roundNum) + (Vector3.forward * 90);
            bullet.transform.Rotate(rot);
        }

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireAround", 1.2f);
        else
            Invoke("Think", 3);
    }

    // 데미지 입었을 때
    public void OnHit(int dmg)
    {
        if (health <= 0)
            return;

        if (health > 0)
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit, 1);

        health -= dmg;
        if (isBoss)
        {
            animator.SetTrigger("onHit");
        }
        else
        {
            spriteRenderer.sprite = sprites[1];
            Invoke("ReturnSprite", 0.1f); 
        }

        if (health <= 0)
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Die, 1);

            // 죽으면 아이템 드랍
            ItemDrop();
            player.score += enemyScore;
            gameObject.SetActive(false);
            gameManager.CallExplosion(transform.position, enemyName);
            transform.rotation = Quaternion.identity; // 기본 회전값 0
        }
    }

    private void ReturnSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }

    private void ItemDrop()
    {
        if (isBoss)
            return;

        // 확률에 따라 아이템 드랍
        GameObject item;
        int ranIndex = Random.Range(0, 10);
        if (ranIndex < 3) // 30%
            Debug.Log("None");
        else if (ranIndex < 6) // 30%
        {
            item = objectManager.MakeObj("coinItem");
            item.transform.position = transform.position;
        }
        else if (ranIndex < 8) // 20%
        {
            item = objectManager.MakeObj("powerItem");
            item.transform.position = transform.position;
        }
        else if (ranIndex < 10) // 20%
        {
            item = objectManager.MakeObj("boomItem");
            item.transform.position = transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 범위 밖으로 나가면 비활성화
        if (collision.CompareTag("Range"))
        {
            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
        }
        else if (collision.CompareTag("PlayerBullet"))
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            OnHit(bullet.dmg);

            collision.gameObject.SetActive(false);
            collision.transform.rotation = Quaternion.identity;
        }
    }

    // 일반 몬스터 공격
    public void Fire()
    {
        if (curTime < coolTime)
            return;

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Fire);

        switch (bulletSpawn)
        {
            // 위에서 나오는 경우
            case Spawn.Up:
                Vector3 pos = transform.position + new Vector3(0, -0.5f, 0);
                GameObject bullet = objectManager.MakeObj(bulletObj);
                bullet.transform.position = pos;
                bullet.transform.rotation = Quaternion.identity;
                Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();
                bulletRigid.AddForce(Vector2.down * bulletSpeed, ForceMode2D.Impulse);
                bullet.transform.Rotate(Vector3.forward, 180);
                break;
            // 왼쪽에서 나오는 경우
            case Spawn.Left:
                pos = transform.position + new Vector3(0.5f, 0, 0);
                bullet = objectManager.MakeObj(bulletObj);
                bullet.transform.position = pos;
                bullet.transform.rotation = Quaternion.identity;
                bulletRigid = bullet.GetComponent<Rigidbody2D>();
                bulletRigid.AddForce(Vector2.right * bulletSpeed, ForceMode2D.Impulse);
                bullet.transform.Rotate(Vector3.forward, 90);
                break;
            // 오른쪽에서 나오는 경우
            case Spawn.Right:
                pos = transform.position + new Vector3(-0.5f, 0, 0);
                bullet = objectManager.MakeObj(bulletObj);
                bullet.transform.position = pos;
                bullet.transform.rotation = Quaternion.identity;
                bulletRigid = bullet.GetComponent<Rigidbody2D>();
                bulletRigid.AddForce(Vector2.left * bulletSpeed, ForceMode2D.Impulse);
                bullet.transform.Rotate(Vector3.back, 90);
                break;
        }

        curTime = 0f;
    }
}
