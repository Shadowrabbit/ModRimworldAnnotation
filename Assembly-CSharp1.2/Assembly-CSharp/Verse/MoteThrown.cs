using System;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020004FD RID: 1277
	public class MoteThrown : Mote
	{
		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x06001FC9 RID: 8137 RVA: 0x0001C07D File Offset: 0x0001A27D
		protected bool Flying
		{
			get
			{
				return this.airTimeLeft > 0f;
			}
		}

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x06001FCA RID: 8138 RVA: 0x0001C08C File Offset: 0x0001A28C
		protected bool Skidding
		{
			get
			{
				return !this.Flying && this.Speed > 0.01f;
			}
		}

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x06001FCB RID: 8139 RVA: 0x0001C0A5 File Offset: 0x0001A2A5
		// (set) Token: 0x06001FCC RID: 8140 RVA: 0x0001C0AD File Offset: 0x0001A2AD
		public Vector3 Velocity
		{
			get
			{
				return this.velocity;
			}
			set
			{
				this.velocity = value;
			}
		}

		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x06001FCD RID: 8141 RVA: 0x0001C0B6 File Offset: 0x0001A2B6
		// (set) Token: 0x06001FCE RID: 8142 RVA: 0x0001C0C3 File Offset: 0x0001A2C3
		public float MoveAngle
		{
			get
			{
				return this.velocity.AngleFlat();
			}
			set
			{
				this.SetVelocity(value, this.Speed);
			}
		}

		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x06001FCF RID: 8143 RVA: 0x0001C0D2 File Offset: 0x0001A2D2
		// (set) Token: 0x06001FD0 RID: 8144 RVA: 0x00100FF4 File Offset: 0x000FF1F4
		public float Speed
		{
			get
			{
				return this.velocity.MagnitudeHorizontal();
			}
			set
			{
				if (value == 0f)
				{
					this.velocity = Vector3.zero;
					return;
				}
				if (this.velocity == Vector3.zero)
				{
					this.velocity = new Vector3(value, 0f, 0f);
					return;
				}
				this.velocity = this.velocity.normalized * value;
			}
		}

		// Token: 0x06001FD1 RID: 8145 RVA: 0x00101058 File Offset: 0x000FF258
		protected override void TimeInterval(float deltaTime)
		{
			base.TimeInterval(deltaTime);
			if (base.Destroyed)
			{
				return;
			}
			if (!this.Flying && !this.Skidding)
			{
				return;
			}
			Vector3 vector = this.NextExactPosition(deltaTime);
			IntVec3 intVec = new IntVec3(vector);
			if (intVec != base.Position)
			{
				if (!intVec.InBounds(base.Map))
				{
					this.Destroy(DestroyMode.Vanish);
					return;
				}
				if (this.def.mote.collide && intVec.Filled(base.Map))
				{
					this.WallHit();
					return;
				}
			}
			base.Position = intVec;
			this.exactPosition = vector;
			if (this.def.mote.rotateTowardsMoveDirection && this.velocity != default(Vector3))
			{
				this.exactRotation = this.velocity.AngleFlat();
			}
			else
			{
				this.exactRotation += this.rotationRate * deltaTime;
			}
			this.velocity += this.def.mote.acceleration * deltaTime;
			if (this.def.mote.speedPerTime != 0f)
			{
				this.Speed = Mathf.Max(this.Speed + this.def.mote.speedPerTime * deltaTime, 0f);
			}
			if (this.airTimeLeft > 0f)
			{
				this.airTimeLeft -= deltaTime;
				if (this.airTimeLeft < 0f)
				{
					this.airTimeLeft = 0f;
				}
				if (this.airTimeLeft <= 0f && !this.def.mote.landSound.NullOrUndefined())
				{
					this.def.mote.landSound.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
				}
			}
			if (this.Skidding)
			{
				this.Speed *= this.skidSpeedMultiplierPerTick;
				this.rotationRate *= this.skidSpeedMultiplierPerTick;
				if (this.Speed < 0.02f)
				{
					this.Speed = 0f;
				}
			}
		}

		// Token: 0x06001FD2 RID: 8146 RVA: 0x0001C0DF File Offset: 0x0001A2DF
		protected virtual Vector3 NextExactPosition(float deltaTime)
		{
			return this.exactPosition + this.velocity * deltaTime;
		}

		// Token: 0x06001FD3 RID: 8147 RVA: 0x0001C0F8 File Offset: 0x0001A2F8
		public void SetVelocity(float angle, float speed)
		{
			this.velocity = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward * speed;
		}

		// Token: 0x06001FD4 RID: 8148 RVA: 0x0001C11B File Offset: 0x0001A31B
		protected virtual void WallHit()
		{
			this.airTimeLeft = 0f;
			this.Speed = 0f;
			this.rotationRate = 0f;
		}

		// Token: 0x0400164F RID: 5711
		public float airTimeLeft = 999999f;

		// Token: 0x04001650 RID: 5712
		protected Vector3 velocity = Vector3.zero;
	}
}
