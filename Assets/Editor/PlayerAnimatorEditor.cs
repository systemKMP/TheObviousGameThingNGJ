using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(PlayerAnimator))]
public class PlayerAnimatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var animator = (PlayerAnimator)target;

        var remove = new List<string>();
        var rename = new Dictionary<string, string>();
        var change = new Dictionary<string, Sprite>();
        GUILayout.BeginVertical();
        foreach (var pair in animator.Sprites)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Delete"))
            {
                remove.Add(pair.Key);
                GUILayout.EndHorizontal();
                break;
            }
            var changeSprite = (Sprite)EditorGUILayout.ObjectField("", pair.Value, typeof(Sprite), false);
            if (changeSprite != pair.Value)
            {
                change.Add(pair.Key, changeSprite);
            }
            var changeName = EditorGUILayout.TextField("", pair.Key);
            if (changeName != pair.Key)
            {
                rename.Add(pair.Key, changeName);
            }
            GUILayout.EndHorizontal();
        }

        foreach (var item in remove)
        {
            animator.Sprites.Remove(item);
        }
        foreach (var pair in change)
        {
            animator.Sprites[pair.Key] = pair.Value;
        }
        foreach (var pair in rename)
        {
            var value = animator.Sprites[pair.Key];
            animator.Sprites.Remove(pair.Key);
            animator.Sprites.Add(pair.Value, value);
        }

        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        var newSprite = (Sprite)EditorGUILayout.ObjectField("New Sprite:", null, typeof(Sprite), false);
        GUILayout.EndHorizontal();
        if (newSprite != null)
        {
            animator.Sprites.Add("Sprite", newSprite);
        }
        GUILayout.EndVertical();
    }
}
