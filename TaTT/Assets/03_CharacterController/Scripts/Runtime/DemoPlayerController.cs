using UnityEngine;
using UnityEngine.InputSystem;

public class DemoPlayerController : MonoBehaviour
{
    public float speed = 5f;

    private Vector2 movementInput;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(movementInput.x, 0, movementInput.y) * speed * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext ctx) => movementInput = ctx.ReadValue<Vector2>();
}