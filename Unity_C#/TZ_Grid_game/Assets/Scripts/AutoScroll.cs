using UnityEngine;
using UnityEngine.UI;

//
//автоскрол списка покупок в начало
//

public class AutoScroll : MonoBehaviour
{
    public ScrollRect ItemsScroll;

    private static AutoScroll instance;

    public static AutoScroll Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AutoScroll>();
            }
            return instance;
        }
    }

    public void AutoScrollItems()
    {
        ItemsScroll.verticalNormalizedPosition = 1f;
    }
}
