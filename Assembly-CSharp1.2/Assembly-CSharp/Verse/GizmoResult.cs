using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000719 RID: 1817
	public struct GizmoResult
	{
		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x06002DE6 RID: 11750 RVA: 0x0002429E File Offset: 0x0002249E
		public GizmoState State
		{
			get
			{
				return this.stateInt;
			}
		}

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x06002DE7 RID: 11751 RVA: 0x000242A6 File Offset: 0x000224A6
		public Event InteractEvent
		{
			get
			{
				return this.interactEventInt;
			}
		}

		// Token: 0x06002DE8 RID: 11752 RVA: 0x000242AE File Offset: 0x000224AE
		public GizmoResult(GizmoState state)
		{
			this.stateInt = state;
			this.interactEventInt = null;
		}

		// Token: 0x06002DE9 RID: 11753 RVA: 0x000242BE File Offset: 0x000224BE
		public GizmoResult(GizmoState state, Event interactEvent)
		{
			this.stateInt = state;
			this.interactEventInt = interactEvent;
		}

		// Token: 0x04001F4C RID: 8012
		private GizmoState stateInt;

		// Token: 0x04001F4D RID: 8013
		private Event interactEventInt;
	}
}
