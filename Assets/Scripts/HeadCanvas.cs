using TMPro;
using UnityEngine;

public class HeadCanvas : MonoBehaviour
{
    private static Transform mainCamera;
    public GameObject canvas;
    public TextMeshProUGUI text;
    public bool lookAtCam;
    void Start () {
        if (mainCamera == null)
            mainCamera = Camera.main.transform;
        Loader.Instance.heads.Add (this);
    }
    void LateUpdate () {
        if(lookAtCam)
            transform.LookAt(mainCamera);
    }
    public void ShowQuestion(string question) {
        canvas.SetActive (true);
        text.text = question;
        Invoke ("Hide", 5f);
    }
    private void Hide () {
        canvas.SetActive (false);
    }
}
