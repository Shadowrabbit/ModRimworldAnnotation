using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001272 RID: 4722
	public class Alert_NeedJoySources : Alert
	{
		// Token: 0x060070FD RID: 28925 RVA: 0x0025A5CE File Offset: 0x002587CE
		public Alert_NeedJoySources()
		{
			this.defaultLabel = "NeedJoySource".Translate();
		}

		// Token: 0x060070FE RID: 28926 RVA: 0x0025A5EC File Offset: 0x002587EC
		public override TaggedString GetExplanation()
		{
			Map map = this.BadMap();
			int value = JoyUtility.JoyKindsOnMapCount(map);
			string label = map.info.parent.Label;
			ExpectationDef expectationDef = ExpectationsUtility.CurrentExpectationFor(map);
			int joyKindsNeeded = expectationDef.joyKindsNeeded;
			string value2 = "AvailableRecreationTypes".Translate().Colorize(ColoredText.TipSectionTitleColor) + ":\n\n" + JoyUtility.JoyKindsOnMapString(map);
			string value3 = "MissingRecreationTypes".Translate().Colorize(ColoredText.TipSectionTitleColor) + ":\n\n" + JoyUtility.JoyKindsNotOnMapString(map);
			return "NeedJoySourceDesc".Translate(value, label, expectationDef.label, joyKindsNeeded, value2, value3);
		}

		// Token: 0x060070FF RID: 28927 RVA: 0x0025A6A9 File Offset: 0x002588A9
		public override AlertReport GetReport()
		{
			return this.BadMap() != null;
		}

		// Token: 0x06007100 RID: 28928 RVA: 0x0025A6BC File Offset: 0x002588BC
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

		// Token: 0x06007101 RID: 28929 RVA: 0x0025A6F8 File Offset: 0x002588F8
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
