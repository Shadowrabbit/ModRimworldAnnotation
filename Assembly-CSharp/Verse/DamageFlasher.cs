using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000384 RID: 900
	public class DamageFlasher
	{
		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x0600168E RID: 5774 RVA: 0x00015F8A File Offset: 0x0001418A
		private int DamageFlashTicksLeft
		{
			get
			{
				return this.lastDamageTick + 16 - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x0600168F RID: 5775 RVA: 0x00015FA0 File Offset: 0x000141A0
		public bool FlashingNowOrRecently
		{
			get
			{
				return this.DamageFlashTicksLeft >= -1;
			}
		}

		// Token: 0x06001690 RID: 5776 RVA: 0x00015FAE File Offset: 0x000141AE
		public DamageFlasher(Pawn pawn)
		{
		}

		// Token: 0x06001691 RID: 5777 RVA: 0x00015FC1 File Offset: 0x000141C1
		public Material GetDamagedMat(Material baseMat)
		{
			return DamagedMatPool.GetDamageFlashMat(baseMat, (float)this.DamageFlashTicksLeft / 16f);
		}

		// Token: 0x06001692 RID: 5778 RVA: 0x00015FD6 File Offset: 0x000141D6
		public void Notify_DamageApplied(DamageInfo dinfo)
		{
			if (dinfo.Def.harmsHealth)
			{
				this.lastDamageTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x04001159 RID: 4441
		private int lastDamageTick = -9999;

		// Token: 0x0400115A RID: 4442
		private const int DamagedMatTicksTotal = 16;
	}
}
