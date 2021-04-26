using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinRoom : MonoBehaviour
{
    [SerializeField] private Text text;
    private Networking networking;
    // Start is called before the first frame update
    void Start()
    {
        networking = FindObjectOfType<Networking>();
    }

    public void JoinChosenRoom()
    {
        networking.JoinRoom(text.text);
    }
}
