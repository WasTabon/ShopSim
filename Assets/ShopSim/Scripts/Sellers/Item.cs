using UnityEngine;

namespace ShopSim.Scripts.Sellers
{
    public class Item
    {
        private Sprite _icon;
        private ItemRarity _rarity;
        private int _price;

        public Sprite GetIcon() => _icon;
        public ItemRarity GetRarity() => _rarity;
        public int GetPrice() => _price;
        
        public Item(Sprite icon, ItemRarity rarity, int price)
        {
            _icon = icon;
            _rarity = rarity;
            _price = price;
        }
    }
}
