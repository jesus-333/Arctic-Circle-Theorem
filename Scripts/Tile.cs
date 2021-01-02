using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int x;
    public int y;
    public int type; // 1 = UP, 2 = RIGHT, 3 = DOWN, 4 = LEFT
    private Color color;

    public bool debug_var = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Move()
    {
        switch (type){
            case 1: // UP
                this.y = this.y + 1;
            break;

            case 2: //RIGHT
                this.x = this.x + 1;
            break;

            case 3: // DOWN
                this.y = this.y - 1;
            break;

            case 4: //LEFT
                this.x = this.x - 1;
            break;
        }
    }
}
