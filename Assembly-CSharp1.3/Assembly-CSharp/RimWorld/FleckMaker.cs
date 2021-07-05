using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001538 RID: 5432
	public static class FleckMaker
	{
		// Token: 0x06008126 RID: 33062 RVA: 0x002DB004 File Offset: 0x002D9204
		public static FleckCreationData GetDataStatic(Vector3 loc, Map map, FleckDef fleckDef, float scale = 1f)
		{
			return new FleckCreationData
			{
				def = fleckDef,
				spawnPosition = loc,
				scale = scale
			};
		}

		// Token: 0x06008127 RID: 33063 RVA: 0x002DB032 File Offset: 0x002D9232
		public static void Static(IntVec3 cell, Map map, FleckDef fleckDef, float scale = 1f)
		{
			FleckMaker.Static(cell.ToVector3Shifted(), map, fleckDef, scale);
		}

		// Token: 0x06008128 RID: 33064 RVA: 0x002DB043 File Offset: 0x002D9243
		public static void Static(Vector3 loc, Map map, FleckDef fleckDef, float scale = 1f)
		{
			map.flecks.CreateFleck(FleckMaker.GetDataStatic(loc, map, fleckDef, scale));
		}

		// Token: 0x06008129 RID: 33065 RVA: 0x002DB05C File Offset: 0x002D925C
		public static FleckCreationData GetDataThrowMetaIcon(IntVec3 cell, Map map, FleckDef fleckDef, float velocitySpeed = 0.42f)
		{
			return new FleckCreationData
			{
				def = fleckDef,
				spawnPosition = cell.ToVector3Shifted() + new Vector3(0.35f, 0f, 0.35f) + new Vector3(Rand.Value, 0f, Rand.Value) * 0.1f,
				velocityAngle = (float)Rand.Range(30, 60),
				velocitySpeed = velocitySpeed,
				rotationRate = Rand.Range(-3f, 3f),
				scale = 0.7f
			};
		}

		// Token: 0x0600812A RID: 33066 RVA: 0x002DB0FF File Offset: 0x002D92FF
		public static void ThrowMetaIcon(IntVec3 cell, Map map, FleckDef fleckDef, float velocitySpeed = 0.42f)
		{
			if (!cell.ShouldSpawnMotesAt(map))
			{
				return;
			}
			map.flecks.CreateFleck(FleckMaker.GetDataThrowMetaIcon(cell, map, fleckDef, velocitySpeed));
		}

		// Token: 0x0600812B RID: 33067 RVA: 0x002DB120 File Offset: 0x002D9320
		public static FleckCreationData GetDataAttachedOverlay(Thing thing, FleckDef fleckDef, Vector3 offset, float scale = 1f, float solidTimeOverride = -1f)
		{
			return new FleckCreationData
			{
				def = fleckDef,
				spawnPosition = thing.DrawPos + offset,
				solidTimeOverride = new float?(solidTimeOverride),
				scale = scale
			};
		}

		// Token: 0x0600812C RID: 33068 RVA: 0x002DB167 File Offset: 0x002D9367
		public static void AttachedOverlay(Thing thing, FleckDef fleckDef, Vector3 offset, float scale = 1f, float solidTimeOverride = -1f)
		{
			thing.MapHeld.flecks.CreateFleck(FleckMaker.GetDataAttachedOverlay(thing, fleckDef, offset, scale, solidTimeOverride));
		}

		// Token: 0x0600812D RID: 33069 RVA: 0x002DB184 File Offset: 0x002D9384
		public static void ThrowMetaPuffs(CellRect rect, Map map)
		{
			if (!Find.TickManager.Paused)
			{
				for (int i = rect.minX; i <= rect.maxX; i++)
				{
					for (int j = rect.minZ; j <= rect.maxZ; j++)
					{
						FleckMaker.ThrowMetaPuffs(new TargetInfo(new IntVec3(i, 0, j), map, false));
					}
				}
			}
		}

		// Token: 0x0600812E RID: 33070 RVA: 0x002DB1E0 File Offset: 0x002D93E0
		public static void ThrowMetaPuffs(TargetInfo targ)
		{
			Vector3 a = targ.HasThing ? targ.Thing.TrueCenter() : targ.Cell.ToVector3Shifted();
			int num = Rand.RangeInclusive(4, 6);
			for (int i = 0; i < num; i++)
			{
				FleckMaker.ThrowMetaPuff(a + new Vector3(Rand.Range(-0.5f, 0.5f), 0f, Rand.Range(-0.5f, 0.5f)), targ.Map);
			}
		}

		// Token: 0x0600812F RID: 33071 RVA: 0x002DB264 File Offset: 0x002D9464
		public static void ThrowMetaPuff(Vector3 loc, Map map)
		{
			if (!loc.ShouldSpawnMotesAt(map))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc, map, FleckDefOf.MetaPuff, 1.9f);
			dataStatic.rotationRate = (float)Rand.Range(-60, 60);
			dataStatic.velocityAngle = (float)Rand.Range(0, 360);
			dataStatic.velocitySpeed = Rand.Range(0.6f, 0.78f);
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x06008130 RID: 33072 RVA: 0x002DB2D4 File Offset: 0x002D94D4
		public static void ThrowAirPuffUp(Vector3 loc, Map map)
		{
			if (!loc.ToIntVec3().ShouldSpawnMotesAt(map))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc + new Vector3(Rand.Range(-0.02f, 0.02f), 0f, Rand.Range(-0.02f, 0.02f)), map, FleckDefOf.AirPuff, 1.5f);
			dataStatic.rotationRate = (float)Rand.RangeInclusive(-240, 240);
			dataStatic.velocityAngle = (float)Rand.Range(-45, 45);
			dataStatic.velocitySpeed = Rand.Range(1.2f, 1.5f);
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x06008131 RID: 33073 RVA: 0x002DB37C File Offset: 0x002D957C
		public static void ThrowBreathPuff(Vector3 loc, Map map, float throwAngle, Vector3 inheritVelocity)
		{
			if (!loc.ToIntVec3().ShouldSpawnMotesAt(map))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc + new Vector3(Rand.Range(-0.005f, 0.005f), 0f, Rand.Range(-0.005f, 0.005f)), map, FleckDefOf.AirPuff, Rand.Range(0.6f, 0.7f));
			dataStatic.rotationRate = (float)Rand.RangeInclusive(-240, 240);
			dataStatic.velocityAngle = throwAngle + (float)Rand.Range(-10, 10);
			dataStatic.velocitySpeed = Rand.Range(0.1f, 0.8f);
			dataStatic.velocity = new Vector3?(inheritVelocity * 0.5f);
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x06008132 RID: 33074 RVA: 0x002DB444 File Offset: 0x002D9644
		public static void ThrowDustPuff(IntVec3 cell, Map map, float scale)
		{
			FleckMaker.ThrowDustPuff(cell.ToVector3() + new Vector3(Rand.Value, 0f, Rand.Value), map, scale);
		}

		// Token: 0x06008133 RID: 33075 RVA: 0x002DB470 File Offset: 0x002D9670
		public static void ThrowDustPuff(Vector3 loc, Map map, float scale)
		{
			if (!loc.ShouldSpawnMotesAt(map))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc, map, FleckDefOf.DustPuff, 1.9f * scale);
			dataStatic.rotationRate = (float)Rand.Range(-60, 60);
			dataStatic.velocityAngle = (float)Rand.Range(0, 360);
			dataStatic.velocitySpeed = Rand.Range(0.6f, 0.75f);
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x06008134 RID: 33076 RVA: 0x002DB4E4 File Offset: 0x002D96E4
		public static void ThrowDustPuffThick(Vector3 loc, Map map, float scale, Color color)
		{
			if (!loc.ShouldSpawnMotesAt(map))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc, map, FleckDefOf.DustPuffThick, scale);
			dataStatic.rotationRate = (float)Rand.Range(-60, 60);
			dataStatic.velocityAngle = (float)Rand.Range(0, 360);
			dataStatic.velocitySpeed = Rand.Range(0.6f, 0.75f);
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x06008135 RID: 33077 RVA: 0x002DB550 File Offset: 0x002D9750
		public static void ThrowTornadoDustPuff(Vector3 loc, Map map, float scale, Color color)
		{
			if (!loc.ShouldSpawnMotesAt(map))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc, map, FleckDefOf.TornadoDustPuff, 1.9f * scale);
			dataStatic.rotationRate = (float)Rand.Range(-60, 60);
			dataStatic.velocityAngle = (float)Rand.Range(0, 360);
			dataStatic.velocitySpeed = Rand.Range(0.6f, 0.75f);
			dataStatic.instanceColor = new Color?(color);
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x06008136 RID: 33078 RVA: 0x002DB5D0 File Offset: 0x002D97D0
		public static void ThrowSmoke(Vector3 loc, Map map, float size)
		{
			if (!loc.ShouldSpawnMotesAt(map))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc, map, FleckDefOf.Smoke, Rand.Range(1.5f, 2.5f) * size);
			dataStatic.rotationRate = Rand.Range(-30f, 30f);
			dataStatic.velocityAngle = (float)Rand.Range(30, 40);
			dataStatic.velocitySpeed = Rand.Range(0.5f, 0.7f);
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x06008137 RID: 33079 RVA: 0x002DB650 File Offset: 0x002D9850
		public static void ThrowFireGlow(Vector3 c, Map map, float size)
		{
			if (!c.ShouldSpawnMotesAt(map))
			{
				return;
			}
			Vector3 vector = c + size * new Vector3(Rand.Value - 0.5f, 0f, Rand.Value - 0.5f);
			if (!vector.InBounds(map))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(vector, map, FleckDefOf.FireGlow, Rand.Range(4f, 6f) * size);
			dataStatic.rotationRate = Rand.Range(-3f, 3f);
			dataStatic.velocityAngle = (float)Rand.Range(0, 360);
			dataStatic.velocitySpeed = 0.12f;
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x06008138 RID: 33080 RVA: 0x002DB700 File Offset: 0x002D9900
		public static void ThrowHeatGlow(IntVec3 c, Map map, float size)
		{
			Vector3 vector = c.ToVector3Shifted();
			if (!vector.ShouldSpawnMotesAt(map))
			{
				return;
			}
			vector += size * new Vector3(Rand.Value - 0.5f, 0f, Rand.Value - 0.5f);
			if (!vector.InBounds(map))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(vector, map, FleckDefOf.HeatGlow, Rand.Range(4f, 6f) * size);
			dataStatic.rotationRate = Rand.Range(-3f, 3f);
			dataStatic.velocityAngle = (float)Rand.Range(0, 360);
			dataStatic.velocitySpeed = 0.12f;
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x06008139 RID: 33081 RVA: 0x002DB7B8 File Offset: 0x002D99B8
		public static void ThrowMicroSparks(Vector3 loc, Map map)
		{
			if (!loc.ShouldSpawnMotesAt(map))
			{
				return;
			}
			loc -= new Vector3(0.5f, 0f, 0.5f);
			loc += new Vector3(Rand.Value, 0f, Rand.Value);
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc, map, FleckDefOf.MicroSparks, Rand.Range(0.8f, 1.2f));
			dataStatic.rotationRate = Rand.Range(-12f, 12f);
			dataStatic.velocityAngle = (float)Rand.Range(35, 45);
			dataStatic.velocitySpeed = 1.2f;
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x0600813A RID: 33082 RVA: 0x002DB864 File Offset: 0x002D9A64
		public static void ThrowLightningGlow(Vector3 loc, Map map, float size)
		{
			if (!loc.ShouldSpawnMotesAt(map))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc + size * new Vector3(Rand.Value - 0.5f, 0f, Rand.Value - 0.5f), map, FleckDefOf.LightningGlow, Rand.Range(4f, 6f) * size);
			dataStatic.rotationRate = Rand.Range(-3f, 3f);
			dataStatic.velocityAngle = (float)Rand.Range(0, 360);
			dataStatic.velocitySpeed = 1.2f;
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x0600813B RID: 33083 RVA: 0x002DB908 File Offset: 0x002D9B08
		public static void PlaceFootprint(Vector3 loc, Map map, float rot)
		{
			if (!loc.ShouldSpawnMotesAt(map))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc, map, FleckDefOf.Footprint, 0.5f);
			dataStatic.rotation = rot;
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x0600813C RID: 33084 RVA: 0x002DB945 File Offset: 0x002D9B45
		public static void ThrowHorseshoe(Pawn thrower, IntVec3 targetCell)
		{
			FleckMaker.ThrowObjectAt(thrower, targetCell, FleckDefOf.Horseshoe);
		}

		// Token: 0x0600813D RID: 33085 RVA: 0x002DB953 File Offset: 0x002D9B53
		public static void ThrowStone(Pawn thrower, IntVec3 targetCell)
		{
			FleckMaker.ThrowObjectAt(thrower, targetCell, FleckDefOf.Stone);
		}

		// Token: 0x0600813E RID: 33086 RVA: 0x002DB964 File Offset: 0x002D9B64
		private static void ThrowObjectAt(Pawn thrower, IntVec3 targetCell, FleckDef fleck)
		{
			if (!thrower.Position.ShouldSpawnMotesAt(thrower.Map))
			{
				return;
			}
			float num = Rand.Range(3.8f, 5.6f);
			Vector3 vector = targetCell.ToVector3Shifted() + Vector3Utility.RandomHorizontalOffset((1f - (float)thrower.skills.GetSkill(SkillDefOf.Shooting).Level / 20f) * 1.8f);
			vector.y = thrower.DrawPos.y;
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(thrower.DrawPos, thrower.Map, fleck, 1f);
			dataStatic.rotationRate = (float)Rand.Range(-300, 300);
			dataStatic.velocityAngle = (vector - dataStatic.spawnPosition).AngleFlat();
			dataStatic.velocitySpeed = num;
			dataStatic.airTimeLeft = new float?((float)Mathf.RoundToInt((dataStatic.spawnPosition - vector).MagnitudeHorizontal() / num));
			thrower.Map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x0600813F RID: 33087 RVA: 0x002DBA68 File Offset: 0x002D9C68
		public static void ThrowExplosionCell(IntVec3 cell, Map map, FleckDef fleckDef, Color color)
		{
			if (!cell.ShouldSpawnMotesAt(map))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(cell.ToVector3Shifted(), map, fleckDef, 1f);
			dataStatic.rotation = (float)(90 * Rand.RangeInclusive(0, 3));
			dataStatic.instanceColor = new Color?(color);
			map.flecks.CreateFleck(dataStatic);
			if (Rand.Value < 0.7f)
			{
				FleckMaker.ThrowDustPuff(cell, map, 1.2f);
			}
		}

		// Token: 0x06008140 RID: 33088 RVA: 0x002DBAD8 File Offset: 0x002D9CD8
		public static void ThrowExplosionInterior(Vector3 loc, Map map, FleckDef fleckDef)
		{
			if (!loc.ShouldSpawnMotesAt(map))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc, map, fleckDef, Rand.Range(3f, 4.5f));
			dataStatic.rotationRate = Rand.Range(-30f, 30f);
			dataStatic.velocityAngle = (float)Rand.Range(0, 360);
			dataStatic.velocitySpeed = Rand.Range(0.48f, 0.72f);
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x06008141 RID: 33089 RVA: 0x002DBB54 File Offset: 0x002D9D54
		public static void WaterSplash(Vector3 loc, Map map, float size, float velocity)
		{
			if (!loc.ShouldSpawnMotesAt(map))
			{
				return;
			}
			map.flecks.CreateFleck(new FleckCreationData
			{
				def = FleckDefOf.WaterSplash,
				targetSize = size,
				velocitySpeed = velocity,
				spawnPosition = loc
			});
		}

		// Token: 0x06008142 RID: 33090 RVA: 0x002DBBA4 File Offset: 0x002D9DA4
		public static void ConnectingLine(Vector3 start, Vector3 end, FleckDef fleckDef, Map map, float width = 1f)
		{
			Vector3 vector = end - start;
			float x = vector.MagnitudeHorizontal();
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(start + vector * 0.5f, map, fleckDef, 1f);
			dataStatic.exactScale = new Vector3?(new Vector3(x, 1f, width));
			dataStatic.rotation = Mathf.Atan2(-vector.z, vector.x) * 57.29578f;
			map.flecks.CreateFleck(dataStatic);
		}
	}
}
