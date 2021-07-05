using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200125E RID: 4702
	public class DamageWatcher : IExposable
	{
		// Token: 0x17000FE8 RID: 4072
		// (get) Token: 0x06006691 RID: 26257 RVA: 0x00046126 File Offset: 0x00044326
		public float DamageTakenEver
		{
			get
			{
				return this.everDamage;
			}
		}

		// Token: 0x06006692 RID: 26258 RVA: 0x0004612E File Offset: 0x0004432E
		public void Notify_DamageTaken(Thing damagee, float amount)
		{
			if (damagee.Faction != Faction.OfPlayer)
			{
				return;
			}
			this.everDamage += amount;
		}

		// Token: 0x06006693 RID: 26259 RVA: 0x0004614C File Offset: 0x0004434C
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.everDamage, "everDamage", 0f, false);
		}

		// Token: 0x04004444 RID: 17476
		private float everDamage;
	}
}
