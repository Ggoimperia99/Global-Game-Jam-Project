using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManClass : MonoBehaviour
{
    public bool dialogueinAction;
    public GameObject textBlock;
    void Start()
    {
        textBlock.SetActive(false);
    }

    void Update()
    {
        if (dialogueinAction)
        {
            textBlock.SetActive(true);
        }
        else
        {
            textBlock.SetActive(false);
        }

    }
}
