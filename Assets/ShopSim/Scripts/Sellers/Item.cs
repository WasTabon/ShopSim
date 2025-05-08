using UnityEngine;

namespace ShopSim.Scripts.Sellers
{
    public class Item
    {
        private Sprite _icon;
        private ItemRarity _rarity;
        private int _price;
        private bool _isFake;
        private bool _isDirty;

        public Sprite GetIcon() => _icon;
        public ItemRarity GetRarity() => _rarity;
        public int GetPrice() => _price;
        public string GetName() => _icon.name;
        public bool GetFake() => _isFake;
        public bool GetDirty() => _isDirty;
        
        public Item(Sprite icon, ItemRarity rarity, int price, bool isFake, bool isDirty)
        {
            _icon = icon;
            _rarity = rarity;
            _price = price;
            _isFake = isFake;
            _isDirty = isDirty;
        }

        public void SetPrice(int price)
        {
            _price = price;
        }
    }
}
