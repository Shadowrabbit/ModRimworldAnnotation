using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200025B RID: 603
	internal struct PawnStatusEffecters
	{
		// Token: 0x06001125 RID: 4389 RVA: 0x0006148A File Offset: 0x0005F68A
		public PawnStatusEffecters(Pawn pawn)
		{
			this.pawn = pawn;
			this.pairs = new List<PawnStatusEffecters.LiveEffecter>();
		}

		// Token: 0x06001126 RID: 4390 RVA: 0x000614A0 File Offset: 0x0005F6A0
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

		// Token: 0x06001127 RID: 4391 RVA: 0x000615A0 File Offset: 0x0005F7A0
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

		// Token: 0x04000D0F RID: 3343
		public Pawn pawn;

		// Token: 0x04000D10 RID: 3344
		private List<PawnStatusEffecters.LiveEffecter> pairs;

		// Token: 0x020019D4 RID: 6612
		private class LiveEffecter : IFullPoolable
		{
			// Token: 0x17001965 RID: 6501
			// (get) Token: 0x06009A39 RID: 39481 RVA: 0x0036386A File Offset: 0x00361A6A
			public bool Expired
			{
				get
				{
					return Find.TickManager.TicksGame > this.lastMaintainTick;
				}
			}

			// Token: 0x06009A3B RID: 39483 RVA: 0x0036387E File Offset: 0x00361A7E
			public void Cleanup()
			{
				if (this.effecter != null)
				{
					this.effecter.Cleanup();
				}
				FullPool<PawnStatusEffecters.LiveEffecter>.Return(this);
			}

			// Token: 0x06009A3C RID: 39484 RVA: 0x00363899 File Offset: 0x00361A99
			public void Reset()
			{
				this.def = null;
				this.effecter = null;
				this.lastMaintainTick = -1;
			}

			// Token: 0x06009A3D RID: 39485 RVA: 0x003638B0 File Offset: 0x00361AB0
			public void Maintain()
			{
				this.lastMaintainTick = Find.TickManager.TicksGame;
			}

			// Token: 0x06009A3E RID: 39486 RVA: 0x003638C2 File Offset: 0x00361AC2
			public void Tick(Pawn pawn)
			{
				if (this.effecter == null)
				{
					this.effecter = this.def.Spawn();
				}
				this.effecter.EffectTick(pawn, null);
			}

			// Token: 0x0400630E RID: 25358
			public EffecterDef def;

			// Token: 0x0400630F RID: 25359
			public Effecter effecter;

			// Token: 0x04006310 RID: 25360
			public int lastMaintainTick;
		}
	}
}
