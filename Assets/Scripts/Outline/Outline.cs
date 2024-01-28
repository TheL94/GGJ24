using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outline : MonoBehaviour
{
    public Material outlineMaterial;
    public float outlineScaleFactor;
    public Color outlineColor;

    private Renderer outlineRenderer;

    void Start()
    {
        outlineRenderer = CreateOutline(outlineMaterial, outlineScaleFactor, outlineColor);
        outlineRenderer.enabled = true;
    }

    private Renderer CreateOutline(Material outlineMaterial, float outlineScaleFactor, Color outlineColor)
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Rigidbody rigidbody = GetComponent<Rigidbody>();

        GameObject outlineGameObject = new GameObject("Outline");

        MeshRenderer outlineMeshRenderer = outlineGameObject.AddComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            outlineMeshRenderer.sharedMaterial = meshRenderer.sharedMaterial;
            outlineMeshRenderer.sharedMaterials = meshRenderer.sharedMaterials;
            outlineMeshRenderer.receiveShadows = meshRenderer.receiveShadows;
            outlineMeshRenderer.shadowCastingMode = meshRenderer.shadowCastingMode;
        }

        MeshFilter outlineMeshFilter = outlineGameObject.AddComponent<MeshFilter>();
        if (meshFilter != null)
        {
            outlineMeshFilter.sharedMesh = meshFilter.sharedMesh;
        }

        Rigidbody outlineRigidBody = outlineGameObject.AddComponent<Rigidbody>();
        if (rigidbody)
        {
            outlineRigidBody.mass = rigidbody.mass;
            outlineRigidBody.drag = rigidbody.drag;
            outlineRigidBody.angularDrag = rigidbody.angularDrag;
            outlineRigidBody.useGravity = rigidbody.useGravity;
            outlineRigidBody.isKinematic = rigidbody.isKinematic;
            outlineRigidBody.interpolation = rigidbody.interpolation;
            outlineRigidBody.collisionDetectionMode = rigidbody.collisionDetectionMode;
            outlineRigidBody.velocity = rigidbody.velocity;
            outlineRigidBody.angularVelocity = rigidbody.angularVelocity;
        }

        outlineGameObject.transform.position = transform.position;
        outlineGameObject.transform.rotation = transform.rotation;

        Renderer renderer = new Renderer();
        renderer.material = outlineMaterial;
        renderer.material.SetColor("_OutlineColor", outlineColor);
        renderer.material.SetFloat("_Scale", outlineScaleFactor);
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        renderer.enabled = false;
        return renderer;
    }
}
