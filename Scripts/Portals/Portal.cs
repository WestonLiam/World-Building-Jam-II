using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Portal : MonoBehaviour
{
    [field: SerializeField]
    public Portal OtherPortal { get; private set; }

    //[SerializeField]
    //private Renderer outlineRenderer;

    [field: SerializeField]
    public Color PortalColour { get; private set; }

    [SerializeField]
    private LayerMask placementMask;

    private List<PortalableObject> portalObjects = new List<PortalableObject>();
    //public bool IsPlaced { get; private set; } = true;

    private PortalCamera _camera;

    // Components.
    public Renderer Renderer { get; private set; }
    private new BoxCollider collider;

    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
        Renderer = GetComponent<Renderer>();

        _camera = Camera.main.GetComponent<PortalCamera>();
    }

    private void Update()
    {
        for (int i = 0; i < portalObjects.Count; ++i)
        {
            Vector3 objPos = transform.InverseTransformPoint(portalObjects[i].transform.position);

            //if (objPos.z > 0.0f)
            //{
                portalObjects[i].Warp();
            //}
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Portal Entered");

        PortalableObject obj = other.GetComponent<PortalableObject>();
        
        if (obj != null)
        {
            portalObjects.Add(obj);
            obj.SetIsInPortal(this, OtherPortal);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var obj = other.GetComponent<PortalableObject>();

        if (portalObjects.Contains(obj))
        {
            portalObjects.Remove(obj);
            obj.ExitPortal();
        }
    }
}