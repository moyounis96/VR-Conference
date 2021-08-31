using UnityEngine;

public class HeadCanvas : MonoBehaviour
{
    private static Transform mainCamera;
    void Start () {
        if (mainCamera == null)
            mainCamera = Camera.main.transform;
        Loader.Instance.heads.Add (this);
        gameObject.SetActive (false);
    }

    void LateUpdate () {
        transform.rotation = mainCamera.rotation;
    }
    public void ShowQuestion(string question) {
        GetComponentInChildren<TMPro.TextMeshProUGUI> ().text = question;
        gameObject.SetActive (true);
        Invoke ("Hide", 5f);
    }
    private void Hide () {
        gameObject.SetActive (false);
    }
}
