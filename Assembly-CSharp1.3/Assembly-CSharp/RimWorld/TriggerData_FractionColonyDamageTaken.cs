using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008E2 RID: 2274
	public class TriggerData_FractionColonyDamageTaken : TriggerData
	{
		// Token: 0x06003BA2 RID: 15266 RVA: 0x0014C752 File Offset: 0x0014A952
		public override void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.startColonyDamage, "startColonyDamage", 0f, false);
		}

		// Token: 0x0400206F RID: 8303
		public float startColonyDamage;
	}
}
