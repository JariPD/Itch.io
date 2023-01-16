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

    [SerializeField] private GameObject room;
    private bool winCoroutine = true, loseCoroutine = true;
    private bool isPlayerTurn = true;
    private int turnCount = 0;
    private int count = 0;
    private int placeCardCount, AttackCount, destroyCardCount;
    private bool destroyingcard = false;
    private bool resetAttack = false;

    [Header("Battling")]
    public List<GameObject> playersHand;               //players hand - used to keep track of the cards in the players hand
    public int playerHealth, opponentHealth;                             //health of the player and opponent
    [SerializeField] private Button attackButton;
    private readonly int maxPlayerHealth = 25, maxOpponentHealth = 25;   //max health of the player and opponent

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

        //sets health
        playerHealth = maxPlayerHealth;
        opponentHealth = maxOpponentHealth;

        //update health ui
        UIManager.instance.UpdateWarHealth(playerHealth, opponentHealth);

        //play ambience
        audioManager.Play("Ambience");
    }

    private void Update()
    {
        //move camera above playing field when placing card
        if (CardSelected)
        {
            resetAttack = true;
            StartCoroutine(SeeCards());
            attackButton.interactable = false;
        }
        else
        {
            if (MainCam.position == vCamTwo.position)
                vCamOne.SetActive(true);

            if (resetAttack)
            {
                resetAttack = false;
                attackButton.interactable = true;
            }
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

            if (loseCoroutine)
                StartCoroutine(LostGame());
        }

        if (PlayerCardsInField.Count >= 3 && !destroyingcard && audioManager.Sounds[8].source.isPlaying == false && destroyCardCount == 0)
        {
            StartCoroutine(DestroyCardVoiceline());
        }
    }

    /// <summary>
    /// function to be called from a button - starts the dice throw
    /// </summary>
    public void StartDiceThrow()
    {
        //gridParent.SetActive(true);
        StartCoroutine(ThrowDice(true, false));
    }

    IEnumerator ThrowDice(bool placeCards, bool giveExtraPlayerCard)
    {
        turnCount++;
        count++;

        //get random dice roll to spawn cards
        diceRoll = count <= 2 ? Random.Range(2, 3) : 1;

        //------------------------------------------------------------------- player dice logic -------------------------------------------------------------------\\

        if (isPlayerTurn && playersHand.Count < 6 && winCoroutine && loseCoroutine) //check if it is the players turn and the player does not have a full hand
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

            if (giveExtraPlayerCard)
                yield return null;
        }
        else if (!isPlayerTurn)  //-------------------------------------------- opponent dice logic ---------------------------------------------\\
        {
            if (enemyCardsInField.Count != 4 && winCoroutine && loseCoroutine)
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
            StartCoroutine(ThrowDice(true, false));
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
        attackButton.interactable = false;

        //make it so you cant pickup cards
        for (int i = 0; i < playersHand.Count; i++)
            playersHand[i].GetComponent<PlayerCard>().AllowCardPickup = false;

        bool usable = true;
        if (usable)
        {
            for (int i = 0; i < PlayerCardsInField.Count; i++)
                PlayerCardsInField[i].GetComponent<PlayerCard>().AttackForward();
        }

        //update player and enemy UI 
        UIManager.instance.UpdateWarHealth(playerHealth, opponentHealth);

        yield return new WaitForSeconds(2);

        usable = true;
        if (usable && winCoroutine && loseCoroutine)
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
            StartCoroutine(ThrowDice(false, false));
        }
        else if (enemyCardsInField.Count < 4)
        {
            isPlayerTurn = false;
            turnCount = 1;
            StartCoroutine(ThrowDice(true, false));
        }

        yield return new WaitForSeconds(1);
        if (AttackCount <= 0)
        {
            AttackCount++;
            audioManager.Play("ReaperHealth");
        }

        //make it so you cant pickup cards
        for (int i = 0; i < playersHand.Count; i++)
            playersHand[i].GetComponent<PlayerCard>().AllowCardPickup = true;

        attackButton.interactable = true;
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

                if (placeCardCount <= 0)
                {
                    placeCardCount++;
                    audioManager.StopPlaying("ReaperPlaceCards");
                    audioManager.Play("ReaperThereYouGo");
                }
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

    #region Audio
    /// <summary>
    /// function to play a sound from the sound manager. Used in a button
    /// </summary>
    /// <param name="name"></param>
    public void PlaySound(string name)
    {
        audioManager.Play(name);
    }

    public void PlayPlaceCardsVoiceline()
    {
        StartCoroutine(PlaceCardsVoiceline());
    }

    private IEnumerator PlaceCardsVoiceline()
    {
        yield return new WaitForSeconds(1);
        audioManager.Play("ReaperPlaceCards");
    }

    private IEnumerator DestroyCardVoiceline()
    {
        attackButton.interactable = false;

        destroyingcard = true;
        destroyCardCount++;
        audioManager.Play("ReaperDestroyCard");

        yield return new WaitForSeconds(4f);

        int r = Random.Range(0, PlayerCardsInField.Count);
        PlayerCardsInField[r].GetComponent<PlayerCard>().health = 0;

        yield return new WaitForSeconds(1f);

        isPlayerTurn = true;
        StartCoroutine(ThrowDice(false, true));

        attackButton.interactable = true;

        yield return null;
    }
    #endregion

    private IEnumerator WonGame()
    {
        winCoroutine = false;

        yield return new WaitForSeconds(3);

        audioManager.Play("ReaperLose");

        #region Deactivation
        for (int i = 0; i < playersHand.Count; i++)
            playersHand[i].GetComponent<PlayerCard>().health = 0;

        for (int i = 0; i < PlayerCardsInField.Count; i++)
            PlayerCardsInField[i].GetComponent<PlayerCard>().health = 0;

        for (int i = 0; i < warAI.opponentsHand.Count; i++)
            warAI.opponentsHand[i].GetComponent<OpponentCard>().health = 0;

        for (int i = 0; i < enemyCardsInField.Count; i++)
            enemyCardsInField[i].GetComponent<OpponentCard>().health = 0;

        yield return new WaitForSeconds(8f);

        for (int i = 0; i < allObjectsInScene.Length; i++)
            allObjectsInScene[i].SetActive(false);

        room.GetComponent<Animator>().SetTrigger("DoAnim");

        #endregion

        yield return new WaitForSeconds(5);
        //switch scene

        /* int index = 2;
         int lodedScene = index;
         index = 3;
         SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
         SceneManager.UnloadSceneAsync(lodedScene);*/
        SceneManager.LoadScene(3);


        yield return null;
    }

    IEnumerator LostGame()
    {
        loseCoroutine = false;

        yield return new WaitForSeconds(3);

        audioManager.Play("ReaperWin");

        #region Deactivation
        for (int i = 0; i < playersHand.Count; i++)
            playersHand[i].GetComponent<PlayerCard>().health = 0;

        for (int i = 0; i < PlayerCardsInField.Count; i++)
            PlayerCardsInField[i].GetComponent<PlayerCard>().health = 0;

        for (int i = 0; i < warAI.opponentsHand.Count; i++)
            warAI.opponentsHand[i].GetComponent<OpponentCard>().health = 0;

        for (int i = 0; i < enemyCardsInField.Count; i++)
            enemyCardsInField[i].GetComponent<OpponentCard>().health = 0;

        yield return new WaitForSeconds(8f);

        for (int i = 0; i < allObjectsInScene.Length; i++)
            allObjectsInScene[i].SetActive(false);

        room.GetComponent<Animator>().SetTrigger("DoAnim");

        #endregion

        yield return new WaitForSeconds(5);
        //switch scene

        int index = 0;
        int loadedScene = index;
        SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
        ///*index*/ = 3;
        //SceneManager.UnloadSceneAsync(loadedScene);
        //SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);

        yield return null;

    }
}