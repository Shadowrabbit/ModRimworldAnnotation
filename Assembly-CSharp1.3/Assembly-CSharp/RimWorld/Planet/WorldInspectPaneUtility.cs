using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001819 RID: 6169
	public static class WorldInspectPaneUtility
	{
		// Token: 0x06009096 RID: 37014 RVA: 0x0033DC48 File Offset: 0x0033BE48
		public static string AdjustedLabelFor(List<WorldObject> worldObjects, Rect rect)
		{
			if (worldObjects.Count == 1)
			{
				return worldObjects[0].LabelCap;
			}
			if (WorldInspectPaneUtility.AllLabelsAreSame(worldObjects))
			{
				return worldObjects[0].LabelCap + " x" + worldObjects.Count.ToStringCached();
			}
			return "VariousLabel".Translate();
		}

		// Token: 0x06009097 RID: 37015 RVA: 0x0033DCA4 File Offset: 0x0033BEA4
		private static bool AllLabelsAreSame(List<WorldObject> worldObjects)
		{
			if (worldObjects.Count <= 1)
			{
				return true;
			}
			string labelCap = worldObjects[0].LabelCap;
			for (int i = 1; i < worldObjects.Count; i++)
			{
				if (worldObjects[i].LabelCap != labelCap)
				{
					return false;
				}
			}
			return true;
		}
	}
}
