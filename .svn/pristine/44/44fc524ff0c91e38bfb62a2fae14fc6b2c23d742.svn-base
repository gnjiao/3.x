using Microsoft.Practices.ServiceLocation;

namespace Core.Collections.Generic.Levels
{
    public abstract class DpGenericStructureMiddle<TThis,
                                                   TChild,
                                                   TThisContext,
                                                   TChildContext> : DpGenericStructureParent<TThis,
                                                                        TChild,
                                                                        TThisContext,
                                                                        TChildContext>,
                                                                    IGenericCTreeChild<TThisContext>
        where TThis : class, IGenericCTreeParent<TThis,
                                 TChild,
                                 TThisContext,
                                 TChildContext>,
            IGenericCTreeChild<TChildContext>
        where TChild : IGenericCTreeChild<TChildContext>
    {
        public int Index { get; set; }
    }
}