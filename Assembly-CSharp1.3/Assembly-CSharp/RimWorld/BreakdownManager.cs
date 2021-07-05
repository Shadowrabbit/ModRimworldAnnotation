using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001114 RID: 4372
	public class BreakdownManager : MapComponent
	{
		// Token: 0x06006904 RID: 26884 RVA: 0x00236F77 File Offset: 0x00235177
		public BreakdownManager(Map map) : base(map)
		{
		}

		// Token: 0x06006905 RID: 26885 RVA: 0x00236F96 File Offset: 0x00235196
		public void Register(CompBreakdownable c)
		{
			this.comps.Add(c);
			if (c.BrokenDown)
			{
				this.brokenDownThings.Add(c.parent);
			}
		}

		// Token: 0x06006906 RID: 26886 RVA: 0x00236FBE File Offset: 0x002351BE
		public void Deregister(CompBreakdownable c)
		{
			this.comps.Remove(c);
			this.brokenDownThings.Remove(c.parent);
		}

		// Token: 0x06006907 RID: 26887 RVA: 0x00236FE0 File Offset: 0x002351E0
		public override void MapComponentTick()
		{
			if (Find.TickManager.TicksGame % 1041 == 0)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].CheckForBreakdown();
				}
			}
		}

		// Token: 0x06006908 RID: 26888 RVA: 0x00237026 File Offset: 0x00235226
		public void Notify_BrokenDown(Thing thing)
		{
			this.brokenDownThings.Add(thing);
		}

		// Token: 0x06006909 RID: 26889 RVA: 0x00237035 File Offset: 0x00235235
		public void Notify_Repaired(Thing thing)
		{
			this.brokenDownThings.Remove(thing);
		}

		// Token: 0x04003ACC RID: 15052
		private List<CompBreakdownable> comps = new List<CompBreakdownable>();

		// Token: 0x04003ACD RID: 15053
		public HashSet<Thing> brokenDownThings = new HashSet<Thing>();

		// Token: 0x04003ACE RID: 15054
		public const int CheckIntervalTicks = 1041;
	}
}
