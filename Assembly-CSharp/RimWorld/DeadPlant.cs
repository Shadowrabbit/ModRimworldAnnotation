using System;

namespace RimWorld
{
	// Token: 0x02001718 RID: 5912
	public class DeadPlant : Plant
	{
		// Token: 0x17001431 RID: 5169
		// (get) Token: 0x0600824E RID: 33358 RVA: 0x0000A2E4 File Offset: 0x000084E4
		protected override bool Resting
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001432 RID: 5170
		// (get) Token: 0x0600824F RID: 33359 RVA: 0x00016647 File Offset: 0x00014847
		public override float GrowthRate
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17001433 RID: 5171
		// (get) Token: 0x06008250 RID: 33360 RVA: 0x00016647 File Offset: 0x00014847
		public override float CurrentDyingDamagePerTick
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17001434 RID: 5172
		// (get) Token: 0x06008251 RID: 33361 RVA: 0x0000FC1E File Offset: 0x0000DE1E
		public override string LabelMouseover
		{
			get
			{
				return this.LabelCap;
			}
		}

		// Token: 0x17001435 RID: 5173
		// (get) Token: 0x06008252 RID: 33362 RVA: 0x0001B6B4 File Offset: 0x000198B4
		public override PlantLifeStage LifeStage
		{
			get
			{
				return PlantLifeStage.Mature;
			}
		}

		// Token: 0x06008253 RID: 33363 RVA: 0x0000C32E File Offset: 0x0000A52E
		public override string GetInspectStringLowPriority()
		{
			return null;
		}

		// Token: 0x06008254 RID: 33364 RVA: 0x0000A713 File Offset: 0x00008913
		public override string GetInspectString()
		{
			return "";
		}

		// Token: 0x06008255 RID: 33365 RVA: 0x00006A05 File Offset: 0x00004C05
		public override void CropBlighted()
		{
		}
	}
}
