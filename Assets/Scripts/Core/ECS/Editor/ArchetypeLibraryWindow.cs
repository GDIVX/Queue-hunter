using Assets.Scripts.Engine.ECS;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

public class ArchetypeLibraryWindow : OdinEditorWindow
{
    private ArchetypeLibrary library;

    [BoxGroup("View")]
    [ShowInInspector, InlineEditor(Expanded = true)]
    private Archetype selectedArchetype;

    private Vector2 scrollPosition;
    private static readonly string libraryPath = "Assets/Data/ArchetypeLibrary.asset"; // Path to your ArchetypeLibrary asset. Do not edit this in the inspector.

    public Archetype SelectedArchetype
    {
        get => selectedArchetype;
        set
        {
            selectedArchetype = value;
        }
    }

    event Action OnSelect;

    [MenuItem("ECS/Archetype Library")]
    public static void ShowWindow()
    {
        var window = GetWindow<ArchetypeLibraryWindow>("Archetype Manager");
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 800, 500);
        window.Show();
    }

    protected override void OnImGUI()
    {
        base.OnImGUI();

        if (library == null)
        {
            library = AssetDatabase.LoadAssetAtPath<ArchetypeLibrary>(libraryPath);
        }

        if (library != null)
        {

            DrawArchetypesList();
        }
        else
        {
            GUILayout.Label("Archetype Library not found at specified path.", EditorStyles.boldLabel);
        }
    }

    private void DrawArchetypesList()
    {
        // Begin a vertical layout that will contain the archetypes list and the scrollbar
        GUILayout.BeginVertical(GUILayout.Width(280), GUILayout.ExpandHeight(true));
        EditorGUILayout.LabelField("Archetypes", EditorStyles.boldLabel);

        if (library.archetypes == null || library.archetypes.Count <= 0)
        {
            EditorGUILayout.HelpBox("No archetypes found in the library.", MessageType.Info);
        }
        else
        {
            // Begin a scroll view for the vertical list of archetypes
            GUILayout.Space(8);
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.ExpandHeight(true));

            foreach (var archetype in library.archetypes)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(archetype.name, GUILayout.Width(150));

                // The button should stay to the right of the label
                if (GUILayout.Button("View", GUILayout.Width(50)))
                {
                    SelectedArchetype = archetype;
                }

                if (GUILayout.Button("Copy", GUILayout.Width(50)))
                {
                    // Copy the archetype
                    Copy(archetype);
                }

                GUILayout.EndHorizontal();
            }

            // End the scroll view
            EditorGUILayout.EndScrollView();
        }

        EditorGUILayout.EndVertical();
    }

    private void Copy(Archetype archetype)
    {
        var copy = archetype.DeppCopy();

        //View the copy so we can edit it.
        SelectedArchetype = copy;

        OnSelect += () =>
        {
            if (SelectedArchetype != copy)
            {
                DestroyImmediate(copy);
            }
        };
    }

    [Button()]
    private void SaveAsNew()
    {
        if (SelectedArchetype == null)
        {
            EditorUtility.DisplayDialog("Error",
            "Please select an archetype to save.", "OK");
            return;
        }

        // Open file path window for the archetype
        string archetypePath = EditorUtility.SaveFilePanel(
            "Save archetype as...",
            "Assets/Data/",
            SelectedArchetype.name + "_copy",
            "asset");

        if (string.IsNullOrEmpty(archetypePath))
        {
            // User cancelled the save operation
            return;
        }
        // Extract the component file name from the asset path
        string archetypeName = Path.GetFileNameWithoutExtension(archetypePath);

        string assetPath = FileUtil.GetProjectRelativePath(archetypePath);

        // Loop over the components and save them too
        foreach (var component in SelectedArchetype.Components)
        {

            // Format the component name to follow {ArchetypeName}_{ComponentName}
            string formattedComponentName = archetypeName + "_" + component.name;

            // Generate the path for the component asset
            string componentPath = Path.GetDirectoryName(assetPath) + "/" + formattedComponentName + ".asset";

            // Check if an asset already exists at this path and generate a unique path if necessary
            componentPath = AssetDatabase.GenerateUniqueAssetPath(componentPath);

            // Create the component asset and save it
            AssetDatabase.CreateAsset(component, componentPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            // Make the new component asset addressable
            MarkAsAddressable(componentPath, formattedComponentName);
        }

        // Create and save the archetype asset
        AssetDatabase.CreateAsset(SelectedArchetype, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        // Make the new archetype asset addressable
        MarkAsAddressable(assetPath, SelectedArchetype.name);
    }


    private void MarkAsAddressable(string assetPath, string addressableName)
    {
        // Get the Addressables settings
        var settings = AddressableAssetSettingsDefaultObject.Settings;

        // Create an entry in the Addressables system
        var guid = AssetDatabase.AssetPathToGUID(assetPath);
        var group = settings.DefaultGroup; // Or choose a specific group if necessary
        var entry = settings.CreateOrMoveEntry(guid, group);

        // Set the addressable name
        entry.address = addressableName;

        // Save the changes to the Addressables system
        settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entry, true);
    }

}
