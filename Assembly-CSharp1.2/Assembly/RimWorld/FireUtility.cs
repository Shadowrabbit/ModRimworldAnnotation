using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001702 RID: 5890
	public static class FireUtility
	{
		// Token: 0x0600818D RID: 33165 RVA: 0x00057008 File Offset: 0x00055208
		public static bool CanEverAttachFire(this Thing t)
		{
			return !t.Destroyed && t.FlammableNow && t.def.category == ThingCategory.Pawn && t.TryGetComp<CompAttachBase>() != null;
		}

		// Token: 0x0600818E RID: 33166 RVA: 0x002672F0 File Offset: 0x002654F0
		public static float ChanceToStartFireIn(IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			float num = c.TerrainFlammableNow(map) ? c.GetTerrain(map).GetStatValueAbstract(StatDefOf.Flammability, null) : 0f;
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing = thingList[i];
				if (thing is Fire)
				{
					return 0f;
				}
				if (thing.def.category != ThingCategory.Pawn && thingList[i].FlammableNow)
				{
					num = Mathf.Max(num, thing.GetStatValue(StatDefOf.Flammability, true));
				}
			}
			if (num > 0f)
			{
				Building edifice = c.GetEdifice(map);
				if (edifice != null && edifice.def.passability == Traversability.Impassable && edifice.OccupiedRect().ContractedBy(1).Contains(c))
				{
					return 0f;
				}
				List<Thing> thingList2 = c.GetThingList(map);
				for (int j = 0; j < thingList2.Count; j++)
				{
					if (thingList2[j].def.category == ThingCategory.Filth && !thingList2[j].def.filth.allowsFire)
					{
						return 0f;
					}
				}
			}
			return num;
		}

		// Token: 0x0600818F RID: 33167 RVA: 0x00057039 File Offset: 0x00055239
		public static bool TryStartFireIn(IntVec3 c, Map map, float fireSize)
		{
			if (FireUtility.ChanceToStartFireIn(c, map) <= 0f)
			{
				return false;
			}
			Fire fire = (Fire)ThingMaker.MakeThing(ThingDefOf.Fire, null);
			fire.fireSize = fireSize;
			GenSpawn.Spawn(fire, c, map, Rot4.North, WipeMode.Vanish, false);
			return true;
		}

		// Token: 0x06008190 RID: 33168 RVA: 0x00057072 File Offset: 0x00055272
		public static float ChanceToAttachFireFromEvent(Thing t)
		{
			return FireUtility.ChanceToAttachFireCumulative(t, 60f);
		}

		// Token: 0x06008191 RID: 33169 RVA: 0x00267420 File Offset: 0x00265620
		public static float ChanceToAttachFireCumulative(Thing t, float freqInTicks)
		{
			if (!t.CanEverAttachFire())
			{
				return 0f;
			}
			if (t.HasAttachment(ThingDefOf.Fire))
			{
				return 0f;
			}
			float num = FireUtility.ChanceToCatchFirePerSecondForPawnFromFlammability.Evaluate(t.GetStatValue(StatDefOf.Flammability, true));
			return 1f - Mathf.Pow(1f - num, freqInTicks / 60f);
		}

		// Token: 0x06008192 RID: 33170 RVA: 0x00267480 File Offset: 0x00265680
		public static void TryAttachFire(this Thing t, float fireSize)
		{
			if (!t.CanEverAttachFire())
			{
				return;
			}
			if (t.HasAttachment(ThingDefOf.Fire))
			{
				return;
			}
			Fire fire = (Fire)ThingMaker.MakeThing(ThingDefOf.Fire, null);
			fire.fireSize = fireSize;
			fire.AttachTo(t);
			GenSpawn.Spawn(fire, t.Position, t.Map, Rot4.North, WipeMode.Vanish, false);
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				pawn.jobs.StopAll(false, true);
				pawn.records.Increment(RecordDefOf.TimesOnFire);
			}
		}

		// Token: 0x06008193 RID: 33171 RVA: 0x0005707F File Offset: 0x0005527F
		public static bool IsBurning(this TargetInfo t)
		{
			if (t.HasThing)
			{
				return t.Thing.IsBurning();
			}
			return t.Cell.ContainsStaticFire(t.Map);
		}

		// Token: 0x06008194 RID: 33172 RVA: 0x00267504 File Offset: 0x00265704
		public static bool IsBurning(this Thing t)
		{
			if (t.Destroyed || !t.Spawned)
			{
				return false;
			}
			if (!(t.def.size == IntVec2.One))
			{
				using (CellRect.Enumerator enumerator = t.OccupiedRect().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.ContainsStaticFire(t.Map))
						{
							return true;
						}
					}
				}
				return false;
			}
			if (t is Pawn)
			{
				return t.HasAttachment(ThingDefOf.Fire);
			}
			return t.Position.ContainsStaticFire(t.Map);
		}

		// Token: 0x06008195 RID: 33173 RVA: 0x002675B8 File Offset: 0x002657B8
		public static bool ContainsStaticFire(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				Fire fire = list[i] as Fire;
				if (fire != null && fire.parent == null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008196 RID: 33174 RVA: 0x00267600 File Offset: 0x00265800
		public static bool ContainsTrap(this IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			return edifice != null && edifice is Building_Trap;
		}

		// Token: 0x06008197 RID: 33175 RVA: 0x000570AA File Offset: 0x000552AA
		public static bool Flammable(this TerrainDef terrain)
		{
			return terrain.GetStatValueAbstract(StatDefOf.Flammability, null) > 0.01f;
		}

		// Token: 0x06008198 RID: 33176 RVA: 0x00267624 File Offset: 0x00265824
		public static bool TerrainFlammableNow(this IntVec3 c, Map map)
		{
			if (!c.GetTerrain(map).Flammable())
			{
				return false;
			}
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].FireBulwark)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0400541B RID: 21531
		private static readonly SimpleCurve ChanceToCatchFirePerSecondForPawnFromFlammability = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(0.1f, 0.07f),
				true
			},
			{
				new CurvePoint(0.3f, 1f),
				true
			},
			{
				new CurvePoint(1f, 1f),
				true
			}
		};
	}
}
