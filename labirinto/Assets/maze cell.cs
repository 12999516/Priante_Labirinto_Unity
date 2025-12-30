using UnityEngine;

public class cella: MonoBehaviour
{
    [SerializeField]
    private GameObject left_wall;

    [SerializeField]
    private GameObject right_wall;

    [SerializeField]
    private GameObject front_wall;

    [SerializeField]
    private GameObject back_wall;

    [SerializeField]
    private GameObject unvisited_block;

    public bool visitato { get; private set; }

    public void visit()
    {
        visitato = true;
        unvisited_block.SetActive(false);
    }

    public void clearleftwall()
    {
        left_wall.SetActive(false);
    }

    public void clearrightwall()
    {
        right_wall.SetActive(false);
    }

    public void clearfrontwall()
    {
        front_wall.SetActive(false);
    }

    public void clearbackwall()
    {
        back_wall.SetActive(false);
    }

    /*public void blocca_muro_destra()
    {
        muro_destra.SetActive(true);
    }

    public void blocca_muro_sinistra()
    {
        muro_sinistra.SetActive(true);
    }

    public void blocca_muro_frontale()
    {
        muro_fronte.SetActive(true);
    }

    public void blocca_muro_dietro()
    {
        muro_dietro.SetActive(true);
    }*/
}
