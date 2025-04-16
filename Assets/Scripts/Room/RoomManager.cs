using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private const int maxRoomCount = 6; // 最大房间数量
    [SerializeField] private int[] allRoomIndex ;
    [SerializeField] private Vector2Int[] allRoomPos;
    [SerializeField] private int[][] allRoomSteps; 
    [SerializeField] private List<int> visibleRoomIndex; 
    [SerializeField] public Room[] allRooms;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 确保在场景切换时不被销毁
        }
        else
        {
            Destroy(gameObject); // 防止多个实例
        }
    }

    void Start(){ //初始化所有房间数据
        allRoomIndex = new int[maxRoomCount] {  1, 2, 3, 4, 5, 6 };
        allRoomPos = new Vector2Int[maxRoomCount] {
            new Vector2Int(-236, 91), // Room 1
            new Vector2Int(26, 91), // Room 2
            new Vector2Int(296, 91), // Room 3
            new Vector2Int(-236, -91), // Room 4
            new Vector2Int(26, -91), // Room 5
            new Vector2Int(296, -91)  // Room 6
        };

        allRoomSteps = new int[maxRoomCount][];

        allRoomSteps[0] = new int[maxRoomCount] { 0, 1, 2, 1, 2, 3 };
        allRoomSteps[1] = new int[maxRoomCount] { 1, 0, 1, 2, 1, 2 };  
        allRoomSteps[2] = new int[maxRoomCount] { 2, 1, 0, 3, 2, 1 };
        allRoomSteps[3] = new int[maxRoomCount] { 1, 2, 3, 0, 1, 2 };
        allRoomSteps[4] = new int[maxRoomCount] { 2, 1, 2, 1, 0, 1 };
        allRoomSteps[5] = new int[maxRoomCount] { 3, 2, 1, 2, 1, 0 };

        visibleRoomIndex = new List<int>();   //可见房间索引列表，初始值为6 -- 后面再改
        for(int i=1;i<=6;i++)
            visibleRoomIndex.Add(i);       


    }

    public Vector2Int Get_room_position(int roomIndex)
    {
        return allRoomPos[roomIndex-1];
    }

    public int Calculate_Steps(int currentRoomIndex, int roomIndex)
    {
        return allRoomSteps[currentRoomIndex-1][roomIndex-1];
    }

    public void Go_to_Room(int roomIndex)  //要完成时间减少……
    {
        if (roomIndex < 1 || roomIndex > allRooms.Length)
        {
            Debug.LogError($"Invalid room index: {roomIndex}");
            return;
        }

        allRooms[roomIndex - 1].gameObject.SetActive(true);
        allRooms[Player.Instance.currentRoomIndex - 1].gameObject.SetActive(false);  //切换房间
        Player.Instance.currentRoomIndex = roomIndex;
        Debug.Log($"Player moved to Room {roomIndex}");
    }

    
    public void EnableRoomInteraction(){
        foreach (var room in allRooms)
        {
            room.allowInteraction = true; //允许交互
        }
    }

    public void DisableRoomInteraction(){
        foreach (var room in allRooms)
        {
            room.allowInteraction = false; //禁止交互
        }
    }
}
