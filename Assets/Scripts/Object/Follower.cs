using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    private SpriteRenderer sprite;
    public int index;
    public int power;
    public float coolTime;
    public float curTime;
    public ObjectManager objectManager;

    public Vector3 followPos;           // 따라갈 위치
    public int followDelay;
    public Transform parent;
    public Queue<Vector3> parentPos;

    public GameObject player;
    public bool isRespawn;
    public bool isActive;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.color = new Color(1, 1, 1, 0);
        parentPos = new Queue<Vector3>();
    }

    private void FixedUpdate()
    {
        Watch();
        Follow();
        if (isActive)
        {
            if (!isRespawn)
            {
                Fire();
                Reload();
            }
            Respawn();
        }
    }

    // 활성화 되었을 때
    public void OnActive()
    {
        isActive = true;
        sprite.color = new Color(1, 1, 1, 0.3f);
        Invoke("Show", 1);
    }

    // 리스폰 되었을 때
    void Respawn()
    {
        if (!player.activeSelf)
        {
            if (index == 0 || index == 1)
                parent.position = Vector2.down * 4;
            sprite.color = new Color(1, 1, 1, 0);
            isRespawn = true;
        }
        else if (isRespawn)
        {
            isRespawn = false;
            sprite.color = new Color(1, 1, 1, 0.3f);
            Invoke("Show", 1);
        }
    }

    private void Show()
    {
        sprite.color = new Color(1, 1, 1, 1);
    }

    // Queue를 활용하여 따라가는 위치를 설정
    private void Watch()
    {
        // Queue : FIFO (First Input First Out)
        // Enqueue() : 큐에 데이터 저장하는 함수
        parentPos.Enqueue(parent.position);

        // 큐에 일정 데이터 개수가 채워지면 그때부터 반환
        if (parentPos.Count > followDelay)
            // Dequeue() : 큐의 첫 데이터를 빼면서 반환하는 함수
            followPos = parentPos.Dequeue();
        else if(parentPos.Count < followDelay)
            followPos = parent.position;
    }

    // 따라갈 위치로 계속 따라가기
    private void Follow()
    {
        Vector3 vec = Vector3.zero;
        switch(index)
        {
            case 0:
                vec = new Vector3(0.85f, -0.15f, 0);
                break;
            case 1:
                vec = new Vector3(-0.85f, -0.15f, 0);
                break;
            case 2:
                vec = new Vector3(0.15f, -0.25f, 0);
                break;
            case 3:
                vec = new Vector3(-0.15f, -0.25f, 0);
                break;
        }
        transform.position = followPos + vec;
    }

    // 총알 발사
    public void Fire()
    {
        if (!Input.GetButton("Fire1"))
            return;

        if (curTime < coolTime)
            return;

        // 파워에 따라 다른 총알
        switch (power)
        {
            case 0:
                Vector3 pos1 = transform.position + new Vector3(0, 0.5f, 0);
                GameObject bullet1 = objectManager.MakeObj("followerBullet");
                bullet1.transform.position = pos1;

                Rigidbody2D bulletRigid1 = bullet1.GetComponent<Rigidbody2D>();
                bulletRigid1.AddForce(Vector2.up * 7, ForceMode2D.Impulse);
                break;
            case 1:
                pos1 = transform.position + new Vector3(-0.1f, 0.5f, 0);
                bullet1 = objectManager.MakeObj("followerBullet");
                bullet1.transform.position = pos1;
                Vector3 pos2 = transform.position + new Vector3(0.1f, 0.5f, 0);
                GameObject bullet2 = objectManager.MakeObj("followerBullet");
                bullet2.transform.position = pos2;

                bulletRigid1 = bullet1.GetComponent<Rigidbody2D>();
                bulletRigid1.AddForce(Vector2.up * 7, ForceMode2D.Impulse);
                Rigidbody2D bulletRigid2 = bullet2.GetComponent<Rigidbody2D>();
                bulletRigid2.AddForce(Vector2.up * 7, ForceMode2D.Impulse);
                break;
            case 2:
                pos1 = transform.position + new Vector3(-0.2f, 0.5f, 0);
                bullet1 = objectManager.MakeObj("followerBullet");
                bullet1.transform.position = pos1;
                pos2 = transform.position + new Vector3(0, 0.5f, 0);
                bullet2 = objectManager.MakeObj("followerBullet");
                bullet2.transform.position = pos2;
                Vector3 pos3 = transform.position + new Vector3(0.2f, 0.5f, 0);
                GameObject bullet3 = objectManager.MakeObj("followerBullet");
                bullet3.transform.position = pos3;

                bulletRigid1 = bullet1.GetComponent<Rigidbody2D>();
                bulletRigid1.AddForce(Vector2.up * 7, ForceMode2D.Impulse);
                bulletRigid2 = bullet2.GetComponent<Rigidbody2D>();
                bulletRigid2.AddForce(Vector2.up * 7, ForceMode2D.Impulse);
                Rigidbody2D bulletRigid3 = bullet3.GetComponent<Rigidbody2D>();
                bulletRigid3.AddForce(Vector2.up * 7, ForceMode2D.Impulse);
                break;
        }

        curTime = 0;
    }

    private void Reload()
    {
        curTime += Time.deltaTime;
    }
}
