using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001895 RID: 6293
	public class CompUsesMeditationFocus : ThingComp
	{
		// Token: 0x06008BAE RID: 35758 RVA: 0x0005D9E4 File Offset: 0x0005BBE4
		public override void PostDrawExtraSelectionOverlays()
		{
			MeditationUtility.DrawMeditationSpotOverlay(this.parent.Position, this.parent.Map);
		}
	}
}
