using UnityEngine;

public class Background : MonoBehaviour
{
    public float speed;
    private float viewHeight;

    private void Awake()
    {
        viewHeight = 12.75f; //Camera.main.orthographicSize * 2;
    }

    void Update()
    {
        // 아래로 계속 이동
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        // viewHeight에 따라 위치 재설정
        if(transform.position.y <= -viewHeight)
            transform.position = new Vector2(0, viewHeight);
    }
}
