using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid : MonoBehaviour
{
    private int size = 2, point_counter = 0;
    public float line_width = 0.1f;
    private float x_tiles_finder = -0.5f, y_tiles_finder = 0.5f, time_counter;
    private int tiles_counter = 0;
    private bool first_section = true;

    public GameObject prefab_UP, prefab_DOWN, prefab_LEFT, prefab_RIGHT, tiles_finder;
    public Material line_material;
    private Vector3 move_up = new Vector3(0, 1, 0), move_down = new Vector3(0, -1, 0), move_right = new Vector3(1, 0, 0), move_left = new Vector3(-1, 0, 0), remove_tile_finder = new Vector3(0, 0, 9);
    public Color bound_color = Color.black;
    private Dictionary<Vector3, GameObject> tiles_position_dictionary = new Dictionary<Vector3, GameObject>();

    public bool debug_var = false;

    // Start is called before the first frame update
    void Start()
    {
        this.size = 0;
        time_counter = 0;
    }

    void Update()
    {
        time_counter += Time.deltaTime;

        if (Input.GetKey("space") && time_counter > 0)
        // if (time_counter > 0)
        {

                if(first_section){
                    if(debug_var){print("START - Increase size");}
                    this.increaseSize();
                    if(debug_var){print("END - Increase size\n");}

                    // if(debug_var){print("START - Draw grid");}
                    // this.drawGrid();
                    // if(debug_var){print("END - Draw grid");}

                    if(debug_var){print("START - Delete colliding tiles and move tiles");}
                    this.deleteCollidingTilesAndmoveTiles();
                    if(debug_var){print("END - Delete colliding tiles and move tiles\n");}
                } else {
                    if(debug_var){print("START - Fill void");}
                    this.updateTilesPositionDictionary();
                    this.fillVoidWithTiles();
                    if(debug_var){print("END - Fill void\n");}

                    time_counter = 0;
                }

                first_section = !first_section;
        }
    }

    //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Gird update related methods

    // Increase the size of the grid and move the camera
    void increaseSize()
    {
        // Increase size
        this.size = this.size + 2;

        // Check the camera height
        Camera cam = Camera.main;
        float cam_height = cam.orthographicSize;

        // If the grid is bigger than the camera enlarge the camera and thick the line
        if((size/2 - 1) >= cam_height){
            // cam.orthographicSize = size * 1.5f;
            cam.orthographicSize = size / 2;

            line_width = line_width * 2;
        }

        if(debug_var){ print("\tSize = " + size);}
    }

    void deleteCollidingTilesAndmoveTiles()
    {
        // Temporary GameObject variable for the current tile
        GameObject tile_object;

        // Iterate through tiles
        foreach(KeyValuePair<Vector3, GameObject> entry in tiles_position_dictionary){
            // Retrieve the tile
            tile_object = entry.Value;

            // Check if there's a collision
            if(!checkCollision(tile_object)){
                // If no collision is found move the tile
                if(tile_object.transform.name.Contains("UP")){
                    tile_object.transform.position = tile_object.transform.position + move_up;
                } else if(tile_object.transform.name.Contains("DOWN")){
                    tile_object.transform.position = tile_object.transform.position + move_down;
                } else if(tile_object.transform.name.Contains("LEFT")){
                    tile_object.transform.position = tile_object.transform.position + move_left;
                } else if(tile_object.transform.name.Contains("RIGHT")){
                    tile_object.transform.position = tile_object.transform.position + move_right;
                }
            }
        }

        // Update tiles position dictionary
        this.updateTilesPositionDictionary();
    }

    // Check if the tiles collide woth another tile moving in its direction and eventually destroy the tiles couple
    bool checkCollision(GameObject tile)
    {
        Vector3 tile_position = tile.transform.position, key;
        GameObject tmp_object;
        if(tile.transform.name.Contains("UP")){
            key = tile_position + move_up;
            if(tiles_position_dictionary.TryGetValue(key, out tmp_object)){
                Destroy(tile);
                Destroy(tiles_position_dictionary[key]);
                //tiles_position_dictionary.Remove(key);
                return true;
            }
        } else if(tile.transform.name.Contains("DOWN")){
            key = tile_position + move_down;
            if(tiles_position_dictionary.TryGetValue(key, out tmp_object)){
                Destroy(tile);
                Destroy(tiles_position_dictionary[key]);
                //tiles_position_dictionary.Remove(key);
                return true;
            }
        } else if(tile.transform.name.Contains("LEFT")){
            key = tile_position + move_left;
            if(tiles_position_dictionary.TryGetValue(key, out tmp_object)){
                Destroy(tile);
                Destroy(tiles_position_dictionary[key]);
                //tiles_position_dictionary.Remove(key);
                return true;
            }
        } else if(tile.transform.name.Contains("RIGHT")){
            key = tile_position + move_right;
            if(tiles_position_dictionary.TryGetValue(key, out tmp_object)){
                Destroy(tile);
                Destroy(tiles_position_dictionary[key]);
                //tiles_position_dictionary.Remove(key);
                return true;
            }
        }

        return false;
    }

    void updateTilesPositionDictionary(){
        // Clean all dictionary
        tiles_position_dictionary.Clear();
        // tiles_position_dictionary = new Dictionary<Vector3, GameObject>();

        // Temporary variable
        GameObject tiles_container = GameObject.Find("Tiles Container"), tmp_tiles;
        string tmp_tiles_name;

        // Iterate thorugh tiles
        foreach(Transform child in tiles_container.transform){
            tmp_tiles_name = child.transform.name;
            // tmp_tiles = GameObject.Find(tmp_tiles_name);
            tmp_tiles = child.gameObject;

            tiles_position_dictionary.Add(tmp_tiles.transform.position, tmp_tiles);
        }

    }

    void fillVoidWithTiles()
    {
        // N.B size is the length of the entire side of the square

        if(size == 2){
            // Spawn the first couple
            this.spawTilesCouple(new Vector3(-0.5f, 0.5f, 0));
        } else {
            // Iteration after the first. Reference corner as the upper left.

            // Set initial position for the tiles finder
            x_tiles_finder = -(this.size/2f - 0.5f);
            y_tiles_finder = this.size/2f - 0.5f;
            Vector3 upper_left_corner = new Vector3(x_tiles_finder, y_tiles_finder, 0);
            tiles_finder.transform.position = upper_left_corner;
            if(debug_var) {print("\t x_tiles_finder = " + x_tiles_finder + " - y_tiles_finder = " + y_tiles_finder);}

            // Variable to chcek the position of the tiles collider
            bool condition_diag_1, condition_diag_2;

            for(int i = 0; i < size; i++){
                for(int j = 0; j < size; j++){
                    // Move one step right
                    tiles_finder.transform.position = upper_left_corner + new Vector3(j, -i, 0);

                    //Variable to check the position (Check the border of the square is useless)
                    if(i < size / 2){
                        // Upper part - Check only the position UNDER the diagonals
                        condition_diag_1 = tiles_finder.transform.position.y <= (tiles_finder.transform.position.x + this.size/2);
                        condition_diag_2 = tiles_finder.transform.position.y <= (-tiles_finder.transform.position.x + this.size/2);
                    } else {
                        // Lower part - Check only the position ABOVE the diagonals
                        condition_diag_1 = tiles_finder.transform.position.y >= (-tiles_finder.transform.position.x - this.size/2);
                        condition_diag_2 = tiles_finder.transform.position.y >= (tiles_finder.transform.position.x - this.size/2);
                    }

                    // Enter only if is in the aztec diamond
                    if(condition_diag_1 && condition_diag_2){
                        //Check if touching something
                        if(checkSurroundingPositionWithDict()){
                            //If is not touch something and there's enough space spawn new block
                            spawTilesCouple(tiles_finder.transform.position);

                            // Skip the next tiles since it's already filled
                            j++;
                        }
                    }
                }
            } // END CYCLE to checl tiles
        } // END IF
    }

    bool checkSurroundingPositionWithDict(){
        Vector3 key, position;
        GameObject tmp_object;

        // UP - DOWN Couple
        position = tiles_finder.transform.position;
        key = position + new Vector3(0.5f, 0, 0);
        if(tiles_position_dictionary.TryGetValue(key, out tmp_object)){ return false; }
        key = position - new Vector3(0.5f, 0, 0);
        if(tiles_position_dictionary.TryGetValue(key, out tmp_object)){ return false; }

        // LEFT - RIGHT Couple
        position = tiles_finder.transform.position;;
        key = position + new Vector3(0, 0.5f, 0);
        if(tiles_position_dictionary.TryGetValue(key, out tmp_object)){ return false; }
        key = position - new Vector3(0, 0.5f, 0);
        if(tiles_position_dictionary.TryGetValue(key, out tmp_object)){ return false; }


        return true;
    }

    // Spawn a couple of tiles. The position is consider as the upper left corner of a square
    void spawTilesCouple(Vector3 position)
    {
        GameObject tile_1, tile_2;
        int type = Random.Range(0, 10);

        if(type % 2 == 0){
            // Spawn tiles (ORIZONTAL)
            tile_1 = Instantiate(prefab_UP, position + new Vector3(0.5f, 0, 0), Quaternion.identity);
            tile_2 = Instantiate(prefab_DOWN, position + new Vector3(0.5f, -1, 0), Quaternion.identity);
        } else {
            // Spawn tiles (VERTICAL)
            tile_1 = Instantiate(prefab_LEFT, position + new Vector3(0, -0.5f, 0), Quaternion.identity);
            tile_2 = Instantiate(prefab_RIGHT, position + new Vector3(1, -0.5f, 0), Quaternion.identity);
        }

        // Change names
        tile_1.transform.name = tile_1.transform.name + " " + tiles_counter;
        tile_2.transform.name = tile_2.transform.name + " " + (tiles_counter + 1);
        tiles_counter = tiles_counter + 2;

        // Put tiles in the container
        tile_1.transform.parent = GameObject.Find("Tiles Container").transform;
        tile_2.transform.parent = GameObject.Find("Tiles Container").transform;

        // Add tiles to the Dictionary
        tiles_position_dictionary.Add(tile_1.transform.position, tile_1);
        tiles_position_dictionary.Add(tile_2.transform.position, tile_2);
    }


    //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Drawing related methods

    // Draw the bound of the grid
    void drawGrid()
    {
        // Check if a line drawer exist and eventually destroy him
        GameObject myLine = GameObject.Find("Line Drawer");
        if(myLine){
            Destroy (myLine);
        }

        // Create a new line drawer and attach the lineRenderer component
        myLine = new GameObject();
        myLine.transform.position = new Vector3(0, this.size/2, 0);
        myLine.name = "Line Drawer";
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();

        // Set line material
        lr.material = line_material;

        // Set line width
        lr.startWidth = this.line_width;
        lr.endWidth = this.line_width;

        // Set number of segments to use
        lr.positionCount = this.size * 4 + 1;

        // Point to draw the line and counter of points
        Vector3 point_1, point_2;
        point_counter = 0;

        //1 - Draw  upper left section
        point_1 = new Vector3(0, this.size/2, 0);
        point_2 = new Vector3(1, this.size/2, 0);
        this.drawSection(point_1, point_2, move_right, move_down, lr);

        //2 - Draw  lower left section
        point_1 = new Vector3(this.size/2, 0, 0);
        point_2 = new Vector3(this.size/2, -1, 0);
        this.drawSection(point_1, point_2, move_down, move_left, lr);

        //Draw  lower right section
        point_1 = new Vector3(0, -this.size/2, 0);
        point_2 = new Vector3(-1, -this.size/2, 0);
        this.drawSection(point_1, point_2, move_left, move_up, lr);

        //Draw  upper right section
        point_1 = new Vector3(-this.size/2, 0, 0);
        point_2 = new Vector3(-this.size/2, 1, 0);
        this.drawSection(point_1, point_2, move_up, move_right, lr);

        // Set color
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(bound_color, 0.0f), new GradientColorKey(bound_color, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        lr.colorGradient = gradient;
    }

    void drawSection(Vector3 point_1, Vector3 point_2, Vector3 direction_1, Vector3 direction_2, LineRenderer lr)
    {
        for(int i = 0; i < this.size; i ++){
            // Draw segment
            lr.SetPosition(point_counter, point_1);
            lr.SetPosition(point_counter + 1, point_2);
            point_counter++;

            //Update points values
            if(i % 2 == 0){
                point_1 = point_1 + direction_1;
                point_2 = point_2 + direction_2;
            } else {
                point_1 = point_1 + direction_2;
                point_2 = point_2 + direction_1;
            }
        }
    }


}
