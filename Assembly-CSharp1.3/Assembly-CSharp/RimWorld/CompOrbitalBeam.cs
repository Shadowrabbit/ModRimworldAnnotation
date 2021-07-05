using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001169 RID: 4457
	[StaticConstructorOnStartup]
	public class CompOrbitalBeam : ThingComp
	{
		// Token: 0x17001267 RID: 4711
		// (get) Token: 0x06006B03 RID: 27395 RVA: 0x0023EAA6 File Offset: 0x0023CCA6
		public CompProperties_OrbitalBeam Props
		{
			get
			{
				return (CompProperties_OrbitalBeam)this.props;
			}
		}

		// Token: 0x17001268 RID: 4712
		// (get) Token: 0x06006B04 RID: 27396 RVA: 0x0023EAB3 File Offset: 0x0023CCB3
		private int TicksPassed
		{
			get
			{
				return Find.TickManager.TicksGame - this.startTick;
			}
		}

		// Token: 0x17001269 RID: 4713
		// (get) Token: 0x06006B05 RID: 27397 RVA: 0x0023EAC6 File Offset: 0x0023CCC6
		private int TicksLeft
		{
			get
			{
				return this.totalDuration - this.TicksPassed;
			}
		}

		// Token: 0x1700126A RID: 4714
		// (get) Token: 0x06006B06 RID: 27398 RVA: 0x0023EAD5 File Offset: 0x0023CCD5
		private float BeamEndHeight
		{
			get
			{
				return this.Props.width * 0.5f;
			}
		}

		// Token: 0x06006B07 RID: 27399 RVA: 0x0023EAE8 File Offset: 0x0023CCE8
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
			Scribe_Values.Look<int>(ref this.totalDuration, "totalDuration", 0, false);
			Scribe_Values.Look<int>(ref this.fadeOutDuration, "fadeOutDuration", 0, false);
			Scribe_Values.Look<float>(ref this.angle, "angle", 0f, false);
		}

		// Token: 0x06006B08 RID: 27400 RVA: 0x0023EB47 File Offset: 0x0023CD47
		public void StartAnimation(int totalDuration, int fadeOutDuration, float angle)
		{
			this.startTick = Find.TickManager.TicksGame;
			this.totalDuration = totalDuration;
			this.fadeOutDuration = fadeOutDuration;
			this.angle = angle;
			this.CheckSpawnSustainer();
		}

		// Token: 0x06006B09 RID: 27401 RVA: 0x0023EB74 File Offset: 0x0023CD74
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.CheckSpawnSustainer();
		}

		// Token: 0x06006B0A RID: 27402 RVA: 0x0023EB83 File Offset: 0x0023CD83
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

		// Token: 0x06006B0B RID: 27403 RVA: 0x0023EBC0 File Offset: 0x0023CDC0
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

		// Token: 0x06006B0C RID: 27404 RVA: 0x0023EDC5 File Offset: 0x0023CFC5
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

		// Token: 0x04003B7D RID: 15229
		private int startTick;

		// Token: 0x04003B7E RID: 15230
		private int totalDuration;

		// Token: 0x04003B7F RID: 15231
		private int fadeOutDuration;

		// Token: 0x04003B80 RID: 15232
		private float angle;

		// Token: 0x04003B81 RID: 15233
		private Sustainer sustainer;

		// Token: 0x04003B82 RID: 15234
		private const float AlphaAnimationSpeed = 0.3f;

		// Token: 0x04003B83 RID: 15235
		private const float AlphaAnimationStrength = 0.025f;

		// Token: 0x04003B84 RID: 15236
		private const float BeamEndHeightRatio = 0.5f;

		// Token: 0x04003B85 RID: 15237
		private static readonly Material BeamMat = MaterialPool.MatFrom("Other/OrbitalBeam", ShaderDatabase.MoteGlow, MapMaterialRenderQueues.OrbitalBeam);

		// Token: 0x04003B86 RID: 15238
		private static readonly Material BeamEndMat = MaterialPool.MatFrom("Other/OrbitalBeamEnd", ShaderDatabase.MoteGlow, MapMaterialRenderQueues.OrbitalBeam);

		// Token: 0x04003B87 RID: 15239
		private static readonly MaterialPropertyBlock MatPropertyBlock = new MaterialPropertyBlock();
	}
}
