using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScDragDrop : MonoBehaviour {
    public enum State { Drag, Drop, Press, None}
    public State TouchState = State.None;

    [SerializeField] GameObject _selectedSloom;
    [SerializeField] LayerMask _groundLayer;

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
            if(_selectedSloom && _selectedSloom.TryGetComponent(out Rigidbody2D sloomBody)) {
                sloomBody.simulated = true;
                if(_selectedSloom.TryGetComponent(out ScSloom sloomComponent)){
                    sloomComponent.Drop();
                }
            }
            _selectedSloom = null;
        }
    }

    public void Move(InputAction.CallbackContext ctx) {
        _touchPos = ctx.ReadValue<Vector2>();
        switch (TouchState) {
            case State.Press :
                GrabSloom();
            break;
            case State.Drag :
                DragSloom();
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

    void DragSloom() {
        float rayRayon = 0.37f;
        _selectedSloom.GetComponent<Rigidbody2D>().simulated = false;
        Vector2 targetPos = WorldPos(_touchPos);
        Vector2 sloomPos = _selectedSloom.transform.position;

        if (Physics2D.OverlapCircle(targetPos, rayRayon, _groundLayer)){
            RaycastHit2D hitY = Physics2D.Raycast(new Vector2(targetPos.x, targetPos.y + 3f), Vector2.down, Mathf.Infinity, _groundLayer);
            if (hitY.collider != null) {
                _selectedSloom.transform.position = Vector2.MoveTowards(_selectedSloom.transform.position, new Vector2(targetPos.x, hitY.point.y+ rayRayon), 15f);
            }
        }
        else {
            _selectedSloom.transform.position = Vector2.MoveTowards(_selectedSloom.transform.position, targetPos, 15f);
        }
    }
}
