using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Load_room_scene(){

        gameObject.SetActive(true); 
            
    }

    public void Unload_room_scene(){

        gameObject.SetActive(false);
    }
}
