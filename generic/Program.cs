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


    #region Q6
    // Generic interface  إنترفيس بـي type parameterr الكلاس اللي بينفذه يحدد النوع.
    public interface IEntity
    {
        int Id { get; }
    }

    public interface IRepository<T> where T : IEntity
    {
        void Add(T item);
        T GetById(int id);
        IEnumerable<T> GetAll();
        bool Remove(int id);
    }
    public class InMemoryRepository<T> : IRepository<T> where T : IEntity
    {
        private readonly Dictionary<int, T> _store = new Dictionary<int, T>();

        public void Add(T item) => _store[item.Id] = item;

        public T GetById(int id) => _store.TryGetValue(id, out var item) ? item : default;

        public IEnumerable<T> GetAll() => _store.Values;

        public bool Remove(int id) => _store.Remove(id);
    }
    #endregion

    #region Q7
    // where T : struct  T لازم يكون Value Type int, double
    public class NumberBox<T> where T : struct
    {
        public T Value { get; set; }

        public NumberBox(T value) => Value = value;

        public bool IsDefault() => EqualityComparer<T>.Default.Equals(Value, default(T));


        public T? ToNullable() => Value;
    }
    #endregion

    #region Q8
    // where T : class   T لازم يكون Reference Type class, interface
    public class ReferenceChecker<T> where T : class
    {
        public bool IsNull(T item) => item == null;   // المقارنة بـ null مسموحة هنا

        public T OrDefault(T item, T fallback) => item ?? fallback;
    }
    #endregion

    #region Q9
    // where T : new()   T لازم يكون ليه Public parameterless
    public class Factory<T> where T : new()
    {
        public T Create() => new T();

        public List<T> CreateMany(int count)
        {
            var list = new List<T>();
            for (int i = 0; i < count; i++) list.Add(new T());
            return list;
        }
    }
    #endregion

    #region Q10
    // where T : IShape  => T لازم ينفذ الإنترفيس ده، فنقدر نستدعي أعضاءه.
    public interface IShape
    {
        double Area();
    }

    public class Circle : IShape
    {
        public double Radius { get; set; }
        public Circle(double r) => Radius = r;
        public double Area() => Math.PI * Radius * Radius;
    }

    public class Rectangle : IShape
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public Rectangle(double w, double h) { Width = w; Height = h; }
        public double Area() => Width * Height;
    }

    public class ShapePrinter<T> where T : IShape
    {
        public void Print(T shape) => Console.WriteLine($"{typeof(T).Name} area = {shape.Area():F2}");
    }
    #endregion

    #region Q11
    // where T : Animal  => T لازم يكون Animal أو كلاس بيورث منه.
    public class Animal
    {
        public string Name { get; set; }
        public virtual string Speak() => "...";
    }

    public class Dog : Animal
    {
        public override string Speak() => "Woof!";
    }

    public class Cat : Animal
    {
        public override string Speak() => "Meow!";
    }

    public class Kennel<T> where T : Animal
    {
        private readonly List<T> _animals = new List<T>();

        public void Add(T animal) => _animals.Add(animal);

        public void MakeAllSpeak()
        {
            foreach (var a in _animals)
                Console.WriteLine($"{a.Name}: {a.Speak()}");
        }
    }
    #endregion

    #region Q12: Multiple constraints
    // بنفصل الـ constraints بفاصلة. الترتيب: class ,struct base class  interfaces  new()
    public class BaseEntity
    {
        public int Id { get; set; }
    }

    public interface IValidatable
    {
        bool IsValid();
    }

    public class Product : BaseEntity, IValidatable
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public bool IsValid() => !string.IsNullOrWhiteSpace(Name) && Price > 0;
    }

    public class EntityManager<T> where T : BaseEntity, IValidatable, new()
    {
        private readonly List<T> _items = new List<T>();

        public T CreateEmpty()
        {
            var item = new T();
            item.Id = _items.Count + 1;
            return item;
        }

        public bool TryAdd(T item)
        {
            if (!item.IsValid()) return false;
            _items.Add(item);
            return true;
        }
    }
    public class Converter<TIn, TOut>
        where TIn : struct
        where TOut : class, new()
    {
        public TOut Create() => new TOut();
    }
    #endregion

    #region Q13: The 'default' keyword
    /*
     * default(T) أو default: بترجّع القيمة الافتراضية للنوع T
     *   - Reference types: null
     *   - int/double.. :0
     *   - bool: false
     *   - struct: كل الحقول بقيمها الافتراضية
     */
    #endregion

    #region Q14
    public class SafeList<T>
    {
        private readonly List<T> _list = new List<T>();

        public void Add(T item) => _list.Add(item);

        public int Count => _list.Count;

        public T this[int index]
        {
            get => (index >= 0 && index < _list.Count) ? _list[index] : default;
        }

        public T Get(int index) => this[index];
    }
    #endregion

    #region Q15: Covariance 
    /*
     * Covariance:
     * معناها إننا نقدر نستخدم Generic Type فيه نوع مشتق
     *
     * مثال:
     * Dog هو نوع مشتق من Animal.
     *
     * IProducer<Dog>
     * ممكن نستخدمه مكان:
     * IProducer<Animal>
     *
     * كلمة out:
     * معناها إن T بيتم استخدامه كـ Output فقط،
     * يعني بنرجعه من Method أو Property،
     * ومينفعش نستخدم T كـ Parameter في Method.
     *
     * مثال:
     * interface IProducer<out T>
     * {
     *     T Get();   // صح
     * }
     */
    public interface IProducer<out T>
    {
        T Produce();
    }

    public class DogProducer : IProducer<Dog>
    {
        public Dog Produce() => new Dog { Name = "Rex" };
    }
    #endregion

    #region Q16: Contravariance
    /*
     * Contravariance:
     * معناها إننا نقدر نستخدم Generic Type فيه نوع أساسي
     * مثال:
     * Animal هو النوع الأساسي، و Dog نوع مشتق منه.
     *
     * IConsumer<Animal>
     * ممكن نستخدمه مكان:
     * IConsumer<Dog>
     *
     * كلمة in:
     * معناها إن T بيتم استخدامه كـ Input فقط،
     * يعني T بيكون Parameter في Method،
     * ومينفعش يكون Return Type.
     *
     * ببساطة:
     * Contravariance = in
     * in = البيانات بتدخل
     * Base → Derived
     */
    public interface IConsumer<in T>
    {
        void Consume(T item);
    }

    public class AnimalConsumer : IConsumer<Animal>
    {
        public void Consume(Animal item) => Console.WriteLine($"Consuming animal: {item.Name}");
    }
    #endregion
    #region Q17: Covariance vs Contravariance
    /*
     * الفرق بين Covariance و Contravariance:
     *
     * Covariance:
     * out → Output → Derived -> Base
     *
     * Contravariance:
     * in → Input → Base - Derived
     *
     * مثال:
     * Animal = Base
     * Dog    = Derived
     *
     * Covariance:
     * IEnumerable<Dog> - IEnumerable<Animal>
     *
     * Contravariance:
     * Action<Animal> - Action<Dog>
     *
     *
     * Invariance:
     * لو مفيش out ولا in،
     * لازم النوع يكون مطابق بالظبط.
     *
     * مثال:
     * List<Dog> مش بتتحول إلى List<Animal>
     *
     * لأن List<T> تقدر:
     * - تقرأ T
     * - وتضيف T
     *
     * لذلك لازم النوع يتطابق بالظبط.
     *
     *
     * ملاحظة:
     * Variance بتشتغل مع:
     * - Interfaces
     * - Delegates
     *
     * وكمان مع Reference Types.
     */
    public static class VarianceDemo
    {
        public static void Run()
        {
            
            IProducer<Dog> dogProducer = new DogProducer();
            IProducer<Animal> animalProducer = dogProducer;
            Console.WriteLine("Produced: " + animalProducer.Produce().Name);
            IEnumerable<Dog> dogs = new List<Dog> { new Dog { Name = "A" } };
            IEnumerable<Animal> animals = dogs;
            IConsumer<Animal> animalConsumer = new AnimalConsumer();
            IConsumer<Dog> dogConsumer = animalConsumer;
            dogConsumer.Consume(new Dog { Name = "Buddy" });
            Action<Animal> actAnimal = a => Console.WriteLine("Action on " + a.Name);
            Action<Dog> actDog = actAnimal;
            actDog(new Dog { Name = "Max" });
        }
    }
    #endregion
    #region Q18
    public class Counter<T>
    {
        public static int Count;

        public Counter() => Count++;
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
