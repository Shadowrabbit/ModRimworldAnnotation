using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001815 RID: 6165
	[StaticConstructorOnStartup]
	public class CompRadialPulse : ThingComp
	{
		// Token: 0x17001545 RID: 5445
		// (get) Token: 0x0600886B RID: 34923 RVA: 0x0005B93C File Offset: 0x00059B3C
		private CompProperties_RadialPulse Props
		{
			get
			{
				return (CompProperties_RadialPulse)this.props;
			}
		}

		// Token: 0x17001546 RID: 5446
		// (get) Token: 0x0600886C RID: 34924 RVA: 0x0005B949 File Offset: 0x00059B49
		private float RingLerpFactor
		{
			get
			{
				return (float)(Find.TickManager.TicksGame % this.Props.ticksBetweenPulses) / (float)this.Props.ticksPerPulse;
			}
		}

		// Token: 0x17001547 RID: 5447
		// (get) Token: 0x0600886D RID: 34925 RVA: 0x0005B96F File Offset: 0x00059B6F
		private float RingScale
		{
			get
			{
				return this.Props.radius * Mathf.Lerp(0f, 2f, this.RingLerpFactor) * 1.1601562f;
			}
		}

		// Token: 0x17001548 RID: 5448
		// (get) Token: 0x0600886E RID: 34926 RVA: 0x0027EF60 File Offset: 0x0027D160
		private bool ParentIsActive
		{
			get
			{
				CompSendSignalOnPawnProximity comp = this.parent.GetComp<CompSendSignalOnPawnProximity>();
				return comp != null && comp.Sent;
			}
		}

		// Token: 0x0600886F RID: 34927 RVA: 0x0027EF84 File Offset: 0x0027D184
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

		// Token: 0x0400578B RID: 22411
		private static readonly Material RingMat = MaterialPool.MatFrom("Other/ForceField", ShaderDatabase.MoteGlow);

		// Token: 0x0400578C RID: 22412
		private static readonly MaterialPropertyBlock MatPropertyBlock = new MaterialPropertyBlock();

		// Token: 0x0400578D RID: 22413
		private const float TextureActualRingSizeFactor = 1.1601562f;
	}
}
