using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CFD RID: 3325
	public class TreeDestructionTracker : IExposable
	{
		// Token: 0x17000D61 RID: 3425
		// (get) Token: 0x06004DAD RID: 19885 RVA: 0x001A10DB File Offset: 0x0019F2DB
		public int PlayerResponsibleTreeDestructionCount
		{
			get
			{
				this.DiscardOldEvents();
				return this.playerTreeDestructionTicks.Count;
			}
		}

		// Token: 0x17000D62 RID: 3426
		// (get) Token: 0x06004DAE RID: 19886 RVA: 0x001A10F0 File Offset: 0x0019F2F0
		public float DensityDestroyed
		{
			get
			{
				float treeDensity = this.map.Biome.TreeDensity;
				if (treeDensity == 0f)
				{
					return 0f;
				}
				return (float)this.PlayerResponsibleTreeDestructionCount * treeDensity;
			}
		}

		// Token: 0x06004DAF RID: 19887 RVA: 0x001A1125 File Offset: 0x0019F325
		public TreeDestructionTracker(Map map)
		{
			this.map = map;
		}

		// Token: 0x06004DB0 RID: 19888 RVA: 0x001A113F File Offset: 0x0019F33F
		public void Notify_TreeDestroyed(DamageInfo dInfo)
		{
			if (dInfo.Instigator != null && dInfo.Instigator.Faction == Faction.OfPlayer)
			{
				this.playerTreeDestructionTicks.Add(Find.TickManager.TicksGame);
			}
		}

		// Token: 0x06004DB1 RID: 19889 RVA: 0x001A1172 File Offset: 0x0019F372
		public void Notify_TreeCut(Pawn by)
		{
			if (by.Faction == Faction.OfPlayer)
			{
				this.playerTreeDestructionTicks.Add(Find.TickManager.TicksGame);
			}
		}

		// Token: 0x06004DB2 RID: 19890 RVA: 0x001A1198 File Offset: 0x0019F398
		private void DiscardOldEvents()
		{
			int num = 0;
			while (num < this.playerTreeDestructionTicks.Count && Find.TickManager.TicksGame >= this.playerTreeDestructionTicks[num] + 900000)
			{
				this.playerTreeDestructionTicks.RemoveAt(num);
				num--;
				num++;
			}
		}

		// Token: 0x06004DB3 RID: 19891 RVA: 0x001A11E9 File Offset: 0x0019F3E9
		public void ExposeData()
		{
			Scribe_Collections.Look<int>(ref this.playerTreeDestructionTicks, "playerTreeDestructionTicks", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x04002EDD RID: 11997
		private Map map;

		// Token: 0x04002EDE RID: 11998
		private List<int> playerTreeDestructionTicks = new List<int>();

		// Token: 0x04002EDF RID: 11999
		private const int DestructionsExpireInTicks = 900000;
	}
}
