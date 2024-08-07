using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform rabbitCameraTransform;

    private void Start()
    {
        this.rabbitCameraTransform = 
            PersistentData.P1Character == Character.Rabbit 
            ? Camera.main.transform 
            : GameObject.FindGameObjectWithTag(Constants.Tag_P2Camera).transform;
    }

    private void Update()
    {
        this.transform.LookAt(this.transform.position + this.rabbitCameraTransform.forward);
    }
}
