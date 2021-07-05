using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001066 RID: 4198
	public class Building_Vent : Building_TempControl
	{
		// Token: 0x170010F9 RID: 4345
		// (get) Token: 0x0600637E RID: 25470 RVA: 0x00219E5A File Offset: 0x0021805A
		public override Graphic Graphic
		{
			get
			{
				return this.flickableComp.CurrentGraphic;
			}
		}

		// Token: 0x0600637F RID: 25471 RVA: 0x00219E67 File Offset: 0x00218067
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.flickableComp = base.GetComp<CompFlickable>();
		}

		// Token: 0x06006380 RID: 25472 RVA: 0x00219E7D File Offset: 0x0021807D
		public override void TickRare()
		{
			if (FlickUtility.WantsToBeOn(this))
			{
				GenTemperature.EqualizeTemperaturesThroughBuilding(this, 14f, true);
			}
		}

		// Token: 0x06006381 RID: 25473 RVA: 0x00219E94 File Offset: 0x00218094
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (!FlickUtility.WantsToBeOn(this))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append("VentClosed".Translate());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400385B RID: 14427
		private CompFlickable flickableComp;
	}
}
