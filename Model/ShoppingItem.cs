namespace ShoppingList.Model
{
    public class ShoppingItem
    {
        public Guid id { get; set; }
        public string Shopping { get; set; }

        public ShoppingItem(string shopping) : this()
        {
            Shopping = shopping;
        }

        public ShoppingItem()
        {
            id = Guid.NewGuid();
        }
    }
}
