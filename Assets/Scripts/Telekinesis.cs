using UnityEngine;

public class Telekinesis : MonoBehaviour {
    public LineRenderer lineRenderer;
    public float distance, speed;

    private bool hasPickedUpEnemy;
    private Rigidbody enemy;
    private Vector3 enemyPos;
    private Vector3 previousMousePosition;
    private bool isMouseDrag = false;

    public Vector3 EnemyPos { get => enemyPos; set => enemyPos = value; }

    void Start() {
        SetLineRenderer();
    }

    public void SetLineRenderer() {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;
        lineRenderer.gameObject.SetActive(false);
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            DetectEnemy();
        }
        if (Input.GetMouseButtonUp(0)) {
            DropEnemy();
        }
        if (Input.GetMouseButton(0)) {
            if (hasPickedUpEnemy) {
                DragEnemy();
            }
        }
        AdjustCam();
    }

    private void AdjustCam() {
        if (enemyPos != null) {
            Quaternion targetRotation = Quaternion.LookRotation(enemyPos - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
        }
    }

    private void DragEnemy() {
        Vector3 inputMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100f));
        Vector3 delta = (inputMousePosition - transform.position);
        Vector3 directionVector = delta.normalized;
        float magnitude = ((enemyPos - transform.position).magnitude);
        Vector3 newPositionVector = transform.position +  (directionVector * magnitude);
        isMouseDrag = !(previousMousePosition == Input.mousePosition);
        previousMousePosition = Input.mousePosition;
        if (isMouseDrag) {
            enemy.transform.position = newPositionVector;
            lineRenderer.SetPosition(1, newPositionVector);
        }
    }

    private void DetectEnemy() {
        RaycastHit raycastHit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out raycastHit, distance)) {
            if (raycastHit.transform != null) {
                if (raycastHit.transform.CompareTag("Enemy")) {
                    enemy = raycastHit.transform.GetComponent<Rigidbody>();
                    enemy.useGravity = false;
                    enemy.isKinematic = true;
                    hasPickedUpEnemy = true;
                    lineRenderer.gameObject.SetActive(true);
                    lineRenderer.SetPosition(1, raycastHit.transform.position);
                    enemyPos = raycastHit.transform.position;
                }
            }
        }
    }

    private void DropEnemy() {
        if (enemy != null) {
            enemy.useGravity = true;
            enemy.isKinematic = false;
            if(enemy.GetComponent<Enemy>().isKilled) {
                GameEventSystem.RaiseEvent(GAME_EVENT_TYPE.ENEMY_KILL);
            }
        }
        hasPickedUpEnemy = false;
        lineRenderer.gameObject.SetActive(false);
    }
}
