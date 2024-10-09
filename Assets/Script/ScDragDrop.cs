using UnityEngine;
using UnityEngine.InputSystem;

public class ScDragDrop : MonoBehaviour {
    public enum State { Drag, Drop, Press, None}
    public State TouchState = State.None;

    [SerializeField] GameObject _selectedSloom;

    Vector2 _touchPos;
    Vector2 _oldTouchPos;
    Camera _mainCamera;

    private void Start() {
        _mainCamera = Camera.main;
    }

    Vector2 WorldPos(Vector2 postion) {
        return _mainCamera.ScreenToWorldPoint(postion);
    }
    public void Touch(InputAction.CallbackContext ctx) {
        if (ctx.started) {
            
        }
        if (ctx.performed){
            TouchState = State.Press;
            GrabSloom();
        }
        if (ctx.canceled) {
            TouchState = State.None;
            _selectedSloom = null;
        }
    }

    public void Move(InputAction.CallbackContext ctx) {
        _touchPos = ctx.ReadValue<Vector2>();
        switch (TouchState) {
            case State.Press :
                GrabSloom();
            break;
        }
    }


    void GrabSloom() {
        RaycastHit2D hit = Physics2D.Raycast(WorldPos(_touchPos), Vector2.zero);
        if (!hit.collider){
            return;
        }
        if (hit.collider.gameObject.TryGetComponent(out ScSloom sloomComponent) && sloomComponent.IsDraggable){
            TouchState = State.Drag;
            _selectedSloom = hit.collider.gameObject;
        }
    }
}
