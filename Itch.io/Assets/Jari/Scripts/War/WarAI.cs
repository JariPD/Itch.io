using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class WarAI : MonoBehaviour
{
    [Header("AI")]
    public Transform[] opponentHandSpawnPos;
    public GameObject[] enemyGrid;
    public List<GameObject> opponentsHand;

    public IEnumerator AICardPlacement()
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < opponentsHand.Count; i++)
        {
            //gets random tile from the grid
            int randomTile = Random.Range(0, enemyGrid.Length);

            yield return new WaitForSeconds(.2f);

            //check if tile is empty
            if (!enemyGrid[randomTile].GetComponent<OpponentWarTile>().HasCard)
            {
                //places cards on a random tile on the enemy grid
                opponentsHand[i].transform.position = new Vector3(enemyGrid[randomTile].transform.position.x, .1f, enemyGrid[randomTile].transform.position.z);
                enemyGrid[randomTile].GetComponent<OpponentWarTile>().HasCard = true;
            }
            else
            {
                //if the tile has a card on it, it will check the next tile
                randomTile++;
                
                if (opponentsHand[i] != null)
                    opponentsHand[i].transform.position = new Vector3(enemyGrid[randomTile].transform.position.x, .1f, enemyGrid[randomTile].transform.position.z);
            }
        }
    }
}
