using System.Collections.Generic;

namespace Core.Collections.Generic
{
    public interface ICompositeNode<TContext,
                                    TChild,
                                    TChildContext> : IComplexNode<TContext>
        where TChild : IComplexNode<TChildContext>
    {
        IList<TChild> Children { get; }
    }
}