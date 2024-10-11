using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScSloom : MonoBehaviour{
    public bool IsDraggable;
    [SerializeField] float _detectionRadius;
    [SerializeField] LayerMask _SloomLayer;

    public List<GameObject> NeighborgSlooms = new List<GameObject>();

    public void Drop() {
        foreach (Collider2D sloomCollider in  Physics2D.OverlapCircleAll(transform.position, _detectionRadius, _SloomLayer)) {
            GameObject newSloom = sloomCollider.gameObject;
            if (newSloom != gameObject && newSloom.TryGetComponent(out ScSloom newSloomComp)) {
                NeighborgSlooms.Add(sloomCollider.gameObject);
                newSloomComp.NeighborgSlooms.Add(gameObject);
                newSloomComp.IsDraggable = false;

                AddJoints(gameObject, newSloom);
            }
        }
        
        if (NeighborgSlooms.Count != 0) {
            IsDraggable = false;
        }
    }

    private void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }

    void AddJoints(GameObject gameObject1, GameObject gameObject2) {
        SpringJoint2D joint = gameObject1.AddComponent<SpringJoint2D>();
        joint.connectedBody = gameObject2.GetComponent<Rigidbody2D>();
        SpringJoint2D newsSloomJoint = gameObject2.AddComponent<SpringJoint2D>();
        newsSloomJoint.connectedBody = gameObject1.GetComponent<Rigidbody2D>();
    }
}
