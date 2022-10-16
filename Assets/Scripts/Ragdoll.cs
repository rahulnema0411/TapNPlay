using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour {

    private void Start() {
        setRigidbodyState(true);
    }

    public void EnableRagdoll() {
        setRigidbodyState(false);
    }
    
    public void DisableRagdoll() {
        setRigidbodyState(true);
    }

    private void setRigidbodyState(bool state) {

        Transform[] rigidbodies = GetComponentsInChildren<Transform>();
        
        foreach (Transform t in rigidbodies) {

            Rigidbody rigidbody = t.GetComponent<Rigidbody>();
            if(rigidbody != null) {
                rigidbody.isKinematic = state;
            }
        }
        transform.GetComponent<Rigidbody>().isKinematic = state;
    }

}
