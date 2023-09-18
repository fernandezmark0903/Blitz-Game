using UnityEngine;

public class GlideController : MonoBehaviour
{
    
    public GameObject middle;
    public GameObject bottom;

    private Vector3 destination;
    private float speed;

    void Start()
    {
        // Set the destination to be the object's position so it will not start off moving
        SetDestination(0);
    }

    void Update()
    {
        // If the object is not at the target destination
        if (destination != gameObject.transform.position)
        {
            // Move towards the destination each frame until the object reaches it
            IncrementPosition();
        }
    }

    void IncrementPosition()
    {
        // Calculate the next position
        float delta = speed * Time.deltaTime;
        Vector3 currentPosition = gameObject.transform.position;
        Vector3 nextPosition = Vector3.MoveTowards(currentPosition, destination, delta);

        // Move the object to the next position
        gameObject.transform.position = nextPosition;
    }

    // Set the destination to cause the object to smoothly glide to the specified location
    public void SetDestination(int index)
    {
        if (index == 0)
        {
            destination = middle.transform.position;
            speed = 1000;
        }
        else
        {
            destination = bottom.transform.position;
            speed =2000;
        }
    }
}