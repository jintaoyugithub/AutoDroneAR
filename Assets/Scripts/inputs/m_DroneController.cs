using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.InputSystem;

public class m_DroneController : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerInput m_playerInput;
    void Start()
    {
        m_playerInput = GetComponent<PlayerInput>();
        m_playerInput.onActionTriggered += callBack =>
        {
            if (callBack.action.name == "Move")
            {
                Move(callBack.ReadValue<Vector2>());
            }
            else if (callBack.action.name == "Look")
            {
                Look(callBack.ReadValue<Vector2>());
            }

        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Move(Vector2 direction)
    {

    }
    private void Look(Vector2 direction)
    {

    }
}
