using UnityEngine;

/* The Credit Manager is the class that will deal with all things
 * related to transactions with the player's credits.
 * Credits will be added or subtracted from this object */
public class CreditManager : MonoBehaviour
{
    // The instance representing this class
    public static CreditManager CreditInstance;

    // Starting credits
    [SerializeField]
    private int credits = 100;

    /* On Awake, ensure that there is only 1 instance of this class
     * running. This will ensure that all things related to managin
     * the player's credits will run smoothly */
    private void Awake()
    {
        if (CreditInstance == null)
        {
            CreditInstance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public int Credits {
        set => credits = Mathf.Max(0, value);
        get => credits;
    }

    // A getter that returns the amount of credits currently
    public int GetCredits()
    {
        return credits;
    }
}
