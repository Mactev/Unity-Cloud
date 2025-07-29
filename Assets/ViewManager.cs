using UnityEngine;

public class ViewManager : MonoBehaviour
{
    public Transform model; // collegalo via Inspector

    public void ShowTopView()
    {
        model.localRotation = Quaternion.Euler(0, 0, 0);
    }

    public void ShowLeftView()
    {
        model.localRotation = Quaternion.Euler(0, -90, -90);
    }

    public void ShowRightView()
    {
        model.localRotation = Quaternion.Euler(0, 90, 90);
    }

    public void ShowPerspectiveView()
    {
        model.localRotation = Quaternion.Euler(45, 45, 45);
    }
}
