using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200164F RID: 5711
	public class Tale_DoublePawnAndDef : Tale_DoublePawn
	{
		// Token: 0x06007C4D RID: 31821 RVA: 0x00053783 File Offset: 0x00051983
		public Tale_DoublePawnAndDef()
		{
		}

		// Token: 0x06007C4E RID: 31822 RVA: 0x0005378B File Offset: 0x0005198B
		public Tale_DoublePawnAndDef(Pawn firstPawn, Pawn secondPawn, Def def) : base(firstPawn, secondPawn)
		{
			this.defData = TaleData_Def.GenerateFrom(def);
		}

		// Token: 0x06007C4F RID: 31823 RVA: 0x000537A1 File Offset: 0x000519A1
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Def>(ref this.defData, "defData", Array.Empty<object>());
		}

		// Token: 0x06007C50 RID: 31824 RVA: 0x000537BE File Offset: 0x000519BE
		protected override IEnumerable<Rule> SpecialTextGenerationRules()
		{
			if (this.def.defSymbol.NullOrEmpty())
			{
				Log.Error(this.def + " uses tale type with def but defSymbol is not set.", false);
			}
			foreach (Rule rule in base.SpecialTextGenerationRules())
			{
				yield return rule;
			}
			IEnumerator<Rule> enumerator = null;
			foreach (Rule rule2 in this.defData.GetRules(this.def.defSymbol))
			{
				yield return rule2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06007C51 RID: 31825 RVA: 0x000537CE File Offset: 0x000519CE
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.defData = TaleData_Def.GenerateFrom((Def)GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), this.def.defType, "GetRandom"));
		}

		// Token: 0x0400515E RID: 20830
		public TaleData_Def defData;
	}
}
