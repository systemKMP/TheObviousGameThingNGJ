using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(WeaponSpawner))]
public class WeaponSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var spawner = (WeaponSpawner)target;

        GUILayout.BeginVertical();
        for(int i = 0; i < spawner.Weapons.Count; i++)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Delete"))
            {
                spawner.Weapons.RemoveAt(i);
                spawner.Rates.RemoveAt(i);
                GUILayout.EndHorizontal();
                break;
            }
            spawner.Weapons[i] = (Weapon)EditorGUILayout.ObjectField("", spawner.Weapons[i], typeof(Weapon), false);
            spawner.Rates[i] = EditorGUILayout.FloatField("", spawner.Rates[i]);
            GUILayout.EndHorizontal();
        }
        GUILayout.BeginHorizontal();
        var newWeapon = (Weapon)EditorGUILayout.ObjectField("New Weapon:", null, typeof(Weapon), false);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Min Spawn Time");
        GUILayout.Label("Max Spawn Time");
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        spawner.SpawnMin = EditorGUILayout.FloatField("", spawner.SpawnMin);
        spawner.SpawnMax = EditorGUILayout.FloatField("", spawner.SpawnMax);
        GUILayout.EndHorizontal();
        if (newWeapon != null)
        {
            spawner.Weapons.Add(newWeapon);
            spawner.Rates.Add(1);
        }
        GUILayout.EndVertical();
    }
}
