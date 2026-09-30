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


    #region Q19
    // 1) الأب generic والابن Concrete 
    // 2) الأب generic والابن generic 
    // 3) الابن بيضيف Type Parameters 
    public class BaseRepo<T>
    {
        protected List<T> Items = new List<T>();
        public virtual void Add(T item) => Items.Add(item);
        public int Count => Items.Count;
    }
    public class StringRepo : BaseRepo<string>
    {
        public override void Add(string item) => base.Add(item.ToUpper());
    }
    public class LoggingRepo<T> : BaseRepo<T>
    {
        public override void Add(T item)
        {
            Console.WriteLine($"Adding {item}");
            base.Add(item);
        }
    }

    public class MetaRepo<T, TMeta> : BaseRepo<T>
    {
        public TMeta Metadata { get; set; }
    }
    #endregion

    #region Q20
    public class Cache<TKey, TValue>
    {
        private class CacheItem
        {
            public TValue Value { get; set; }
            public DateTime ExpiresAt { get; set; }
            public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        }

        private readonly Dictionary<TKey, CacheItem> _store = new Dictionary<TKey, CacheItem>();
        private readonly TimeSpan _defaultExpiration;

        public Cache() : this(TimeSpan.FromMinutes(5)) { }

        public Cache(TimeSpan defaultExpiration)
        {
            _defaultExpiration = defaultExpiration;
        }

        public void Add(TKey key, TValue value, TimeSpan? expiration = null)
        {
            _store[key] = new CacheItem
            {
                Value = value,
                ExpiresAt = DateTime.UtcNow + (expiration ?? _defaultExpiration)
            };
        }
        public bool TryGet(TKey key, out TValue value)
        {
            if (_store.TryGetValue(key, out var item))
            {
                if (!item.IsExpired)
                {
                    value = item.Value;
                    return true;
                }
                _store.Remove(key);
            }
            value = default;
            return false;
        }
        public TValue Get(TKey key) => TryGet(key, out var v) ? v : default;

        public bool Remove(TKey key) => _store.Remove(key);

        public bool Contains(TKey key) => TryGet(key, out _);

        public int RemoveExpired()
        {
            var expired = new List<TKey>();
            foreach (var kv in _store)
                if (kv.Value.IsExpired) expired.Add(kv.Key);

            foreach (var k in expired) _store.Remove(k);
            return expired.Count;
        }

        public int Count => _store.Count;
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

            var c = new Container<int>();
            c.Add(10); c.Add(20);
            Console.WriteLine($"Q2: {c.Get(1)}");


            var p = new Pair<string, int>("Age", 25);
            Console.WriteLine($"Q3: {p}");

            int a = 1, b = 2;
            GenericMethods.Swap(ref a, ref b);
            Console.WriteLine($"Q4: a={a}, b={b}");
            Console.WriteLine($"Q5: {GenericMethods.FindMax(new[] { 3, 9, 4 })}");
            Console.WriteLine($"Q5: {GenericMethods.FindMax(new[] { "apple", "pear", "banana" })}");
            var repo = new InMemoryRepository<Product>();
            repo.Add(new Product { Id = 1, Name = "Pen", Price = 5 });
            Console.WriteLine($"Q6: {repo.GetById(1).Name}, missing => {(repo.GetById(99) == null ? "null" : "found")}");

            Console.WriteLine($"Q7: {new NumberBox<int>(0).IsDefault()}");
            Console.WriteLine($"Q8: {new ReferenceChecker<string>().IsNull(null)}");
            Console.WriteLine($"Q9: {new Factory<List<int>>().Create().Count}");
            new ShapePrinter<Circle>().Print(new Circle(2));                   
            var kennel = new Kennel<Dog>();                                    
            kennel.Add(new Dog { Name = "Rex" });
            kennel.MakeAllSpeak();
            var mgr = new EntityManager<Product>();                           
            var prod = mgr.CreateEmpty();
            Console.WriteLine($"Q12: empty added? {mgr.TryAdd(prod)}");
            prod.Name = "Book"; prod.Price = 20;
            Console.WriteLine($"Q12: valid added? {mgr.TryAdd(prod)}");
            Console.WriteLine($"Q13: int={DefaultDemo.GetDefault<int>()}, string={(DefaultDemo.GetDefault<string>() ?? "null")}, bool={DefaultDemo.GetDefault<bool>()}");
            var safe = new SafeList<int>();
            safe.Add(5);
            Console.WriteLine($"Q14: [0]={safe[0]}, [10]={safe[10]}");
            VarianceDemo.Run();
            new Counter<int>(); new Counter<int>(); new Counter<string>();
            Console.WriteLine($"Q18: int={Counter<int>.Count}, string={Counter<string>.Count}");
            var sr = new StringRepo(); sr.Add("abc");
            var lr = new LoggingRepo<int>(); lr.Add(42);
            Console.WriteLine($"Q19: {sr.Count}, {lr.Count}");
            var cache = new Cache<string, string>(TimeSpan.FromSeconds(1));
            cache.Add("user", "Ahmed");
            cache.Add("temp", "x", TimeSpan.FromMilliseconds(100));
            Console.WriteLine($"Q20: Contains(user)={cache.Contains("user")}, Get={cache.Get("user")}");
            System.Threading.Thread.Sleep(200);
            Console.WriteLine($"Q20: temp expired => Contains={cache.Contains("temp")}");
            Console.WriteLine($"Q20: Remove(user)={cache.Remove("user")}");

        }
    }
}
