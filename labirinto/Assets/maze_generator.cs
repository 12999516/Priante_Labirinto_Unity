using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class maze_generator : MonoBehaviour
{
    [SerializeField]
    private plain entrata_uscita_prefab;

    [SerializeField]
    private cella maze_Cell_prefab;

    [SerializeField]
    private int maze_width;

    [SerializeField]
    private int maze_depth;

    [SerializeField]
    GameObject player;

    private cella[,] maze_grid;
    private cella cellafine;
    int livello;
    plain ingresso;
    plain uscita;
    int current_maze_width;
    int current_maze_depth;
    cella cellainizio;

    void Start()
    {
        livello = 0;
        inizializza_current_dimensions();
        prepara_campo();
        player_respawn();
    }

    void inizializza_current_dimensions()
    {
        current_maze_width = maze_width + livello * 5;
        current_maze_depth = maze_depth + livello * 5;
    }

    void prepara_campo()
    {
        maze_grid = new cella[current_maze_width, current_maze_depth];

        for (int x = 0; x < current_maze_width; x++)
        {
            for (int z = 0; z < current_maze_depth; z++)
            {
                maze_grid[x, z] = Instantiate(maze_Cell_prefab, new Vector3(x, 0, z), Quaternion.identity);
            }
        }

        GenerateMaze(null, maze_grid[0, 0]);
        cellafine = maze_grid[current_maze_width - 1, current_maze_depth - 1];
        cellainizio = maze_grid[0, 0];

        cellainizio.clearbackwall(); // ingresso
        cellafine.clearfrontwall(); // uscita

        creapp();
    }

    void creapp()
    {
        ingresso = Instantiate(entrata_uscita_prefab, cellainizio.transform.position + Vector3.up * 0.05f, Quaternion.identity);
        uscita = Instantiate(entrata_uscita_prefab, cellafine.transform.position + Vector3.up * 0.05f, Quaternion.identity);
        ingresso.transform.position += Vector3.back * 3;
        uscita.transform.position += Vector3.forward * 3f;
        uscita.transform.rotation = Quaternion.Euler(0, 180, 0);
        uscita.player_uscito += onplayeruscito;
    }

    void onplayeruscito(object sender, EventArgs e)
    {
        distruggi();
        livello++;
        inizializza_current_dimensions();
        prepara_campo();
        player_respawn();
    }

    void distruggi()
    {
        for (int x = 0; x < current_maze_width; x++)
        {
            for (int z = 0; z < current_maze_depth; z++)
            {
                Destroy(maze_grid[x, z].gameObject);
            }
        }
        Destroy(ingresso.gameObject);
        Destroy(uscita);
    }

    void player_respawn()
    {
        CharacterController character_controller = player.GetComponent<CharacterController>();

        character_controller.enabled = false;
        player.transform.position = ingresso.transform.position;
        player.transform.position += Vector3.up * 0.5f;
        player.transform.position += Vector3.back * 1.5f;
        character_controller.enabled = true;

    }

    private void GenerateMaze(cella previouscell, cella currencell)
    {
        currencell.visit();
        clearwall(previouscell, currencell);

        cella nextcell;

        do
        {
            nextcell = getnextunvisitedcell(currencell);

            if (nextcell != null)
            {
                GenerateMaze(currencell, nextcell);
            }
        } while (nextcell != null);
    }

    private cella getnextunvisitedcell(cella currentcell)
    {
        var unvisitedcells = getunvisitedcells(currentcell);

        return unvisitedcells.OrderBy(_ => UnityEngine.Random.Range(1, 10)).FirstOrDefault();
    }

    private IEnumerable<cella> getunvisitedcells(cella currentcell)
    {
        int x = (int)currentcell.transform.position.x;
        int z = (int)currentcell.transform.position.z;

        if (x + 1 < current_maze_width)
        {
            var celltoright = maze_grid[x + 1, z];

            if (!celltoright.visitato)
            {
                yield return celltoright;
            }
        }

        if (x - 1 >= 0)
        {
            var celltoleft = maze_grid[x - 1, z];
            if (!celltoleft.visitato)
            {
                yield return celltoleft;
            }
        }

        if (z + 1 < current_maze_depth)
        {
            var celltofront = maze_grid[x, z + 1];
            if (!celltofront.visitato)
            {
                yield return celltofront;
            }
        }

        if (z - 1 >= 0)
        {
            var celltoback = maze_grid[x, z - 1];
            if (!celltoback.visitato)
            {
                yield return celltoback;
            }
        }
    }

    private void clearwall(cella previouscell, cella currentcell)
    {
        if (previouscell == null)
        {
            return;
        }

        if (previouscell.transform.position.x < currentcell.transform.position.x)
        {
            previouscell.clearrightwall();
            currentcell.clearleftwall();
            return;
        }

        if (previouscell.transform.position.x > currentcell.transform.position.x)
        {
            previouscell.clearleftwall();
            currentcell.clearrightwall();
            return;
        }

        if (previouscell.transform.position.z < currentcell.transform.position.z)
        {
            previouscell.clearfrontwall();
            currentcell.clearbackwall();
            return;
        }

        if (previouscell.transform.position.z > currentcell.transform.position.z)
        {
            previouscell.clearbackwall();
            currentcell.clearfrontwall();
            return;
        }
    }

    void Update()
    {

    }
}

/*using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class maze_generator : MonoBehaviour
{

    [SerializeField]
    private cella maze_Cell_prefab;

    [SerializeField]
    private int maze_width;

    [SerializeField]
    private int maze_depth;

    private cella[,] maze_grid;

    private cella cellainizio;
    private cella cellafine;

    void Start()
    {
        maze_grid = new cella[maze_width, maze_depth];
        cellafine = maze_grid[maze_width - 1, maze_depth - 1];
        cellainizio = maze_grid[0, 0];

        for (int x = 0; x < maze_width; x++)
        {
            for (int z = 0; z < maze_depth; z++)
            {
                maze_grid[x, z] = Instantiate(maze_Cell_prefab, new Vector3(x, 0, z), Quaternion.identity);
            }
        }

        GenerateMaze(null, maze_grid[0, 0]);

        // APERTURA INGRESSO E USCITA DEL LABIRINTO
        maze_grid[0, 0].clearbackwall(); // ingresso
        maze_grid[maze_width - 1, maze_depth - 1].clearfrontwall(); // uscita
    }

    private void GenerateMaze(cella previouscell, cella currencell)
    {
        currencell.visit();
        clearwall(previouscell, currencell);

        cella nextcell;

        do
        {
            nextcell = getnextunvisitedcell(currencell);

            if (nextcell != null)
            {
                GenerateMaze(currencell, nextcell);
            }
        } while (nextcell != null);
    }

    private cella getnextunvisitedcell(cella currentcell)
    {
        var unvisitedcells = getunvisitedcells(currentcell);

        return unvisitedcells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }

    private IEnumerable<cella> getunvisitedcells(cella currentcell)
    {
        int x = (int)currentcell.transform.position.x;
        int z = (int)currentcell.transform.position.z;

        if (x + 1 < maze_width)
        {
            var celltoright = maze_grid[x + 1, z];

            if (!celltoright.visitato)
            {
                yield return celltoright;
            }
        }

        if (x - 1 >= 0)
        {
            var celltoleft = maze_grid[x - 1, z];
            if (!celltoleft.visitato)
            {
                yield return celltoleft;
            }
        }

        if (z + 1 < maze_depth)
        {
            var celltofront = maze_grid[x, z + 1];
            if (!celltofront.visitato)
            {
                yield return celltofront;
            }
        }

        if (z - 1 >= 0)
        {
            var celltoback = maze_grid[x, z - 1];
            if (!celltoback.visitato)
            {
                yield return celltoback;
            }
        }
    }

    private void clearwall(cella previouscell, cella currentcell)
    {
        if (previouscell == null)
        {
            return;
        }

        if (previouscell.transform.position.x < currentcell.transform.position.x)
        {
            previouscell.clearrightwall();
            currentcell.clearleftwall();
            return;
        }

        if (previouscell.transform.position.x > currentcell.transform.position.x)
        {
            previouscell.clearleftwall();
            currentcell.clearrightwall();
            return;
        }

        if (previouscell.transform.position.z < currentcell.transform.position.z)
        {
            previouscell.clearfrontwall();
            currentcell.clearbackwall();
            return;
        }

        if (previouscell.transform.position.z > currentcell.transform.position.z)
        {
            previouscell.clearbackwall();
            currentcell.clearfrontwall();
            return;
        }
    }

    void Update()
    {

    }
}*/