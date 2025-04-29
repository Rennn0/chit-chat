using System.Collections;

namespace API.Source.Libs;

public class CircularList<T> : IEnumerable<T>
{
    private readonly LinkedList<T> m_list = [];
    private const int c_cap = 5;

    public void Add(T item)
    {
        lock (this)
        {
            if (m_list.Count >= c_cap)
            {
                m_list.RemoveFirst();
            }

            m_list.AddLast(item);
        }
    }

    public IReadOnlyCollection<T> List => m_list;

    public IEnumerator<T> GetEnumerator() =>
        m_list.Count == 0 ? Enumerable.Empty<T>().GetEnumerator() : m_list.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
