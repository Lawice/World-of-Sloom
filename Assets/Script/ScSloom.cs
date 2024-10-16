using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ScSloom : MonoBehaviour{
    public SLOOMSTATE SloomState = SLOOMSTATE.Movable;
    [SerializeField] float _detectionRadius;
    [SerializeField] LayerMask _SloomLayer;
    [SerializeField] float _SloomJointFrequency;

    [SerializeField] GameObject _linePrefab;
    public List<GameObject> NeighborSlooms = new List<GameObject>();
    public List<Line> Lines = new List<Line>();



    private void Update() {
        AddLines();
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }

    public void Drop() {
        foreach (Collider2D sloomCollider in  Physics2D.OverlapCircleAll(transform.position, _detectionRadius, _SloomLayer)) {
            GameObject newSloom = sloomCollider.gameObject;
            if (newSloom != gameObject && newSloom.TryGetComponent(out ScSloom newSloomComp) && (newSloomComp.SloomState == SLOOMSTATE.Static || newSloomComp.SloomState == SLOOMSTATE.Placed)) {
                NeighborSlooms.Add(sloomCollider.gameObject);
                newSloomComp.NeighborSlooms.Add(gameObject);

                AddJoints(gameObject, newSloom);
            }
        }
        
        if (NeighborSlooms.Count != 0) {
            SloomState = SLOOMSTATE.Placed;
        }
    }

    void AddJoints(GameObject gameObject1, GameObject gameObject2) {
        SpringJoint2D joint = gameObject1.AddComponent<SpringJoint2D>();
        joint.connectedBody = gameObject2.GetComponent<Rigidbody2D>();
        joint.autoConfigureDistance = false;
        joint.frequency = _SloomJointFrequency;

        SpringJoint2D newsSloomJoint = gameObject2.AddComponent<SpringJoint2D>();
        newsSloomJoint.connectedBody = gameObject1.GetComponent<Rigidbody2D>();
        newsSloomJoint.autoConfigureDistance = false;
        newsSloomJoint.frequency = _SloomJointFrequency;
    }

    void AddLines() {
        switch (SloomState) {
            case SLOOMSTATE.Static:
            case SLOOMSTATE.Placed:
                foreach (GameObject closeSloom in NeighborSlooms) {
                    ScSloom closeSloomComp = closeSloom.GetComponent<ScSloom>();
                    bool isLineDrawed = false;
                    foreach(Line closeSloomLine in closeSloomComp.Lines) {
                        if(closeSloomLine.destination == transform) {
                            isLineDrawed =true; break;
                        }
                    }

                    if (!isLineDrawed) {
                        AddLine(gameObject, closeSloom);
                    }
                }
                foreach (Line oldLine in Lines.ToList()) {
                    bool isLinked = false;
                    foreach (GameObject closeSloom in NeighborSlooms) {
                        if (closeSloom.transform == oldLine.destination) {
                            isLinked = true;
                            break;
                        }
                    }

                    if (!isLinked) {
                        Destroy(oldLine.line);
                        Lines.Remove(oldLine);
                    }
                }
            break;
            case SLOOMSTATE.Movable:
                Collider2D[] ClosestSlooms = Physics2D.OverlapCircleAll(transform.position, _detectionRadius, _SloomLayer);
                foreach (Collider2D sloomCollider in ClosestSlooms) {
                    GameObject newSloom = sloomCollider.gameObject;
                    if (newSloom != gameObject && newSloom.TryGetComponent(out ScSloom newSloomComp) && (newSloomComp.SloomState == SLOOMSTATE.Static || newSloomComp.SloomState == SLOOMSTATE.Placed)) {
                        AddLine(gameObject, newSloom, true);
                    }
                }
                foreach (Line oldLine in Lines) {
                    bool isLinked = false;
                    foreach (Collider2D collider in ClosestSlooms) {
                        if (collider.transform == oldLine.destination) {
                            isLinked = true; break;
                        }
                    }

                    if (!isLinked) {
                        oldLine.line.SetActive(false);
                    } 
                }
            break;

        }

    }

    void AddLine(GameObject gameObject1, GameObject gameObject2, bool isTransparent = false) {
        if(!IsSloomInList(gameObject2.transform)) {
            GameObject newLineObject = Instantiate(_linePrefab,transform.parent);

            newLineObject = PlaceLine(newLineObject, gameObject1.transform.position, gameObject2.transform.position);

            if (isTransparent) {
                SpriteRenderer spriteRenderer = newLineObject.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null) {
                    Color color = spriteRenderer.color;
                    color.a = 0.5f;
                    spriteRenderer.color = color;
                }
            }

            Line newLine = new Line { destination = gameObject2.transform, line = newLineObject};
            Lines.Add(newLine);
        }
        else {
            GameObject line = GetSloomInList(gameObject2.transform);
            line.SetActive(true);

            line = PlaceLine(line, gameObject1.transform.position, gameObject2.transform.position);
            if (!isTransparent) {
                SpriteRenderer spriteRenderer = line.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null) {
                    Color color = spriteRenderer.color;
                    color.a = 1f;
                    spriteRenderer.color = color;
                }
            }
        }
    }

    GameObject PlaceLine(GameObject line, Vector2 startPoint, Vector2 endPoint) {
        line.transform.position = (startPoint + endPoint) / 2;

        float distance = Vector2.Distance(startPoint, endPoint);
        line.transform.localScale = new Vector3(distance, line.transform.localScale.y, line.transform.localScale.z);

        Vector2 lineDirection = endPoint - startPoint;
        float lineOrientation = Mathf.Atan2(lineDirection.y, lineDirection.x) * Mathf.Rad2Deg;
        line.transform.rotation = Quaternion.Euler(0, 0, lineOrientation);

        return line;
    }
   
    bool IsSloomInList(Transform target) {
        foreach (Line line in Lines) { 
            if(line.destination == target) return true;
        }
        return false;
    }

    GameObject GetSloomInList(Transform target) {
        foreach (Line line in Lines) {
            if (line.destination == target) return line.line;
        }
        return null;
    }

}
[System.Serializable]
public struct Line {
    public Transform destination;
    public GameObject line;
}