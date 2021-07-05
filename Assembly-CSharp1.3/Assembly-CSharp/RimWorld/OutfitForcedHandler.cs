using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E61 RID: 3681
	public class OutfitForcedHandler : IExposable
	{
		// Token: 0x17000EAE RID: 3758
		// (get) Token: 0x0600552D RID: 21805 RVA: 0x001CD7D7 File Offset: 0x001CB9D7
		public bool SomethingIsForced
		{
			get
			{
				return this.forcedAps.Count > 0;
			}
		}

		// Token: 0x17000EAF RID: 3759
		// (get) Token: 0x0600552E RID: 21806 RVA: 0x001CD7E7 File Offset: 0x001CB9E7
		public List<Apparel> ForcedApparel
		{
			get
			{
				return this.forcedAps;
			}
		}

		// Token: 0x0600552F RID: 21807 RVA: 0x001CD7EF File Offset: 0x001CB9EF
		public void Reset()
		{
			this.forcedAps.Clear();
		}

		// Token: 0x06005530 RID: 21808 RVA: 0x001CD7FC File Offset: 0x001CB9FC
		public bool AllowedToAutomaticallyDrop(Apparel ap)
		{
			return !this.forcedAps.Contains(ap);
		}

		// Token: 0x06005531 RID: 21809 RVA: 0x001CD80D File Offset: 0x001CBA0D
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

		// Token: 0x06005532 RID: 21810 RVA: 0x001CD848 File Offset: 0x001CBA48
		public void ExposeData()
		{
			Scribe_Collections.Look<Apparel>(ref this.forcedAps, "forcedAps", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x06005533 RID: 21811 RVA: 0x001CD860 File Offset: 0x001CBA60
		public bool IsForced(Apparel ap)
		{
			if (ap.Destroyed)
			{
				Log.Error("Apparel was forced while Destroyed: " + ap);
				if (this.forcedAps.Contains(ap))
				{
					this.forcedAps.Remove(ap);
				}
				return false;
			}
			return this.forcedAps.Contains(ap);
		}

		// Token: 0x0400327D RID: 12925
		private List<Apparel> forcedAps = new List<Apparel>();
	}
}
