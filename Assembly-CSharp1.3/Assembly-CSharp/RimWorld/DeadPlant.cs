using System;

namespace RimWorld
{
	// Token: 0x020010BB RID: 4283
	public class DeadPlant : Plant
	{
		// Token: 0x17001180 RID: 4480
		// (get) Token: 0x0600665B RID: 26203 RVA: 0x0001276E File Offset: 0x0001096E
		protected override bool Resting
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001181 RID: 4481
		// (get) Token: 0x0600665C RID: 26204 RVA: 0x000682C5 File Offset: 0x000664C5
		public override float GrowthRate
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17001182 RID: 4482
		// (get) Token: 0x0600665D RID: 26205 RVA: 0x000682C5 File Offset: 0x000664C5
		public override float CurrentDyingDamagePerTick
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17001183 RID: 4483
		// (get) Token: 0x0600665E RID: 26206 RVA: 0x00029737 File Offset: 0x00027937
		public override string LabelMouseover
		{
			get
			{
				return this.LabelCap;
			}
		}

		// Token: 0x17001184 RID: 4484
		// (get) Token: 0x0600665F RID: 26207 RVA: 0x0009007E File Offset: 0x0008E27E
		public override PlantLifeStage LifeStage
		{
			get
			{
				return PlantLifeStage.Mature;
			}
		}

		// Token: 0x06006660 RID: 26208 RVA: 0x00002688 File Offset: 0x00000888
		public override string GetInspectStringLowPriority()
		{
			return null;
		}

		// Token: 0x06006661 RID: 26209 RVA: 0x00014F75 File Offset: 0x00013175
		public override string GetInspectString()
		{
			return "";
		}

		// Token: 0x06006662 RID: 26210 RVA: 0x0000313F File Offset: 0x0000133F
		public override void CropBlighted()
		{
		}
	}
}
