using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // Configuration Parameters
    [SerializeField] float movementSpeed = 10;

    bool canMove = true;

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            PlayerMovement();
        }
    }

    void PlayerMovement()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * movementSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed;

        var newXPos = transform.position.x + deltaX;
        var newYPos = transform.position.y + deltaY;
        transform.position = new Vector2(newXPos, newYPos);
    }

    public bool canMoveNow()
    {
        return canMove = true;
    }

    public bool playerNoMove()
    {
        return canMove = false;
    }
}
