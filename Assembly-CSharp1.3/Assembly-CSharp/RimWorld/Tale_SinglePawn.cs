using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001035 RID: 4149
	public class Tale_SinglePawn : Tale
	{
		// Token: 0x170010AA RID: 4266
		// (get) Token: 0x060061FB RID: 25083 RVA: 0x00214860 File Offset: 0x00212A60
		public override Pawn DominantPawn
		{
			get
			{
				return this.pawnData.pawn;
			}
		}

		// Token: 0x170010AB RID: 4267
		// (get) Token: 0x060061FC RID: 25084 RVA: 0x0021486D File Offset: 0x00212A6D
		public override string ShortSummary
		{
			get
			{
				return this.def.LabelCap + ": " + this.pawnData.name;
			}
		}

		// Token: 0x060061FD RID: 25085 RVA: 0x002145EC File Offset: 0x002127EC
		public Tale_SinglePawn()
		{
		}

		// Token: 0x060061FE RID: 25086 RVA: 0x00214899 File Offset: 0x00212A99
		public Tale_SinglePawn(Pawn pawn)
		{
			this.pawnData = TaleData_Pawn.GenerateFrom(pawn);
			if (pawn.SpawnedOrAnyParentSpawned)
			{
				this.surroundings = TaleData_Surroundings.GenerateFrom(pawn.PositionHeld, pawn.MapHeld);
			}
		}

		// Token: 0x060061FF RID: 25087 RVA: 0x002148CC File Offset: 0x00212ACC
		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || this.pawnData.pawn == th;
		}

		// Token: 0x06006200 RID: 25088 RVA: 0x002148E7 File Offset: 0x00212AE7
		public override void Notify_FactionRemoved(Faction faction)
		{
			if (this.pawnData.faction == faction)
			{
				this.pawnData.faction = null;
			}
		}

		// Token: 0x06006201 RID: 25089 RVA: 0x00214903 File Offset: 0x00212B03
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Pawn>(ref this.pawnData, "pawnData", Array.Empty<object>());
		}

		// Token: 0x06006202 RID: 25090 RVA: 0x00214920 File Offset: 0x00212B20
		protected override IEnumerable<Rule> SpecialTextGenerationRules(Dictionary<string, string> outConstants)
		{
			foreach (Rule rule in this.pawnData.GetRules("ANYPAWN", outConstants))
			{
				yield return rule;
			}
			IEnumerator<Rule> enumerator = null;
			foreach (Rule rule2 in this.pawnData.GetRules("PAWN", outConstants))
			{
				yield return rule2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06006203 RID: 25091 RVA: 0x00214937 File Offset: 0x00212B37
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.pawnData = TaleData_Pawn.GenerateRandom();
		}

		// Token: 0x040037D2 RID: 14290
		public TaleData_Pawn pawnData;
	}
}
