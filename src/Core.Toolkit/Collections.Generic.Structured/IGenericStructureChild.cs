using System.Collections.Generic;

namespace Core.Collections.Generic
{
    public interface IGenericStructureChild<TThisContext,
                                            TParent,
                                            TParentContext> : IGenericStructureBase<TThisContext>,
                                                              IStructureChild<TParent>
        where TParent : IGenericStructureBase<TParentContext>
    {
    }
}