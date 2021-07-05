using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200151E RID: 5406
	public class TreeSighting : IExposable
	{
		// Token: 0x170015E1 RID: 5601
		// (get) Token: 0x0600809B RID: 32923 RVA: 0x002D9144 File Offset: 0x002D7344
		public Thing Tree
		{
			get
			{
				return this.tree;
			}
		}

		// Token: 0x170015E2 RID: 5602
		// (get) Token: 0x0600809C RID: 32924 RVA: 0x002D914C File Offset: 0x002D734C
		public int TicksSinceSighting
		{
			get
			{
				return Find.TickManager.TicksGame - this.tickSighted;
			}
		}

		// Token: 0x0600809D RID: 32925 RVA: 0x000033AC File Offset: 0x000015AC
		public TreeSighting()
		{
		}

		// Token: 0x0600809E RID: 32926 RVA: 0x002D915F File Offset: 0x002D735F
		public TreeSighting(Thing tree, int tickSighted)
		{
			this.tree = tree;
			this.tickSighted = tickSighted;
		}

		// Token: 0x0600809F RID: 32927 RVA: 0x002D9175 File Offset: 0x002D7375
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.tickSighted, "tickSighted", 0, false);
			Scribe_References.Look<Thing>(ref this.tree, "tree", false);
		}

		// Token: 0x04005016 RID: 20502
		private int tickSighted;

		// Token: 0x04005017 RID: 20503
		public Thing tree;
	}
}
