using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200037F RID: 895
	internal struct PawnStatusEffecters
	{
		// Token: 0x06001678 RID: 5752 RVA: 0x00015E78 File Offset: 0x00014078
		public PawnStatusEffecters(Pawn pawn)
		{
			this.pawn = pawn;
			this.pairs = new List<PawnStatusEffecters.LiveEffecter>();
		}

		// Token: 0x06001679 RID: 5753 RVA: 0x000D68F0 File Offset: 0x000D4AF0
		public void EffectersTick()
		{
			List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				HediffComp_Effecter hediffComp_Effecter = hediffs[i].TryGetComp<HediffComp_Effecter>();
				if (hediffComp_Effecter != null)
				{
					EffecterDef effecterDef = hediffComp_Effecter.CurrentStateEffecter();
					if (effecterDef != null)
					{
						this.AddOrMaintain(effecterDef);
					}
				}
			}
			if (this.pawn.mindState.mentalStateHandler.CurState != null)
			{
				EffecterDef effecterDef2 = this.pawn.mindState.mentalStateHandler.CurState.CurrentStateEffecter();
				if (effecterDef2 != null)
				{
					this.AddOrMaintain(effecterDef2);
				}
			}
			for (int j = this.pairs.Count - 1; j >= 0; j--)
			{
				if (this.pairs[j].Expired)
				{
					this.pairs[j].Cleanup();
					this.pairs.RemoveAt(j);
				}
				else
				{
					this.pairs[j].Tick(this.pawn);
				}
			}
		}

		// Token: 0x0600167A RID: 5754 RVA: 0x000D69F0 File Offset: 0x000D4BF0
		private void AddOrMaintain(EffecterDef def)
		{
			for (int i = 0; i < this.pairs.Count; i++)
			{
				if (this.pairs[i].def == def)
				{
					this.pairs[i].Maintain();
					return;
				}
			}
			PawnStatusEffecters.LiveEffecter liveEffecter = FullPool<PawnStatusEffecters.LiveEffecter>.Get();
			liveEffecter.def = def;
			liveEffecter.Maintain();
			this.pairs.Add(liveEffecter);
		}

		// Token: 0x04001149 RID: 4425
		public Pawn pawn;

		// Token: 0x0400114A RID: 4426
		private List<PawnStatusEffecters.LiveEffecter> pairs;

		// Token: 0x02000380 RID: 896
		private class LiveEffecter : IFullPoolable
		{
			// Token: 0x1700041B RID: 1051
			// (get) Token: 0x0600167B RID: 5755 RVA: 0x00015E8C File Offset: 0x0001408C
			public bool Expired
			{
				get
				{
					return Find.TickManager.TicksGame > this.lastMaintainTick;
				}
			}

			// Token: 0x0600167D RID: 5757 RVA: 0x00015EA0 File Offset: 0x000140A0
			public void Cleanup()
			{
				if (this.effecter != null)
				{
					this.effecter.Cleanup();
				}
				FullPool<PawnStatusEffecters.LiveEffecter>.Return(this);
			}

			// Token: 0x0600167E RID: 5758 RVA: 0x00015EBB File Offset: 0x000140BB
			public void Reset()
			{
				this.def = null;
				this.effecter = null;
				this.lastMaintainTick = -1;
			}

			// Token: 0x0600167F RID: 5759 RVA: 0x00015ED2 File Offset: 0x000140D2
			public void Maintain()
			{
				this.lastMaintainTick = Find.TickManager.TicksGame;
			}

			// Token: 0x06001680 RID: 5760 RVA: 0x00015EE4 File Offset: 0x000140E4
			public void Tick(Pawn pawn)
			{
				if (this.effecter == null)
				{
					this.effecter = this.def.Spawn();
				}
				this.effecter.EffectTick(pawn, null);
			}

			// Token: 0x0400114B RID: 4427
			public EffecterDef def;

			// Token: 0x0400114C RID: 4428
			public Effecter effecter;

			// Token: 0x0400114D RID: 4429
			public int lastMaintainTick;
		}
	}
}
