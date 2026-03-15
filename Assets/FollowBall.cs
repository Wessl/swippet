using UnityEngine;

public class FollowBall : MonoBehaviour
{
    public float zDistToBall;
    private float _heightOverGround;
    private Ball _ball;
    Vector3 _goalPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _ball = FindAnyObjectByType<Ball>();
        zDistToBall = transform.position.z - _ball.transform.position.z;
        _heightOverGround = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (_ball.released)
        {
            Vector3 zero = Vector3.zero;
            _goalPosition = new Vector3(transform.position.x, _heightOverGround, _ball.transform.position.z + zDistToBall);
            float speed = Vector3.Distance(transform.position, _goalPosition) * 2f;
            transform.position = Vector3.MoveTowards(transform.position, _goalPosition, speed * Time.deltaTime);
        }
    }
}
