using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalableObject : MonoBehaviour
{
    //private GameObject cloneObject;

    private int inPortalCount = 0;

    private Portal inPortal;
    private Portal outPortal;

    private new Rigidbody rigidbody;

    private static readonly Quaternion halfTurn = Quaternion.Euler(0.0f, 180.0f, 0.0f);

    [SerializeField] private List<PortalableObject> _warpCompanions;

    private void Awake()
    {
        //cloneObject = new GameObject();
        //cloneObject.SetActive(false);

        //SetUpCloneObject();

        rigidbody = GetComponent<Rigidbody>();
    }

    private void LateUpdate()
    {
        if (inPortal == null || outPortal == null)
        {
            return;
        }

        //if (cloneObject.activeSelf)
        //{
        //    var inTransform = inPortal.transform;
        //    var outTransform = outPortal.transform;
        //
        //    // Update position of clone.
        //    Vector3 relativePos = inTransform.InverseTransformPoint(transform.position);
        //    relativePos = halfTurn * relativePos;
        //    cloneObject.transform.position = outTransform.TransformPoint(relativePos);
        //
        //    // Update rotation of clone.
        //    Quaternion relativeRot = Quaternion.Inverse(inTransform.rotation) * transform.rotation;
        //    relativeRot = halfTurn * relativeRot;
        //    cloneObject.transform.rotation = outTransform.rotation * relativeRot;
        //}
        //else
        //{
        //    cloneObject.transform.position = new Vector3(-1000.0f, 1000.0f, -1000.0f);
        //}
    }

    //private void SetUpCloneObject()
    //{
    //    for (int i = 0; i < transform.childCount; i++)
    //    {
    //        Transform child = transform.GetChild(i);
    //        MeshFilter childMesh = child.GetComponent<MeshFilter>();
    //
    //        if (childMesh != null)
    //        {
    //            GameObject cloneChild = new GameObject();
    //            cloneChild.transform.parent = cloneObject.transform;
    //
    //            cloneChild.AddComponent<MeshFilter>();
    //            cloneChild.GetComponent<MeshFilter>().mesh = childMesh.mesh;
    //
    //            MeshRenderer childRenderer = child.GetComponent<MeshRenderer>();
    //            
    //            if (childRenderer != null)
    //            {
    //                MeshRenderer cloneRenderer = cloneChild.AddComponent<MeshRenderer>();
    //                cloneRenderer.materials = childRenderer.materials;
    //            }
    //
    //            else
    //            {
    //                SkinnedMeshRenderer skinnedRenderer = child.GetComponent<SkinnedMeshRenderer>();
    //
    //                if (skinnedRenderer != null)
    //                {
    //                    SkinnedMeshRenderer cloneRenderer = cloneChild.AddComponent<SkinnedMeshRenderer>();
    //                    cloneRenderer.materials = childRenderer.materials;
    //                    cloneRenderer.rootBone = skinnedRenderer.rootBone;
    //                }
    //            }
    //
    //            cloneChild.transform.localScale = transform.localScale;
    //        }
    //    }
    //    
    //    MeshFilter meshFilter = cloneObject.AddComponent<MeshFilter>();
    //    SkinnedMeshRenderer meshRenderer = cloneObject.AddComponent<SkinnedMeshRenderer>();
    //
    //    //meshFilter.mesh = GetComponent<MeshFilter>().mesh;
    //    //meshRenderer.materials = GetComponent<MeshRenderer>().materials;
    //    cloneObject.transform.localScale = transform.localScale;
    //}

    public void SetIsInPortal(Portal inPortal, Portal outPortal)
    {
        this.inPortal = inPortal;
        this.outPortal = outPortal;

        //cloneObject.SetActive(false);

        ++inPortalCount;
    }

    public void ExitPortal()
    {
        --inPortalCount;

        if (inPortalCount == 0)
        {
            //cloneObject.SetActive(false);
        }
    }

    public virtual void WarpAll()
    {
        //for (int i = 0; i < _warpCompanions.Count; i++)
        //{
        //    _warpCompanions[i].SetIsInPortal(inPortal, outPortal);
        //    _warpCompanions[i].Warp();
        //}

        Warp();
    }

    public virtual void Warp()
    {
        var inTransform = inPortal.transform;
        var outTransform = outPortal.transform;

        // Update position of object.
        Vector3 relativePos = inTransform.InverseTransformPoint(transform.position);
        relativePos = halfTurn * relativePos;

        transform.position = outTransform.TransformPoint(relativePos);

        // Update rotation of object.
        Quaternion relativeRot = Quaternion.Inverse(inTransform.rotation) * transform.rotation;
        relativeRot = halfTurn * relativeRot;
        transform.rotation = outTransform.rotation * relativeRot;

        if (rigidbody != null)
        {
            // Update velocity of rigidbody.
            Vector3 relativeVel = inTransform.InverseTransformDirection(rigidbody.velocity);
            relativeVel = halfTurn * relativeVel;
            rigidbody.velocity = outTransform.TransformDirection(relativeVel);
        }

        // Swap portal references.
        var tmp = inPortal;
        inPortal = outPortal;
        outPortal = tmp;
    }
}