using UnityEngine;

public class ScLine : MonoBehaviour {
    [SerializeField] Sprite _static;
    [SerializeField] Sprite _placed;

    private SpriteRenderer _spriteRenderer;
    
    public void OnInstantiate() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
        
    public void SetTransparancy(bool isTransparent) {
        Color color = _spriteRenderer.color;
        if (isTransparent) {
            color.a = 0.4f;
            _spriteRenderer.color = color;
        }
        else {
            color.a = 1f;
            _spriteRenderer.color = color;
        }
    }

    public void SetLineType(SLOOMSTATE lineState) {
        switch (lineState) {
            case SLOOMSTATE.Moving:
            case SLOOMSTATE.Placed:
                _spriteRenderer.sprite = _placed;
                break;
            case SLOOMSTATE.Static:
                _spriteRenderer.sprite = _static;
                break;
        }
    }
}
