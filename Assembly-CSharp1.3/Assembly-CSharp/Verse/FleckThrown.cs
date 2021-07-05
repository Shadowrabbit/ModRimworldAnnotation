using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200019F RID: 415
	public struct FleckThrown : IFleck
	{
		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000BAA RID: 2986 RVA: 0x0003F1DD File Offset: 0x0003D3DD
		public bool Flying
		{
			get
			{
				return this.airTimeLeft > 0f;
			}
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000BAB RID: 2987 RVA: 0x0003F1EC File Offset: 0x0003D3EC
		public bool Skidding
		{
			get
			{
				return !this.Flying && this.Speed > 0.01f;
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000BAC RID: 2988 RVA: 0x0003F205 File Offset: 0x0003D405
		// (set) Token: 0x06000BAD RID: 2989 RVA: 0x0003F20D File Offset: 0x0003D40D
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

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000BAE RID: 2990 RVA: 0x0003F216 File Offset: 0x0003D416
		// (set) Token: 0x06000BAF RID: 2991 RVA: 0x0003F223 File Offset: 0x0003D423
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

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000BB0 RID: 2992 RVA: 0x0003F232 File Offset: 0x0003D432
		// (set) Token: 0x06000BB1 RID: 2993 RVA: 0x0003F240 File Offset: 0x0003D440
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

		// Token: 0x06000BB2 RID: 2994 RVA: 0x0003F2A4 File Offset: 0x0003D4A4
		public void Setup(FleckCreationData creationData)
		{
			this.baseData = default(FleckStatic);
			this.baseData.Setup(creationData);
			this.airTimeLeft = (creationData.airTimeLeft ?? 999999f);
			this.attacheeLastPosition = new Vector3(-1000f, -1000f, -1000f);
			this.link = creationData.link;
			if (this.link.Linked)
			{
				this.attacheeLastPosition = this.link.LastDrawPos;
			}
			this.baseData.exactPosition = this.baseData.exactPosition + creationData.def.attachedDrawOffset;
			this.rotationRate = creationData.rotationRate;
			this.SetVelocity(creationData.velocityAngle, creationData.velocitySpeed);
			if (creationData.velocity != null)
			{
				this.velocity += creationData.velocity.Value;
			}
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x0003F3A0 File Offset: 0x0003D5A0
		public bool TimeInterval(float deltaTime, Map map)
		{
			if (this.baseData.TimeInterval(deltaTime, map))
			{
				return true;
			}
			if (!this.Flying && !this.Skidding)
			{
				return false;
			}
			Vector3 vector = this.NextExactPosition(deltaTime);
			IntVec3 intVec = new IntVec3(vector);
			if (intVec != new IntVec3(this.baseData.exactPosition))
			{
				if (!intVec.InBounds(map))
				{
					return true;
				}
				if (this.baseData.def.collide && intVec.Filled(map))
				{
					this.WallHit();
					return false;
				}
			}
			this.baseData.exactPosition = vector;
			if (this.baseData.def.rotateTowardsMoveDirection && this.velocity != default(Vector3))
			{
				this.baseData.exactRotation = this.velocity.AngleFlat();
			}
			else
			{
				this.baseData.exactRotation = this.baseData.exactRotation + this.rotationRate * deltaTime;
			}
			this.velocity += this.baseData.def.acceleration * deltaTime;
			if (this.baseData.def.speedPerTime != 0f)
			{
				this.Speed = Mathf.Max(this.Speed + this.baseData.def.speedPerTime * deltaTime, 0f);
			}
			if (this.airTimeLeft > 0f)
			{
				this.airTimeLeft -= deltaTime;
				if (this.airTimeLeft < 0f)
				{
					this.airTimeLeft = 0f;
				}
				if (this.airTimeLeft <= 0f && !this.baseData.def.landSound.NullOrUndefined())
				{
					this.baseData.def.landSound.PlayOneShot(new TargetInfo(new IntVec3(this.baseData.exactPosition), map, false));
				}
			}
			if (this.Skidding)
			{
				this.Speed *= this.baseData.skidSpeedMultiplierPerTick;
				this.rotationRate *= this.baseData.skidSpeedMultiplierPerTick;
				if (this.Speed < 0.02f)
				{
					this.Speed = 0f;
				}
			}
			return false;
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x0003F5CC File Offset: 0x0003D7CC
		private Vector3 NextExactPosition(float deltaTime)
		{
			Vector3 vector = this.baseData.exactPosition + this.velocity * deltaTime;
			if (this.link.Linked)
			{
				bool flag = this.link.detachAfterTicks == -1 || this.baseData.ageTicks < this.link.detachAfterTicks;
				if (!this.link.Target.ThingDestroyed && flag)
				{
					this.link.UpdateDrawPos();
				}
				Vector3 b = this.baseData.def.attachedDrawOffset;
				if (this.baseData.def.attachedToHead)
				{
					Pawn pawn = this.link.Target.Thing as Pawn;
					if (pawn != null && pawn.story != null)
					{
						b = pawn.Drawer.renderer.BaseHeadOffsetAt((pawn.GetPosture() == PawnPosture.Standing) ? Rot4.North : pawn.Drawer.renderer.LayingFacing()).RotatedBy(pawn.Drawer.renderer.BodyAngle());
					}
				}
				Vector3 b2 = this.link.LastDrawPos - this.attacheeLastPosition;
				vector += b2;
				vector += b;
				vector.y = AltitudeLayer.MoteOverhead.AltitudeFor();
				this.attacheeLastPosition = this.link.LastDrawPos;
			}
			return vector;
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x0003F731 File Offset: 0x0003D931
		public void SetVelocity(float angle, float speed)
		{
			this.velocity = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward * speed;
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x0003F754 File Offset: 0x0003D954
		public void Draw(DrawBatch batch)
		{
			this.baseData.Draw(batch);
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x0003F762 File Offset: 0x0003D962
		private void WallHit()
		{
			this.airTimeLeft = 0f;
			this.Speed = 0f;
			this.rotationRate = 0f;
		}

		// Token: 0x040009B5 RID: 2485
		public FleckStatic baseData;

		// Token: 0x040009B6 RID: 2486
		public float airTimeLeft;

		// Token: 0x040009B7 RID: 2487
		public Vector3 velocity;

		// Token: 0x040009B8 RID: 2488
		public float rotationRate;

		// Token: 0x040009B9 RID: 2489
		public FleckAttachLink link;

		// Token: 0x040009BA RID: 2490
		private Vector3 attacheeLastPosition;

		// Token: 0x040009BB RID: 2491
		public const float MinSpeed = 0.02f;
	}
}
