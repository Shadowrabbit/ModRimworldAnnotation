using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200019D RID: 413
	public struct FleckStatic : IFleck
	{
		// Token: 0x17000230 RID: 560
		// (set) Token: 0x06000BA0 RID: 2976 RVA: 0x0003EE43 File Offset: 0x0003D043
		public float Scale
		{
			set
			{
				this.exactScale = new Vector3(value, 1f, value);
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000BA1 RID: 2977 RVA: 0x0003EE57 File Offset: 0x0003D057
		public float SolidTime
		{
			get
			{
				if (this.solidTimeOverride >= 0f)
				{
					return this.solidTimeOverride;
				}
				return this.def.solidTime;
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000BA2 RID: 2978 RVA: 0x0003EE78 File Offset: 0x0003D078
		public Vector3 DrawPos
		{
			get
			{
				return this.exactPosition + this.def.unattachedDrawOffset;
			}
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000BA3 RID: 2979 RVA: 0x0003EE90 File Offset: 0x0003D090
		public bool EndOfLife
		{
			get
			{
				return this.ageSecs >= this.def.Lifespan;
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000BA4 RID: 2980 RVA: 0x0003EEA8 File Offset: 0x0003D0A8
		public float Alpha
		{
			get
			{
				float num = this.ageSecs;
				if (num <= this.def.fadeInTime)
				{
					if (this.def.fadeInTime > 0f)
					{
						return num / this.def.fadeInTime;
					}
					return 1f;
				}
				else
				{
					if (num <= this.def.fadeInTime + this.SolidTime)
					{
						return 1f;
					}
					if (this.def.fadeOutTime > 0f)
					{
						return 1f - Mathf.InverseLerp(this.def.fadeInTime + this.SolidTime, this.def.fadeInTime + this.SolidTime + this.def.fadeOutTime, num);
					}
					return 1f;
				}
			}
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x0003EF60 File Offset: 0x0003D160
		public void Setup(FleckCreationData creationData)
		{
			this.def = creationData.def;
			this.exactScale = Vector3.one;
			this.instanceColor = (creationData.instanceColor ?? Color.white);
			this.solidTimeOverride = (creationData.solidTimeOverride ?? -1f);
			this.skidSpeedMultiplierPerTick = Rand.Range(0.3f, 0.95f);
			this.ageSecs = 0f;
			if (creationData.exactScale != null)
			{
				this.exactScale = creationData.exactScale.Value;
			}
			else
			{
				this.Scale = creationData.scale;
			}
			this.exactPosition = creationData.spawnPosition;
			this.exactRotation = creationData.rotation;
			this.setupTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x0003F044 File Offset: 0x0003D244
		public bool TimeInterval(float deltaTime, Map map)
		{
			if (this.EndOfLife)
			{
				return true;
			}
			this.ageSecs += deltaTime;
			this.ageTicks++;
			if (this.def.growthRate != 0f)
			{
				this.exactScale = new Vector3(this.exactScale.x + this.def.growthRate * deltaTime, this.exactScale.y, this.exactScale.z + this.def.growthRate * deltaTime);
				this.exactScale.x = Mathf.Max(this.exactScale.x, 0.0001f);
				this.exactScale.z = Mathf.Max(this.exactScale.z, 0.0001f);
			}
			return false;
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x0003F115 File Offset: 0x0003D315
		public void Draw(DrawBatch batch)
		{
			this.Draw(this.def.altitudeLayer.AltitudeFor(this.def.altitudeLayerIncOffset), batch);
		}

		// Token: 0x06000BA8 RID: 2984 RVA: 0x0003F13C File Offset: 0x0003D33C
		public void Draw(float altitude, DrawBatch batch)
		{
			this.exactPosition.y = altitude;
			((Graphic_Fleck)this.def.GetGraphicData(this.setupTick).Graphic).DrawFleck(new FleckDrawData
			{
				alpha = this.Alpha,
				color = this.instanceColor,
				drawLayer = 0,
				pos = this.DrawPos,
				rotation = this.exactRotation,
				scale = this.exactScale,
				ageSecs = this.ageSecs
			}, batch);
		}

		// Token: 0x040009AA RID: 2474
		public FleckDef def;

		// Token: 0x040009AB RID: 2475
		public Map map;

		// Token: 0x040009AC RID: 2476
		public Vector3 exactPosition;

		// Token: 0x040009AD RID: 2477
		public float exactRotation;

		// Token: 0x040009AE RID: 2478
		public Vector3 exactScale;

		// Token: 0x040009AF RID: 2479
		public Color instanceColor;

		// Token: 0x040009B0 RID: 2480
		public float solidTimeOverride;

		// Token: 0x040009B1 RID: 2481
		public float ageSecs;

		// Token: 0x040009B2 RID: 2482
		public int ageTicks;

		// Token: 0x040009B3 RID: 2483
		public int setupTick;

		// Token: 0x040009B4 RID: 2484
		public float skidSpeedMultiplierPerTick;
	}
}
