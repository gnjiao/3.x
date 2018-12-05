using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Core.Mvvm;
using Microsoft.Practices.ServiceLocation;

namespace Core.Collections.Generic
{
    public abstract class ContextNodeMiddleParentTerminal<TThis,
                                                          TThisContext,
                                                          TChild,
                                                          TChildContext> : ContextNodeMiddle<
                                                                               TThis,
                                                                               TThisContext,
                                                                               IContextNodeParentTerminal
                                                                               <TThis, TThisContext>,
                                                                               object,
                                                                               TChild,
                                                                               TChildContext>,
                                                                           IContextNodeMiddleParentTerminal
                                                                               <TThis,
                                                                               TThisContext,
                                                                               TChild,
                                                                               TChildContext>
        where TChild : IContextNodeChild<
                           TChild,
                           TChildContext,
                           TThis,
                           TThisContext>
        where TThis : class, IContextNodeMiddleParentTerminal<
                                 TThis,
                                 TThisContext,
                                 TChild,
                                 TChildContext>
    {
    }
}