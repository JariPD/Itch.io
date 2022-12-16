using UnityEngine;

public class DiceCheck : MonoBehaviour
{
    Vector3 velocity;

    private void FixedUpdate()
    {
        velocity = DiceThrow.Velocity;
    }

    private void OnTriggerStay(Collider other)
    {
        if (velocity.x == 0 && velocity.y == 0 && velocity.z == 0)
        {
            switch (other.gameObject.name)
            {
                case "side1":
                    WarManager.instance.diceRoll = 6;
                    break;

                case "side2":
                    WarManager.instance.diceRoll = 5;
                    break;

                case "side3":
                    WarManager.instance.diceRoll = 4;
                    break;

                case "side4":
                    WarManager.instance.diceRoll = 3;
                    break;

                case "side5":
                    WarManager.instance.diceRoll = 2;
                    break;

                case "side6":
                    WarManager.instance.diceRoll = 1;
                    break;
            }
        }
    }
}
