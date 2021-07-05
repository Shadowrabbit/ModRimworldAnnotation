using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000366 RID: 870
	public abstract class Mote : Thing
	{
		// Token: 0x170004F9 RID: 1273
		// (set) Token: 0x0600189E RID: 6302 RVA: 0x0009172D File Offset: 0x0008F92D
		public float Scale
		{
			set
			{
				this.exactScale = new Vector3(value, 1f, value);
			}
		}

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x0600189F RID: 6303 RVA: 0x00091741 File Offset: 0x0008F941
		public float AgeSecs
		{
			get
			{
				if (this.def.mote.realTime)
				{
					return Time.realtimeSinceStartup - this.spawnRealTime;
				}
				return (float)(Find.TickManager.TicksGame - this.spawnTick) / 60f;
			}
		}

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x060018A0 RID: 6304 RVA: 0x0009177A File Offset: 0x0008F97A
		public float AgeSecsPausable
		{
			get
			{
				return (float)this.currentAnimationTick / 60f;
			}
		}

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x060018A1 RID: 6305 RVA: 0x00091789 File Offset: 0x0008F989
		protected float SolidTime
		{
			get
			{
				if (this.solidTimeOverride >= 0f)
				{
					return this.solidTimeOverride;
				}
				return this.def.mote.solidTime;
			}
		}

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x060018A2 RID: 6306 RVA: 0x000917AF File Offset: 0x0008F9AF
		public override Vector3 DrawPos
		{
			get
			{
				return this.exactPosition + this.def.mote.unattachedDrawOffset;
			}
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x060018A3 RID: 6307 RVA: 0x000917CC File Offset: 0x0008F9CC
		protected virtual bool EndOfLife
		{
			get
			{
				return this.AgeSecs >= this.def.mote.Lifespan;
			}
		}

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x060018A4 RID: 6308 RVA: 0x000917EC File Offset: 0x0008F9EC
		public virtual float Alpha
		{
			get
			{
				float ageSecs = this.AgeSecs;
				if (this.def.mote.fadeOutUnmaintained && Find.TickManager.TicksGame - this.lastMaintainTick > 0)
				{
					if (this.def.mote.fadeOutTime > 0f)
					{
						float num = (Find.TickManager.TicksGame - this.lastMaintainTick).TicksToSeconds();
						return 1f - num / this.def.mote.fadeOutTime;
					}
					return 1f;
				}
				else if (ageSecs <= this.def.mote.fadeInTime)
				{
					if (this.def.mote.fadeInTime > 0f)
					{
						return ageSecs / this.def.mote.fadeInTime;
					}
					return 1f;
				}
				else
				{
					if (ageSecs <= this.def.mote.fadeInTime + this.SolidTime)
					{
						return 1f;
					}
					if (this.def.mote.fadeOutTime > 0f)
					{
						return 1f - Mathf.InverseLerp(this.def.mote.fadeInTime + this.SolidTime, this.def.mote.fadeInTime + this.SolidTime + this.def.mote.fadeOutTime, ageSecs);
					}
					return 1f;
				}
			}
		}

		// Token: 0x060018A5 RID: 6309 RVA: 0x00091940 File Offset: 0x0008FB40
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.spawnTick = Find.TickManager.TicksGame;
			this.spawnRealTime = Time.realtimeSinceStartup;
			RealTime.moteList.MoteSpawned(this);
			base.Map.moteCounter.Notify_MoteSpawned();
			this.exactPosition.y = this.def.altitudeLayer.AltitudeFor() + this.yOffset;
		}

		// Token: 0x060018A6 RID: 6310 RVA: 0x000919AD File Offset: 0x0008FBAD
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			RealTime.moteList.MoteDespawned(this);
			map.moteCounter.Notify_MoteDespawned();
		}

		// Token: 0x060018A7 RID: 6311 RVA: 0x000919D1 File Offset: 0x0008FBD1
		public override void Tick()
		{
			if (!this.def.mote.realTime)
			{
				this.TimeInterval(0.016666668f);
			}
			if (!this.animationPaused)
			{
				this.currentAnimationTick++;
			}
		}

		// Token: 0x060018A8 RID: 6312 RVA: 0x00091A06 File Offset: 0x0008FC06
		public void RealtimeUpdate()
		{
			if (this.def.mote.realTime)
			{
				this.TimeInterval(Time.deltaTime);
			}
		}

		// Token: 0x060018A9 RID: 6313 RVA: 0x00091A28 File Offset: 0x0008FC28
		protected virtual void TimeInterval(float deltaTime)
		{
			if (this.EndOfLife && !base.Destroyed)
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			if (this.def.mote.needsMaintenance && Find.TickManager.TicksGame - 1 > this.lastMaintainTick)
			{
				int num = this.def.mote.fadeOutTime.SecondsToTicks();
				if (!this.def.mote.fadeOutUnmaintained || Find.TickManager.TicksGame - this.lastMaintainTick > num)
				{
					this.Destroy(DestroyMode.Vanish);
					return;
				}
			}
			if (this.def.mote.growthRate != 0f)
			{
				this.exactScale = new Vector3(this.exactScale.x + this.def.mote.growthRate * deltaTime, this.exactScale.y, this.exactScale.z + this.def.mote.growthRate * deltaTime);
				this.exactScale.x = Mathf.Max(this.exactScale.x, 0.0001f);
				this.exactScale.z = Mathf.Max(this.exactScale.z, 0.0001f);
			}
		}

		// Token: 0x060018AA RID: 6314 RVA: 0x00091B63 File Offset: 0x0008FD63
		public override void Draw()
		{
			this.Draw(this.def.altitudeLayer.AltitudeFor());
		}

		// Token: 0x060018AB RID: 6315 RVA: 0x00091B7B File Offset: 0x0008FD7B
		public void Draw(float altitude)
		{
			this.exactPosition.y = altitude + this.yOffset;
			base.Draw();
		}

		// Token: 0x060018AC RID: 6316 RVA: 0x00091B96 File Offset: 0x0008FD96
		public void Maintain()
		{
			this.lastMaintainTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060018AD RID: 6317 RVA: 0x00091BA8 File Offset: 0x0008FDA8
		public void Attach(TargetInfo a, Vector3 offset)
		{
			this.link1 = new MoteAttachLink(a, offset);
		}

		// Token: 0x060018AE RID: 6318 RVA: 0x00091BB7 File Offset: 0x0008FDB7
		public void Attach(TargetInfo a)
		{
			this.link1 = new MoteAttachLink(a, Vector3.zero);
		}

		// Token: 0x060018AF RID: 6319 RVA: 0x00091BCA File Offset: 0x0008FDCA
		public override void Notify_MyMapRemoved()
		{
			base.Notify_MyMapRemoved();
			RealTime.moteList.MoteDespawned(this);
		}

		// Token: 0x040010B0 RID: 4272
		public Vector3 exactPosition;

		// Token: 0x040010B1 RID: 4273
		public float exactRotation;

		// Token: 0x040010B2 RID: 4274
		public Vector3 exactScale = new Vector3(1f, 1f, 1f);

		// Token: 0x040010B3 RID: 4275
		public float rotationRate;

		// Token: 0x040010B4 RID: 4276
		public float yOffset;

		// Token: 0x040010B5 RID: 4277
		public Color instanceColor = Color.white;

		// Token: 0x040010B6 RID: 4278
		private int lastMaintainTick;

		// Token: 0x040010B7 RID: 4279
		private int currentAnimationTick;

		// Token: 0x040010B8 RID: 4280
		public float solidTimeOverride = -1f;

		// Token: 0x040010B9 RID: 4281
		public int spawnTick;

		// Token: 0x040010BA RID: 4282
		public bool animationPaused;

		// Token: 0x040010BB RID: 4283
		public int detachAfterTicks = -1;

		// Token: 0x040010BC RID: 4284
		public float spawnRealTime;

		// Token: 0x040010BD RID: 4285
		public MoteAttachLink link1 = MoteAttachLink.Invalid;

		// Token: 0x040010BE RID: 4286
		protected float skidSpeedMultiplierPerTick = Rand.Range(0.3f, 0.95f);

		// Token: 0x040010BF RID: 4287
		protected const float MinSpeed = 0.02f;
	}
}
