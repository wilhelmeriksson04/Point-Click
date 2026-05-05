using UnityEngine;

// Marker component — attach to any GameObject with a Collider to make it a walkable area.
// The FirstPersonController raycasts for this component on hit to decide where to move.
[RequireComponent(typeof(Collider))]
public class ClickableArea : MonoBehaviour { }
