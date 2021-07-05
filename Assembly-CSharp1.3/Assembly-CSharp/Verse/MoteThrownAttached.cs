using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200036D RID: 877
	internal class MoteThrownAttached : MoteThrown
	{
		// Token: 0x060018D8 RID: 6360 RVA: 0x000926D4 File Offset: 0x000908D4
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (this.link1.Linked)
			{
				this.attacheeLastPosition = this.link1.LastDrawPos;
			}
			this.exactPosition += this.def.mote.attachedDrawOffset;
		}

		// Token: 0x060018D9 RID: 6361 RVA: 0x00092728 File Offset: 0x00090928
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

		// Token: 0x040010D0 RID: 4304
		private Vector3 attacheeLastPosition = new Vector3(-1000f, -1000f, -1000f);
	}
}
