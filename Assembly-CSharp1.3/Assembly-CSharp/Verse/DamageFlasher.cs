using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000260 RID: 608
	public class DamageFlasher
	{
		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06001140 RID: 4416 RVA: 0x0006213D File Offset: 0x0006033D
		private int DamageFlashTicksLeft
		{
			get
			{
				return this.lastDamageTick + 16 - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06001141 RID: 4417 RVA: 0x00062153 File Offset: 0x00060353
		public bool FlashingNowOrRecently
		{
			get
			{
				return this.DamageFlashTicksLeft >= -1;
			}
		}

		// Token: 0x06001142 RID: 4418 RVA: 0x00062161 File Offset: 0x00060361
		public DamageFlasher(Pawn pawn)
		{
		}

		// Token: 0x06001143 RID: 4419 RVA: 0x00062174 File Offset: 0x00060374
		public Material GetDamagedMat(Material baseMat)
		{
			return DamagedMatPool.GetDamageFlashMat(baseMat, (float)this.DamageFlashTicksLeft / 16f);
		}

		// Token: 0x06001144 RID: 4420 RVA: 0x00062189 File Offset: 0x00060389
		public void Notify_DamageApplied(DamageInfo dinfo)
		{
			if (dinfo.Def.harmsHealth)
			{
				this.lastDamageTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x04000D25 RID: 3365
		private int lastDamageTick = -9999;

		// Token: 0x04000D26 RID: 3366
		private const int DamagedMatTicksTotal = 16;
	}
}
