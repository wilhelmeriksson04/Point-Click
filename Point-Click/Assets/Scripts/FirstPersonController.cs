using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Smooth glide between click points. Unchecked = instant snap.")]
    public bool smoothTransition = true;
    public float moveSpeed = 4f;

    [Header("Hand Graphic")]
    [Tooltip("Drag any GameObject (e.g. a UI Image) here to act as the on-screen hand.")]
    public GameObject handGraphic;

    [Header("Walk Bob  (only active during smooth movement)")]
    public float walkBobSpeed  = 8f;
    public float walkBobAmount = 0.05f;

    [Header("Idle Bob  (breathing — always active when stationary)")]
    public float idleBobSpeed  = 1.2f;
    public float idleBobAmount = 0.008f;

    // ── private state ────────────────────────────────────────────────────────
    private Camera _cam;
    private Vector3 _basePosition;   // XZ position without bob offset
    private Vector3 _targetPosition;
    private bool    _isMoving;
    private float   _bobTime;

    // ── lifecycle ────────────────────────────────────────────────────────────
    void Awake()
    {
        _cam            = GetComponent<Camera>();
        _basePosition   = transform.position;
        _targetPosition = transform.position;
    }

    void Update()
    {
        HandleClick();
        HandleMovement();
        ApplyBob();
    }

    // ── input ────────────────────────────────────────────────────────────────
    void HandleClick()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit)) return;
        if (hit.collider.GetComponent<ClickableArea>() == null) return;

        // Keep the camera's current height — only move on the XZ plane.
        Vector3 dest = hit.point;
        dest.y = _basePosition.y;

        if (smoothTransition)
        {
            _targetPosition = dest;
            _isMoving       = true;
        }
        else
        {
            // Snap: jump immediately, no walk bob.
            _basePosition   = dest;
            _targetPosition = dest;
            _isMoving       = false;
        }
    }

    // ── movement ─────────────────────────────────────────────────────────────
    void HandleMovement()
    {
        if (!smoothTransition || !_isMoving) return;

        _basePosition = Vector3.MoveTowards(_basePosition, _targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(_basePosition, _targetPosition) < 0.01f)
        {
            _basePosition = _targetPosition;
            _isMoving     = false;
        }
    }

    // ── bob ──────────────────────────────────────────────────────────────────
    void ApplyBob()
    {
        bool walking = _isMoving && smoothTransition;

        float speed  = walking ? walkBobSpeed  : idleBobSpeed;
        float amount = walking ? walkBobAmount : idleBobAmount;

        _bobTime += Time.deltaTime * speed;
        transform.position = _basePosition + Vector3.up * (Mathf.Sin(_bobTime) * amount);
    }
}
