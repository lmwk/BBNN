using System;
using UnityEngine;

namespace Scripts
{
    public class Cam : MonoBehaviour
    {
        public float cameraSens = 90f;
        public float climbSpeed = 4f;
        public float normalMoveSpeed = 10f;
        public float slowMoveFactor = 0.25f;
        public float fastMoveFactor = 3f;

        private float rotX = 0.0f;
        private float rotY = 0.0f;

        private void Start()
        {
            Application.targetFrameRate = 90;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            rotX += Input.GetAxis("Mouse X") * cameraSens * Time.deltaTime;
            rotY += Input.GetAxis("Mouse Y") * cameraSens * Time.deltaTime;
            rotY = Mathf.Clamp(rotY, -90f, 90f);

            transform.localRotation = Quaternion.AngleAxis(rotX, Vector3.up);
            transform.localRotation *= Quaternion.AngleAxis(rotY, Vector3.left);

            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                transform.position += transform.forward * (normalMoveSpeed * fastMoveFactor) *
                                      Input.GetAxis("Vertical") * Time.deltaTime;
                transform.position +=  transform.right * (normalMoveSpeed * fastMoveFactor) *
                                      Input.GetAxis("Horizontal") * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                transform.position += transform.forward * (normalMoveSpeed * slowMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
                transform.position += transform.right * (normalMoveSpeed * slowMoveFactor) *
                                      Input.GetAxis("Horizontal") * Time.deltaTime;
            }
            else
            {
                transform.position += transform.forward * normalMoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
                transform.position += transform.right * normalMoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
            }
            
            if (Input.GetKey(KeyCode.Q)) {transform.position += transform.up * climbSpeed * Time.deltaTime;}
            if (Input.GetKey(KeyCode.E)) {transform.position -= transform.up * climbSpeed * Time.deltaTime;}

            if (Input.GetKeyDown(KeyCode.End))
            {
                Cursor.lockState = (Cursor.lockState != CursorLockMode.Locked) ? CursorLockMode.Locked : CursorLockMode.None;
            }
        }
    }
}