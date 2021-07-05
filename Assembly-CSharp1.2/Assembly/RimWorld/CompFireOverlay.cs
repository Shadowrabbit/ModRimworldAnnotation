using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020017BE RID: 6078
	[StaticConstructorOnStartup]
	public class CompFireOverlay : ThingComp
	{
		// Token: 0x170014D6 RID: 5334
		// (get) Token: 0x06008671 RID: 34417 RVA: 0x0005A2E9 File Offset: 0x000584E9
		public CompProperties_FireOverlay Props
		{
			get
			{
				return (CompProperties_FireOverlay)this.props;
			}
		}

		// Token: 0x06008672 RID: 34418 RVA: 0x00278E74 File Offset: 0x00277074
		public override void PostDraw()
		{
			base.PostDraw();
			if (this.refuelableComp != null && !this.refuelableComp.HasFuel)
			{
				return;
			}
			Vector3 drawPos = this.parent.DrawPos;
			drawPos.y += 0.042857144f;
			CompFireOverlay.FireGraphic.Draw(drawPos, Rot4.North, this.parent, 0f);
		}

		// Token: 0x06008673 RID: 34419 RVA: 0x0005A2F6 File Offset: 0x000584F6
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.refuelableComp = this.parent.GetComp<CompRefuelable>();
		}

		// Token: 0x0400568A RID: 22154
		protected CompRefuelable refuelableComp;

		// Token: 0x0400568B RID: 22155
		public static readonly Graphic FireGraphic = GraphicDatabase.Get<Graphic_Flicker>("Things/Special/Fire", ShaderDatabase.TransparentPostLight, Vector2.one, Color.white);
	}
}
