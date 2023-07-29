namespace PathCreation.Utility
{
    public static class HandleIds
    {
        private static int nextId = 0;

        public static int NextId
        {
            get { return nextId++; }
        }
    }
}