using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001656 RID: 5718
	public class Tale_SinglePawnAndDef : Tale_SinglePawn
	{
		// Token: 0x06007C83 RID: 31875 RVA: 0x00053A70 File Offset: 0x00051C70
		public Tale_SinglePawnAndDef()
		{
		}

		// Token: 0x06007C84 RID: 31876 RVA: 0x00053A78 File Offset: 0x00051C78
		public Tale_SinglePawnAndDef(Pawn pawn, Def def) : base(pawn)
		{
			this.defData = TaleData_Def.GenerateFrom(def);
		}

		// Token: 0x06007C85 RID: 31877 RVA: 0x00053A8D File Offset: 0x00051C8D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Def>(ref this.defData, "defData", Array.Empty<object>());
		}

		// Token: 0x06007C86 RID: 31878 RVA: 0x00053AAA File Offset: 0x00051CAA
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

		// Token: 0x06007C87 RID: 31879 RVA: 0x00053ABA File Offset: 0x00051CBA
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.defData = TaleData_Def.GenerateFrom((Def)GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), this.def.defType, "GetRandom"));
		}

		// Token: 0x04005170 RID: 20848
		public TaleData_Def defData;
	}
}
