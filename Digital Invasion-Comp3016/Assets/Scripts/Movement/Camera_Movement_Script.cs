using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Movement_Script : MonoBehaviour
{
    public int speed;

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
    }
}
