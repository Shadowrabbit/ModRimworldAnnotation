using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000687 RID: 1671
	public static class DebugOutputsTechnical
	{
		// Token: 0x06002BBE RID: 11198 RVA: 0x0012C5A4 File Offset: 0x0012A7A4
		[DebugOutput]
		public static void KeyStrings()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (object obj in Enum.GetValues(typeof(KeyCode)))
			{
				KeyCode k = (KeyCode)obj;
				stringBuilder.AppendLine(k.ToString() + " - " + k.ToStringReadable());
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x06002BBF RID: 11199 RVA: 0x0012C638 File Offset: 0x0012A838
		[DebugOutput]
		public static void DefNames()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (IEnumerator<Type> enumerator = (from def in GenDefDatabase.AllDefTypesWithDatabases()
			orderby def.Name
			select def).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Type type = enumerator.Current;
					DebugMenuOption item = new DebugMenuOption(type.Name, DebugMenuOptionMode.Action, delegate()
					{
						IEnumerable source = (IEnumerable)GenGeneric.GetStaticPropertyOnGenericType(typeof(DefDatabase<>), type, "AllDefs");
						int num = 0;
						StringBuilder stringBuilder = new StringBuilder();
						foreach (Def def in source.Cast<Def>())
						{
							stringBuilder.AppendLine(def.defName);
							num++;
							if (num >= 500)
							{
								Log.Message(stringBuilder.ToString(), false);
								stringBuilder = new StringBuilder();
								num = 0;
							}
						}
						Log.Message(stringBuilder.ToString(), false);
					});
					list.Add(item);
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06002BC0 RID: 11200 RVA: 0x0012C6E8 File Offset: 0x0012A8E8
		[DebugOutput]
		public static void DefNamesAll()
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (Type type in from def in GenDefDatabase.AllDefTypesWithDatabases()
			orderby def.Name
			select def)
			{
				IEnumerable source = (IEnumerable)GenGeneric.GetStaticPropertyOnGenericType(typeof(DefDatabase<>), type, "AllDefs");
				stringBuilder.AppendLine("--    " + type.ToString());
				foreach (Def def2 in source.Cast<Def>().OrderBy((Def def) => def.defName))
				{
					stringBuilder.AppendLine(def2.defName);
					num++;
					if (num >= 500)
					{
						Log.Message(stringBuilder.ToString(), false);
						stringBuilder = new StringBuilder();
						num = 0;
					}
				}
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x06002BC1 RID: 11201 RVA: 0x0012C838 File Offset: 0x0012AA38
		[DebugOutput]
		public static void DefLabels()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (IEnumerator<Type> enumerator = (from def in GenDefDatabase.AllDefTypesWithDatabases()
			orderby def.Name
			select def).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Type type = enumerator.Current;
					DebugMenuOption item = new DebugMenuOption(type.Name, DebugMenuOptionMode.Action, delegate()
					{
						IEnumerable source = (IEnumerable)GenGeneric.GetStaticPropertyOnGenericType(typeof(DefDatabase<>), type, "AllDefs");
						int num = 0;
						StringBuilder stringBuilder = new StringBuilder();
						foreach (Def def in source.Cast<Def>())
						{
							stringBuilder.AppendLine(def.label);
							num++;
							if (num >= 500)
							{
								Log.Message(stringBuilder.ToString(), false);
								stringBuilder = new StringBuilder();
								num = 0;
							}
						}
						Log.Message(stringBuilder.ToString(), false);
					});
					list.Add(item);
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06002BC2 RID: 11202 RVA: 0x0012C8E8 File Offset: 0x0012AAE8
		[DebugOutput]
		public static void BestThingRequestGroup()
		{
			IEnumerable<ThingDef> dataSources = from x in DefDatabase<ThingDef>.AllDefs
			where ListerThings.EverListable(x, ListerThingsUse.Global) || ListerThings.EverListable(x, ListerThingsUse.Region)
			orderby x.label
			select x;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[3];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("best local", delegate(ThingDef d)
			{
				IEnumerable<ThingRequestGroup> source;
				if (!ListerThings.EverListable(d, ListerThingsUse.Region))
				{
					source = Enumerable.Empty<ThingRequestGroup>();
				}
				else
				{
					source = from x in (ThingRequestGroup[])Enum.GetValues(typeof(ThingRequestGroup))
					where x.StoreInRegion() && x.Includes(d)
					select x;
				}
				if (!source.Any<ThingRequestGroup>())
				{
					return "-";
				}
				ThingRequestGroup best = source.MinBy((ThingRequestGroup x) => DefDatabase<ThingDef>.AllDefs.Count((ThingDef y) => ListerThings.EverListable(y, ListerThingsUse.Region) && x.Includes(y)));
				return string.Concat(new object[]
				{
					best,
					" (defs: ",
					DefDatabase<ThingDef>.AllDefs.Count((ThingDef x) => ListerThings.EverListable(x, ListerThingsUse.Region) && best.Includes(x)),
					")"
				});
			});
			array[2] = new TableDataGetter<ThingDef>("best global", delegate(ThingDef d)
			{
				IEnumerable<ThingRequestGroup> source;
				if (!ListerThings.EverListable(d, ListerThingsUse.Global))
				{
					source = Enumerable.Empty<ThingRequestGroup>();
				}
				else
				{
					source = from x in (ThingRequestGroup[])Enum.GetValues(typeof(ThingRequestGroup))
					where x.Includes(d)
					select x;
				}
				if (!source.Any<ThingRequestGroup>())
				{
					return "-";
				}
				ThingRequestGroup best = source.MinBy((ThingRequestGroup x) => DefDatabase<ThingDef>.AllDefs.Count((ThingDef y) => ListerThings.EverListable(y, ListerThingsUse.Global) && x.Includes(y)));
				return string.Concat(new object[]
				{
					best,
					" (defs: ",
					DefDatabase<ThingDef>.AllDefs.Count((ThingDef x) => ListerThings.EverListable(x, ListerThingsUse.Global) && best.Includes(x)),
					")"
				});
			});
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06002BC3 RID: 11203 RVA: 0x0012C9D4 File Offset: 0x0012ABD4
		[DebugOutput]
		public static void DamageTest()
		{
			ThingDef thingDef = ThingDef.Named("Bullet_BoltActionRifle");
			PawnKindDef slave = PawnKindDefOf.Slave;
			Faction faction = FactionUtility.DefaultFactionFrom(slave.defaultFactionType);
			DamageInfo dinfo = new DamageInfo(thingDef.projectile.damageDef, (float)thingDef.projectile.GetDamageAmount(null, null), thingDef.projectile.GetArmorPenetration(null, null), -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null);
			dinfo.SetIgnoreInstantKillProtection(true);
			int num = 0;
			int num2 = 0;
			DefMap<BodyPartDef, int> defMap = new DefMap<BodyPartDef, int>();
			for (int i = 0; i < 500; i++)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(slave, faction, PawnGenerationContext.NonPlayer, -1, true, false, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null));
				List<BodyPartDef> list = (from hd in pawn.health.hediffSet.GetMissingPartsCommonAncestors()
				select hd.Part.def).ToList<BodyPartDef>();
				for (int j = 0; j < 2; j++)
				{
					pawn.TakeDamage(dinfo);
					if (pawn.Dead)
					{
						num++;
						break;
					}
				}
				List<BodyPartDef> list2 = (from hd in pawn.health.hediffSet.GetMissingPartsCommonAncestors()
				select hd.Part.def).ToList<BodyPartDef>();
				if (list2.Count > list.Count)
				{
					num2++;
					foreach (BodyPartDef bodyPartDef in list2)
					{
						DefMap<BodyPartDef, int> defMap2 = defMap;
						BodyPartDef def = bodyPartDef;
						int num3 = defMap2[def];
						defMap2[def] = num3 + 1;
					}
					foreach (BodyPartDef bodyPartDef2 in list)
					{
						DefMap<BodyPartDef, int> defMap3 = defMap;
						BodyPartDef def = bodyPartDef2;
						int num3 = defMap3[def];
						defMap3[def] = num3 - 1;
					}
				}
				Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Damage test");
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"Hit ",
				500,
				" ",
				slave.label,
				"s with ",
				2,
				"x ",
				thingDef.label,
				" (",
				thingDef.projectile.GetDamageAmount(null, null),
				" damage) each. Results:"
			}));
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"Killed: ",
				num,
				" / ",
				500,
				" (",
				((float)num / 500f).ToStringPercent(),
				")"
			}));
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"Part losers: ",
				num2,
				" / ",
				500,
				" (",
				((float)num2 / 500f).ToStringPercent(),
				")"
			}));
			stringBuilder.AppendLine("Parts lost:");
			foreach (BodyPartDef bodyPartDef3 in DefDatabase<BodyPartDef>.AllDefs)
			{
				if (defMap[bodyPartDef3] > 0)
				{
					stringBuilder.AppendLine(string.Concat(new object[]
					{
						"   ",
						bodyPartDef3.label,
						": ",
						defMap[bodyPartDef3]
					}));
				}
			}
			Log.Message(stringBuilder.ToString(), false);
		}
	}
}
