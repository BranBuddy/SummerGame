using UnityEngine;
using UnityEngine.InputSystem;

public class TestCamera : MonoBehaviour
{
    public void OnMove(InputValue dir)
    {
        gameObject.transform.position = gameObject.transform.position + new Vector3(dir.Get<Vector2>().x, 0, dir.Get<Vector2>().y);
    }
}