using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour, ITimeTracker
{
    // 声明静态单例实例，方便其他脚本访问 UiManager
    public static UiManager Instance { get; private set; }

    [Header("Status Bar")]
    public Image toolEquipSlot;

    public Text timeText;
    public Text dateText;

    [Header("Inventory System")]
    // 序列化字段，指向库存 UI 面板的 GameObject，在 Inspector 中赋值
    [SerializeField] private GameObject inventoryPanel;

    [SerializeField] private HandInventorySlot toolHandSlot;
    [SerializeField] private HandInventorySlot itemHandSlot;

    // 工具槽位数组
    [SerializeField] private InventorySlot[] toolSlots;
    // 物品槽位数组
    [SerializeField] private InventorySlot[] itemSlots;

    [SerializeField] private Text itemNameText;
    [SerializeField] private Text itemDescriptionText;


   private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if (inventoryPanel == null)
        {
            Debug.LogError("库存面板未在 Inspector 中赋值");
        }
        // 渲染库存 UI，显示初始库存状态
        RenderInventory();
        AssignSlotIndexes();

        TimeManager.Instance.RegisterTracker(this);

    }

    public void AssignSlotIndexes()
    {
        for(int i = 0; i < toolSlots.Length; i++)
        {
            toolSlots[i].AssignIndex(i);
            itemSlots[i].AssignIndex(i);
        }
    }

    public void RenderInventory()
    {
        if (toolSlots == null || itemSlots == null)
        {
            Debug.LogError("工具槽或物品槽未赋值！");
            return;
        }
        // 从 InventoryManager 获取工具和物品的库存数据
        ItemData[] inventoryToolSlots = InventoryManager.Instance.tools;
        ItemData[] inventoryItemSlots = InventoryManager.Instance.items;
        // 渲染工具槽位 UI，将数据同步到 UI 槽位
        RenderInventoryPanel(inventoryToolSlots, toolSlots);
        // 渲染物品槽位 UI，将数据同步到 UI 槽位
        RenderInventoryPanel(inventoryItemSlots, itemSlots);

        toolHandSlot.Display(InventoryManager.Instance.equippedTool);
        itemHandSlot.Display(InventoryManager.Instance.equippedItem);

        ItemData equippedTool = InventoryManager.Instance.equippedTool;


        if (toolEquipSlot == null)
        {
            Debug.LogError($"itemDisplayImage 未赋值！槽位: {gameObject.name}", gameObject);
            return;
        }
        Debug.Log($"槽位: {gameObject.name}, 物品: {(equippedTool != null ? equippedTool.name : "null")}, Thumbnail: {(equippedTool?.thumbnail != null ? equippedTool.thumbnail.name : "null")}");
        if (equippedTool != null && equippedTool.thumbnail != null)
        {
            toolEquipSlot.sprite = equippedTool.thumbnail;
            toolEquipSlot.color = new Color(1, 1, 1, 1); // 确保不透明

            if (!toolEquipSlot.gameObject.activeSelf)
                toolEquipSlot.gameObject.SetActive(true);
            return;
        }
        if (toolEquipSlot.gameObject.activeSelf)
            toolEquipSlot.gameObject.SetActive(false);

    }

    void RenderInventoryPanel(ItemData[] slots, InventorySlot[] uiSlots)
    {
        for(int i = 0; i < uiSlots.Length; i++)
        {
            if (uiSlots[i] != null)
            {
                ItemData item = (slots != null && i < slots.Length) ? slots[i] : null;
                uiSlots[i].Display(item);
            }
            else
            {
                Debug.LogWarning($"UI 槽位 {i} 未赋值！");
            }
        }
        
    }

    // 切换库存面板的显示/隐藏状态
    public void ToggleInventoryPanel()
    {
        if (inventoryPanel == null)
        {
            Debug.LogError("库存面板未赋值！");
            return;
        }
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        // xuanran
        RenderInventory();
    }

    public void DisplayItemInfo(ItemData data)
    {
        if(data == null)
        {
            itemNameText.text = "";
            itemDescriptionText.text = "";

            return;
        }

        itemNameText.text = data.name;
        itemDescriptionText.text = data.description;
    }

    public void ClockUpdate(GameTimestamp timestamp)
    {


        int hours = timestamp.hour;
        int minutes = timestamp.minute;

        string prefix = "AM";

        if(hours > 12)
        {
            prefix = "PM";
            hours -= 12;
        }

        timeText.text = prefix + hours + ":"+ minutes.ToString("00");

        int day = timestamp.day;
        string season = timestamp.season.ToString();
        string dayOfTheWeek = timestamp.GetDayOfTheWeek().ToString();

        dateText.text = season + " " + day +"(" + dayOfTheWeek +")";

    }

}
 