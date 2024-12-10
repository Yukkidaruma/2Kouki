using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum ITEM_TYPE
//{
//    NON,        //�����l
//    FOOD,
//    RECOVERY,
//    WEAPON,
//}

//public enum ITEM_ID
//{
//    NON,        //�����l
//    FOOD_1,
//    FOOD_2,
//    FOOD_3,
//    FOOD_4,
//    DRINK_1,
//    DRINK_2,
//    EMERGENCY_PACK,
//    BULLET,
//    PISTOL,
//    ASSAULT,
//    SHOTGUN,
//    HAND,
//    KNIFE,
//    DOG_DIRECTION,
//}

//[System.Serializable]
//public class ItemInformation
//{
//    public ITEM_TYPE type;
//    public ITEM_ID id;
//    public int get_num;
//    public int stack_max;
//    public Sprite sprite;

//    //�񕜃A�C�e���i���H�A�̗́j
//    public RecoveryItemInformation recoveryitem_info;
//    //����A�C�e��
//    public WeaponItemInformation weaponitem_info;


//    //�R���X�g���N�^(�����Ȃ�)
//    public ItemInformation()
//    {
//        type        = ITEM_TYPE.NON;
//        id          = ITEM_ID.NON;
//        get_num     = 0;
//        stack_max   = 0;
//        sprite      = null;
//    }
//    public ItemInformation(ItemInformation _item)
//    {
//        type = _item.type;
//        id = _item.id;
//        get_num = _item.get_num;
//        stack_max = _item.stack_max;
//        sprite = _item.sprite;
//    }

//    //�R���X�g���N�^(��ށAID�A�擾�\���A�X�^�b�N�\���A�摜)
//    public ItemInformation(ITEM_TYPE _type, ITEM_ID _id, int _get_num, int _stack_max, Sprite _sprite)
//    {
//        type        = _type;
//        id          = _id;
//        get_num     = _get_num;
//        stack_max   = _stack_max;
//        sprite      = _sprite;
//    }

//    //�R���X�g���N�^(��ށAID�A�擾�\���A�X�^�b�N�\���A�摜�A��ޕʈ����i�񕜐�or�e���j)
//    public ItemInformation(ITEM_TYPE _type, ITEM_ID _id, int _get_num, int _stack_max, Sprite _sprite, int _num)
//    {
//        type        = _type;
//        id          = _id;
//        get_num     = _get_num;
//        stack_max   = _stack_max;
//        sprite      = _sprite;

//        switch(type)
//        {
//            case ITEM_TYPE.FOOD:
//            case ITEM_TYPE.RECOVERY:
//                recoveryitem_info = new RecoveryItemInformation(_num);
//                break;
//            case ITEM_TYPE.WEAPON:
//                weaponitem_info = new WeaponItemInformation(_num);
//                break;
//        }
//    }

//    public ItemInformation GetItemInfo()
//    {
//        return new ItemInformation();
//    }


//    public void DebugLog()
//    {
//        for (int sloat = 0; sloat < 10; sloat++)
//        {
//            Debug.Log(type);
//            Debug.Log(id);
//            Debug.Log(get_num);
//            Debug.Log(stack_max);
//            Debug.Log(sprite);
//            if (type== ITEM_TYPE.FOOD)
//            {
//                Debug.Log(recoveryitem_info);
//                Debug.Log(recoveryitem_info.recovery_num);
//            }
//            if (type == ITEM_TYPE.WEAPON)
//            {
//                Debug.Log(weaponitem_info);
//                Debug.Log(weaponitem_info.bullet_num);
//            }
//        }
//    }
//}

public enum ITEM_TYPE
{
    NON,
    FOOD,
    RECOVERY,
    WEAPON,
}

public enum ITEM_ID
{
    NON,
    FOOD_1,
    FOOD_2,
    FOOD_3,
    FOOD_4,
    DRINK_1,
    DRINK_2,
    EMERGENCY_PACK,
    BULLET,
    PISTOL,
    ASSAULT,
    SHOTGUN,
    HAND,
    KNIFE,
    DOG_DIRECTION,
}

[System.Serializable]
public class ItemInformation
{
    public ITEM_TYPE type;
    public ITEM_ID id;
    public int get_num;
    public int stack_max;
    public Sprite sprite;

    public RecoveryItemInformation recoveryitem_info;
    public WeaponItemInformation weaponitem_info;

    public ItemInformation() { }

    public ItemInformation(ITEM_TYPE _type, ITEM_ID _id, int _get_num, int _stack_max, Sprite _sprite)
    {
        type = _type;
        id = _id;
        get_num = _get_num;
        stack_max = _stack_max;
        sprite = _sprite;
    }

    public ItemInformation(ITEM_TYPE _type, ITEM_ID _id, int _get_num, int _stack_max, Sprite _sprite, int _num)
    {
        type = _type;
        id = _id;
        get_num = _get_num;
        stack_max = _stack_max;
        sprite = _sprite;

        switch (type)
        {
            case ITEM_TYPE.FOOD:
            case ITEM_TYPE.RECOVERY:
                recoveryitem_info = new RecoveryItemInformation(_num);
                break;
            case ITEM_TYPE.WEAPON:
                weaponitem_info = new WeaponItemInformation(_num);
                break;
        }
    }

    public int AddGetItem(int _get_num,int _stack_max)
    {
        int add_num = 0;//��������

        while (add_num != _stack_max)
        {
            _get_num--;
            add_num++;

            if (_get_num == 0) return 0;
        }

        //�c��������Ԃ�
        return get_num = _get_num;
    }
}

[System.Serializable]
public class RecoveryItemInformation
{
    public int recovery_num = -1;
    public RecoveryItemInformation(int _recovery_num)
    {
        recovery_num = _recovery_num;
    }
}

[System.Serializable]
public class WeaponItemInformation
{
    public int bullet_num = -1;
    public WeaponItemInformation(int _bullet_num)
    {
        bullet_num = _bullet_num;
    }
}

public class ItemPickup : MonoBehaviour 
{ 
    public ItemInformation ItemInfo; 
}