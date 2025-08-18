using UnityEngine;

public class PlayerController : MonoBehaviour
{
    bool acting;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer sprite;
    void Start()
    {

    }
    void FixedUpdate()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        


        if (!acting && InputHandler.Instance.attack.pressed)
        {
            StartAction();
            animator.SetTrigger("attack");
        }



        bool moving = false;
        if (!acting && InputHandler.Instance.move.down)
        {
            UpdateFacing();
            moving = true;
        }
        animator.SetBool("moving", moving);
    }

    private void UpdateFacing()
    {
        sprite.flipX = InputHandler.Instance.dir < 0;
    }
    private void StartAction()
    {
        UpdateFacing();
        acting = true;
    }

    public void EndAction()
    {
        acting = false;
    }
}
