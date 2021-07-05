using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000369 RID: 873
	public class MoteDualAttached : Mote
	{
		// Token: 0x060018BA RID: 6330 RVA: 0x00091E52 File Offset: 0x00090052
		public void Attach(TargetInfo a, TargetInfo b)
		{
			this.link1 = new MoteAttachLink(a, Vector3.zero);
			this.link2 = new MoteAttachLink(b, Vector3.zero);
		}

		// Token: 0x060018BB RID: 6331 RVA: 0x00091E76 File Offset: 0x00090076
		public override void Draw()
		{
			this.UpdatePositionAndRotation();
			base.Draw();
		}

		// Token: 0x060018BC RID: 6332 RVA: 0x00091E84 File Offset: 0x00090084
		protected void UpdatePositionAndRotation()
		{
			if (this.link1.Linked)
			{
				if (this.link2.Linked)
				{
					if (!this.link1.Target.ThingDestroyed)
					{
						this.link1.UpdateDrawPos();
					}
					if (!this.link2.Target.ThingDestroyed)
					{
						this.link2.UpdateDrawPos();
					}
					this.exactPosition = (this.link1.LastDrawPos + this.link2.LastDrawPos) * 0.5f;
					if (this.def.mote.rotateTowardsTarget)
					{
						this.exactRotation = this.link1.LastDrawPos.AngleToFlat(this.link2.LastDrawPos) + 90f;
					}
					if (this.def.mote.scaleToConnectTargets)
					{
						this.exactScale = new Vector3(this.def.graphicData.drawSize.y, 1f, (this.link2.LastDrawPos - this.link1.LastDrawPos).MagnitudeHorizontal());
					}
				}
				else
				{
					if (!this.link1.Target.ThingDestroyed)
					{
						this.link1.UpdateDrawPos();
					}
					this.exactPosition = this.link1.LastDrawPos + this.def.mote.attachedDrawOffset;
				}
			}
			this.exactPosition.y = this.def.altitudeLayer.AltitudeFor();
		}

		// Token: 0x040010C4 RID: 4292
		protected MoteAttachLink link2 = MoteAttachLink.Invalid;
	}
}
