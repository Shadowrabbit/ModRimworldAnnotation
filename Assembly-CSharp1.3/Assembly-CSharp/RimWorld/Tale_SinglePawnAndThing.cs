using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001037 RID: 4151
	public class Tale_SinglePawnAndThing : Tale_SinglePawn
	{
		// Token: 0x0600620A RID: 25098 RVA: 0x0021494A File Offset: 0x00212B4A
		public Tale_SinglePawnAndThing()
		{
		}

		// Token: 0x0600620B RID: 25099 RVA: 0x002149DB File Offset: 0x00212BDB
		public Tale_SinglePawnAndThing(Pawn pawn, Thing item) : base(pawn)
		{
			this.thingData = TaleData_Thing.GenerateFrom(item);
		}

		// Token: 0x0600620C RID: 25100 RVA: 0x002149F0 File Offset: 0x00212BF0
		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || th.thingIDNumber == this.thingData.thingID;
		}

		// Token: 0x0600620D RID: 25101 RVA: 0x00214A10 File Offset: 0x00212C10
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Thing>(ref this.thingData, "thingData", Array.Empty<object>());
		}

		// Token: 0x0600620E RID: 25102 RVA: 0x00214A2D File Offset: 0x00212C2D
		protected override IEnumerable<Rule> SpecialTextGenerationRules(Dictionary<string, string> outConstants)
		{
			foreach (Rule rule in base.SpecialTextGenerationRules(outConstants))
			{
				yield return rule;
			}
			IEnumerator<Rule> enumerator = null;
			foreach (Rule rule2 in this.thingData.GetRules("THING", null))
			{
				yield return rule2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600620F RID: 25103 RVA: 0x00214A44 File Offset: 0x00212C44
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.thingData = TaleData_Thing.GenerateRandom();
		}

		// Token: 0x040037D4 RID: 14292
		public TaleData_Thing thingData;
	}
}
