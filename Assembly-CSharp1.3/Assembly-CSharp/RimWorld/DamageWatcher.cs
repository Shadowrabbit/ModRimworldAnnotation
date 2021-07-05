using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C74 RID: 3188
	public class DamageWatcher : IExposable
	{
		// Token: 0x17000CE2 RID: 3298
		// (get) Token: 0x06004A62 RID: 19042 RVA: 0x00189C2A File Offset: 0x00187E2A
		public float DamageTakenEver
		{
			get
			{
				return this.everDamage;
			}
		}

		// Token: 0x06004A63 RID: 19043 RVA: 0x00189C32 File Offset: 0x00187E32
		public void Notify_DamageTaken(Thing damagee, float amount)
		{
			if (damagee.Faction != Faction.OfPlayer)
			{
				return;
			}
			this.everDamage += amount;
		}

		// Token: 0x06004A64 RID: 19044 RVA: 0x00189C50 File Offset: 0x00187E50
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.everDamage, "everDamage", 0f, false);
		}

		// Token: 0x04002D2E RID: 11566
		private float everDamage;
	}
}
