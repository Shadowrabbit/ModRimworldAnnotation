using System;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200036C RID: 876
	public class MoteThrown : Mote
	{
		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x060018CB RID: 6347 RVA: 0x0009237A File Offset: 0x0009057A
		protected bool Flying
		{
			get
			{
				return this.airTimeLeft > 0f;
			}
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x060018CC RID: 6348 RVA: 0x00092389 File Offset: 0x00090589
		protected bool Skidding
		{
			get
			{
				return !this.Flying && this.Speed > 0.01f;
			}
		}

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x060018CD RID: 6349 RVA: 0x000923A2 File Offset: 0x000905A2
		// (set) Token: 0x060018CE RID: 6350 RVA: 0x000923AA File Offset: 0x000905AA
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

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x060018CF RID: 6351 RVA: 0x000923B3 File Offset: 0x000905B3
		// (set) Token: 0x060018D0 RID: 6352 RVA: 0x000923C0 File Offset: 0x000905C0
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

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x060018D1 RID: 6353 RVA: 0x000923CF File Offset: 0x000905CF
		// (set) Token: 0x060018D2 RID: 6354 RVA: 0x000923DC File Offset: 0x000905DC
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

		// Token: 0x060018D3 RID: 6355 RVA: 0x00092440 File Offset: 0x00090640
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

		// Token: 0x060018D4 RID: 6356 RVA: 0x00092657 File Offset: 0x00090857
		protected virtual Vector3 NextExactPosition(float deltaTime)
		{
			return this.exactPosition + this.velocity * deltaTime;
		}

		// Token: 0x060018D5 RID: 6357 RVA: 0x00092670 File Offset: 0x00090870
		public void SetVelocity(float angle, float speed)
		{
			this.velocity = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward * speed;
		}

		// Token: 0x060018D6 RID: 6358 RVA: 0x00092693 File Offset: 0x00090893
		protected virtual void WallHit()
		{
			this.airTimeLeft = 0f;
			this.Speed = 0f;
			this.rotationRate = 0f;
		}

		// Token: 0x040010CE RID: 4302
		public float airTimeLeft = 999999f;

		// Token: 0x040010CF RID: 4303
		protected Vector3 velocity = Vector3.zero;
	}
}
