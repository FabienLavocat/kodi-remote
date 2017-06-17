using System;
using System.Collections.Generic;
using System.Linq;

namespace KodiRemote.Uwp.Core
{
    public static class Group
    {
        public static char GetGroupKey(string value)
        {
            if (value.Length == 0) return '#';

            char first = value.ToLowerInvariant().StartsWith("the ")
                ? value.ToLowerInvariant()[4]
                : value.ToLowerInvariant()[0];

            if (first < 'a' || first > 'z') return '#';

            return first;
        }

        public static List<Group<T>> CreateGroups<T>(IEnumerable<T> itemList, Func<T, char> getKeyFunc)
        {
            const string keys = "#abcdefghijklmnopqrstuvwxyz";

            var groups = keys.Select(key => new Group<T>(key)).ToList();

            foreach (T item in itemList.ToList())
            {
                char key = getKeyFunc(item);
                var group = groups.FirstOrDefault(g => g.Key == key);
                if (group != null) group.Add(item);
            }

            return groups;
        }
    }

    public class Group<T> : Group<T, char>
    {
        public Group(char key)
            : base(key)
        {
        }
    }

    public class Group<T, TKey> : List<T>
    {
        public TKey Key { get; private set; }

        public Group(TKey key)
        {
            Key = key;
        }

        public Group(TKey key, IEnumerable<T> values)
            : base(values)
        {
            Key = key;
        }
    }
}
