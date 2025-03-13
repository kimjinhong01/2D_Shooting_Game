using UnityEngine;

public class Item : MonoBehaviour
{
    public string type;
    private Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        // 계속 아래로 이동
        rigid.AddForce(Vector2.down, ForceMode2D.Impulse);
    }
}
