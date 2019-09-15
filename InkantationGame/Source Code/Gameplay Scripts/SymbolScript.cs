using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolScript : MonoBehaviour
{
    public float sprayDist = 2.5f;
    // public GameObject symbol;

    private Camera cam;
    private RaycastHit hit;
    private Ray ray;

    private void Start()
    {
        cam = Camera.main;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    }

    void Update()
    {
        UpdateRay();
        Physics.Raycast(ray.origin, ray.direction, out hit, sprayDist);
        
        /*transform.position, rayDir * sprayDist, 
        // Bit shift the index of the layer (8) to get a bit mask
        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        int layerMask = 1 << 9;
        layerMask = ~layerMask;
        
        if (Input.GetButtonDown("Fire1") && Physics.Raycast(transform.position, rayDir, out hit, sprayDist, layerMask))
            Instantiate(symbol, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
        */
    }
    
    private void UpdateRay()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, sprayDist);

        if (hit.collider == null)
            return;

        if (hit.collider.tag == "Enemy")
            Debug.DrawRay(ray.origin, ray.direction * sprayDist, Color.red);
        else
            Debug.DrawRay(ray.origin, ray.direction * sprayDist, Color.green);
    }

    public RaycastHit GetRayHit(Vector2 position)
    {
        ray = Camera.main.ScreenPointToRay(position);
        UpdateRay();
        
        return hit;
    }
}