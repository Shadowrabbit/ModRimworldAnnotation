using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x02001E16 RID: 7702
	public class SketchResolver_DamageBuildings : SketchResolver
	{
		// Token: 0x0600A6AB RID: 42667 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool CanResolveInt(ResolveParams parms)
		{
			return true;
		}

		// Token: 0x0600A6AC RID: 42668 RVA: 0x0030607C File Offset: 0x0030427C
		protected override void ResolveInt(ResolveParams parms)
		{
			CellRect occupiedRect = parms.sketch.OccupiedRect;
			Rot4 random = Rot4.Random;
			int num = 0;
			int num2 = parms.sketch.Buildables.Count<SketchBuildable>();
			foreach (SketchBuildable buildable in parms.sketch.Buildables.InRandomOrder(null).ToList<SketchBuildable>())
			{
				bool flag;
				this.Damage(buildable, occupiedRect, random, parms.sketch, out flag);
				if (flag)
				{
					num++;
					if ((float)num > (float)num2 * 0.65f)
					{
						break;
					}
				}
			}
		}

		// Token: 0x0600A6AD RID: 42669 RVA: 0x00306128 File Offset: 0x00304328
		private void Damage(SketchBuildable buildable, CellRect rect, Rot4 dir, Sketch sketch, out bool destroyed)
		{
			float num;
			if (dir.IsHorizontal)
			{
				num = (float)(buildable.pos.x - rect.minX) / (float)rect.Width;
			}
			else
			{
				num = (float)(buildable.pos.z - rect.minZ) / (float)rect.Height;
			}
			if (dir == Rot4.East || dir == Rot4.South)
			{
				num = 1f - num;
			}
			if (Rand.Chance(Mathf.Pow(num, 1.32f)))
			{
				sketch.Remove(buildable);
				destroyed = true;
				SketchTerrain sketchTerrain = buildable as SketchTerrain;
				if (sketchTerrain != null && sketchTerrain.def.burnedDef != null)
				{
					sketch.AddTerrain(sketchTerrain.def.burnedDef, sketchTerrain.pos, true);
					return;
				}
			}
			else
			{
				destroyed = false;
				SketchThing sketchThing = buildable as SketchThing;
				if (sketchThing != null)
				{
					sketchThing.hitPoints = new int?(Mathf.Clamp(Mathf.RoundToInt((float)sketchThing.MaxHitPoints * (1f - num) * Rand.Range(1f, 1.2f)), 1, sketchThing.MaxHitPoints));
				}
			}
		}

		// Token: 0x0400710C RID: 28940
		private const float MaxPctOfTotalDestroyed = 0.65f;

		// Token: 0x0400710D RID: 28941
		private const float HpRandomFactor = 1.2f;

		// Token: 0x0400710E RID: 28942
		private const float DestroyChanceExp = 1.32f;
	}
}
