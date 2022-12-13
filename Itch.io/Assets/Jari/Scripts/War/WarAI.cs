using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarAI : MonoBehaviour
{
    public static WarAI instance;

    [Header("AI")]
    public Transform[] opponentHandSpawnPos;
    public GameObject[] enemyGrid;
    public List<GameObject> opponentsHand;
    private void Awake()
    {
        instance = this;    
    }

    public IEnumerator AICardPlacement()
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < opponentsHand.Count; i++)
        {
            //gets random tile from the grid
            var randomTile = Random.Range(0, enemyGrid.Length);

            yield return new WaitForSeconds(.2f);

            //check if tile is empty
            if (!enemyGrid[randomTile].GetComponent<OpponentWarTile>().HasCard)
            {
                //places cards on a random tile on the enemy grid
                opponentsHand[i].transform.position = new Vector3(enemyGrid[randomTile].transform.position.x, enemyGrid[randomTile].transform.position.y + .1f, enemyGrid[randomTile].transform.position.z);
            }
            else if (enemyGrid[randomTile].GetComponent<OpponentWarTile>().HasCard)
            {
                //gets another random tile if the tile is occupied
                randomTile = Random.Range(0, enemyGrid.Length);

                if (opponentsHand[i] != null && !enemyGrid[randomTile].GetComponent<OpponentWarTile>().HasCard)
                    opponentsHand[i].transform.position = new Vector3(enemyGrid[randomTile].transform.position.x, enemyGrid[randomTile].transform.position.y + .1f, enemyGrid[randomTile].transform.position.z);
            }
        }
    }
}
