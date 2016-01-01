using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum state { walking, groundFalling, falling, jumping }

public class GameController : MonoBehaviour {
    public GameObject goPlayer, goSpike, goSpikeDouble, goGoal, goTile;
    private int plx, ply;
    int steps = 4, stepsLeft = 4;
    state st = state.walking;
    float time = 0.5f, timeDelay = 0.5f;
    bool groupsCreated = false;

    private List<Group> grps;
    bool[,] grped;

    int[,] arr = 
        new int[,] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                     { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                     { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                     { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                     { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0 },
                     { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                     { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                     { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                     { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                     { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                     { 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0 },
                     { 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0 }};
    
    GameObject[,] arrGo = new GameObject[12, 16];
    
    void Start () {
	    for(int y = 0; y < 12; y++)
        {
            for(int x = 0; x < 16; x++)
            {
                Vector3 pos = new Vector3(x, 11 - y, 0);
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
        switch (st)
        {
            case state.walking:
                Walking();
                break;
            case state.falling:
                Falling();
                break;
            case state.groundFalling:
                FallingGround();
                break;
            case state.jumping:
                Jumping();
                break;
        }
    }

    void Jumping()
    {
        st = state.walking;
    }

    void createGroups()
    {
        grps.Clear();
        grped = new bool[12, 16];

        for (int y = 0; y < 12; y++)
        {
            for (int x = 0; x < 16; x++)
            {
                if(arrGo[y, x].tag == "tile" && !grped[y, x])
                {
                    Group g = new Group();
                    grps.Add(g);
                    addToGroup(g, x, y);
                }
            }
        }
    }

    void addToGroup(Group g, int x, int y)
    {
        g.all.Add(new Coord(x,y));
        grped[y, x] = true;

        if (11 - y == 0) {
            g.stuck = true;
        }
        else if (arrGo[y - 1, x].tag == "tile" && !grped[y - 1, x])
        {
            addToGroup(g, x, y - 1);
        }
        else if(arrGo[y - 1, x].tag != "tile")
        {
            g.beneath.Add(new Coord(x, y));
        }

        if (y+1 < 12 && arrGo[y + 1, x].tag == "tile" && !grped[y + 1, x])
        {
            addToGroup(g, x, y + 1);
        }

        if (x + 1 < 16 && arrGo[y, x + 1].tag == "tile" && !grped[y, x + 1])
        {
            addToGroup(g, x + 1, y);
        }

        if (x - 1 > -1 && arrGo[y, x - 1].tag == "tile" && !grped[y, x - 1])
        {
            addToGroup(g, x - 1, y);
        }
    }


    void FallingGround()
    {
        if (!groupsCreated)
        {
            createGroups();
            groupsCreated = true;
        }
    }

    
    void Falling()
    {
        time -= Time.deltaTime;
        if(time < 0)
        {
            time = timeDelay;
            if (ply - 1 < 0) { 
                st = state.jumping;
                return;
            }

            GameObject cur = arrGo[ply - 1, plx];

            if (cur != null)
            {
                if (cur.tag == "tile")
                {
                    st = state.jumping;
                    return;
                }

                if (cur.tag == "goal")
                {
                    Debug.Log("Success");
                }
            }

            GameObject pl = arrGo[ply, plx];
            pl.transform.Translate(0, -1, 0);
            arrGo[ply, plx] = null;
            ply--;
            arrGo[ply, plx] = pl;
        }
    }

    void Walking()
    {
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

            if (cur.tag == "goal") {
                Debug.Log("Success");
            }
        }

        GameObject pl = arrGo[ply, plx];
        pl.transform.Translate(dx,dy,0);
        arrGo[ply, plx] = Instantiate(goTile, new Vector3(plx, ply), Quaternion.identity) as GameObject;
        plx += dx;
        ply += dy;
        arrGo[ply, plx] = pl;
        stepsLeft--;

        if (stepsLeft == 0)
        {
            st = state.groundFalling;
            stepsLeft = steps;
        }
    }
}
