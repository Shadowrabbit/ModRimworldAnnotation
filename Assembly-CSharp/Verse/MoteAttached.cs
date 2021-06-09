using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004F8 RID: 1272
	public class MoteAttached : Mote
	{
		// Token: 0x06001FAE RID: 8110 RVA: 0x0001BEB8 File Offset: 0x0001A0B8
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.exactPosition += this.def.mote.attachedDrawOffset;
		}

		// Token: 0x06001FAF RID: 8111 RVA: 0x00100A24 File Offset: 0x000FEC24
		protected override void TimeInterval(float deltaTime)
		{
			base.TimeInterval(deltaTime);
			if (this.link1.Linked)
			{
				bool flag = this.detachAfterTicks == -1 || Find.TickManager.TicksGame - this.spawnTick < this.detachAfterTicks;
				if (!this.link1.Target.ThingDestroyed && flag)
				{
					this.link1.UpdateDrawPos();
				}
				Vector3 b = this.def.mote.attachedDrawOffset;
				if (this.def.mote.attachedToHead)
				{
					Pawn pawn = this.link1.Target.Thing as Pawn;
					if (pawn != null && pawn.story != null)
					{
						b = pawn.Drawer.renderer.BaseHeadOffsetAt((pawn.GetPosture() == PawnPosture.Standing) ? Rot4.North : pawn.Drawer.renderer.LayingFacing()).RotatedBy(pawn.Drawer.renderer.BodyAngle());
					}
				}
				this.exactPosition = this.link1.LastDrawPos + b;
			}
		}
	}
}
