using System;

namespace RimWorld.Planet
{
	// Token: 0x0200176B RID: 5995
	public class WorldLayer_WorldObjects_NonExpandable : WorldLayer_WorldObjects
	{
		// Token: 0x06008A4F RID: 35407 RVA: 0x00319FE1 File Offset: 0x003181E1
		protected override bool ShouldSkip(WorldObject worldObject)
		{
			return worldObject.def.expandingIcon;
		}
	}
}
