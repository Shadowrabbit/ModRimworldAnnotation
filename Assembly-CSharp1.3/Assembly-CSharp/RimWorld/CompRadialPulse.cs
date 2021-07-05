using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001178 RID: 4472
	[StaticConstructorOnStartup]
	public class CompRadialPulse : ThingComp
	{
		// Token: 0x1700127F RID: 4735
		// (get) Token: 0x06006B75 RID: 27509 RVA: 0x00241640 File Offset: 0x0023F840
		private CompProperties_RadialPulse Props
		{
			get
			{
				return (CompProperties_RadialPulse)this.props;
			}
		}

		// Token: 0x17001280 RID: 4736
		// (get) Token: 0x06006B76 RID: 27510 RVA: 0x0024164D File Offset: 0x0023F84D
		private float RingLerpFactor
		{
			get
			{
				return (float)(Find.TickManager.TicksGame % this.Props.ticksBetweenPulses) / (float)this.Props.ticksPerPulse;
			}
		}

		// Token: 0x17001281 RID: 4737
		// (get) Token: 0x06006B77 RID: 27511 RVA: 0x00241673 File Offset: 0x0023F873
		private float RingScale
		{
			get
			{
				return this.Props.radius * Mathf.Lerp(0f, 2f, this.RingLerpFactor) * 1.1601562f;
			}
		}

		// Token: 0x17001282 RID: 4738
		// (get) Token: 0x06006B78 RID: 27512 RVA: 0x0024169C File Offset: 0x0023F89C
		private bool ParentIsActive
		{
			get
			{
				CompSendSignalOnPawnProximity comp = this.parent.GetComp<CompSendSignalOnPawnProximity>();
				return comp != null && comp.Sent;
			}
		}

		// Token: 0x06006B79 RID: 27513 RVA: 0x002416C0 File Offset: 0x0023F8C0
		public override void PostDraw()
		{
			if (this.ParentIsActive)
			{
				return;
			}
			Vector3 pos = this.parent.Position.ToVector3Shifted();
			pos.y = AltitudeLayer.MoteOverhead.AltitudeFor();
			Color color = this.Props.color;
			color.a = Mathf.Lerp(this.Props.color.a, 0f, this.RingLerpFactor);
			CompRadialPulse.MatPropertyBlock.SetColor(ShaderPropertyIDs.Color, color);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(pos, Quaternion.identity, new Vector3(this.RingScale, 1f, this.RingScale));
			Graphics.DrawMesh(MeshPool.plane10, matrix, CompRadialPulse.RingMat, 0, null, 0, CompRadialPulse.MatPropertyBlock);
		}

		// Token: 0x04003BC7 RID: 15303
		private static readonly Material RingMat = MaterialPool.MatFrom("Other/ForceField", ShaderDatabase.MoteGlow);

		// Token: 0x04003BC8 RID: 15304
		private static readonly MaterialPropertyBlock MatPropertyBlock = new MaterialPropertyBlock();

		// Token: 0x04003BC9 RID: 15305
		private const float TextureActualRingSizeFactor = 1.1601562f;
	}
}
