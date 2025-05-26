/*
    TestCamera.cs
    
    Last edited by:
    Luke Cullen

    5/26/25
*/
using UnityEngine;
using UnityEngine.InputSystem;

//Basic camera movement for testing
public class TestCamera : MonoBehaviour
{
    public int speed = 1; //speed to move the camera per press
    public void OnMove(InputValue dir)
    {
        //dir contains +/- values for each x and y direction
        gameObject.transform.position = gameObject.transform.position + new Vector3(dir.Get<Vector2>().x * speed, 0, dir.Get<Vector2>().y * speed);
    }
}