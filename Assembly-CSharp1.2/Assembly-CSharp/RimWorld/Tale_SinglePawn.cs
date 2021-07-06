using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001654 RID: 5716
	public class Tale_SinglePawn : Tale
	{
		// Token: 0x17001325 RID: 4901
		// (get) Token: 0x06007C70 RID: 31856 RVA: 0x00053947 File Offset: 0x00051B47
		public override Pawn DominantPawn
		{
			get
			{
				return this.pawnData.pawn;
			}
		}

		// Token: 0x17001326 RID: 4902
		// (get) Token: 0x06007C71 RID: 31857 RVA: 0x00053954 File Offset: 0x00051B54
		public override string ShortSummary
		{
			get
			{
				return this.def.LabelCap + ": " + this.pawnData.name;
			}
		}

		// Token: 0x06007C72 RID: 31858 RVA: 0x00053664 File Offset: 0x00051864
		public Tale_SinglePawn()
		{
		}

		// Token: 0x06007C73 RID: 31859 RVA: 0x00053980 File Offset: 0x00051B80
		public Tale_SinglePawn(Pawn pawn)
		{
			this.pawnData = TaleData_Pawn.GenerateFrom(pawn);
			if (pawn.SpawnedOrAnyParentSpawned)
			{
				this.surroundings = TaleData_Surroundings.GenerateFrom(pawn.PositionHeld, pawn.MapHeld);
			}
		}

		// Token: 0x06007C74 RID: 31860 RVA: 0x000539B3 File Offset: 0x00051BB3
		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || this.pawnData.pawn == th;
		}

		// Token: 0x06007C75 RID: 31861 RVA: 0x000539CE File Offset: 0x00051BCE
		public override void Notify_FactionRemoved(Faction faction)
		{
			if (this.pawnData.faction == faction)
			{
				this.pawnData.faction = null;
			}
		}

		// Token: 0x06007C76 RID: 31862 RVA: 0x000539EA File Offset: 0x00051BEA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Pawn>(ref this.pawnData, "pawnData", Array.Empty<object>());
		}

		// Token: 0x06007C77 RID: 31863 RVA: 0x00053A07 File Offset: 0x00051C07
		protected override IEnumerable<Rule> SpecialTextGenerationRules()
		{
			foreach (Rule rule in this.pawnData.GetRules("ANYPAWN"))
			{
				yield return rule;
			}
			IEnumerator<Rule> enumerator = null;
			foreach (Rule rule2 in this.pawnData.GetRules("PAWN"))
			{
				yield return rule2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06007C78 RID: 31864 RVA: 0x00053A17 File Offset: 0x00051C17
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.pawnData = TaleData_Pawn.GenerateRandom();
		}

		// Token: 0x0400516A RID: 20842
		public TaleData_Pawn pawnData;
	}
}
