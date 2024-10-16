using UnityEngine;
using UnityEngine.InputSystem;

public class ScDragDrop : MonoBehaviour {
    public static ScDragDrop Instance;

    public TOUCHSTATE TouchState = TOUCHSTATE.None;

    public GameObject SelectedSloom;
    [SerializeField] LayerMask _groundLayer;

    Vector2 _touchPos;
    Vector2 _oldTouchPos;
    Camera _mainCamera;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        _mainCamera = Camera.main;
    }

    Vector2 WorldPos(Vector2 postion) {
        return _mainCamera.ScreenToWorldPoint(postion);
    }
    public void Touch(InputAction.CallbackContext ctx) {
        if (ctx.performed){
            if(TouchState != TOUCHSTATE.Drag) { 
                TouchState = TOUCHSTATE.Press; 
            }

            GrabSloom();
        }
        if (ctx.canceled) {
            TouchState = TOUCHSTATE.None;
            if(SelectedSloom && SelectedSloom.TryGetComponent(out Rigidbody2D sloomBody)) {
                sloomBody.simulated = true;
                if(SelectedSloom.TryGetComponent(out ScSloom sloomComponent)){
                    sloomComponent.Drop();
                }
            }
            SelectedSloom = null;
        }
    }

    public void Move(InputAction.CallbackContext ctx) {
        _touchPos = ctx.ReadValue<Vector2>();
        switch (TouchState) {
            case TOUCHSTATE.Press :
                GrabSloom();
            break;
            case TOUCHSTATE.Drag :
                DragSloom();
                break;
            default:
            return;
        }
    }


    void GrabSloom() {
        RaycastHit2D hit = Physics2D.Raycast(WorldPos(_touchPos), Vector2.zero);
        if (!hit.collider){
            return;
        }
        if (hit.collider.gameObject.TryGetComponent(out ScSloom sloomComponent) && sloomComponent.SloomState == SLOOMSTATE.Movable){
            TouchState = TOUCHSTATE.Drag;
            SelectedSloom = hit.collider.gameObject;
        }
    }

    public void DragSloom() {
        float rayRayon = 0.37f;
        SelectedSloom.GetComponent<Rigidbody2D>().simulated = false;
        Vector2 targetPos = WorldPos(_touchPos);
        Vector2 sloomPos = SelectedSloom.transform.position;

        if (Physics2D.OverlapCircle(targetPos, rayRayon, _groundLayer)){
            RaycastHit2D hitY = Physics2D.Raycast(new Vector2(targetPos.x, targetPos.y + rayRayon), Vector2.down, Mathf.Infinity, _groundLayer);
            if (hitY.collider != null) {
                SelectedSloom.transform.position = Vector2.MoveTowards(SelectedSloom.transform.position, new Vector2(targetPos.x, hitY.point.y+ rayRayon), 15f);
            }
        }
        else {
            SelectedSloom.transform.position = Vector2.MoveTowards(SelectedSloom.transform.position, targetPos, 15f);
        }
    }
}
