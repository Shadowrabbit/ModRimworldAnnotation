using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001031 RID: 4145
	public class Tale_DoublePawn : Tale
	{
		// Token: 0x170010A8 RID: 4264
		// (get) Token: 0x060061E3 RID: 25059 RVA: 0x00214587 File Offset: 0x00212787
		public override Pawn DominantPawn
		{
			get
			{
				return this.firstPawnData.pawn;
			}
		}

		// Token: 0x170010A9 RID: 4265
		// (get) Token: 0x060061E4 RID: 25060 RVA: 0x00214594 File Offset: 0x00212794
		public override string ShortSummary
		{
			get
			{
				string text = this.def.LabelCap + ": " + this.firstPawnData.name;
				if (this.secondPawnData != null)
				{
					text = text + ", " + this.secondPawnData.name;
				}
				return text;
			}
		}

		// Token: 0x060061E5 RID: 25061 RVA: 0x002145EC File Offset: 0x002127EC
		public Tale_DoublePawn()
		{
		}

		// Token: 0x060061E6 RID: 25062 RVA: 0x002145F4 File Offset: 0x002127F4
		public Tale_DoublePawn(Pawn firstPawn, Pawn secondPawn)
		{
			this.firstPawnData = TaleData_Pawn.GenerateFrom(firstPawn);
			if (secondPawn != null)
			{
				this.secondPawnData = TaleData_Pawn.GenerateFrom(secondPawn);
			}
			if (firstPawn.SpawnedOrAnyParentSpawned)
			{
				this.surroundings = TaleData_Surroundings.GenerateFrom(firstPawn.PositionHeld, firstPawn.MapHeld);
			}
		}

		// Token: 0x060061E7 RID: 25063 RVA: 0x00214641 File Offset: 0x00212841
		public override bool Concerns(Thing th)
		{
			return (this.secondPawnData != null && this.secondPawnData.pawn == th) || base.Concerns(th) || this.firstPawnData.pawn == th;
		}

		// Token: 0x060061E8 RID: 25064 RVA: 0x00214674 File Offset: 0x00212874
		public override void Notify_FactionRemoved(Faction faction)
		{
			if (this.firstPawnData.faction == faction)
			{
				this.firstPawnData.faction = null;
			}
			if (this.secondPawnData != null && this.secondPawnData.faction == faction)
			{
				this.secondPawnData.faction = null;
			}
		}

		// Token: 0x060061E9 RID: 25065 RVA: 0x002146B2 File Offset: 0x002128B2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Pawn>(ref this.firstPawnData, "firstPawnData", Array.Empty<object>());
			Scribe_Deep.Look<TaleData_Pawn>(ref this.secondPawnData, "secondPawnData", Array.Empty<object>());
		}

		// Token: 0x060061EA RID: 25066 RVA: 0x002146E4 File Offset: 0x002128E4
		protected override IEnumerable<Rule> SpecialTextGenerationRules(Dictionary<string, string> outConstants = null)
		{
			if (this.def.firstPawnSymbol.NullOrEmpty() || this.def.secondPawnSymbol.NullOrEmpty())
			{
				Log.Error(this.def + " uses DoublePawn tale class but firstPawnSymbol and secondPawnSymbol are not both set");
			}
			foreach (Rule rule in this.firstPawnData.GetRules("ANYPAWN", null))
			{
				yield return rule;
			}
			IEnumerator<Rule> enumerator = null;
			foreach (Rule rule2 in this.firstPawnData.GetRules(this.def.firstPawnSymbol, outConstants))
			{
				yield return rule2;
			}
			enumerator = null;
			if (this.secondPawnData != null)
			{
				foreach (Rule rule3 in this.firstPawnData.GetRules("ANYPAWN", null))
				{
					yield return rule3;
				}
				enumerator = null;
				foreach (Rule rule4 in this.secondPawnData.GetRules(this.def.secondPawnSymbol, outConstants))
				{
					yield return rule4;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x060061EB RID: 25067 RVA: 0x002146FB File Offset: 0x002128FB
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.firstPawnData = TaleData_Pawn.GenerateRandom();
			this.secondPawnData = TaleData_Pawn.GenerateRandom();
		}

		// Token: 0x040037CE RID: 14286
		public TaleData_Pawn firstPawnData;

		// Token: 0x040037CF RID: 14287
		public TaleData_Pawn secondPawnData;
	}
}
