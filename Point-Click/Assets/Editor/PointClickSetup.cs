using UnityEngine;
using UnityEditor;
using System.IO;

// Unity menu: Point-Click ▸ Create ClickableArea Prefab
//             Point-Click ▸ Setup Camera in Scene
public static class PointClickSetup
{
    const string PrefabFolder = "Assets/Prefabs";

    // ── ClickableArea ────────────────────────────────────────────────────────
    // Creates an invisible, flat BoxCollider-only prefab.
    // Scale it freely on X/Z to cover any floor area — nothing is rendered.
    [MenuItem("Point-Click/Create ClickableArea Prefab")]
    public static void CreateClickableAreaPrefab()
    {
        EnsureFolder(PrefabFolder);

        GameObject go = new GameObject("ClickableArea");
        go.layer = LayerMask.NameToLayer("Default");

        BoxCollider col = go.AddComponent<BoxCollider>();
        col.size = new Vector3(1f, 0.02f, 1f); // flat slab, 1×1 at default scale
        col.isTrigger = false;                  // must be solid for Physics.Raycast

        go.AddComponent<ClickableArea>();

        SavePrefab(go, PrefabFolder + "/ClickableArea.prefab");
        Object.DestroyImmediate(go);
    }

    // ── Setup Camera in Scene ────────────────────────────────────────────────
    // Adds FirstPersonController to the Main Camera already in your scene.
    // Run this once — no prefab needed, the component lives on the camera.
    [MenuItem("Point-Click/Setup Camera in Scene")]
    public static void SetupCameraInScene()
    {
        Camera cam = Camera.main;

        if (cam == null)
        {
            Debug.LogError("[Point-Click] No Main Camera found. Make sure your camera is tagged 'MainCamera'.");
            return;
        }

        if (cam.GetComponent<FirstPersonController>() != null)
        {
            Debug.LogWarning("[Point-Click] FirstPersonController is already on the Main Camera.");
            return;
        }

        cam.gameObject.AddComponent<FirstPersonController>();
        EditorUtility.SetDirty(cam.gameObject);
        Debug.Log($"[Point-Click] FirstPersonController added to '{cam.gameObject.name}'.");
    }

    // ── helpers ──────────────────────────────────────────────────────────────
    static void EnsureFolder(string path)
    {
        if (!AssetDatabase.IsValidFolder(path))
        {
            string parent = Path.GetDirectoryName(path).Replace('\\', '/');
            string folder = Path.GetFileName(path);
            AssetDatabase.CreateFolder(parent, folder);
        }
    }

    static void SavePrefab(GameObject go, string path)
    {
        PrefabUtility.SaveAsPrefabAsset(go, path);
        AssetDatabase.Refresh();
        Debug.Log($"[Point-Click] Prefab saved → {path}");
    }
}
