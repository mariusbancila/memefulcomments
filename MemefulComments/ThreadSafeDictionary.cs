using System.Collections;
using System.Collections.Generic;

namespace MemefulComments
{
   class ThreadSafeDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
   {
      Dictionary<TKey, TValue> _dictionary;
      object locker = new object();

      public ThreadSafeDictionary()
      {
         _dictionary = new Dictionary<TKey, TValue>();
      }
      
      public ThreadSafeDictionary(int capacity)
      {
         _dictionary = new Dictionary<TKey, TValue>(capacity);
      }      

      public ThreadSafeDictionary(IEqualityComparer<TKey> comparer)
      {
         _dictionary = new Dictionary<TKey, TValue>(comparer);
      }
      
      public ThreadSafeDictionary(IDictionary<TKey, TValue> dictionary)
      {
         _dictionary = new Dictionary<TKey, TValue>(dictionary);
      }
      
      public ThreadSafeDictionary(int capacity, IEqualityComparer<TKey> comparer)
      {
         _dictionary = new Dictionary<TKey, TValue>(capacity, comparer);
      }
      
      public ThreadSafeDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
      {
         _dictionary = new Dictionary<TKey, TValue>(dictionary, comparer);
      }

      public Dictionary<TKey, TValue>.ValueCollection Values
      {
         get
         {
            lock(locker)
            {
               return _dictionary.Values;
            }
         }
      }

      public Dictionary<TKey, TValue>.KeyCollection Keys
      {
         get
         {
            lock(locker)
            {
               return _dictionary.Keys;
            }
         }
      }

      public int Count
      {
         get
         {
            lock (locker)
            {
               return _dictionary.Count;
            }
         }
      }

      public TValue this[TKey key]
      {
         get
         {
            lock (locker)
            {
               return _dictionary[key];
            }
         }

         set
         {
            lock(locker)
            {
               _dictionary[key] = value;
            }
         }
      }

      public void Add(TKey key, TValue value)
      {
         lock(locker)
         {
            _dictionary.Add(key, value);
         }
      }

      public void Clear()
      {
         lock (locker)
         {
            _dictionary.Clear();
         }
      }

      public bool ContainsKey(TKey key)
      {
         lock (locker)
         {
            return _dictionary.ContainsKey(key);
         }
      }

      public bool ContainsValue(TValue value)
      {
         lock (locker)
         {
            return _dictionary.ContainsValue(value);
         }
      }

      public bool Remove(TKey key)
      {
         lock (locker)
         {
            return _dictionary.Remove(key);
         }
      }

      public bool TryGetValue(TKey key, out TValue value)
      {
         lock (locker)
         {
            return _dictionary.TryGetValue(key, out value);
         }
      }

      IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
      {
         lock(locker)
         {
            foreach (var kvp in _dictionary)
               yield return kvp;
         }
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
         return ((IEnumerable<KeyValuePair<TKey, TValue>>)this).GetEnumerator();
      }
   }
}
