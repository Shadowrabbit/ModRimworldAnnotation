using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001961 RID: 6497
	public class Alert_NeedJoySources : Alert
	{
		// Token: 0x06008FC8 RID: 36808 RVA: 0x00060612 File Offset: 0x0005E812
		public Alert_NeedJoySources()
		{
			this.defaultLabel = "NeedJoySource".Translate();
		}

		// Token: 0x06008FC9 RID: 36809 RVA: 0x00296508 File Offset: 0x00294708
		public override TaggedString GetExplanation()
		{
			Map map = this.BadMap();
			int value = JoyUtility.JoyKindsOnMapCount(map);
			string label = map.info.parent.Label;
			ExpectationDef expectationDef = ExpectationsUtility.CurrentExpectationFor(map);
			int joyKindsNeeded = expectationDef.joyKindsNeeded;
			string value2 = "AvailableRecreationTypes".Translate() + ":\n\n" + JoyUtility.JoyKindsOnMapString(map);
			string value3 = "MissingRecreationTypes".Translate() + ":\n\n" + JoyUtility.JoyKindsNotOnMapString(map);
			return "NeedJoySourceDesc".Translate(value, label, expectationDef.label, joyKindsNeeded, value2, value3);
		}

		// Token: 0x06008FCA RID: 36810 RVA: 0x0006062F File Offset: 0x0005E82F
		public override AlertReport GetReport()
		{
			return this.BadMap() != null;
		}

		// Token: 0x06008FCB RID: 36811 RVA: 0x002965C8 File Offset: 0x002947C8
		private Map BadMap()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (this.NeedJoySource(maps[i]))
				{
					return maps[i];
				}
			}
			return null;
		}

		// Token: 0x06008FCC RID: 36812 RVA: 0x00296604 File Offset: 0x00294804
		private bool NeedJoySource(Map map)
		{
			if (!map.IsPlayerHome)
			{
				return false;
			}
			if (!map.mapPawns.AnyColonistSpawned)
			{
				return false;
			}
			int num = JoyUtility.JoyKindsOnMapCount(map);
			int joyKindsNeeded = ExpectationsUtility.CurrentExpectationFor(map).joyKindsNeeded;
			return num < joyKindsNeeded;
		}
	}
}
