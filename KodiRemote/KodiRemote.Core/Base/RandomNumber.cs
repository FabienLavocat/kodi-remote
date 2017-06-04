using System;

namespace KodiRemote.Core.Base
{
    internal static class RandomNumber
    {
        private static readonly Random Random = new Random();
        private static readonly object _lock = new object();

        internal static int GetRandomNumber(int min, int max)
        {
            lock (_lock)
            {
                return Random.Next(min, max);
            }
        }
    }
}