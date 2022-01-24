using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

namespace Scripts
{
    public class Manager : MonoBehaviour
    {
        public GameObject beyPrefab;

        private GameObject enemyBey;

        private bool IsTraining = false;

        private int popSize = 10;

        public int generation = 0;

        private int[] layers = new int[] {2, 10, 10, 2};

        private List<NeuralNetwork> nets;

        private List<Bey> beyList = null;

        void Timer()
        {
            IsTraining = false;
        }

        private void Update()
        {
            if (IsTraining == false)
            {
                if (generation == 0)
                {
                    InitBeyNeuralNetworks();
                }
                else
                {
                    float averageWacrossNN = 0.0f;
                    nets.Sort();
                    
                    for (int i = 0; i < popSize; i++)
                    {
                        averageWacrossNN += averageWacrossNN + nets[i].GetAverageWeight();
                    }
                    
                    for (int i = 0; i < popSize / 2; i++)
                    {
                        nets[i] = new NeuralNetwork(nets[i + (popSize / 2)]);
                        nets[i].Mutate();

                        nets[i + (popSize / 2)] =
                            new NeuralNetwork(
                                nets[i + (popSize / 2)]); 
                    }
                    
                    using (StreamWriter file = new StreamWriter("AverageWData.txt", true))
                    {
                        file.Write($"Generation {generation}: {averageWacrossNN / popSize}\n");
                    }

                    for (int i = 0; i < popSize; i++)
                    {
                        nets[i].SetFitness(0f);
                    }
                    
                }

                generation++;

                IsTraining = true;
                Invoke("Timer", 15f);
                CreateBeyBodies();
            }

        }

        private void CreateBeyBodies()
        {
            if (beyList != null)
            {
                for (int i = 0; i < beyList.Count; i++)
                {
                    Destroy(beyList[i].gameObject);
                }
            }

            beyList = new List<Bey>();

            for (int i = 0; i < popSize; i++)
            {
                Bey bey = ((GameObject)Instantiate(beyPrefab,
                    new Vector3(UnityEngine.Random.Range(-70f, 70f), 1.2f, UnityEngine.Random.Range(-70f, 70f)), beyPrefab.transform.rotation)).GetComponent<Bey>();
                bey.Init(nets[i]);
                beyList.Add(bey);
            }
        }

        void InitBeyNeuralNetworks()
        {
            if (popSize % 2 != 0)
            {
                popSize = 10;
            }

            nets = new List<NeuralNetwork>();

            for (int i = 0; i < popSize; i++)
            {
                NeuralNetwork net = new NeuralNetwork(layers);
                net.Mutate();
                net.AddFitness(100f);
                nets.Add(net);
            }

        }
        
    }
}