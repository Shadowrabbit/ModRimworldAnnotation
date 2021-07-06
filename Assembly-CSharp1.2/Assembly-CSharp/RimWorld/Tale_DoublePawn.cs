using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200164D RID: 5709
	public class Tale_DoublePawn : Tale
	{
		// Token: 0x1700131D RID: 4893
		// (get) Token: 0x06007C38 RID: 31800 RVA: 0x00053657 File Offset: 0x00051857
		public override Pawn DominantPawn
		{
			get
			{
				return this.firstPawnData.pawn;
			}
		}

		// Token: 0x1700131E RID: 4894
		// (get) Token: 0x06007C39 RID: 31801 RVA: 0x002537BC File Offset: 0x002519BC
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

		// Token: 0x06007C3A RID: 31802 RVA: 0x00053664 File Offset: 0x00051864
		public Tale_DoublePawn()
		{
		}

		// Token: 0x06007C3B RID: 31803 RVA: 0x00253814 File Offset: 0x00251A14
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

		// Token: 0x06007C3C RID: 31804 RVA: 0x0005366C File Offset: 0x0005186C
		public override bool Concerns(Thing th)
		{
			return (this.secondPawnData != null && this.secondPawnData.pawn == th) || base.Concerns(th) || this.firstPawnData.pawn == th;
		}

		// Token: 0x06007C3D RID: 31805 RVA: 0x0005369F File Offset: 0x0005189F
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

		// Token: 0x06007C3E RID: 31806 RVA: 0x000536DD File Offset: 0x000518DD
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Pawn>(ref this.firstPawnData, "firstPawnData", Array.Empty<object>());
			Scribe_Deep.Look<TaleData_Pawn>(ref this.secondPawnData, "secondPawnData", Array.Empty<object>());
		}

		// Token: 0x06007C3F RID: 31807 RVA: 0x0005370F File Offset: 0x0005190F
		protected override IEnumerable<Rule> SpecialTextGenerationRules()
		{
			if (this.def.firstPawnSymbol.NullOrEmpty() || this.def.secondPawnSymbol.NullOrEmpty())
			{
				Log.Error(this.def + " uses DoublePawn tale class but firstPawnSymbol and secondPawnSymbol are not both set", false);
			}
			foreach (Rule rule in this.firstPawnData.GetRules("ANYPAWN"))
			{
				yield return rule;
			}
			IEnumerator<Rule> enumerator = null;
			foreach (Rule rule2 in this.firstPawnData.GetRules(this.def.firstPawnSymbol))
			{
				yield return rule2;
			}
			enumerator = null;
			if (this.secondPawnData != null)
			{
				foreach (Rule rule3 in this.firstPawnData.GetRules("ANYPAWN"))
				{
					yield return rule3;
				}
				enumerator = null;
				foreach (Rule rule4 in this.secondPawnData.GetRules(this.def.secondPawnSymbol))
				{
					yield return rule4;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06007C40 RID: 31808 RVA: 0x0005371F File Offset: 0x0005191F
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.firstPawnData = TaleData_Pawn.GenerateRandom();
			this.secondPawnData = TaleData_Pawn.GenerateRandom();
		}

		// Token: 0x04005157 RID: 20823
		public TaleData_Pawn firstPawnData;

		// Token: 0x04005158 RID: 20824
		public TaleData_Pawn secondPawnData;
	}
}
