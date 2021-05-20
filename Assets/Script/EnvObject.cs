using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class EnvObject : MonoBehaviour
{

    public Transform[] cutPlane;
    public LayerMask layerMask;
    public Material crossMaterial;

    public void Ondamage() {
        OnDie();
    }
    void OnDie() {
        Slice();
    }
    public void Slice() {
        for (int i = 0; i < cutPlane.Length; i++) {
            SlicedHull hull = SliceObject(gameObject, cutPlane[i], crossMaterial);
            
            if (hull != null)
            {
                GameObject one = hull.CreateLowerHull(gameObject, crossMaterial);
                GameObject other = hull.CreateUpperHull(gameObject, crossMaterial);
                AddHullComponents(one);
                AddHullComponents(other);
                one.layer = 10;
                other.layer = 10;
                Destroy(gameObject);
            }
        }
    }

    public void AddHullComponents(GameObject go)
    {
        go.layer = 9;
        Rigidbody rb = go.AddComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        MeshCollider collider = go.AddComponent<MeshCollider>();
        collider.convex = true;

        rb.AddExplosionForce(100, go.transform.position, 20);
    }

    public SlicedHull SliceObject(GameObject obj, Transform cutPlane, Material crossSectionMaterial = null) {
        // slice the provided object using the transforms of this object
        if (obj.GetComponent<MeshFilter>() == null)
            return null;

        return obj.Slice(cutPlane.position, cutPlane.up, crossSectionMaterial);
    }
}
