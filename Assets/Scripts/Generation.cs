using System;
using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;
using UnityEngine.UI;

public class Generation : MonoBehaviour
{
    private Text textComp;
    public Text BeyText;
    public GameObject managerObj;
    void Start()
    {
        textComp = GetComponent<Text>();
    }
    private void FixedUpdate()
    {
        textComp.text = $"Generation: {managerObj.GetComponent<Manager>().generation}";
    }
}
