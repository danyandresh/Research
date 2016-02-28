using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CSharpCoffeeMaker
{
    public class CyclicIterator<T>
    {
        private IEnumerator<T> _head;
        private readonly Func<IEnumerator<T>> _closeLoop;

        public CyclicIterator(IEnumerator<T> iterator, Func<IEnumerator<T>> closeLoop)
        {
            _head = iterator;
            _closeLoop = closeLoop;
        }

        public T MoveNext()
        {
            if (!_head.MoveNext())
            {
                _head = _closeLoop();
                _head.MoveNext();
            }

            return _head.Current;
        }
    }

    public static class EnumeratorExtensions
    {
        public static IEnumerator<T> Seek<T>(this IEnumerator<T> source, Func<T, bool> predicate)
        {
            while (source.MoveNext())
            {
                if (predicate(source.Current))
                {
                    break;
                }
            }

            return source;
        }

        public static IReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> source)
        {
            var list = source.ToList();
            var result = new ReadOnlyCollection<T>(list);

            return result;
        }
    }

    public static class EnumExtensions
    {
        public static T GetNextState<T>(this T currentValue)
        {
            var typeofT = typeof(T);
            if (!typeofT.IsEnum)
            {
                throw new ArgumentException(string.Format("{0} is not an enum", typeofT));
            }

            var valuesEnumeration = Enum.GetValues(typeofT)
                .OfType<T>().ToReadOnlyCollection();
            var valuesIterator = valuesEnumeration.GetEnumerator();

            valuesIterator = valuesIterator.Seek(x => x.Equals(currentValue));

            var cyclic = new CyclicIterator<T>(valuesIterator, valuesEnumeration.GetEnumerator);
            var nextValue = cyclic.MoveNext();

            return nextValue;
        }

    }
}