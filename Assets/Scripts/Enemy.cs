using UnityEngine;

public class Enemy : MonoBehaviour {

    public int level;
    public Material inactiveMaterial;
    public MeshRenderer meshRenderer;
    public bool isKilled = false;

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Wall")) {
            meshRenderer.material = inactiveMaterial;
            isKilled = true;
        }
    }
}
