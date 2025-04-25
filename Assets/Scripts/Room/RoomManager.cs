using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private const int maxRoomCount = 10; // 最大房间数量
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
        allRoomIndex = new int[maxRoomCount] {  1, 2, 3, 4, 5, 6,7,8,9,10 }; //房间索引
        allRoomPos = new Vector2Int[maxRoomCount] {
            new Vector2Int(-328, 140), 
            new Vector2Int(-163,140), 
            new Vector2Int(-163,-20), 
            new Vector2Int(0,295),
            new Vector2Int(0,140), 
            new Vector2Int(0,-20),  // Room 6
            new Vector2Int(0,-180), // Room 7
            new Vector2Int(165,295), // Room 8
            new Vector2Int(165,140), // Room 9
            new Vector2Int(165,-20)  // Room 10
        };

        allRoomSteps = new int[maxRoomCount][];

        allRoomSteps[0] = new int[maxRoomCount] { 0, 1, 2, 3, 2, 3 ,4,4,3,4};
        allRoomSteps[1] = new int[maxRoomCount] { 1, 0, 1, 2, 1, 2 ,3,3,2,3};  
        allRoomSteps[2] = new int[maxRoomCount] { 2, 1, 0, 3, 2, 1 ,2,4,3,2};
        allRoomSteps[3] = new int[maxRoomCount] { 3, 2, 3, 0, 1, 2 ,3,1,2,3};
        allRoomSteps[4] = new int[maxRoomCount] { 2, 1, 2, 1, 0, 1 ,2,2,1,2};
        allRoomSteps[5] = new int[maxRoomCount] { 3, 2, 1, 2, 1, 0 ,1,3,2,1};
        allRoomSteps[6] = new int[maxRoomCount] { 4,3,2,3,2,1,0,4,3,2};
        allRoomSteps[7] = new int[maxRoomCount] { 4,3,4,1,2,3,4,0,1,2};
        allRoomSteps[8] = new int[maxRoomCount] { 3,2,3,2,1,2,3,1,0,1};
        allRoomSteps[9] = new int[maxRoomCount] { 4,3,2,3,2,1,2,2,1,0};

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
        if(visibleRoomIndex.Contains(roomIndex) == false) //如果房间不可见
        {
            return 0;
        }
        return allRoomSteps[currentRoomIndex-1][roomIndex-1];
    }

    public bool Go_to_Room(int roomIndex)  //要完成时间减少……
    {
        if (roomIndex < 1 || roomIndex > allRooms.Length)
        {
            Debug.LogError($"Invalid room index: {roomIndex}");
            return false;
        }
        
        if (visibleRoomIndex.Contains(roomIndex) == false) //如果房间不可见
        {
            return false;
        }

        allRooms[roomIndex - 1].Load_room_scene(); //加载房间场景
        allRooms[Player.Instance.currentRoomIndex - 1].Unload_room_scene(); //卸载当前房间场景
        Player.Instance.ModifyTemperature(-Calculate_Steps(Player.Instance.currentRoomIndex, roomIndex));
        Player.Instance.currentRoomIndex = roomIndex;
        TimeManager.Instance.AddTime(Calculate_Steps(Player.Instance.currentRoomIndex, roomIndex)); 
        Debug.Log($"Player moved to Room {roomIndex}");
        return true;
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
