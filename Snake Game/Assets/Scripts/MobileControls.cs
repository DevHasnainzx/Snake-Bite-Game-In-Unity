using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MobileControls : MonoBehaviour
{
    [Header("Button References")]
    public Button upButton;
    public Button downButton;
    public Button leftButton;
    public Button rightButton;
    [Space]
    public SnakeMovement snakeController;

    [Header("Visual Feedback")]
    public Color pressedColor = Color.gray;
    private Color normalColor;

    void Start()
    {
        // Store original button colors
        normalColor = upButton.image.color;

        // Setup button events
        SetupButton(upButton, Vector2.up);
        SetupButton(downButton, Vector2.down);
        SetupButton(leftButton, Vector2.left);
        SetupButton(rightButton, Vector2.right);
    }

    void SetupButton(Button button, Vector2 direction)
    {
        // Add event triggers for better mobile response
        EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();

        // Pointer down event
        var pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((e) => {
            snakeController.ChangeDirection(direction);
            button.image.color = pressedColor;
        });
        trigger.triggers.Add(pointerDown);

        // Pointer up event (visual feedback only)
        var pointerUp = new EventTrigger.Entry();
        pointerUp.eventID = EventTriggerType.PointerUp;
        pointerUp.callback.AddListener((e) => {
            button.image.color = normalColor;
        });
        trigger.triggers.Add(pointerUp);

        // Pointer exit event (if finger slides off button)
        var pointerExit = new EventTrigger.Entry();
        pointerExit.eventID = EventTriggerType.PointerExit;
        pointerExit.callback.AddListener((e) => {
            button.image.color = normalColor;
        });
        trigger.triggers.Add(pointerExit);
    }
}