using System.Linq;
using UnityEngine;
using UnityEditor;

public class JamEditorTools
{
    private const string MENU_PATH = "Tools/Jam/";

    [MenuItem(MENU_PATH + "Find Duplicate Meshes")]
    private static void FindDuplicateMeshes() {
        FindDuplicatesOfType<MeshRenderer>();
    }

    [MenuItem(MENU_PATH + "Find Duplicate Colliders")]
    private static void FindDuplicateColliders() {
        FindDuplicatesOfType<Collider>();
    }

    private static void FindDuplicatesOfType<T>() where T : Component {
        var transforms = GameObject.FindObjectsOfType<Transform>();

        var dups = transforms
            .Where(t => t.gameObject.GetComponent<T>() != null)
            .GroupBy(t => t.position.ToString() + t.name.Split(" ")[0])
            .Where(g => g.Count() > 1)
            .Select(y => y.ToList())
            .ToList();

        foreach (var duplicate in dups) {
            var msg = "Duplicates:\n";

            foreach (var t in duplicate) {
                msg += t.name + '\n';
            }

            Debug.LogWarning(msg, duplicate[0]);
        }
    }
}
