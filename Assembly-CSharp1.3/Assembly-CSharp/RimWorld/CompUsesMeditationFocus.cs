using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011CD RID: 4557
	public class CompUsesMeditationFocus : ThingComp
	{
		// Token: 0x06006DFB RID: 28155 RVA: 0x0024DF53 File Offset: 0x0024C153
		public override void PostDrawExtraSelectionOverlays()
		{
			MeditationUtility.DrawMeditationSpotOverlay(this.parent.Position, this.parent.Map);
		}
	}
}
