using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001685 RID: 5765
	public class TurretTop
	{
		// Token: 0x17001370 RID: 4976
		// (get) Token: 0x06007DF6 RID: 32246 RVA: 0x00054B49 File Offset: 0x00052D49
		// (set) Token: 0x06007DF7 RID: 32247 RVA: 0x00258EB0 File Offset: 0x002570B0
		private float CurRotation
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

		// Token: 0x06007DF8 RID: 32248 RVA: 0x00258F04 File Offset: 0x00257104
		public void SetRotationFromOrientation()
		{
			this.CurRotation = this.parentTurret.Rotation.AsAngle;
		}

		// Token: 0x06007DF9 RID: 32249 RVA: 0x00054B51 File Offset: 0x00052D51
		public TurretTop(Building_Turret ParentTurret)
		{
			this.parentTurret = ParentTurret;
		}

		// Token: 0x06007DFA RID: 32250 RVA: 0x00258F2C File Offset: 0x0025712C
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

		// Token: 0x06007DFB RID: 32251 RVA: 0x00259034 File Offset: 0x00257234
		public void DrawTurret()
		{
			Vector3 b = new Vector3(this.parentTurret.def.building.turretTopOffset.x, 0f, this.parentTurret.def.building.turretTopOffset.y).RotatedBy(this.CurRotation);
			float turretTopDrawSize = this.parentTurret.def.building.turretTopDrawSize;
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(this.parentTurret.DrawPos + Altitudes.AltIncVect + b, (this.CurRotation + (float)TurretTop.ArtworkRotation).ToQuat(), new Vector3(turretTopDrawSize, 1f, turretTopDrawSize));
			Graphics.DrawMesh(MeshPool.plane10, matrix, this.parentTurret.def.building.turretTopMat, 0);
		}

		// Token: 0x04005210 RID: 21008
		private Building_Turret parentTurret;

		// Token: 0x04005211 RID: 21009
		private float curRotationInt;

		// Token: 0x04005212 RID: 21010
		private int ticksUntilIdleTurn;

		// Token: 0x04005213 RID: 21011
		private int idleTurnTicksLeft;

		// Token: 0x04005214 RID: 21012
		private bool idleTurnClockwise;

		// Token: 0x04005215 RID: 21013
		private const float IdleTurnDegreesPerTick = 0.26f;

		// Token: 0x04005216 RID: 21014
		private const int IdleTurnDuration = 140;

		// Token: 0x04005217 RID: 21015
		private const int IdleTurnIntervalMin = 150;

		// Token: 0x04005218 RID: 21016
		private const int IdleTurnIntervalMax = 350;

		// Token: 0x04005219 RID: 21017
		public static readonly int ArtworkRotation = -90;
	}
}
