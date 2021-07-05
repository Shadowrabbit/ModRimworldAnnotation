using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020017FE RID: 6142
	[StaticConstructorOnStartup]
	public class CompOrbitalBeam : ThingComp
	{
		// Token: 0x1700152A RID: 5418
		// (get) Token: 0x060087E5 RID: 34789 RVA: 0x0005B2B1 File Offset: 0x000594B1
		public CompProperties_OrbitalBeam Props
		{
			get
			{
				return (CompProperties_OrbitalBeam)this.props;
			}
		}

		// Token: 0x1700152B RID: 5419
		// (get) Token: 0x060087E6 RID: 34790 RVA: 0x0005B2BE File Offset: 0x000594BE
		private int TicksPassed
		{
			get
			{
				return Find.TickManager.TicksGame - this.startTick;
			}
		}

		// Token: 0x1700152C RID: 5420
		// (get) Token: 0x060087E7 RID: 34791 RVA: 0x0005B2D1 File Offset: 0x000594D1
		private int TicksLeft
		{
			get
			{
				return this.totalDuration - this.TicksPassed;
			}
		}

		// Token: 0x1700152D RID: 5421
		// (get) Token: 0x060087E8 RID: 34792 RVA: 0x0005B2E0 File Offset: 0x000594E0
		private float BeamEndHeight
		{
			get
			{
				return this.Props.width * 0.5f;
			}
		}

		// Token: 0x060087E9 RID: 34793 RVA: 0x0027CD00 File Offset: 0x0027AF00
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
			Scribe_Values.Look<int>(ref this.totalDuration, "totalDuration", 0, false);
			Scribe_Values.Look<int>(ref this.fadeOutDuration, "fadeOutDuration", 0, false);
			Scribe_Values.Look<float>(ref this.angle, "angle", 0f, false);
		}

		// Token: 0x060087EA RID: 34794 RVA: 0x0005B2F3 File Offset: 0x000594F3
		public void StartAnimation(int totalDuration, int fadeOutDuration, float angle)
		{
			this.startTick = Find.TickManager.TicksGame;
			this.totalDuration = totalDuration;
			this.fadeOutDuration = fadeOutDuration;
			this.angle = angle;
			this.CheckSpawnSustainer();
		}

		// Token: 0x060087EB RID: 34795 RVA: 0x0005B320 File Offset: 0x00059520
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.CheckSpawnSustainer();
		}

		// Token: 0x060087EC RID: 34796 RVA: 0x0005B32F File Offset: 0x0005952F
		public override void CompTick()
		{
			base.CompTick();
			if (this.sustainer != null)
			{
				this.sustainer.Maintain();
				if (this.TicksLeft < this.fadeOutDuration)
				{
					this.sustainer.End();
					this.sustainer = null;
				}
			}
		}

		// Token: 0x060087ED RID: 34797 RVA: 0x0027CD60 File Offset: 0x0027AF60
		public override void PostDraw()
		{
			base.PostDraw();
			if (this.TicksLeft <= 0)
			{
				return;
			}
			Vector3 drawPos = this.parent.DrawPos;
			float num = ((float)this.parent.Map.Size.z - drawPos.z) * 1.4142135f;
			Vector3 a = Vector3Utility.FromAngleFlat(this.angle - 90f);
			Vector3 a2 = drawPos + a * num * 0.5f;
			a2.y = AltitudeLayer.MetaOverlays.AltitudeFor();
			float num2 = Mathf.Min((float)this.TicksPassed / 10f, 1f);
			Vector3 b = a * ((1f - num2) * num);
			float num3 = 0.975f + Mathf.Sin((float)this.TicksPassed * 0.3f) * 0.025f;
			if (this.TicksLeft < this.fadeOutDuration)
			{
				num3 *= (float)this.TicksLeft / (float)this.fadeOutDuration;
			}
			Color color = this.Props.color;
			color.a *= num3;
			CompOrbitalBeam.MatPropertyBlock.SetColor(ShaderPropertyIDs.Color, color);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(a2 + a * this.BeamEndHeight * 0.5f + b, Quaternion.Euler(0f, this.angle, 0f), new Vector3(this.Props.width, 1f, num));
			Graphics.DrawMesh(MeshPool.plane10, matrix, CompOrbitalBeam.BeamMat, 0, null, 0, CompOrbitalBeam.MatPropertyBlock);
			Vector3 pos = drawPos + b;
			pos.y = AltitudeLayer.MetaOverlays.AltitudeFor();
			Matrix4x4 matrix2 = default(Matrix4x4);
			matrix2.SetTRS(pos, Quaternion.Euler(0f, this.angle, 0f), new Vector3(this.Props.width, 1f, this.BeamEndHeight));
			Graphics.DrawMesh(MeshPool.plane10, matrix2, CompOrbitalBeam.BeamEndMat, 0, null, 0, CompOrbitalBeam.MatPropertyBlock);
		}

		// Token: 0x060087EE RID: 34798 RVA: 0x0005B36A File Offset: 0x0005956A
		private void CheckSpawnSustainer()
		{
			if (this.TicksLeft >= this.fadeOutDuration && this.Props.sound != null)
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.sustainer = this.Props.sound.TrySpawnSustainer(SoundInfo.InMap(this.parent, MaintenanceType.PerTick));
				});
			}
		}

		// Token: 0x04005726 RID: 22310
		private int startTick;

		// Token: 0x04005727 RID: 22311
		private int totalDuration;

		// Token: 0x04005728 RID: 22312
		private int fadeOutDuration;

		// Token: 0x04005729 RID: 22313
		private float angle;

		// Token: 0x0400572A RID: 22314
		private Sustainer sustainer;

		// Token: 0x0400572B RID: 22315
		private const float AlphaAnimationSpeed = 0.3f;

		// Token: 0x0400572C RID: 22316
		private const float AlphaAnimationStrength = 0.025f;

		// Token: 0x0400572D RID: 22317
		private const float BeamEndHeightRatio = 0.5f;

		// Token: 0x0400572E RID: 22318
		private static readonly Material BeamMat = MaterialPool.MatFrom("Other/OrbitalBeam", ShaderDatabase.MoteGlow, MapMaterialRenderQueues.OrbitalBeam);

		// Token: 0x0400572F RID: 22319
		private static readonly Material BeamEndMat = MaterialPool.MatFrom("Other/OrbitalBeamEnd", ShaderDatabase.MoteGlow, MapMaterialRenderQueues.OrbitalBeam);

		// Token: 0x04005730 RID: 22320
		private static readonly MaterialPropertyBlock MatPropertyBlock = new MaterialPropertyBlock();
	}
}
