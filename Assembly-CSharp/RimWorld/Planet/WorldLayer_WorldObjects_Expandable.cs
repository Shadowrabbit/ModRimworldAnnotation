using System;

namespace RimWorld.Planet
{
	// Token: 0x02002066 RID: 8294
	public class WorldLayer_WorldObjects_Expandable : WorldLayer_WorldObjects
	{
		// Token: 0x17001A09 RID: 6665
		// (get) Token: 0x0600AFE6 RID: 45030 RVA: 0x000725B6 File Offset: 0x000707B6
		protected override float Alpha
		{
			get
			{
				return 1f - ExpandableWorldObjectsUtility.TransitionPct;
			}
		}

		// Token: 0x0600AFE7 RID: 45031 RVA: 0x000725C3 File Offset: 0x000707C3
		protected override bool ShouldSkip(WorldObject worldObject)
		{
			return !worldObject.def.expandingIcon;
		}
	}
}
