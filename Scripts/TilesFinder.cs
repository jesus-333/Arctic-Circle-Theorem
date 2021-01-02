using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesFinder : MonoBehaviour
{
    public bool touch_tile = false;

    private void OnCollisionEnter(Collision collision)
    {
        print("ENTER COLLISION");
        print("Position:" + this.transform.position);
        setTouchTile(true);
    }

    private void OnCollisionExit(Collision collision)
    {
        print("EXIT COLLISION");
        setTouchTile(false);
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     print("ASDASD");
    //     setTouchTile(true);
    // }
    //
    //
    // private void OnTriggerExit(Collider other)
    // {
    //     print("ASDASD");
    //     setTouchTile(false);
    // }

    // private void Update()
    // {
    //     setTouchTile(false);
    // }

    // private void OnCollisionStay()
    // {
    //     setTouchTile(true);
    // }

    // private void OnTriggerStay()
    // {
    //     setTouchTile(true);
    // }

    private void setTouchTile(bool var)
    {
        touch_tile = var;
        // print("SET COLLIDER to: " + touch_tile);
    }

    public void printState(){ print("touch_tile = " + touch_tile);}

}
