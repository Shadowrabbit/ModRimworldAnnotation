using System;

namespace RimWorld.Planet
{
	// Token: 0x02001769 RID: 5993
	public class WorldLayer_WorldObjects_Expandable : WorldLayer_WorldObjects
	{
		// Token: 0x1700168F RID: 5775
		// (get) Token: 0x06008A43 RID: 35395 RVA: 0x00319BAA File Offset: 0x00317DAA
		protected override float Alpha
		{
			get
			{
				return 1f - ExpandableWorldObjectsUtility.TransitionPct;
			}
		}

		// Token: 0x06008A44 RID: 35396 RVA: 0x00319BB7 File Offset: 0x00317DB7
		protected override bool ShouldSkip(WorldObject worldObject)
		{
			return !worldObject.def.expandingIcon;
		}
	}
}
