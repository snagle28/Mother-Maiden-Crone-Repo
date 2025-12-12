using UnityEngine;
using Yarn.Unity;

public class ClickToAdvance : MonoBehaviour
{
    public LineView lineView;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (lineView.IsDeliveringLine)
            {
                lineView.InterruptLine();
            }
            else
            {
                lineView.UserRequestedViewAdvancement();
            }
        }
    }
}
