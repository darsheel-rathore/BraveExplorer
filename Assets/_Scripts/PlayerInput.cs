using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float horizontalInput;
    public float verticalInput;

    public bool mouseBtnDown;

    // Update is called once per frame
    void Update()
    {
        if (!mouseBtnDown && Time.timeScale != 0.0f)
            mouseBtnDown = Input.GetMouseButtonDown(0);

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void OnDisable()
    {
        mouseBtnDown = false;
        horizontalInput = 0;
        verticalInput = 0;
    }
}
