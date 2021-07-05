using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020010AA RID: 4266
	public static class FireUtility
	{
		// Token: 0x060065CB RID: 26059 RVA: 0x0022661C File Offset: 0x0022481C
		public static bool CanEverAttachFire(this Thing t)
		{
			return !t.Destroyed && t.FlammableNow && t.def.category == ThingCategory.Pawn && t.TryGetComp<CompAttachBase>() != null;
		}

		// Token: 0x060065CC RID: 26060 RVA: 0x00226650 File Offset: 0x00224850
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

		// Token: 0x060065CD RID: 26061 RVA: 0x0022677E File Offset: 0x0022497E
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

		// Token: 0x060065CE RID: 26062 RVA: 0x002267B7 File Offset: 0x002249B7
		public static float ChanceToAttachFireFromEvent(Thing t)
		{
			return FireUtility.ChanceToAttachFireCumulative(t, 60f);
		}

		// Token: 0x060065CF RID: 26063 RVA: 0x002267C4 File Offset: 0x002249C4
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

		// Token: 0x060065D0 RID: 26064 RVA: 0x00226824 File Offset: 0x00224A24
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

		// Token: 0x060065D1 RID: 26065 RVA: 0x002268A6 File Offset: 0x00224AA6
		public static bool IsBurning(this TargetInfo t)
		{
			if (t.HasThing)
			{
				return t.Thing.IsBurning();
			}
			return t.Cell.ContainsStaticFire(t.Map);
		}

		// Token: 0x060065D2 RID: 26066 RVA: 0x002268D4 File Offset: 0x00224AD4
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

		// Token: 0x060065D3 RID: 26067 RVA: 0x00226988 File Offset: 0x00224B88
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

		// Token: 0x060065D4 RID: 26068 RVA: 0x002269D0 File Offset: 0x00224BD0
		public static bool ContainsTrap(this IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			return edifice != null && edifice is Building_Trap;
		}

		// Token: 0x060065D5 RID: 26069 RVA: 0x002269F3 File Offset: 0x00224BF3
		public static bool Flammable(this TerrainDef terrain)
		{
			return terrain.GetStatValueAbstract(StatDefOf.Flammability, null) > 0.01f;
		}

		// Token: 0x060065D6 RID: 26070 RVA: 0x00226A08 File Offset: 0x00224C08
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

		// Token: 0x04003982 RID: 14722
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
