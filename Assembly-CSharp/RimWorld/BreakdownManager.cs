using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020017A6 RID: 6054
	public class BreakdownManager : MapComponent
	{
		// Token: 0x060085CD RID: 34253 RVA: 0x00059B53 File Offset: 0x00057D53
		public BreakdownManager(Map map) : base(map)
		{
		}

		// Token: 0x060085CE RID: 34254 RVA: 0x00059B72 File Offset: 0x00057D72
		public void Register(CompBreakdownable c)
		{
			this.comps.Add(c);
			if (c.BrokenDown)
			{
				this.brokenDownThings.Add(c.parent);
			}
		}

		// Token: 0x060085CF RID: 34255 RVA: 0x00059B9A File Offset: 0x00057D9A
		public void Deregister(CompBreakdownable c)
		{
			this.comps.Remove(c);
			this.brokenDownThings.Remove(c.parent);
		}

		// Token: 0x060085D0 RID: 34256 RVA: 0x00276F10 File Offset: 0x00275110
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

		// Token: 0x060085D1 RID: 34257 RVA: 0x00059BBB File Offset: 0x00057DBB
		public void Notify_BrokenDown(Thing thing)
		{
			this.brokenDownThings.Add(thing);
		}

		// Token: 0x060085D2 RID: 34258 RVA: 0x00059BCA File Offset: 0x00057DCA
		public void Notify_Repaired(Thing thing)
		{
			this.brokenDownThings.Remove(thing);
		}

		// Token: 0x0400564A RID: 22090
		private List<CompBreakdownable> comps = new List<CompBreakdownable>();

		// Token: 0x0400564B RID: 22091
		public HashSet<Thing> brokenDownThings = new HashSet<Thing>();

		// Token: 0x0400564C RID: 22092
		public const int CheckIntervalTicks = 1041;
	}
}
