﻿using System.Collections.Generic;

namespace Core.Collections.Generic
{
    public interface IBidirectionStructureParent<TThis, TChild>
        where TChild : IBidirectionStructureChild<TChild, TThis>
        where TThis : IBidirectionStructureParent<TThis, TChild>
    {
        IList<TChild> Children { get; set; }
    }
}