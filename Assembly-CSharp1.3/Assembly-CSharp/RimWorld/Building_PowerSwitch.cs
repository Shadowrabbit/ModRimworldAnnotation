using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200104E RID: 4174
	[StaticConstructorOnStartup]
	public class Building_PowerSwitch : Building
	{
		// Token: 0x170010C8 RID: 4296
		// (get) Token: 0x060062B6 RID: 25270 RVA: 0x00217621 File Offset: 0x00215821
		public override bool TransmitsPowerNow
		{
			get
			{
				return FlickUtility.WantsToBeOn(this);
			}
		}

		// Token: 0x170010C9 RID: 4297
		// (get) Token: 0x060062B7 RID: 25271 RVA: 0x00217629 File Offset: 0x00215829
		public override Graphic Graphic
		{
			get
			{
				return this.flickableComp.CurrentGraphic;
			}
		}

		// Token: 0x060062B8 RID: 25272 RVA: 0x00217636 File Offset: 0x00215836
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.flickableComp = base.GetComp<CompFlickable>();
		}

		// Token: 0x060062B9 RID: 25273 RVA: 0x0021764C File Offset: 0x0021584C
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.flickableComp == null)
				{
					this.flickableComp = base.GetComp<CompFlickable>();
				}
				this.wantsOnOld = !FlickUtility.WantsToBeOn(this);
				this.UpdatePowerGrid();
			}
		}

		// Token: 0x060062BA RID: 25274 RVA: 0x00217685 File Offset: 0x00215885
		protected override void ReceiveCompSignal(string signal)
		{
			if (signal == "FlickedOff" || signal == "FlickedOn" || signal == "ScheduledOn" || signal == "ScheduledOff")
			{
				this.UpdatePowerGrid();
			}
		}

		// Token: 0x060062BB RID: 25275 RVA: 0x002176C4 File Offset: 0x002158C4
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (stringBuilder.Length != 0)
			{
				stringBuilder.AppendLine();
			}
			stringBuilder.Append("PowerSwitch_Power".Translate() + ": ");
			if (FlickUtility.WantsToBeOn(this))
			{
				stringBuilder.Append("On".Translate().ToLower());
			}
			else
			{
				stringBuilder.Append("Off".Translate().ToLower());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060062BC RID: 25276 RVA: 0x0021775F File Offset: 0x0021595F
		private void UpdatePowerGrid()
		{
			if (FlickUtility.WantsToBeOn(this) != this.wantsOnOld)
			{
				if (base.Spawned)
				{
					base.Map.powerNetManager.Notfiy_TransmitterTransmitsPowerNowChanged(base.PowerComp);
				}
				this.wantsOnOld = FlickUtility.WantsToBeOn(this);
			}
		}

		// Token: 0x04003818 RID: 14360
		private bool wantsOnOld = true;

		// Token: 0x04003819 RID: 14361
		private CompFlickable flickableComp;
	}
}
