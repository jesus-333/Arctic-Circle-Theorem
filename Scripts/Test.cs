using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject obj;
    public Color color;
    public Material line_material;

    public Vector3 position_1;
    public Vector3 position_2;
    public Vector3 position_3;

    public bool debug_var;

    // Start is called before the first frame update
    void Start()
    {
        obj = Instantiate(obj, new Vector3(0,0,0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        // renderer = obj.GetComponent<Renderer>();
        // obj.GetComponent<MeshRenderer>().sharedMaterial.color = color; //C#
        obj.GetComponent<MeshRenderer>().material.color = color;

    }
}
