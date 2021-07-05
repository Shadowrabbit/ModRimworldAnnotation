using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003FD RID: 1021
	public struct GizmoResult
	{
		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x06001EA4 RID: 7844 RVA: 0x000BF781 File Offset: 0x000BD981
		public GizmoState State
		{
			get
			{
				return this.stateInt;
			}
		}

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x06001EA5 RID: 7845 RVA: 0x000BF789 File Offset: 0x000BD989
		public Event InteractEvent
		{
			get
			{
				return this.interactEventInt;
			}
		}

		// Token: 0x06001EA6 RID: 7846 RVA: 0x000BF791 File Offset: 0x000BD991
		public GizmoResult(GizmoState state)
		{
			this.stateInt = state;
			this.interactEventInt = null;
		}

		// Token: 0x06001EA7 RID: 7847 RVA: 0x000BF7A1 File Offset: 0x000BD9A1
		public GizmoResult(GizmoState state, Event interactEvent)
		{
			this.stateInt = state;
			this.interactEventInt = interactEvent;
		}

		// Token: 0x040012AC RID: 4780
		private GizmoState stateInt;

		// Token: 0x040012AD RID: 4781
		private Event interactEventInt;
	}
}
