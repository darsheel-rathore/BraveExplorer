using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float horizontalInput;
    public float verticalInput;

    public bool mouseBtnDown;
    public bool spaceKeyDown;

    // Update is called once per frame
    void Update()
    {
        if (!mouseBtnDown && Time.timeScale != 0.0f)
            mouseBtnDown = Input.GetMouseButtonDown(0);

        if (!spaceKeyDown && Time.timeScale != 0)
            spaceKeyDown = Input.GetKeyDown(KeyCode.Space);

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void OnDisable()
    {
        ClearCache();
    }

    public void ClearCache()
    {
        mouseBtnDown = false;
        spaceKeyDown = false;
        horizontalInput = 0;
        verticalInput = 0;
    }
}
