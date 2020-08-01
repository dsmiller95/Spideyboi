using QuikGraph;

namespace Assets
{

    public class Connection<T> : IEdge<T>
    {
        public Connection(T source, T target)
        {
            Source = source;
            Target = target;
        }

        public T Source { get; set; }
        public T Target { get; set; }
    }
}
