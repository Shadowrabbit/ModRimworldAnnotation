using System;
using System.Linq;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x02001E22 RID: 7714
	public class SketchResolver_Symmetry : SketchResolver
	{
		// Token: 0x0600A6DC RID: 42716 RVA: 0x00307CF4 File Offset: 0x00305EF4
		protected override void ResolveInt(ResolveParams parms)
		{
			bool flag = parms.symmetryClear ?? true;
			int origin = parms.symmetryOrigin ?? 0;
			bool flag2 = parms.symmetryVertical ?? false;
			bool flag3 = parms.requireFloor ?? false;
			bool originIncluded = parms.symmetryOriginIncluded ?? false;
			if (flag)
			{
				this.Clear(parms.sketch, origin, flag2, originIncluded);
			}
			foreach (SketchBuildable sketchBuildable in parms.sketch.Buildables.ToList<SketchBuildable>())
			{
				if (!this.ShouldKeepAlreadySymmetricalInTheMiddle(sketchBuildable, origin, flag2, originIncluded))
				{
					SketchBuildable sketchBuildable2 = (SketchBuildable)sketchBuildable.DeepCopy();
					SketchThing sketchThing = sketchBuildable2 as SketchThing;
					if (sketchThing != null && sketchThing.def.rotatable)
					{
						if (flag2)
						{
							if (!sketchThing.rot.IsHorizontal)
							{
								sketchThing.rot = sketchThing.rot.Opposite;
							}
						}
						else if (sketchThing.rot.IsHorizontal)
						{
							sketchThing.rot = sketchThing.rot.Opposite;
						}
					}
					this.MoveUntilSymmetrical(sketchBuildable2, sketchBuildable.OccupiedRect, origin, flag2, originIncluded);
					if (flag3 && sketchBuildable2.Buildable != ThingDefOf.Wall && sketchBuildable2.Buildable != ThingDefOf.Door)
					{
						bool flag4 = true;
						foreach (IntVec3 pos in sketchBuildable2.OccupiedRect)
						{
							if (!parms.sketch.AnyTerrainAt(pos))
							{
								flag4 = false;
								break;
							}
						}
						if (flag4)
						{
							parms.sketch.Add(sketchBuildable2, true);
						}
					}
					else
					{
						parms.sketch.Add(sketchBuildable2, true);
					}
				}
			}
		}

		// Token: 0x0600A6DD RID: 42717 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool CanResolveInt(ResolveParams parms)
		{
			return true;
		}

		// Token: 0x0600A6DE RID: 42718 RVA: 0x00307F34 File Offset: 0x00306134
		private void Clear(Sketch sketch, int origin, bool vertical, bool originIncluded)
		{
			foreach (SketchBuildable sketchBuildable in sketch.Buildables.ToList<SketchBuildable>())
			{
				CellRect occupiedRect = sketchBuildable.OccupiedRect;
				if (((occupiedRect.maxX >= origin && !vertical) || (occupiedRect.maxZ >= origin && vertical)) && !this.ShouldKeepAlreadySymmetricalInTheMiddle(sketchBuildable, origin, vertical, originIncluded))
				{
					sketch.Remove(sketchBuildable);
				}
			}
		}

		// Token: 0x0600A6DF RID: 42719 RVA: 0x00307FC0 File Offset: 0x003061C0
		private bool ShouldKeepAlreadySymmetricalInTheMiddle(SketchBuildable buildable, int origin, bool vertical, bool originIncluded)
		{
			CellRect occupiedRect = buildable.OccupiedRect;
			if (vertical)
			{
				if (originIncluded)
				{
					return occupiedRect.maxZ - origin == origin - occupiedRect.minZ;
				}
				return occupiedRect.maxZ - origin + 1 == origin - occupiedRect.minZ;
			}
			else
			{
				if (originIncluded)
				{
					return occupiedRect.maxX - origin == origin - occupiedRect.minX;
				}
				return occupiedRect.maxX - origin + 1 == origin - occupiedRect.minX;
			}
		}

		// Token: 0x0600A6E0 RID: 42720 RVA: 0x00308030 File Offset: 0x00306230
		private void MoveUntilSymmetrical(SketchBuildable buildable, CellRect initial, int origin, bool vertical, bool originIncluded)
		{
			if (vertical)
			{
				buildable.pos.x = buildable.pos.x + (initial.minX - buildable.OccupiedRect.minX);
				int num;
				if (originIncluded)
				{
					num = origin - initial.maxZ + origin;
				}
				else
				{
					num = origin - initial.maxZ - 1 + origin;
				}
				buildable.pos.z = buildable.pos.z + (num - buildable.OccupiedRect.minZ);
				return;
			}
			buildable.pos.z = buildable.pos.z + (initial.minZ - buildable.OccupiedRect.minZ);
			int num2;
			if (originIncluded)
			{
				num2 = origin - initial.maxX + origin;
			}
			else
			{
				num2 = origin - initial.maxX - 1 + origin;
			}
			buildable.pos.x = buildable.pos.x + (num2 - buildable.OccupiedRect.minX);
		}
	}
}
