using System;

namespace RimWorld.Planet
{
	// Token: 0x02002069 RID: 8297
	public class WorldLayer_WorldObjects_NonExpandable : WorldLayer_WorldObjects
	{
		// Token: 0x0600AFF6 RID: 45046 RVA: 0x00072640 File Offset: 0x00070840
		protected override bool ShouldSkip(WorldObject worldObject)
		{
			return worldObject.def.expandingIcon;
		}
	}
}
