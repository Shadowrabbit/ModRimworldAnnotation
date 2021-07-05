using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000463 RID: 1123
	public class Pawn_RotationTracker : IExposable
	{
		// Token: 0x06001C7F RID: 7295 RVA: 0x00019C35 File Offset: 0x00017E35
		public Pawn_RotationTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06001C80 RID: 7296 RVA: 0x00019C44 File Offset: 0x00017E44
		public void Notify_Spawned()
		{
			this.UpdateRotation();
		}

		// Token: 0x06001C81 RID: 7297 RVA: 0x000F0F3C File Offset: 0x000EF13C
		public void UpdateRotation()
		{
			if (this.pawn.Destroyed)
			{
				return;
			}
			if (this.pawn.jobs.HandlingFacing)
			{
				return;
			}
			if (this.pawn.pather.Moving)
			{
				if (this.pawn.pather.curPath == null || this.pawn.pather.curPath.NodesLeftCount < 1)
				{
					return;
				}
				this.FaceAdjacentCell(this.pawn.pather.nextCell);
				return;
			}
			else
			{
				Stance_Busy stance_Busy = this.pawn.stances.curStance as Stance_Busy;
				if (stance_Busy == null || !stance_Busy.focusTarg.IsValid)
				{
					if (this.pawn.jobs.curJob != null)
					{
						LocalTargetInfo target = this.pawn.CurJob.GetTarget(this.pawn.jobs.curDriver.rotateToFace);
						this.FaceTarget(target);
					}
					if (this.pawn.Drafted)
					{
						this.pawn.Rotation = Rot4.South;
					}
					return;
				}
				if (stance_Busy.focusTarg.HasThing)
				{
					this.Face(stance_Busy.focusTarg.Thing.DrawPos);
					return;
				}
				this.FaceCell(stance_Busy.focusTarg.Cell);
				return;
			}
		}

		// Token: 0x06001C82 RID: 7298 RVA: 0x00019C44 File Offset: 0x00017E44
		public void RotationTrackerTick()
		{
			this.UpdateRotation();
		}

		// Token: 0x06001C83 RID: 7299 RVA: 0x000F1078 File Offset: 0x000EF278
		private void FaceAdjacentCell(IntVec3 c)
		{
			if (c == this.pawn.Position)
			{
				return;
			}
			IntVec3 intVec = c - this.pawn.Position;
			if (intVec.x > 0)
			{
				this.pawn.Rotation = Rot4.East;
				return;
			}
			if (intVec.x < 0)
			{
				this.pawn.Rotation = Rot4.West;
				return;
			}
			if (intVec.z > 0)
			{
				this.pawn.Rotation = Rot4.North;
				return;
			}
			this.pawn.Rotation = Rot4.South;
		}

		// Token: 0x06001C84 RID: 7300 RVA: 0x000F110C File Offset: 0x000EF30C
		public void FaceCell(IntVec3 c)
		{
			if (c == this.pawn.Position)
			{
				return;
			}
			float angle = (c - this.pawn.Position).ToVector3().AngleFlat();
			this.pawn.Rotation = Pawn_RotationTracker.RotFromAngleBiased(angle);
		}

		// Token: 0x06001C85 RID: 7301 RVA: 0x000F1160 File Offset: 0x000EF360
		public void Face(Vector3 p)
		{
			if (p == this.pawn.DrawPos)
			{
				return;
			}
			float angle = (p - this.pawn.DrawPos).AngleFlat();
			this.pawn.Rotation = Pawn_RotationTracker.RotFromAngleBiased(angle);
		}

		// Token: 0x06001C86 RID: 7302 RVA: 0x000F11AC File Offset: 0x000EF3AC
		public void FaceTarget(LocalTargetInfo target)
		{
			if (!target.IsValid)
			{
				return;
			}
			if (target.HasThing)
			{
				Thing thing = target.Thing.Spawned ? target.Thing : ThingOwnerUtility.GetFirstSpawnedParentThing(target.Thing);
				if (thing != null && thing.Spawned)
				{
					bool flag = false;
					IntVec3 c = default(IntVec3);
					CellRect cellRect = thing.OccupiedRect();
					for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
					{
						for (int j = cellRect.minX; j <= cellRect.maxX; j++)
						{
							if (this.pawn.Position == new IntVec3(j, 0, i))
							{
								this.Face(thing.DrawPos);
								return;
							}
						}
					}
					for (int k = cellRect.minZ; k <= cellRect.maxZ; k++)
					{
						for (int l = cellRect.minX; l <= cellRect.maxX; l++)
						{
							IntVec3 intVec = new IntVec3(l, 0, k);
							if (intVec.AdjacentToCardinal(this.pawn.Position))
							{
								this.FaceAdjacentCell(intVec);
								return;
							}
							if (intVec.AdjacentTo8Way(this.pawn.Position))
							{
								flag = true;
								c = intVec;
							}
						}
					}
					if (flag)
					{
						if (DebugViewSettings.drawPawnRotatorTarget)
						{
							this.pawn.Map.debugDrawer.FlashCell(this.pawn.Position, 0.6f, "jbthing", 50);
							GenDraw.DrawLineBetween(this.pawn.Position.ToVector3Shifted(), c.ToVector3Shifted());
						}
						this.FaceAdjacentCell(c);
						return;
					}
					this.Face(thing.DrawPos);
					return;
				}
			}
			else
			{
				if (this.pawn.Position.AdjacentTo8Way(target.Cell))
				{
					if (DebugViewSettings.drawPawnRotatorTarget)
					{
						this.pawn.Map.debugDrawer.FlashCell(this.pawn.Position, 0.2f, "jbloc", 50);
						GenDraw.DrawLineBetween(this.pawn.Position.ToVector3Shifted(), target.Cell.ToVector3Shifted());
					}
					this.FaceAdjacentCell(target.Cell);
					return;
				}
				if (target.Cell.IsValid && target.Cell != this.pawn.Position)
				{
					this.Face(target.Cell.ToVector3());
					return;
				}
			}
		}

		// Token: 0x06001C87 RID: 7303 RVA: 0x00019C4C File Offset: 0x00017E4C
		public static Rot4 RotFromAngleBiased(float angle)
		{
			if (angle < 30f)
			{
				return Rot4.North;
			}
			if (angle < 150f)
			{
				return Rot4.East;
			}
			if (angle < 210f)
			{
				return Rot4.South;
			}
			if (angle < 330f)
			{
				return Rot4.West;
			}
			return Rot4.North;
		}

		// Token: 0x06001C88 RID: 7304 RVA: 0x00006A05 File Offset: 0x00004C05
		public void ExposeData()
		{
		}

		// Token: 0x0400146E RID: 5230
		private Pawn pawn;
	}
}
