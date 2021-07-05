using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000E2F RID: 3631
	public class TriggerData_FractionColonyDamageTaken : TriggerData
	{
		// Token: 0x0600525E RID: 21086 RVA: 0x000399C4 File Offset: 0x00037BC4
		public override void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.startColonyDamage, "startColonyDamage", 0f, false);
		}

		// Token: 0x040034C7 RID: 13511
		public float startColonyDamage;
	}
}
