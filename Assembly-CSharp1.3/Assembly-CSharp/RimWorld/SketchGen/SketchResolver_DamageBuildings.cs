using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x0200158E RID: 5518
	public class SketchResolver_DamageBuildings : SketchResolver
	{
		// Token: 0x06008257 RID: 33367 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool CanResolveInt(ResolveParams parms)
		{
			return true;
		}

		// Token: 0x06008258 RID: 33368 RVA: 0x002E2BA0 File Offset: 0x002E0DA0
		protected override void ResolveInt(ResolveParams parms)
		{
			CellRect occupiedRect = parms.sketch.OccupiedRect;
			Rot4 random = Rot4.Random;
			int num = 0;
			int num2 = parms.sketch.Buildables.Count<SketchBuildable>();
			foreach (SketchBuildable buildable in parms.sketch.Buildables.InRandomOrder(null).ToList<SketchBuildable>())
			{
				bool flag;
				this.Damage(buildable, occupiedRect, random, parms.sketch, out flag, parms.destroyChanceExp);
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

		// Token: 0x06008259 RID: 33369 RVA: 0x002E2C54 File Offset: 0x002E0E54
		private void Damage(SketchBuildable buildable, CellRect rect, Rot4 dir, Sketch sketch, out bool destroyed, float? destroyChanceExp = null)
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
			if (Rand.Chance(Mathf.Pow(num, destroyChanceExp ?? 1.32f)))
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

		// Token: 0x0400511E RID: 20766
		private const float MaxPctOfTotalDestroyed = 0.65f;

		// Token: 0x0400511F RID: 20767
		private const float HpRandomFactor = 1.2f;

		// Token: 0x04005120 RID: 20768
		private const float DestroyChanceExp = 1.32f;
	}
}
