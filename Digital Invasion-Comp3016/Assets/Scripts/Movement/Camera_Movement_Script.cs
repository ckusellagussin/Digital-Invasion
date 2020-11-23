using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Movement_Script : MonoBehaviour
{
    public int speed;
    public Camera camera;

    private bool waiting;
    private int direction; //0 = left 1 = right
    private float scroll;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (Input.GetKey(KeyCode.A))
                {
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward + -transform.right, speed * 2 * Time.deltaTime);
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward + transform.right, speed * 2 * Time.deltaTime);
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, speed * 2 * Time.deltaTime);
                }
            }
            else if (Input.GetKey(KeyCode.A))
            {
                transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward + -transform.right, speed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward + transform.right, speed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, speed * Time.deltaTime);
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (Input.GetKey(KeyCode.A))
                {
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + -transform.forward + -transform.right, speed * 2 * Time.deltaTime);
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + -transform.forward + transform.right, speed * 2 * Time.deltaTime);
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + -transform.forward, speed * 2 * Time.deltaTime);
                }
            }
            else if (Input.GetKey(KeyCode.A))
            {
                transform.position = Vector3.MoveTowards(transform.position, transform.position + -transform.forward + -transform.right, speed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.position = Vector3.MoveTowards(transform.position, transform.position + -transform.forward + transform.right, speed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, transform.position + -transform.forward, speed * Time.deltaTime);
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                transform.position = Vector3.MoveTowards(transform.position, transform.position + -transform.right, speed * 2 * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, transform.position + -transform.right, speed * Time.deltaTime);
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.right, speed * 2 * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.right, speed * Time.deltaTime);
            }
        }
        if (Input.GetKey(KeyCode.Q) && !waiting)
        {
            waiting = true;
            direction = 0;
            Invoke("Rotate", 0.15f);
        }
        if (Input.GetKey(KeyCode.E) && !waiting)
        {
            waiting = true;
            direction = 1;
            Invoke("Rotate", 0.15f);
        }

        scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f)
        {
            if (camera.gameObject.transform.position.y > 5)
            {
                camera.gameObject.transform.position = Vector3.MoveTowards(camera.gameObject.transform.position, camera.gameObject.transform.position + camera.gameObject.transform.forward, speed * 2 * Time.deltaTime);
            }
        }
        if (scroll < 0f)
        {
            if (camera.gameObject.transform.position.y < 12)
            {
                camera.gameObject.transform.position = Vector3.MoveTowards(camera.gameObject.transform.position, camera.gameObject.transform.position + -camera.gameObject.transform.forward, speed * 2 * Time.deltaTime);
            }
        }
    }

    void Rotate()
    {
        if (direction == 0)
        {
            waiting = false;
            transform.Rotate(0, 10, 0);
        }
        if (direction == 1)
        {
            waiting = false;
            transform.Rotate(0, -10, 0);
        }
    }
}
