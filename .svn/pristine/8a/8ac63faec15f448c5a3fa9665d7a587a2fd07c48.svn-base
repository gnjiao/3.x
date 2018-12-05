using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Core.Mvvm;
using Microsoft.Practices.ServiceLocation;

namespace Core.Collections.Generic
{
    public abstract class ContextNodeMiddleChildTerminal<TThis,
                                                         TThisContext,
                                                         TParent,
                                                         TParentContext> : ContextNodeMiddle<
                                                                               TThis,
                                                                               TThisContext,
                                                                               TParent,
                                                                               TParentContext,
                                                                               IContextNodeChildTerminal
                                                                               <TThis, TThisContext>,
                                                                               object>,
                                                                           IContextNodeMiddleChildTerminal<
                                                                               TThis,
                                                                               TThisContext,
                                                                               TParent,
                                                                               TParentContext>
        where TParent : IContextNodeParent<
                            TParent,
                            TParentContext,
                            TThis,
                            TThisContext>
        where TThis : class, IContextNodeMiddleChildTerminal<
                                 TThis,
                                 TThisContext,
                                 TParent,
                                 TParentContext>
    {
        protected override IEnumerable<object> GetChildContexts(TThisContext thisContext)
        {
            yield break;
        }
    }
}