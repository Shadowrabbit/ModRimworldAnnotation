using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200019B RID: 411
	public struct FleckSplash : IFleck
	{
		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000B98 RID: 2968 RVA: 0x0003EB73 File Offset: 0x0003CD73
		public bool EndOfLife
		{
			get
			{
				return this.ageSecs >= this.targetSize / this.velocity;
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000B99 RID: 2969 RVA: 0x0003EB90 File Offset: 0x0003CD90
		public float Alpha
		{
			get
			{
				Mathf.Clamp01(this.ageSecs * 10f);
				float num = 1f;
				float num2 = Mathf.Clamp01(1f - this.ageSecs / (this.targetSize / this.velocity));
				return num * num2 * this.CalculatedIntensity();
			}
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x0003EBE0 File Offset: 0x0003CDE0
		public bool TimeInterval(float deltaTime, Map map)
		{
			if (this.EndOfLife)
			{
				return true;
			}
			this.ageSecs += deltaTime;
			if (this.def.growthRate != 0f)
			{
				this.exactScale = new Vector3(this.exactScale.x + this.def.growthRate * deltaTime, this.exactScale.y, this.exactScale.z + this.def.growthRate * deltaTime);
				this.exactScale.x = Mathf.Max(this.exactScale.x, 0.0001f);
				this.exactScale.z = Mathf.Max(this.exactScale.z, 0.0001f);
			}
			float d = this.ageSecs * this.velocity;
			this.exactScale = Vector3.one * d;
			this.position += map.waterInfo.GetWaterMovement(this.position) * deltaTime;
			return false;
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x0003ECEC File Offset: 0x0003CEEC
		public void Draw(DrawBatch batch)
		{
			this.position.y = this.def.altitudeLayer.AltitudeFor(this.def.altitudeLayerIncOffset);
			((Graphic_Fleck)this.def.GetGraphicData(this.setupTick).Graphic).DrawFleck(new FleckDrawData
			{
				alpha = this.Alpha,
				color = Color.white,
				drawLayer = 0,
				pos = this.position,
				rotation = 0f,
				scale = this.exactScale,
				ageSecs = this.ageSecs,
				calculatedShockwaveSpan = this.CalculatedShockwaveSpan()
			}, batch);
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x0003EDAC File Offset: 0x0003CFAC
		public void Setup(FleckCreationData creationData)
		{
			this.def = creationData.def;
			this.position = creationData.spawnPosition;
			this.velocity = creationData.velocitySpeed;
			this.targetSize = creationData.targetSize;
			this.setupTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x0003EDF9 File Offset: 0x0003CFF9
		public float CalculatedIntensity()
		{
			return Mathf.Sqrt(this.targetSize) / 10f;
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x0003EE0C File Offset: 0x0003D00C
		public float CalculatedShockwaveSpan()
		{
			return Mathf.Min(Mathf.Sqrt(this.targetSize) * 0.8f, this.exactScale.x) / this.exactScale.x;
		}

		// Token: 0x0400099D RID: 2461
		public const float VelocityFootstep = 1.5f;

		// Token: 0x0400099E RID: 2462
		public const float SizeFootstep = 2f;

		// Token: 0x0400099F RID: 2463
		public const float VelocityGunfire = 4f;

		// Token: 0x040009A0 RID: 2464
		public const float SizeGunfire = 1f;

		// Token: 0x040009A1 RID: 2465
		public const float VelocityExplosion = 20f;

		// Token: 0x040009A2 RID: 2466
		public const float SizeExplosion = 6f;

		// Token: 0x040009A3 RID: 2467
		public FleckDef def;

		// Token: 0x040009A4 RID: 2468
		private float ageSecs;

		// Token: 0x040009A5 RID: 2469
		private int setupTick;

		// Token: 0x040009A6 RID: 2470
		private float targetSize;

		// Token: 0x040009A7 RID: 2471
		private float velocity;

		// Token: 0x040009A8 RID: 2472
		private Vector3 position;

		// Token: 0x040009A9 RID: 2473
		private Vector3 exactScale;
	}
}
