namespace generic
{
    #region Q2
    public class Container<T>
    {
        private readonly List<T> _items = new List<T>();

        public void Add(T item) => _items.Add(item);

        public T Get(int index)
        {
            if (index < 0 || index >= _items.Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            return _items[index];
        }

        public int Count => _items.Count;
    }
    #endregion
    internal class Program
    {
        static void Main(string[] args)
        {
            #region Q1: What is a generic class? Why use generics?
            /*
             * A generic class is a class that uses a type parameter such as <T>
             * instead of a specific type
             * 
             * 1) Type safety
             * 2) Code reusability
             * 3) Performance
             * 4) No casting
             */
            #endregion

        }
    }
}
