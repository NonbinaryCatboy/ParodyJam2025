using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D rbody;
    [SerializeField] private AnimationCurve accelerationCurve;
    private float accelerationTime;
    [SerializeField] private AnimationCurve decelerationCurve;
    private float decelerationTime;
    private float timestamp;
    private float dir;
    private float decelSpeed;
    bool moving = false;
    // Update is called once per frame


    void Awake() {
        accelerationTime = accelerationCurve[accelerationCurve.length - 1].time;
        decelerationTime = decelerationCurve[decelerationCurve.length - 1].time;
    }

    void FixedUpdate()
    {
        if (Time.time < timestamp) {
            if (moving)
                rbody.linearVelocity = new Vector2(speed * dir * accelerationCurve.Evaluate(Time.time - timestamp + accelerationTime), rbody.linearVelocityY);
            else
                rbody.linearVelocity = new Vector2(decelSpeed * dir * decelerationCurve.Evaluate(Time.time - timestamp + decelerationTime), rbody.linearVelocityY);
        } else {
            if (moving)
                rbody.linearVelocity = new Vector2(speed * dir, rbody.linearVelocityY);
        }
    }

    public void StartDeceleration() {
        moving = false;
        timestamp = Time.time + decelerationTime;
        decelSpeed = speed;
        if (Mathf.Abs(rbody.linearVelocityX) < decelSpeed)
            decelSpeed = Mathf.Abs(rbody.linearVelocityX);
    }

    public void StartAcceleration(float dir) {
        moving = true;
        timestamp = Time.time + accelerationTime;
    }

    public void UpdateMovement(float dir) {
        this.dir = dir;
        moving = true;
    }


    


}
