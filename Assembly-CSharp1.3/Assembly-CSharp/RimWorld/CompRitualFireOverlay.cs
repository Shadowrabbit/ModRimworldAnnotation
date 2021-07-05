using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001132 RID: 4402
	[StaticConstructorOnStartup]
	public class CompRitualFireOverlay : CompFireOverlayBase
	{
		// Token: 0x1700121D RID: 4637
		// (get) Token: 0x060069CD RID: 27085 RVA: 0x0023A4D2 File Offset: 0x002386D2
		public new CompProperties_FireOverlayRitual Props
		{
			get
			{
				return (CompProperties_FireOverlayRitual)this.props;
			}
		}

		// Token: 0x060069CE RID: 27086 RVA: 0x0023A4E0 File Offset: 0x002386E0
		public override void PostDraw()
		{
			base.PostDraw();
			LordJob_Ritual lordJob_Ritual = this.parent.TargetOfRitual();
			if (lordJob_Ritual == null)
			{
				return;
			}
			if (lordJob_Ritual.Progress < this.Props.minRitualProgress)
			{
				return;
			}
			Vector3 loc = this.parent.TrueCenter();
			loc.y = AltitudeLayer.BuildingOnTop.AltitudeFor();
			CompFireOverlay.FireGraphic.Draw(loc, Rot4.North, this.parent, 0f);
		}

		// Token: 0x060069CF RID: 27087 RVA: 0x0023A54C File Offset: 0x0023874C
		public override void CompTick()
		{
			LordJob_Ritual lordJob_Ritual = this.parent.TargetOfRitual();
			if (lordJob_Ritual == null)
			{
				return;
			}
			if (lordJob_Ritual.Progress < this.Props.minRitualProgress)
			{
				return;
			}
			if (this.startedGrowingAtTick < 0)
			{
				this.startedGrowingAtTick = GenTicks.TicksAbs;
			}
			if (GenTicks.TicksAbs % 30 == 0)
			{
				FleckMaker.ThrowFireGlow(this.parent.TrueCenter(), this.parent.Map, 1f);
			}
		}

		// Token: 0x04003B11 RID: 15121
		public const int FireGlowIntervalTicks = 30;
	}
}
