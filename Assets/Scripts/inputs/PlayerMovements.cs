using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovements : MonoBehaviour
{
    // read from the right stick
    Vector2 move;
    // read from the left stick
    Vector2 lookDir;
    
    public void OnMove(InputAction.CallbackContext context) {
        //Debug.Log("Read value from right stick");
        move = context.ReadValue<Vector2>();
    }
    
    public void OnTurn(InputAction.CallbackContext context) {
        //Debug.Log("Read value from left stick");
        lookDir = context.ReadValue<Vector2>();
    }

    public void Update() {
        //Debug.Log("move dir" + moveDir);
        //Debug.Log("look dir" + lookDir);
        // read the view dir
        Vector3 moveAmount = new Vector3(move.x, 0, move.y);

        transform.Translate(moveAmount * Time.deltaTime, Space.Self);

        transform.Rotate(0, 30 * lookDir.x * Time.deltaTime, 0);
        transform.Translate(new Vector3(0, lookDir.y, 0) * Time.deltaTime, Space.Self);
    }
}
