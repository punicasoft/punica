namespace ExpressionDynamicTest.Parsing.Models
{
    public class Person
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public DateTime Dob { get; set; }
        public int NoOfChildren { get; set; }
        public bool IsMarried { get; set; }
        public bool IsMale { get; set;}
        public Account Account { get; set; }

        public List<Child> Childrens { get; set; }
    }
}
