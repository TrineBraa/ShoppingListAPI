namespace ShoppingList.Model
{
    public class TodoTask
    {
        public Guid Id { get; set; }
        public string Task { get; set; }
        public DateTime? Done { get; set; }

        public TodoTask(string task) : this()
        {
            Task = task;
        }

        public TodoTask()
        {
            Id = Guid.NewGuid();
        }
    }
}
