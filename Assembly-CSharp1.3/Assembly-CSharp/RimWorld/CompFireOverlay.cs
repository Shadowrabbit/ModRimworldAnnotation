using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001131 RID: 4401
	[StaticConstructorOnStartup]
	public class CompFireOverlay : CompFireOverlayBase
	{
		// Token: 0x1700121C RID: 4636
		// (get) Token: 0x060069C7 RID: 27079 RVA: 0x0023A37D File Offset: 0x0023857D
		public new CompProperties_FireOverlay Props
		{
			get
			{
				return (CompProperties_FireOverlay)this.props;
			}
		}

		// Token: 0x060069C8 RID: 27080 RVA: 0x0023A40C File Offset: 0x0023860C
		public override void PostDraw()
		{
			base.PostDraw();
			if (this.refuelableComp != null && !this.refuelableComp.HasFuel)
			{
				return;
			}
			Vector3 drawPos = this.parent.DrawPos;
			drawPos.y += 0.04054054f;
			CompFireOverlay.FireGraphic.Draw(drawPos, Rot4.North, this.parent, 0f);
		}

		// Token: 0x060069C9 RID: 27081 RVA: 0x0023A46C File Offset: 0x0023866C
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.refuelableComp = this.parent.GetComp<CompRefuelable>();
		}

		// Token: 0x060069CA RID: 27082 RVA: 0x0023A486 File Offset: 0x00238686
		public override void CompTick()
		{
			if (this.refuelableComp != null && !this.refuelableComp.HasFuel)
			{
				return;
			}
			if (this.startedGrowingAtTick < 0)
			{
				this.startedGrowingAtTick = GenTicks.TicksAbs;
			}
		}

		// Token: 0x04003B0F RID: 15119
		protected CompRefuelable refuelableComp;

		// Token: 0x04003B10 RID: 15120
		public static readonly Graphic FireGraphic = GraphicDatabase.Get<Graphic_Flicker>("Things/Special/Fire", ShaderDatabase.TransparentPostLight, Vector2.one, Color.white);
	}
}
