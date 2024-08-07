using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CameraFollow : MonoBehaviour
{
    public Transform Target;

    [SerializeField]
    private float smoothSpeed = 10f;

    [SerializeField]
    private Vector3 rabbitOffset;

    [SerializeField]
    private Vector3 dogOffset;

    private Vector3 offset;

    private void Start()
    {
        this.offset = this.Target.CompareTag(Constants.Tag_Rabbit) ? this.rabbitOffset : this.dogOffset;
    }

    private void LateUpdate()
    {
        // Normally I wouldn't call this every frame, but it's useful for debugging since it allows you to adjust offset while playtesting.
        this.offset = this.Target.CompareTag(Constants.Tag_Rabbit) ? this.rabbitOffset : this.dogOffset;

        Vector3 desiredPosition = this.Target.position + this.offset;
        Vector3 smoothedPosition = Vector3.Lerp(this.transform.position, desiredPosition, this.smoothSpeed * Time.deltaTime);
        this.transform.position = smoothedPosition;
    }
}
