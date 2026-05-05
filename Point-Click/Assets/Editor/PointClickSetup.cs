using UnityEngine;
using UnityEditor;
using System.IO;

// Unity menu: Point-Click ▸ Create ClickableArea Prefab
//             Point-Click ▸ Create FirstPersonCamera Prefab
//
// Run either item once — it writes a prefab to Assets/Prefabs/ that you can
// then drag directly into any scene.
public static class PointClickSetup
{
    const string PrefabFolder = "Assets/Prefabs";

    // ── ClickableArea ────────────────────────────────────────────────────────
    [MenuItem("Point-Click/Create ClickableArea Prefab")]
    public static void CreateClickableAreaPrefab()
    {
        EnsureFolder(PrefabFolder);

        // A Unity Plane sits flat on the XZ plane and already has a MeshCollider.
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Plane);
        go.name = "ClickableArea";

        go.layer = LayerMask.NameToLayer("Default");
        go.AddComponent<ClickableArea>();

        // The built-in Plane is 10 × 10 world-units at scale (1,1,1).
        // Scale it down to 1 × 1 so the user starts with predictable sizing.
        go.transform.localScale = new Vector3(0.1f, 1f, 0.1f);

        SavePrefab(go, PrefabFolder + "/ClickableArea.prefab");
        Object.DestroyImmediate(go);
    }

    // ── FirstPersonCamera ────────────────────────────────────────────────────
    [MenuItem("Point-Click/Create FirstPersonCamera Prefab")]
    public static void CreateFirstPersonCameraPrefab()
    {
        EnsureFolder(PrefabFolder);

        GameObject root = new GameObject("FirstPersonCamera");
        root.AddComponent<Camera>();
        root.AddComponent<AudioListener>();
        root.AddComponent<FirstPersonController>();

        SavePrefab(root, PrefabFolder + "/FirstPersonCamera.prefab");
        Object.DestroyImmediate(root);
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
