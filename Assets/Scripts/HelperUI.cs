using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperUI : MonoBehaviour
{
    public static HelperUI Instance { get; private set; }
    [Header("UI Elements")]
    [SerializeField] private GameObject mapHelperUI_open;
    [SerializeField] private GameObject mapHelperUI_close;
    [SerializeField] private GameObject interactionHelperUI;

    [Header("GameObject References")]
    [SerializeField] public int maxOpenTimes = 3; // 最大打开次数

    [SerializeField] private int mapOpenTimes = 0; 
    [SerializeField] private int mapCloseTimes = 0; 
    [SerializeField] private int interactionOpenTimes = 0;
    
    
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        mapHelperUI_open.SetActive(true);
        mapHelperUI_close.SetActive(false);
        interactionHelperUI.SetActive(false);
        mapOpenTimes = 1;
        mapCloseTimes = 0;
        interactionOpenTimes = 0;
    }


    public void ShowMapOpenUI()
    {
        
        mapHelperUI_close.SetActive(false);
        interactionHelperUI.SetActive(false);
        if ( mapOpenTimes<maxOpenTimes)
        {
            mapHelperUI_open.SetActive(true);
            mapOpenTimes++;
            Debug.Log("Map opened");
        }        
    }

    public void ShowMapCloseUI()
    {
        interactionHelperUI.SetActive(false);
        mapHelperUI_open.SetActive(false);
        if ( mapCloseTimes<maxOpenTimes)
        {
            mapHelperUI_close.SetActive(true);
            mapCloseTimes++;
            Debug.Log("Map closed");
        }        
    }

    public void ShowInteractionUI()
    {        
        mapHelperUI_open.SetActive(false);
        mapHelperUI_close.SetActive(false);
        if ( interactionOpenTimes<maxOpenTimes)
        {
            interactionHelperUI.SetActive(true);
            interactionOpenTimes++;
            Debug.Log("Interaction opened");
        }        
    }

    public void HideInteractionUI()
    {
        interactionHelperUI.SetActive(false);
        Debug.Log("Interaction closed");
    }
}