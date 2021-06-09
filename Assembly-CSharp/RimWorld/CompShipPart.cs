using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020018AF RID: 6319
	public class CompShipPart : ThingComp
	{
		// Token: 0x06008C3C RID: 35900 RVA: 0x0005E0A8 File Offset: 0x0005C2A8
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			yield return new Command_Action
			{
				action = new Action(this.ShowReport),
				defaultLabel = "CommandShipLaunchReport".Translate(),
				defaultDesc = "CommandShipLaunchReportDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc4,
				icon = ContentFinder<Texture2D>.Get("UI/Commands/LaunchReport", true)
			};
			yield break;
		}

		// Token: 0x06008C3D RID: 35901 RVA: 0x0028C450 File Offset: 0x0028A650
		public void ShowReport()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!ShipUtility.LaunchFailReasons((Building)this.parent).Any<string>())
			{
				stringBuilder.AppendLine("ShipReportCanLaunch".Translate());
			}
			else
			{
				stringBuilder.AppendLine("ShipReportCannotLaunch".Translate());
				foreach (string value in ShipUtility.LaunchFailReasons((Building)this.parent))
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine(value);
				}
			}
			Dialog_MessageBox window = new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null);
			Find.WindowStack.Add(window);
		}
	}
}
