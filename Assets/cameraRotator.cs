using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraRotator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float rotationSpeed = 120f;
        float angle = rotationSpeed * Time.deltaTime;
        rotationSpeed = Mathf.Abs(rotationSpeed);


        if (Input.GetKey(KeyCode.E))
        {
            this.transform.Rotate(Vector3.up, angle);
        }

        if (Input.GetKey(KeyCode.Q)) 
        {
            this.transform.Rotate(Vector3.up, -angle);
        }

    }
}
