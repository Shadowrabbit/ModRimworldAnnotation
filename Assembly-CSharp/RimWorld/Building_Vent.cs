using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200169E RID: 5790
	public class Building_Vent : Building_TempControl
	{
		// Token: 0x17001397 RID: 5015
		// (get) Token: 0x06007EA0 RID: 32416 RVA: 0x0005518B File Offset: 0x0005338B
		public override Graphic Graphic
		{
			get
			{
				return this.flickableComp.CurrentGraphic;
			}
		}

		// Token: 0x06007EA1 RID: 32417 RVA: 0x00055198 File Offset: 0x00053398
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.flickableComp = base.GetComp<CompFlickable>();
		}

		// Token: 0x06007EA2 RID: 32418 RVA: 0x000551AE File Offset: 0x000533AE
		public override void TickRare()
		{
			if (FlickUtility.WantsToBeOn(this))
			{
				GenTemperature.EqualizeTemperaturesThroughBuilding(this, 14f, true);
			}
		}

		// Token: 0x06007EA3 RID: 32419 RVA: 0x0025A55C File Offset: 0x0025875C
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

		// Token: 0x04005264 RID: 21092
		private CompFlickable flickableComp;
	}
}
