using QuikGraph;

namespace Assets
{
    public interface INode<T>
    {
        T GetData();
    }

    public class Connection<T> : IEdge<INode<T>>
    {
        public Connection(INode<T> source, INode<T> target)
        {
            Source = source;
            Target = target;
        }

        public INode<T> Source { get; set; }
        public INode<T> Target { get; set; }
    }
}
