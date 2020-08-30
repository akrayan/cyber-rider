using Sirenix.OdinInspector;
using UnityEngine;

public interface IMotionHandler
{
    void Update(Transform transform);
}

public abstract class BaseMotionHandler : IMotionHandler
{
    [Title("Motion Controller")]
    [SerializeField] private bool enabled;
    public void Update(Transform transform)
    {
        if (enabled)
        {
            InternalUpdate(transform);
        }
    }

    protected abstract void InternalUpdate(Transform transform);
}

public class VelocityMotionHandler : BaseMotionHandler
{
    [InfoBox("Controls the vertical velocity of the object. When the specified key is pressed a force will be added upwards on the body")]    
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private KeyCode key;
    [SerializeField] private float forceMultiplier;
    
    protected override void InternalUpdate(Transform transform)
    {
        if (Input.GetKey(key))
        {
            rigidbody.AddForce(Vector2.up * forceMultiplier, ForceMode2D.Force);
        }
    }
}

public class RotationMotionHandler : BaseMotionHandler
{
    [InfoBox("Controls the rotation of the object. Use the velocity to angle to change the effect of the rotation based on velocity.")]
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private AnimationCurve velocityToAngle;
    [SerializeField] private float angleMultiplier = 1;
    [SerializeField] [ReadOnly] private float angle;
    protected override void InternalUpdate(Transform transform)
    {
        angle = Mathf.Sign(rigidbody.velocity.y) * velocityToAngle.Evaluate(Mathf.Abs(rigidbody.velocity.y));
        transform.eulerAngles = new Vector3(0, 0, angle * angleMultiplier);
    }
}

public class JoystickMotionHandler : BaseMotionHandler
{
    [InfoBox("Uses the joysticks vertical input (and keyboard) to add velocity to the ship.")]
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private float forceMultiplier;
    [SerializeField] [ReadOnly] private float verticalInput = 0;
    [SerializeField] [ReadOnly] private float horizontalInput = 0;
    protected override void InternalUpdate(Transform transform)
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        rigidbody.AddForce(verticalInput * forceMultiplier * Vector2.up, ForceMode2D.Force);
        rigidbody.AddForce(horizontalInput * forceMultiplier * Vector2.right, ForceMode2D.Force);
    }
}

//public class ForwardVelocityHandler : IMotionHandler
//{
////    [InfoBox("Uses the joysticks vertical input (and keyboard) to add velocity to the ship.")]
//    [SerializeField] private Rigidbody2D rigidbody;
//    public void Update(Transform transform)
//    {
//        var original = rigidbody.velocity;
//        original.x = Time.time;
//    }
//}

public class Movement : SerializedMonoBehaviour
{
    private Rigidbody2D _body;
    [SerializeField] private IMotionHandler[] _motionHandlers;

    Vector2 _screenBoundary;
    Vector2 _objectBoundary;
    // Start is called before the first frame update
    void Start()
    {
        _screenBoundary = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        //Debug.Log("Screen Boundary " + _screenBoundary);
        _body = GetComponentInChildren<Rigidbody2D>();
        _objectBoundary = GetComponentInChildren<SpriteRenderer>().bounds.size / 2;
    }

    private void FixedUpdate()
    {
        foreach (var handler in _motionHandlers)
        {
            handler?.Update(transform);
        }
    }

    private void LateUpdate()
    {
        Vector3 viewPos = transform.position;

        viewPos.x = Mathf.Clamp(viewPos.x, _screenBoundary.x * -1 + _objectBoundary.x, _screenBoundary.x - _objectBoundary.x);
        viewPos.y = Mathf.Clamp(viewPos.y, _screenBoundary.y * -1 + _objectBoundary.y, _screenBoundary.y - _objectBoundary.y);
        transform.position = viewPos;
    }

}
