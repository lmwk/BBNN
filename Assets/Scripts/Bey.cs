using System;
using System.Linq;
using UnityEngine;

namespace Scripts
{
    public class Bey : MonoBehaviour, IComparable<Bey>
    {
        private bool initilized = false;

        private NeuralNetwork net;
        private Rigidbody rBody;

        private LayerMask beyLayer;
        
        private float speed;

        private void Start()
        {
            rBody = GetComponent<Rigidbody>();
            speed = UnityEngine.Random.Range(20.24f, 24.25f);
        }

        private void FixedUpdate()
        {

            if (initilized == true)
            {
                var distance = GetClosestEnemy(GameObject.FindGameObjectsWithTag("Bey"));
                float[] inputs = new float[2];

                inputs[0] = distance.position.x;
                inputs[1] = distance.position.z;

                float[] output = net.FeedForward(inputs);

                Vector3 MoveVector = transform.TransformDirection(output[0], 0f, output[1]) * speed;
                rBody.velocity = MoveVector;
            }
        }
        
        Transform GetClosestEnemy(GameObject[] enemies) {
            if (Physics.SphereCast(transform.position, 20f, transform.forward, out RaycastHit hit, 30f, beyLayer))
            {
                return hit.transform;
            }

            return transform;
        }
        
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent<Bey>(out Bey bey))
            {
                this.net.AddFitness(-1f);
                bey.net.AddFitness(-1f);
                var posnorm = (other.transform.forward - this.transform.forward).normalized;
                var relativeVec = this.rBody.velocity - other.rigidbody.velocity;
                relativeVec.Scale(posnorm);
                relativeVec.Scale(posnorm);
                
                var normalVec = relativeVec;
                
                Vector3 MoveVector = transform.TransformDirection(normalVec.x, 0f, normalVec.z) * speed;

                var velocity1 = rBody.velocity;
                velocity1 -= (velocity1 - MoveVector);
                rBody.velocity = velocity1;
                var velocity = other.rigidbody.velocity;
                velocity += velocity + MoveVector;
                other.rigidbody.velocity = velocity;
            }
        }
        
        
        public void Init(NeuralNetwork net)
        {
            this.net = net;
            initilized = true;
        }

        public int CompareTo(Bey other)
        {
            if (other == null) return 1;

            if (net.GetFitness() > other.net.GetFitness())
                return 1;
            else if (net.GetFitness() < other.net.GetFitness())
                return -1;
            else
                return 0;
        }
    }
}