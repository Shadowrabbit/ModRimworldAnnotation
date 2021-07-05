using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010A0 RID: 4256
	public class SignalAction_OpenCasket : SignalAction_Delay
	{
		// Token: 0x1700115D RID: 4445
		// (get) Token: 0x06006570 RID: 25968 RVA: 0x002241DF File Offset: 0x002223DF
		public override Alert_ActionDelay Alert
		{
			get
			{
				if (this.cachedAlert == null)
				{
					this.cachedAlert = new Alert_CasketOpening(this);
				}
				return this.cachedAlert;
			}
		}

		// Token: 0x1700115E RID: 4446
		// (get) Token: 0x06006571 RID: 25969 RVA: 0x002241FC File Offset: 0x002223FC
		public override bool ShouldRemoveNow
		{
			get
			{
				for (int i = 0; i < this.caskets.Count; i++)
				{
					if (this.caskets[i] != null && !this.caskets[i].Destroyed && ((Building_Casket)this.caskets[i]).HasAnyContents)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x06006572 RID: 25970 RVA: 0x0022425C File Offset: 0x0022245C
		protected override void Complete()
		{
			base.Complete();
			for (int i = 0; i < this.caskets.Count; i++)
			{
				Building_Casket building_Casket = (Building_Casket)this.caskets[i];
				if (building_Casket.CanOpen)
				{
					building_Casket.Open();
				}
			}
		}

		// Token: 0x06006573 RID: 25971 RVA: 0x002242A5 File Offset: 0x002224A5
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Thing>(ref this.caskets, "caskets", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x04003925 RID: 14629
		public List<Thing> caskets = new List<Thing>();

		// Token: 0x04003926 RID: 14630
		private Alert_ActionDelay cachedAlert;
	}
}
