namespace TaskManager
{
    public class CCustomer
    {
        public int ID { get; set; }
        public string Name { get; set; }
        
        public CCustomer(int iD, string name)
        {
            ID = iD;
            Name = name;
        }
    }
}
