using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001658 RID: 5720
	public class Tale_SinglePawnAndThing : Tale_SinglePawn
	{
		// Token: 0x06007C93 RID: 31891 RVA: 0x00053A70 File Offset: 0x00051C70
		public Tale_SinglePawnAndThing()
		{
		}

		// Token: 0x06007C94 RID: 31892 RVA: 0x00053B3F File Offset: 0x00051D3F
		public Tale_SinglePawnAndThing(Pawn pawn, Thing item) : base(pawn)
		{
			this.thingData = TaleData_Thing.GenerateFrom(item);
		}

		// Token: 0x06007C95 RID: 31893 RVA: 0x00053B54 File Offset: 0x00051D54
		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || th.thingIDNumber == this.thingData.thingID;
		}

		// Token: 0x06007C96 RID: 31894 RVA: 0x00053B74 File Offset: 0x00051D74
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Thing>(ref this.thingData, "thingData", Array.Empty<object>());
		}

		// Token: 0x06007C97 RID: 31895 RVA: 0x00053B91 File Offset: 0x00051D91
		protected override IEnumerable<Rule> SpecialTextGenerationRules()
		{
			foreach (Rule rule in base.SpecialTextGenerationRules())
			{
				yield return rule;
			}
			IEnumerator<Rule> enumerator = null;
			foreach (Rule rule2 in this.thingData.GetRules("THING"))
			{
				yield return rule2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06007C98 RID: 31896 RVA: 0x00053BA1 File Offset: 0x00051DA1
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.thingData = TaleData_Thing.GenerateRandom();
		}

		// Token: 0x04005176 RID: 20854
		public TaleData_Thing thingData;
	}
}
