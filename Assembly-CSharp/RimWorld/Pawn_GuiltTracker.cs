using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001544 RID: 5444
	public class Pawn_GuiltTracker : IExposable
	{
		// Token: 0x17001243 RID: 4675
		// (get) Token: 0x060075EA RID: 30186 RVA: 0x0004F83E File Offset: 0x0004DA3E
		public bool IsGuilty
		{
			get
			{
				return this.TicksUntilInnocent > 0 || this.pawn.InAggroMentalState;
			}
		}

		// Token: 0x17001244 RID: 4676
		// (get) Token: 0x060075EB RID: 30187 RVA: 0x0004F856 File Offset: 0x0004DA56
		public int TicksUntilInnocent
		{
			get
			{
				return Mathf.Max(0, this.lastGuiltyTick + 60000 - Find.TickManager.TicksGame);
			}
		}

		// Token: 0x060075EC RID: 30188 RVA: 0x0004F875 File Offset: 0x0004DA75
		public Pawn_GuiltTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060075ED RID: 30189 RVA: 0x0004F88F File Offset: 0x0004DA8F
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastGuiltyTick, "lastGuiltyTick", -99999, false);
		}

		// Token: 0x060075EE RID: 30190 RVA: 0x0004F8A7 File Offset: 0x0004DAA7
		public void Notify_Guilty()
		{
			this.lastGuiltyTick = Find.TickManager.TicksGame;
		}

		// Token: 0x04004DE1 RID: 19937
		private Pawn pawn;

		// Token: 0x04004DE2 RID: 19938
		public int lastGuiltyTick = -99999;

		// Token: 0x04004DE3 RID: 19939
		private const int GuiltyDuration = 60000;
	}
}
