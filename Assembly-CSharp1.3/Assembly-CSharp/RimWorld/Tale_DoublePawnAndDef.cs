using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001032 RID: 4146
	public class Tale_DoublePawnAndDef : Tale_DoublePawn
	{
		// Token: 0x060061EC RID: 25068 RVA: 0x00214719 File Offset: 0x00212919
		public Tale_DoublePawnAndDef()
		{
		}

		// Token: 0x060061ED RID: 25069 RVA: 0x00214721 File Offset: 0x00212921
		public Tale_DoublePawnAndDef(Pawn firstPawn, Pawn secondPawn, Def def) : base(firstPawn, secondPawn)
		{
			this.defData = TaleData_Def.GenerateFrom(def);
		}

		// Token: 0x060061EE RID: 25070 RVA: 0x00214737 File Offset: 0x00212937
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Def>(ref this.defData, "defData", Array.Empty<object>());
		}

		// Token: 0x060061EF RID: 25071 RVA: 0x00214754 File Offset: 0x00212954
		protected override IEnumerable<Rule> SpecialTextGenerationRules(Dictionary<string, string> outConstants)
		{
			if (this.def.defSymbol.NullOrEmpty())
			{
				Log.Error(this.def + " uses tale type with def but defSymbol is not set.");
			}
			foreach (Rule rule in base.SpecialTextGenerationRules(outConstants))
			{
				yield return rule;
			}
			IEnumerator<Rule> enumerator = null;
			foreach (Rule rule2 in this.defData.GetRules(this.def.defSymbol, null))
			{
				yield return rule2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060061F0 RID: 25072 RVA: 0x0021476B File Offset: 0x0021296B
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.defData = TaleData_Def.GenerateFrom((Def)GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), this.def.defType, "GetRandom"));
		}

		// Token: 0x040037D0 RID: 14288
		public TaleData_Def defData;
	}
}
