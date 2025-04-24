using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;

public class Map : MonoBehaviour
{
    public static Map Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI info;    
    [SerializeField] private Image marker; 
    [SerializeField] private Button[] roomButtons;
    
    private int currentRoomIndex; 
    private Vector2Int currentRoomPos;
    private int selectedRoomIndex;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Prevent multiple instances of Map
        }
    }

    void Start()
    {
        panel.SetActive(false);
        
        foreach (Button button in roomButtons) 
        {   
            int roomIndex = GetRoomIndex(button);
            button.onClick.AddListener(() => OnRoomButtonClicked(roomIndex));
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && !panel.activeSelf){
            Debug.Log("M key pressed, displaying map...");
            DisplayMap();
        }
        
            
        if (Input.GetKeyDown(KeyCode.E) && panel.activeSelf){
            Debug.Log("E key pressed, closing map...");
            CloseMap();
        }
            
    }

    private void DisplayMap()
    {
        panel.SetActive(true);
        GameManager.Instance.PauseGame();
        currentRoomIndex=Player.Instance.currentRoomIndex;
        currentRoomPos = RoomManager.Instance.Get_room_position(currentRoomIndex);
        marker.GetComponent<RectTransform>().anchoredPosition = currentRoomPos;  // display the marker on the maps
        HelperUI.Instance.ShowMapCloseUI(); // Show the close UI
    }


    private void CloseMap()
    {
        GameManager.Instance.ResumeGame();
        info.text = ""; // Clear the info text when closing the map
        panel.SetActive(false);
        HelperUI.Instance.ShowMapOpenUI(); // Show the open UI
    }


    public void OnRoomButtonClicked(int roomIndex){               
        int steps = RoomManager.Instance.Calculate_Steps(currentRoomIndex, roomIndex);
        info.text = $"到达该房间需要 {steps} 步。";
        selectedRoomIndex = roomIndex;
    }

    private int GetRoomIndex(Button button)
    {
        return int.Parse(button.name.Replace("Room", ""));
    }
  

    public void OnGoClicked()
    {
        if (selectedRoomIndex == currentRoomIndex) return;
        RoomManager.Instance.Go_to_Room(selectedRoomIndex);  
        CloseMap();
    }


}


