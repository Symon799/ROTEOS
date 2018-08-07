using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Characters.FirstPerson
{
    public class FirstPersonController : MonoBehaviour
    {
        public float speed = 10;
        [SerializeField] private MouseLook m_MouseLook;
        [SerializeField] private bool m_UseFovKick;
        [SerializeField] private FOVKick m_FovKick = new FOVKick();


        private Camera m_Camera;

        // Use this for initialization
        private void Start()
        {
            m_Camera = Camera.main;
			m_MouseLook.Init(transform , m_Camera.transform);
            CrossPlatformInputManager.SwitchActiveInputMethod(CrossPlatformInputManager.ActiveInputMethod.Hardware);
        }


        // Update is called once per frame
        private void Update()
        {
            if (Input.GetAxis("Vertical") > 0)
                transform.position += transform.forward * speed * Time.deltaTime;
            else if (Input.GetAxis("Vertical") < 0)
                transform.position -= transform.forward * speed * Time.deltaTime;

            if (Input.GetAxis("Horizontal") > 0)
                transform.position += transform.right * speed * Time.deltaTime;
            else if (Input.GetAxis("Horizontal") < 0)
                transform.position -= transform.right * speed * Time.deltaTime;

            if (Input.GetMouseButton(1))
                RotateView();

            if (Input.GetKey(KeyCode.Space))
                transform.position += transform.up * speed * Time.deltaTime;
            else if (Input.GetKey(KeyCode.LeftShift))
                transform.position -= transform.up * speed * Time.deltaTime;


            
        }

        private void RotateView()
        {
            m_MouseLook.LookRotation (transform, m_Camera.transform);
        }

        
    }
}
