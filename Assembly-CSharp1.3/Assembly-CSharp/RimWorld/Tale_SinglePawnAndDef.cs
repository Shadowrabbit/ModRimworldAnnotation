using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001036 RID: 4150
	public class Tale_SinglePawnAndDef : Tale_SinglePawn
	{
		// Token: 0x06006204 RID: 25092 RVA: 0x0021494A File Offset: 0x00212B4A
		public Tale_SinglePawnAndDef()
		{
		}

		// Token: 0x06006205 RID: 25093 RVA: 0x00214952 File Offset: 0x00212B52
		public Tale_SinglePawnAndDef(Pawn pawn, Def def) : base(pawn)
		{
			this.defData = TaleData_Def.GenerateFrom(def);
		}

		// Token: 0x06006206 RID: 25094 RVA: 0x00214967 File Offset: 0x00212B67
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Def>(ref this.defData, "defData", Array.Empty<object>());
		}

		// Token: 0x06006207 RID: 25095 RVA: 0x00214984 File Offset: 0x00212B84
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

		// Token: 0x06006208 RID: 25096 RVA: 0x0021499B File Offset: 0x00212B9B
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.defData = TaleData_Def.GenerateFrom((Def)GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), this.def.defType, "GetRandom"));
		}

		// Token: 0x040037D3 RID: 14291
		public TaleData_Def defData;
	}
}
