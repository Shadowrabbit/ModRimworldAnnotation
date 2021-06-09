using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200182A RID: 6186
	public class CompReportWorkSpeed : ThingComp
	{
		// Token: 0x06008925 RID: 35109 RVA: 0x002813F0 File Offset: 0x0027F5F0
		public override string CompInspectStringExtra()
		{
			if (this.parent.def.statBases == null)
			{
				return null;
			}
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			foreach (StatDef statDef in DefDatabase<StatDef>.AllDefsListForReading)
			{
				if (statDef != null && statDef.parts != null && !statDef.Worker.IsDisabledFor(this.parent))
				{
					foreach (StatPart statPart in statDef.parts)
					{
						if (statPart is StatPart_WorkTableOutdoors || statPart is StatPart_Outdoors)
						{
							flag = true;
						}
						else if (statPart is StatPart_WorkTableTemperature)
						{
							flag2 = true;
						}
						else if (statPart is StatPart_WorkTableUnpowered)
						{
							flag3 = true;
						}
					}
				}
			}
			bool flag4 = flag && StatPart_WorkTableOutdoors.Applies(this.parent.def, this.parent.Map, this.parent.Position);
			bool flag5 = flag2 && StatPart_WorkTableTemperature.Applies(this.parent);
			bool flag6 = flag3 && StatPart_WorkTableUnpowered.Applies(this.parent);
			if (flag4 || flag5 || flag6)
			{
				string str = "WorkSpeedPenalty".Translate() + ": ";
				string text = "";
				if (flag4)
				{
					text += "Outdoors".Translate().ToLower();
				}
				if (flag5)
				{
					if (!text.NullOrEmpty())
					{
						text += ", ";
					}
					text += "BadTemperature".Translate().ToLower();
				}
				if (flag6)
				{
					if (!text.NullOrEmpty())
					{
						text += ", ";
					}
					text += "NoPower".Translate().ToLower();
				}
				return str + text.CapitalizeFirst();
			}
			return null;
		}
	}
}
