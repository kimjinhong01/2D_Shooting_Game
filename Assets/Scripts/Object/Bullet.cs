using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int dmg;
    public bool isRotate;

    private void Update()
    {
        if (isRotate)
            transform.Rotate(Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 가상 벽에 부딪히면 총알 제거
        if (collision.CompareTag("Range"))
        {
            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
        }
    }
}
