using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum INVENTORY
{
    NON,
    ITEM,
    CHEST,
    WEAPON,
}

public class inventoryManager : MonoBehaviour
{
    InventoryItem InventoryItem;
    WeaponInventory WeaponInventory;
    ChestInventory[] ChestInventory;

    //定数
    const int GUN_SLOT = 3;

    //インベントリ情報を持っているオブジェクト
    public GameObject player_obj;
    public GameObject[] chest_inventory;
    //インベントリの状態(閉じている、どのインベントリを開いているか)
    public INVENTORY inventory_state = INVENTORY.NON;

    //アイテム移動
    SELECT_SLOAT can_catch_slot;     //掴むことが可能なスロットの情報
    SELECT_SLOAT catch_slot;         //掴んでいるスロットの情報
    SELECT_SLOAT destination_slot;   //移動先のスロット情報

    /// <summary>
    /// どのスロットを選択しているのかを保存
    /// </summary>
    struct SELECT_SLOAT
    {
        public GameObject sloat_obj;   //掴むオブジェクト
        public int slot_no;            //スロットの位置
        public int slot_inventory;     //どのインベントリのスロットか
        public int chest_no;           //どのチェストか
    }

    // Start is called before the first frame update
    void Start()
    {
        InventoryItem = player_obj.GetComponent<InventoryItem>();
        WeaponInventory = player_obj.GetComponent<WeaponInventory>();

        //ChestInventory = new ChestInventory[chest_inventory.Length];
        //for (int i = 0; i < chest_inventory.Length; i++)
        //{
        //    ChestInventory[i] = ChestInventory[i].GetComponent<ChestInventory>();
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (inventory_state!=INVENTORY.NON)
        {
            CheckInventoryItem();
            MoveItem();
        }
    }

    void CheckInventoryItem()    //カーソルのあっているアイテムを調べる
    {
        SlotInfoInitialization(ref destination_slot);
        SlotInfoInitialization(ref can_catch_slot);

        //掴むアイテムを決定
        foreach (RaycastResult result in HitResult())
        {
            //アイテムをつかんでいない
            if (can_catch_slot.slot_inventory == (int)INVENTORY.NON)
            {
                //アイテムインベントリ
                for (int i = 0; i < InventoryItem.slot_size; i++)
                {
                    if (result.gameObject == InventoryItem.sprite[i].gameObject)
                    {
                        if (InventoryItem.Inventory.Slots[i].ItemInfo != null)
                        {
                            Debug.Log(InventoryItem.Inventory.Slots[i].ItemInfo + " " + i);
                        }
                        else
                        {
                            Debug.Log("null " + i);
                        }

                        can_catch_slot.sloat_obj = InventoryItem.sprite[i].gameObject;
                        can_catch_slot.slot_no = i;
                        can_catch_slot.slot_inventory = (int)INVENTORY.ITEM;
                        break;
                    }
                }

                ////チェストインベントリ
                //for (int j = 0; j < chest_inventory.Length; j++)
                //{
                //    for (int i = 0; i < ChestInventory[j].sloat_size; i++)
                //    {
                //        if (result.gameObject == ChestInventory[j].sprite_pos[i].gameObject)
                //        {
                //            can_catch_slot.sloat_obj = InventoryItem.sprite[i].gameObject;
                //            can_catch_slot.slot_no = i;
                //            can_catch_slot.chest_no = j;
                //            can_catch_slot.slot_inventory = (int)INVENTORY.CHEST;
                //            break;
                //        }
                //    }
                //    if (can_catch_slot.slot_inventory != (int)INVENTORY.NON) break;
                //}
            }
        }
        foreach (RaycastResult result in HitResult())
        {
            //アイテムをつかんでいる

            //アイテムインベントリ
            for (int i = 0; i < InventoryItem.slot_size; i++)
            {
                if (result.gameObject == InventoryItem.slot_box[i].gameObject)
                {
                    destination_slot.sloat_obj = InventoryItem.slot_box[i].gameObject;
                    destination_slot.slot_no = i;
                    destination_slot.slot_inventory = (int)INVENTORY.ITEM;
                    break;
                }
            }

            ////武器インベントリ
            //if (result.gameObject == WeaponInventory.sloat_box[GUN_SLOT].gameObject)
            //{
            //    destination_slot.sloat_obj = WeaponInventory.sloat_box[GUN_SLOT].gameObject;
            //    destination_slot.slot_no = GUN_SLOT;
            //    destination_slot.slot_inventory = (int)INVENTORY.WEAPON;
            //    break;
            //}

            ////チェストインベントリ
            //for (int j = 0; j < chest_inventory.Length; j++)
            //{
            //    for (int i = 0; i < ChestInventory[j].sloat_size; i++)
            //    {
            //        if (result.gameObject == ChestInventory[j].slot_box[i].gameObject)
            //        {
            //            destination_slot.sloat_obj = ChestInventory[j].slot_box[i].gameObject;
            //            destination_slot.slot_no = i;
            //            destination_slot.chest_no = j;
            //            destination_slot.slot_inventory = (int)INVENTORY.CHEST;
            //            break;
            //        }
            //    }

            //    if (destination_slot.slot_inventory != (int)INVENTORY.NON) break;
            //}
        }
    
    }


    /// <summary>
    /// アイテム移動
    /// CheckInventoryItem()で掴めるアイテムを取得した後で使う処理
    /// 移動させるアイテム選択、選択したアイテムを移動
    /// 移動先によって少し処理が異なる
    /// 中身が何もなければそのまま移動
    /// 同じアイテム同士の場合スタック上限でなければスタックし、残った場合は残った個数を所持したまま元の場所へ戻す
    /// 異なるアイテム同士の場合、場所を入れ替える
    /// 武器の場合、アイテムを入れ替える
    /// </summary>
    void MoveItem()
    {
        //左クリックされたら情報を入れる
        if(Input.GetMouseButtonDown(0))
        {
            catch_slot = can_catch_slot;
        }

        //情報がなければ終了
        if (catch_slot.slot_inventory == (int)INVENTORY.NON) return;

        //左クリック長押しの間マウスに追従
        if (Input.GetMouseButton(0))
        {
            catch_slot.sloat_obj.transform.position = Input.mousePosition;
        }
        else
        {
            //クリックが離されたら

            //掴んでいるスロットのインベントリがアイテムインベントリ
            if (catch_slot.slot_inventory == (int)INVENTORY.ITEM)
            {
                //オブジェクトは元の位置に、情報だけ渡す
                catch_slot.sloat_obj.transform.position = InventoryItem.slot_box[catch_slot.slot_no].position;

                //移動先がアイテムインベントリ
                if (destination_slot.slot_inventory == (int)INVENTORY.ITEM)
                {
                    //アイテムを重ねられる場合の処理
                    if (InventoryItem.Inventory.Slots[destination_slot.slot_no].CanAddStackItem(InventoryItem.Inventory.Slots[catch_slot.slot_no]))
                    {
                        InventoryItem.Inventory.Slots[destination_slot.slot_no].AddStackItem(ref InventoryItem.Inventory.Slots[catch_slot.slot_no]);
                    }
                    else
                    {
                        //できない場合の処理
                        ItemInfoChange(ref InventoryItem.Inventory.Slots[catch_slot.slot_no], ref InventoryItem.Inventory.Slots[destination_slot.slot_no]);
                    }
                }
                else if (destination_slot.slot_inventory == (int)INVENTORY.WEAPON)
                {
                    ItemInfoChange(ref InventoryItem.Inventory.Slots[catch_slot.slot_no], ref WeaponInventory.InventoryClass.Slots[destination_slot.slot_no]);
                }
                else if (destination_slot.slot_inventory == (int)INVENTORY.CHEST)
                {
                    //アイテムを重ねられる場合の処理

                    //できない場合の処理
                    ItemInfoChange(ref InventoryItem.Inventory.Slots[catch_slot.slot_no], ref ChestInventory[destination_slot.chest_no].InventoryClass.Slots[destination_slot.slot_no]);
                }

            }

            SlotInfoInitialization(ref catch_slot);
        }
    }

    /// <summary>
    /// アイテム情報入れ替え
    /// スロットの情報を入れ替える
    /// </summary>
    /// <param name="_slot1">入れ替えたいスロット１つ目</param>
    /// <param name="_slot2">入れ替えたいスロット２つ目</param>
    void ItemInfoChange(ref SlotClass _slot1 ,ref SlotClass _slot2)
    {
        SlotClass temp = _slot1;
        _slot1 = _slot2;
        _slot2 = temp;
    }

    /// <summary>
    /// スロット情報初期化
    /// 選んでいたスロットの中身を初期化
    /// </summary>
    /// <param name="_slot">初期化したい中身</param>
    void SlotInfoInitialization(ref SELECT_SLOAT _slot)
    {
        //初期化
        _slot.sloat_obj = null;
        _slot.slot_inventory = (int)INVENTORY.NON;
        _slot.slot_no = -1;
        _slot.chest_no = -1;
    }

    /// <summary>
    /// ヒット結果
    /// レイが当たっているオブジェクトを取得
    /// </summary>
    /// <returns>当たったオブジェクトをリストで返す</returns>
    public List<RaycastResult> HitResult()
    {
        //マウスの位置からUIを取得する
        //RaycastAllの引数（PointerEventData）作成
        PointerEventData pointData = new PointerEventData(EventSystem.current);
        //RaycastAllの結果格納用List
        List<RaycastResult> RayResult = new List<RaycastResult>();

        //PointerEventDataにマウスの位置をセット
        pointData.position = Input.mousePosition;

        //RayCast（スクリーン座標）
        EventSystem.current.RaycastAll(pointData, RayResult);

        return RayResult;
    }

}
