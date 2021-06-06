using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Loader : MonoBehaviour
{
    public VideoClip[] clips;
    public VideoPlayer player;

    public GameObject[] buttons;

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
    public void PlayVideo(int index)
    {
        foreach (GameObject gameObject in buttons)
        {
            gameObject.SetActive(false);
        }
        player.gameObject.SetActive(true);
        player.Stop();
        player.clip = clips[index];
        player.Play();
    }

    public void StopVideo()
    {
        player.Stop();
        foreach (GameObject gameObject in buttons)
        {
            gameObject.SetActive(true);
        }
        player.gameObject.SetActive(false);
    }
}
