namespace Contellation.ViewModels
{
    public class TestVM
    {
        private List<string> stringss = new List<string>();
        public List<string> strings { get => stringss; set => stringss = value; }
        public TestVM() 
        {
            strings.Add("aaa");
            strings.Add("aaB");
            strings.Add("aaV");
            strings.Add("aaD");
            strings.Add("aaC");
        }
    }
}
