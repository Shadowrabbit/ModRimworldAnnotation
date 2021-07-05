using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001056 RID: 4182
	public class TurretTop
	{
		// Token: 0x170010E2 RID: 4322
		// (get) Token: 0x06006318 RID: 25368 RVA: 0x00218E44 File Offset: 0x00217044
		// (set) Token: 0x06006319 RID: 25369 RVA: 0x00218E4C File Offset: 0x0021704C
		public float CurRotation
		{
			get
			{
				return this.curRotationInt;
			}
			set
			{
				this.curRotationInt = value;
				if (this.curRotationInt > 360f)
				{
					this.curRotationInt -= 360f;
				}
				if (this.curRotationInt < 0f)
				{
					this.curRotationInt += 360f;
				}
			}
		}

		// Token: 0x0600631A RID: 25370 RVA: 0x00218EA0 File Offset: 0x002170A0
		public void SetRotationFromOrientation()
		{
			this.CurRotation = this.parentTurret.Rotation.AsAngle;
		}

		// Token: 0x0600631B RID: 25371 RVA: 0x00218EC6 File Offset: 0x002170C6
		public TurretTop(Building_Turret ParentTurret)
		{
			this.parentTurret = ParentTurret;
		}

		// Token: 0x0600631C RID: 25372 RVA: 0x00218ED8 File Offset: 0x002170D8
		public void TurretTopTick()
		{
			LocalTargetInfo currentTarget = this.parentTurret.CurrentTarget;
			if (currentTarget.IsValid)
			{
				float curRotation = (currentTarget.Cell.ToVector3Shifted() - this.parentTurret.DrawPos).AngleFlat();
				this.CurRotation = curRotation;
				this.ticksUntilIdleTurn = Rand.RangeInclusive(150, 350);
				return;
			}
			if (this.ticksUntilIdleTurn > 0)
			{
				this.ticksUntilIdleTurn--;
				if (this.ticksUntilIdleTurn == 0)
				{
					if (Rand.Value < 0.5f)
					{
						this.idleTurnClockwise = true;
					}
					else
					{
						this.idleTurnClockwise = false;
					}
					this.idleTurnTicksLeft = 140;
					return;
				}
			}
			else
			{
				if (this.idleTurnClockwise)
				{
					this.CurRotation += 0.26f;
				}
				else
				{
					this.CurRotation -= 0.26f;
				}
				this.idleTurnTicksLeft--;
				if (this.idleTurnTicksLeft <= 0)
				{
					this.ticksUntilIdleTurn = Rand.RangeInclusive(150, 350);
				}
			}
		}

		// Token: 0x0600631D RID: 25373 RVA: 0x00218FE0 File Offset: 0x002171E0
		public void DrawTurret(Vector3 recoilDrawOffset, float recoilAngleOffset)
		{
			Vector3 vector = new Vector3(this.parentTurret.def.building.turretTopOffset.x, 0f, this.parentTurret.def.building.turretTopOffset.y).RotatedBy(this.CurRotation);
			float turretTopDrawSize = this.parentTurret.def.building.turretTopDrawSize;
			vector = vector.RotatedBy(recoilAngleOffset);
			vector += recoilDrawOffset;
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(this.parentTurret.DrawPos + Altitudes.AltIncVect + vector, (this.CurRotation + (float)TurretTop.ArtworkRotation).ToQuat(), new Vector3(turretTopDrawSize, 1f, turretTopDrawSize));
			Graphics.DrawMesh(MeshPool.plane10, matrix, this.parentTurret.def.building.turretTopMat, 0);
		}

		// Token: 0x04003836 RID: 14390
		private Building_Turret parentTurret;

		// Token: 0x04003837 RID: 14391
		private float curRotationInt;

		// Token: 0x04003838 RID: 14392
		private int ticksUntilIdleTurn;

		// Token: 0x04003839 RID: 14393
		private int idleTurnTicksLeft;

		// Token: 0x0400383A RID: 14394
		private bool idleTurnClockwise;

		// Token: 0x0400383B RID: 14395
		private const float IdleTurnDegreesPerTick = 0.26f;

		// Token: 0x0400383C RID: 14396
		private const int IdleTurnDuration = 140;

		// Token: 0x0400383D RID: 14397
		private const int IdleTurnIntervalMin = 150;

		// Token: 0x0400383E RID: 14398
		private const int IdleTurnIntervalMax = 350;

		// Token: 0x0400383F RID: 14399
		public static readonly int ArtworkRotation = -90;
	}
}
