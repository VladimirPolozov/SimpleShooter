using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayViewer : MonoBehaviour
{
    public float WeaponRange = 50f;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
        Vector3 lineOrigin = _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        Debug.DrawRay(lineOrigin, _camera.transform.forward * WeaponRange, Color.green);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
}
