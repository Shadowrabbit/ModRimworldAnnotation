using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014FE RID: 5374
	public class OutfitForcedHandler : IExposable
	{
		// Token: 0x170011D7 RID: 4567
		// (get) Token: 0x060073B7 RID: 29623 RVA: 0x0004DFFF File Offset: 0x0004C1FF
		public bool SomethingIsForced
		{
			get
			{
				return this.forcedAps.Count > 0;
			}
		}

		// Token: 0x170011D8 RID: 4568
		// (get) Token: 0x060073B8 RID: 29624 RVA: 0x0004E00F File Offset: 0x0004C20F
		public List<Apparel> ForcedApparel
		{
			get
			{
				return this.forcedAps;
			}
		}

		// Token: 0x060073B9 RID: 29625 RVA: 0x0004E017 File Offset: 0x0004C217
		public void Reset()
		{
			this.forcedAps.Clear();
		}

		// Token: 0x060073BA RID: 29626 RVA: 0x0004E024 File Offset: 0x0004C224
		public bool AllowedToAutomaticallyDrop(Apparel ap)
		{
			return !this.forcedAps.Contains(ap);
		}

		// Token: 0x060073BB RID: 29627 RVA: 0x0004E035 File Offset: 0x0004C235
		public void SetForced(Apparel ap, bool forced)
		{
			if (forced)
			{
				if (!this.forcedAps.Contains(ap))
				{
					this.forcedAps.Add(ap);
					return;
				}
			}
			else if (this.forcedAps.Contains(ap))
			{
				this.forcedAps.Remove(ap);
			}
		}

		// Token: 0x060073BC RID: 29628 RVA: 0x0004E070 File Offset: 0x0004C270
		public void ExposeData()
		{
			Scribe_Collections.Look<Apparel>(ref this.forcedAps, "forcedAps", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x060073BD RID: 29629 RVA: 0x00234DE8 File Offset: 0x00232FE8
		public bool IsForced(Apparel ap)
		{
			if (ap.Destroyed)
			{
				Log.Error("Apparel was forced while Destroyed: " + ap, false);
				if (this.forcedAps.Contains(ap))
				{
					this.forcedAps.Remove(ap);
				}
				return false;
			}
			return this.forcedAps.Contains(ap);
		}

		// Token: 0x04004C76 RID: 19574
		private List<Apparel> forcedAps = new List<Apparel>();
	}
}
