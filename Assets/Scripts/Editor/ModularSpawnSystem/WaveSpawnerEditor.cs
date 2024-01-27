using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;
using System.Linq;

namespace SplitFace.ModularSpawnSystem.SpawnSystemEditor
{
    [CustomEditor(typeof(WaveSpawner))]
    public class WaveSpawnerEditor : Editor
    {
        bool editWaves = false;

        WaveSpawner waveSpawnerTarget { get { return target as WaveSpawner; } }

        void OnEnable()
        {

        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            serializedObject.Update();

            if (editWaves)
            {
                editWaves = !GUILayout.Button("Edit Settings");

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(serializedObject.FindProperty("waves"));

                EditorGUILayout.Space();

                if (waveSpawnerTarget.enableFodderUnits)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("fodderUnits"));
                }
            }
            else
            {
                editWaves = GUILayout.Button("Edit Waves");

                EditorGUILayout.Space();

                DisplaySettingsInspector();
            }

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        void DisplaySettingsInspector()
        {
            LayerMask tempMask = EditorGUILayout.MaskField("Spawn Layer Check",
                InternalEditorUtility.LayerMaskToConcatenatedLayersMask(waveSpawnerTarget.unitLayer),
                InternalEditorUtility.layers);

            waveSpawnerTarget.unitLayer = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMask);
            waveSpawnerTarget.spawnRadiusCheck = EditorGUILayout.FloatField("Check Radius", waveSpawnerTarget.spawnRadiusCheck);

            waveSpawnerTarget.maxSlots = EditorGUILayout.IntField("Max slots available", waveSpawnerTarget.maxSlots);
            waveSpawnerTarget.maxFodderSlots = EditorGUILayout.IntField("Max fodder slots available", waveSpawnerTarget.maxFodderSlots);

            waveSpawnerTarget.spawnDelay = EditorGUILayout.FloatField("Delay before spawns", waveSpawnerTarget.spawnDelay);

            waveSpawnerTarget.enableFodderUnits = EditorGUILayout.Toggle("Enable fodder enemies", waveSpawnerTarget.enableFodderUnits);
            waveSpawnerTarget.waitForEmptyWave = EditorGUILayout.Toggle("Wait for empty wave", waveSpawnerTarget.waitForEmptyWave);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("spawnPoints"));
        }
    }
}
