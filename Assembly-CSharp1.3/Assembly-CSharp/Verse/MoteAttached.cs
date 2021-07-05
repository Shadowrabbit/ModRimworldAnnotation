using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000368 RID: 872
	public class MoteAttached : Mote
	{
		// Token: 0x060018B7 RID: 6327 RVA: 0x00091D0A File Offset: 0x0008FF0A
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.exactPosition += this.def.mote.attachedDrawOffset;
		}

		// Token: 0x060018B8 RID: 6328 RVA: 0x00091D38 File Offset: 0x0008FF38
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
