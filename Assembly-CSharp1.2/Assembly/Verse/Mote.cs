using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004F6 RID: 1270
	public abstract class Mote : Thing
	{
		// Token: 0x170005D9 RID: 1497
		// (set) Token: 0x06001F97 RID: 8087 RVA: 0x0001BCFE File Offset: 0x00019EFE
		public float Scale
		{
			set
			{
				this.exactScale = new Vector3(value, 1f, value);
			}
		}

		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x06001F98 RID: 8088 RVA: 0x0001BD12 File Offset: 0x00019F12
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

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x06001F99 RID: 8089 RVA: 0x0001BD4B File Offset: 0x00019F4B
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

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x06001F9A RID: 8090 RVA: 0x0001BD71 File Offset: 0x00019F71
		public override Vector3 DrawPos
		{
			get
			{
				return this.exactPosition + this.def.mote.unattachedDrawOffset;
			}
		}

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x06001F9B RID: 8091 RVA: 0x0001BD8E File Offset: 0x00019F8E
		protected virtual bool EndOfLife
		{
			get
			{
				return this.AgeSecs >= this.def.mote.Lifespan;
			}
		}

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x06001F9C RID: 8092 RVA: 0x00100724 File Offset: 0x000FE924
		public virtual float Alpha
		{
			get
			{
				float ageSecs = this.AgeSecs;
				if (ageSecs <= this.def.mote.fadeInTime)
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

		// Token: 0x06001F9D RID: 8093 RVA: 0x00100804 File Offset: 0x000FEA04
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.spawnTick = Find.TickManager.TicksGame;
			this.spawnRealTime = Time.realtimeSinceStartup;
			RealTime.moteList.MoteSpawned(this);
			base.Map.moteCounter.Notify_MoteSpawned();
			this.exactPosition.y = this.def.altitudeLayer.AltitudeFor();
		}

		// Token: 0x06001F9E RID: 8094 RVA: 0x0001BDAB File Offset: 0x00019FAB
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			RealTime.moteList.MoteDespawned(this);
			map.moteCounter.Notify_MoteDespawned();
		}

		// Token: 0x06001F9F RID: 8095 RVA: 0x0001BDCF File Offset: 0x00019FCF
		public override void Tick()
		{
			if (!this.def.mote.realTime)
			{
				this.TimeInterval(0.016666668f);
			}
		}

		// Token: 0x06001FA0 RID: 8096 RVA: 0x0001BDEE File Offset: 0x00019FEE
		public void RealtimeUpdate()
		{
			if (this.def.mote.realTime)
			{
				this.TimeInterval(Time.deltaTime);
			}
		}

		// Token: 0x06001FA1 RID: 8097 RVA: 0x0010086C File Offset: 0x000FEA6C
		protected virtual void TimeInterval(float deltaTime)
		{
			if (this.EndOfLife && !base.Destroyed)
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			if (this.def.mote.needsMaintenance && Find.TickManager.TicksGame - 1 > this.lastMaintainTick)
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			if (this.def.mote.growthRate != 0f)
			{
				this.exactScale = new Vector3(this.exactScale.x + this.def.mote.growthRate * deltaTime, this.exactScale.y, this.exactScale.z + this.def.mote.growthRate * deltaTime);
				this.exactScale.x = Mathf.Max(this.exactScale.x, 0.0001f);
				this.exactScale.z = Mathf.Max(this.exactScale.z, 0.0001f);
			}
		}

		// Token: 0x06001FA2 RID: 8098 RVA: 0x0001BE0D File Offset: 0x0001A00D
		public override void Draw()
		{
			this.Draw(this.def.altitudeLayer.AltitudeFor());
		}

		// Token: 0x06001FA3 RID: 8099 RVA: 0x0001BE25 File Offset: 0x0001A025
		public void Draw(float altitude)
		{
			this.exactPosition.y = altitude;
			base.Draw();
		}

		// Token: 0x06001FA4 RID: 8100 RVA: 0x0001BE39 File Offset: 0x0001A039
		public void Maintain()
		{
			this.lastMaintainTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06001FA5 RID: 8101 RVA: 0x0001BE4B File Offset: 0x0001A04B
		public void Attach(TargetInfo a)
		{
			this.link1 = new MoteAttachLink(a);
		}

		// Token: 0x06001FA6 RID: 8102 RVA: 0x0001BE59 File Offset: 0x0001A059
		public override void Notify_MyMapRemoved()
		{
			base.Notify_MyMapRemoved();
			RealTime.moteList.MoteDespawned(this);
		}

		// Token: 0x0400162E RID: 5678
		public Vector3 exactPosition;

		// Token: 0x0400162F RID: 5679
		public float exactRotation;

		// Token: 0x04001630 RID: 5680
		public Vector3 exactScale = new Vector3(1f, 1f, 1f);

		// Token: 0x04001631 RID: 5681
		public float rotationRate;

		// Token: 0x04001632 RID: 5682
		public Color instanceColor = Color.white;

		// Token: 0x04001633 RID: 5683
		private int lastMaintainTick;

		// Token: 0x04001634 RID: 5684
		public float solidTimeOverride = -1f;

		// Token: 0x04001635 RID: 5685
		public int spawnTick;

		// Token: 0x04001636 RID: 5686
		public int detachAfterTicks = -1;

		// Token: 0x04001637 RID: 5687
		public float spawnRealTime;

		// Token: 0x04001638 RID: 5688
		public MoteAttachLink link1 = MoteAttachLink.Invalid;

		// Token: 0x04001639 RID: 5689
		protected float skidSpeedMultiplierPerTick = Rand.Range(0.3f, 0.95f);

		// Token: 0x0400163A RID: 5690
		protected const float MinSpeed = 0.02f;
	}
}
