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
    #region Q3: Multiple 

    public class Pair<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }

        public Pair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public override string ToString() => $"({Key}, {Value})";
    }
    #endregion
    #region Q4 & Q5: Generic methods - Swap<T> and FindMax<T>
    /*
     * A generic method has its own type parameter. 
     */
    public static class GenericMethods
    {
        // Q4: swaps two values 
        public static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }

        // Q5: T must implement <T> so that values can be compared
        public static T FindMax<T>(IEnumerable<T> items) where T : IComparable<T>
        {
            using (var e = items.GetEnumerator())
            {
                if (!e.MoveNext())
                    throw new InvalidOperationException("Sequence is empty.");

                T max = e.Current;
                while (e.MoveNext())
                {
                    if (e.Current.CompareTo(max) > 0)
                        max = e.Current;
                }
                return max;
            }
        }
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
