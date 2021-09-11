using UnityEngine;

public class NPC : MonoBehaviour
{
    private Animator animator;
    private HeadCanvas head;
    void Awake()
    {
        animator = GetComponent<Animator>();
        head = GetComponentInChildren<HeadCanvas>();
    }
    void Start()
    {
        Invoke("RandomMove", Random.Range(10.0f, 50.0f));
    }
    void RandomMove()
    {
        int random = Random.Range(2, 7);
        animator.SetInteger("random", Random.Range(2, 7));
        head.lookAtCam = false;
        Invoke("RandomMove", Random.Range(10.0f, 50.0f));
        Invoke("ResetAnimator", 2f);
    }
    void ResetAnimator()
    {
        head.lookAtCam = true;
        animator.SetInteger("random", 0);
    }
    public void Clap()
    {
        CancelInvoke();
        animator.SetInteger("random", 1);
        Invoke("ResetAnimator", 2f);
        Start();
    }
}
