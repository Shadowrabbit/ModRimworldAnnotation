using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004FE RID: 1278
	internal class MoteThrownAttached : MoteThrown
	{
		// Token: 0x06001FD6 RID: 8150 RVA: 0x00101270 File Offset: 0x000FF470
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (this.link1.Linked)
			{
				this.attacheeLastPosition = this.link1.LastDrawPos;
			}
			this.exactPosition += this.def.mote.attachedDrawOffset;
		}

		// Token: 0x06001FD7 RID: 8151 RVA: 0x001012C4 File Offset: 0x000FF4C4
		protected override Vector3 NextExactPosition(float deltaTime)
		{
			Vector3 vector = base.NextExactPosition(deltaTime);
			if (this.link1.Linked)
			{
				bool flag = this.detachAfterTicks == -1 || Find.TickManager.TicksGame - this.spawnTick < this.detachAfterTicks;
				if (!this.link1.Target.ThingDestroyed && flag)
				{
					this.link1.UpdateDrawPos();
				}
				Vector3 b = this.link1.LastDrawPos - this.attacheeLastPosition;
				vector += b;
				vector.y = AltitudeLayer.MoteOverhead.AltitudeFor();
				this.attacheeLastPosition = this.link1.LastDrawPos;
			}
			return vector;
		}

		// Token: 0x04001651 RID: 5713
		private Vector3 attacheeLastPosition = new Vector3(-1000f, -1000f, -1000f);
	}
}
