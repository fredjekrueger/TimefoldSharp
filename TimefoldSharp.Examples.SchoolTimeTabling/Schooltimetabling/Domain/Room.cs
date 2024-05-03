namespace TimefoldSharp.Schooltimetabling.Domain
{
    public class Room
    {
        private string name;

        public Room(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }

        public string GetName()
        {
            return name;
        }

    }
}