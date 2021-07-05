using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200111D RID: 4381
	[StaticConstructorOnStartup]
	public class CompDarklightOverlay : CompFireOverlayBase
	{
		// Token: 0x17001203 RID: 4611
		// (get) Token: 0x06006939 RID: 26937 RVA: 0x00237919 File Offset: 0x00235B19
		public new CompProperties_DarklightOverlay Props
		{
			get
			{
				return (CompProperties_DarklightOverlay)this.props;
			}
		}

		// Token: 0x0600693A RID: 26938 RVA: 0x00237928 File Offset: 0x00235B28
		public override void PostDraw()
		{
			base.PostDraw();
			if (this.refuelableComp != null && !this.refuelableComp.HasFuel)
			{
				return;
			}
			Vector3 drawPos = this.parent.DrawPos;
			drawPos.y += 0.04054054f;
			CompDarklightOverlay.DarklightGraphic.Draw(drawPos, Rot4.North, this.parent, 0f);
		}

		// Token: 0x0600693B RID: 26939 RVA: 0x00237988 File Offset: 0x00235B88
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.refuelableComp = this.parent.GetComp<CompRefuelable>();
		}

		// Token: 0x04003AE3 RID: 15075
		protected CompRefuelable refuelableComp;

		// Token: 0x04003AE4 RID: 15076
		public static readonly Graphic DarklightGraphic = GraphicDatabase.Get<Graphic_Flicker>("Things/Special/Darklight", ShaderDatabase.TransparentPostLight, Vector2.one, Color.white);
	}
}
