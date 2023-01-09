using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class WarManager : MonoBehaviour
{
    public static WarManager instance;

    [Header("References")]
    public AudioManager audioManager;
    [SerializeField] private Transform[] cardSpawnPos;                   //spawn positions for cards
    [SerializeField] private GameObject card, opponentCard;              //cards prefabs
    [SerializeField] private GameObject[] allObjectsInScene;
    private WarOpponentCardPlace warAI;

    [Header("Card Placement")]
    public List<GameObject> PlayerCardsInField = new(); //all playercards in field
    public List<GameObject> enemyCardsInField = new();  //all enemy cards in field
    public GameObject CurrentSelectedCard;                               //reference to the card that is currently selected
    public bool PlacingCard = false;                                     //checks if the card is being placed
    public bool CardSelected = false;                                    //checks if the card is selected
    public int placeToSpawn;
    public int diceRoll;

    private bool winCoroutine = true;
    private bool isPlayerTurn = true;
    private int turnCount = 0;
    private int count = 0;

    [Header("Battling")]
    public List<GameObject> playersHand;               //players hand - used to keep track of the cards in the players hand
    public int playerHealth, opponentHealth;                             //health of the player and opponent
    [SerializeField] private Button attackButton;
    private readonly int maxPlayerHealth = 20, maxOpponentHealth = 20;   //max health of the player and opponent

    [Header("Movement")]
    [SerializeField] private GameObject vCamOne;
    [SerializeField] private Transform MainCam;
    [SerializeField] private Transform vCamTwo;

    private void Awake()
    {
        //singleton
        instance = this;

        //gets components
        warAI = GetComponent<WarOpponentCardPlace>();
        //checkForCardsOnField = GetComponent<CheckForCardsOnField>();

        //sets health
        playerHealth = maxPlayerHealth;
        opponentHealth = maxOpponentHealth;

        UIManager.instance.UpdateWarHealth(playerHealth, opponentHealth);

        //play ambience
        audioManager.Play("Ambience");
    }

    private void Update()
    {
        //move camera above playing field when placing card
        if (CardSelected)
            StartCoroutine(SeeCards());
        else
        {
            if (MainCam.position == vCamTwo.position)
                vCamOne.SetActive(true);
        }

        if (CurrentSelectedCard == null)
            CardSelected = false;

        //health check
        if (opponentHealth <= 0)
        {
            opponentHealth = 0;

            if (winCoroutine)
                StartCoroutine(WonGame());
        }
        if (playerHealth <= 0)
        {
            playerHealth = 0;
            //play fade animation
            SceneManager.LoadScene(0);
        }
    }

    /// <summary>
    /// function to be called from a button - starts the dice throw
    /// </summary>
    public void StartDiceThrow()
    {
        //gridParent.SetActive(true);
        StartCoroutine(ThrowDice(true));
    }

    IEnumerator ThrowDice(bool placeCards)
    {
        turnCount++;
        count++;

        //get random dice roll to spawn cards
        diceRoll = count <= 2 ? Random.Range(2, 3) : 1;

        //------------------------------------------------------------------- player dice logic -------------------------------------------------------------------\\

        if (isPlayerTurn && playersHand.Count < 6 && winCoroutine) //check if it is the players turn and the player does not have a full hand
        {
            //loop trough the dice roll and give cards to the player
            for (int i = 0; i < diceRoll; i++)
            {
                if (diceRoll >= 2)
                {
                    //if diceroll is 2 or more spawn cards based on count
                    placeToSpawn = playersHand.Count;
                }
                else
                {
                    for (int a = 0; a < cardSpawnPos.Length; a++) //loops trough the possible spawn positions
                    {
                        if (!cardSpawnPos[a].GetComponent<CardCheck>().HasCard) //check if place to spawn does not already have a card
                        {
                            //sets spawn pos to current pos in the loop
                            placeToSpawn = a;
                            break;
                        }
                    }
                }

                //instantiates players cards
                playersHand.Add(Instantiate(card, new Vector3(cardSpawnPos[placeToSpawn].position.x, cardSpawnPos[placeToSpawn].position.y - .2f, cardSpawnPos[placeToSpawn].position.z), cardSpawnPos[placeToSpawn].rotation));
            }

            //updates the dice roll text
            UIManager.instance.UpdateDiceRollText(diceRoll, isPlayerTurn);

            //updates throw dice button
            UIManager.instance.DisableThrowDiceButton();
        }
        else if (!isPlayerTurn)  //-------------------------------------------- opponent dice logic ---------------------------------------------\\
        {
            if (enemyCardsInField.Count != 4 && winCoroutine)
            {
                //loop trough the dice roll and give cards to the opponent
                for (int i = 0; i < diceRoll; i++)
                    warAI.opponentsHand.Add(Instantiate(opponentCard, new Vector3(warAI.opponentHandSpawnPos[i].position.x, warAI.opponentHandSpawnPos[i].position.y - .2f, warAI.opponentHandSpawnPos[i].position.z), warAI.opponentHandSpawnPos[i].rotation));

                //updates the dice roll text
                UIManager.instance.UpdateDiceRollText(diceRoll, isPlayerTurn);

                //places oppenent cards
                if (placeCards)
                    StartCoroutine(warAI.AICardPlacement());
            }
        }

        yield return new WaitForSeconds(3);

        isPlayerTurn = false;

        if (turnCount == 2)
            yield return null;
        else
            StartCoroutine(ThrowDice(true));
    }

    #region Battle

    /// <summary>
    /// function that moves the camera above the playing field
    /// </summary>
    /// <returns></returns>
    private IEnumerator SeeCards()
    {
        if (vCamOne.activeInHierarchy)
        {
            if (MainCam.position == vCamOne.transform.position)
                vCamOne.SetActive(false);
        }

        yield return new WaitForSeconds(.1f);

        if (!CardSelected)
        {
            if (MainCam.position == vCamTwo.position)
                vCamOne.SetActive(true);
        }
    }

    /// <summary>
    /// deactivate button with settings
    /// </summary>
    /// <param name="timeDeactive"></param>
    public void Deactivate(float timeDeactive)
    {
        StartCoroutine(UnActive(timeDeactive));
    }

    /// <summary>
    /// deactivate button
    /// </summary>
    /// <param name="timeDeactive"></param>
    /// <returns></returns>
    public IEnumerator UnActive(float timeDeactive)
    {
        attackButton.interactable = false;
        yield return new WaitForSeconds(timeDeactive);
        attackButton.interactable = true;
    }

    /// <summary>
    /// only be able to attack if there are cards on the table from both sides
    /// </summary>
    public void AttackTurn()
    {
        if (PlayerCardsInField.Count > 0 || enemyCardsInField.Count > 0)
            StartCoroutine(TradingAttack());
    }

    /// <summary>
    /// first player can attack then opponent can attack
    /// </summary>
    /// <returns></returns>
    private IEnumerator TradingAttack()
    {
        bool usable = true;
        if (usable)
        {
            for (int i = 0; i < PlayerCardsInField.Count; i++)
            {
                //StartCoroutine(AttackInterval(PlayerCardsInField));
                PlayerCardsInField[i].GetComponent<PlayerCard>().AttackForward();
            }
        }

        //update player and enemy UI 
        UIManager.instance.UpdateWarHealth(playerHealth, opponentHealth);

        yield return new WaitForSeconds(2);

        usable = true;
        if (usable && winCoroutine)
        {
            for (int i = 0; i < enemyCardsInField.Count; i++)
                enemyCardsInField[i].GetComponent<OpponentCard>().AttackForward();
        }

        //update player and enemy UI 
        UIManager.instance.UpdateWarHealth(playerHealth, opponentHealth);

        if (PlayerCardsInField.Count < 4)
        {
            isPlayerTurn = true;
            turnCount = 0;
            StartCoroutine(ThrowDice(false));
        }
        else if (enemyCardsInField.Count < 4)
        {
            isPlayerTurn = false;
            turnCount = 1;
            StartCoroutine(ThrowDice(true));
        }
    }

    #endregion

    /// <summary>
    /// function that places a selected card on a tile in the grid
    /// </summary>
    /// <param name="pos"></param>
    public void PlacePlayerCard(Vector3 pos)
    {
        if (CardSelected)
        {
            if (pos != null)
            {
                audioManager.Play("CardDrop");
                PlacingCard = true;
                CurrentSelectedCard.transform.position = pos;
                CurrentSelectedCard.GetComponent<BoxCollider>().enabled = true;
                CurrentSelectedCard = null;
            }
        }
    }

    public void PlayerTileHover(Vector3 newPos)
    {
        if (CurrentSelectedCard != null)
        {
            audioManager.Play("CardHover");
            CurrentSelectedCard.transform.position = newPos;
            CurrentSelectedCard.GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void PlaySound(string name)
    {
        audioManager.Play(name);
    }

    private IEnumerator WonGame()
    {
        winCoroutine = false;
        //do animation or transition

        yield return new WaitForSeconds(5);

        for (int i = 0; i < playersHand.Count; i++)
            playersHand[i].GetComponent<PlayerCard>().health = 0;

        for (int i = 0; i < PlayerCardsInField.Count; i++)
            PlayerCardsInField[i].GetComponent<PlayerCard>().health = 0;

        for (int i = 0; i < warAI.opponentsHand.Count; i++)
            warAI.opponentsHand[i].GetComponent<OpponentCard>().health = 0;

        for (int i = 0; i < enemyCardsInField.Count; i++)
            enemyCardsInField[i].GetComponent<OpponentCard>().health = 0;

        yield return new WaitForSeconds(5f);

        for (int i = 0; i < allObjectsInScene.Length; i++)
            allObjectsInScene[i].SetActive(false);

        isPlayerTurn = true;

        yield return new WaitForSeconds(5);
        //switch scene

        int index = 3;
        int loadedScene = index;
        SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
        ///*index*/ = 3;
        //SceneManager.UnloadSceneAsync(loadedScene);
        //SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);

        yield return null;
    }
}