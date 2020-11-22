using System;
using System.Collections.Generic;

using NetRx.Store;

namespace NetRx.Store.Tests.State.TestStateActions
{
    public class ClearNameAction : Action { }

    public class LoadItemsAction : Action { }

    public class SetItemsAction : Action<List<string>>
    {
        public SetItemsAction(List<string> payload) : base(payload)
        {
        }
    }
    
    public class SetReferenceObjectAction : Action<ReferenceObect> 
    {
        public SetReferenceObjectAction(ReferenceObect payload)  :base(payload)
        {

        }
    }
}
