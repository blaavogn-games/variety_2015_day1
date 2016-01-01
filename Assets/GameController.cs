using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
    public GameObject goPlayer, goSpike, goSpikeDouble, goGoal, goTile;
    private int plx, ply;

    int[,] arr = 
        new int[,] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                     { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                     { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                     { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                     { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0 },
                     { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                     { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                     { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                     { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                     { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                     { 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0 },
                     { 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0 }};

    GameObject[,] arrGo = new GameObject[12, 16];

    void Start () {
	    for(int y = 0; y < 12; y++)
        {
            for(int x = 0; x < 16; x++)
            {
                Vector3 pos = new Vector3(x, 11 - y, 0);
                Debug.Log(x + " - " + y);
                GameObject g = null;
                switch (arr[y,x])
                {
                    case 1:
                        g = Instantiate(goPlayer, pos, Quaternion.identity) as GameObject;
                        plx = x;
                        ply = 11 - y;
                        break;
                    case 2:
                        g = Instantiate(goSpike, pos, Quaternion.identity) as GameObject;
                        break;
                    case 3:
                        g = Instantiate(goSpikeDouble, pos, Quaternion.identity) as GameObject;
                        break;
                    case 4:
                        g = Instantiate(goGoal, pos, Quaternion.identity) as GameObject;
                        break;
                    case 5:
                        g = Instantiate(goTile, pos, Quaternion.identity) as GameObject;
                        break;
                }
                arrGo[11 - y, x] = g;
            }
        }
	}
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            move(-1, 0);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            move(1, 0);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            move(0, 1);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            move(0, -1);
        }
    }

    void move(int dx, int dy)
    {
        //Border check
        if (dx + plx < 0 || dx + plx > 15 || dy + ply < 0 || dy + ply > 11) { 
            return;
        }
        GameObject cur = arrGo[ply + dy, plx + dx];

        if (cur != null)
        {
            if (cur.tag == "tile")
                return;
        }

        GameObject pl = arrGo[ply, plx];
        pl.transform.Translate(dx,dy,0);
        arrGo[ply, plx] = Instantiate(goTile, new Vector3(plx, ply), Quaternion.identity) as GameObject;
        plx += dx;
        ply += dy;
        Debug.Log(ply);
        arrGo[ply, plx] = pl;
    }
}
