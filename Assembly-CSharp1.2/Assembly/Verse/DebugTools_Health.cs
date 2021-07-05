using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x020006B1 RID: 1713
	public static class DebugTools_Health
	{
		// Token: 0x06002C7C RID: 11388 RVA: 0x0012F4BC File Offset: 0x0012D6BC
		public static List<DebugMenuOption> Options_RestorePart(Pawn p)
		{
			if (p == null)
			{
				throw new ArgumentNullException("p");
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (BodyPartRecord localPart2 in p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null))
			{
				BodyPartRecord localPart = localPart2;
				list.Add(new DebugMenuOption(localPart.LabelCap, DebugMenuOptionMode.Action, delegate()
				{
					p.health.RestorePart(localPart, null, true);
				}));
			}
			return list;
		}

		// Token: 0x06002C7D RID: 11389 RVA: 0x0012F578 File Offset: 0x0012D778
		public static List<DebugMenuOption> Options_ApplyDamage()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (DamageDef localDef2 in DefDatabase<DamageDef>.AllDefs)
			{
				DamageDef localDef = localDef2;
				list.Add(new DebugMenuOption(localDef.LabelCap, DebugMenuOptionMode.Tool, delegate()
				{
					Pawn pawn = Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).OfType<Pawn>().FirstOrDefault<Pawn>();
					if (pawn != null)
					{
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_Damage_BodyParts(pawn, localDef)));
					}
				}));
			}
			return list;
		}

		// Token: 0x06002C7E RID: 11390 RVA: 0x0012F5FC File Offset: 0x0012D7FC
		private static List<DebugMenuOption> Options_Damage_BodyParts(Pawn p, DamageDef def)
		{
			if (p == null)
			{
				throw new ArgumentNullException("p");
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("(no body part)", DebugMenuOptionMode.Action, delegate()
			{
				p.TakeDamage(new DamageInfo(def, 5f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}));
			foreach (BodyPartRecord localPart2 in p.RaceProps.body.AllParts)
			{
				BodyPartRecord localPart = localPart2;
				list.Add(new DebugMenuOption(localPart.LabelCap, DebugMenuOptionMode.Action, delegate()
				{
					p.TakeDamage(new DamageInfo(def, 5f, 0f, -1f, null, localPart, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
				}));
			}
			return list;
		}

		// Token: 0x06002C7F RID: 11391 RVA: 0x0012F6E0 File Offset: 0x0012D8E0
		public static List<DebugMenuOption> Options_AddHediff()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (HediffDef localDef2 in from d in DefDatabase<HediffDef>.AllDefs
			orderby d.hediffClass.ToStringSafe<Type>()
			select d)
			{
				HediffDef localDef = localDef2;
				list.Add(new DebugMenuOption(localDef.LabelCap, DebugMenuOptionMode.Tool, delegate()
				{
					Pawn pawn = Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).Where((Thing t) => t is Pawn).Cast<Pawn>().FirstOrDefault<Pawn>();
					if (pawn != null)
					{
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_Hediff_BodyParts(pawn, localDef)));
					}
				}));
			}
			return list;
		}

		// Token: 0x06002C80 RID: 11392 RVA: 0x0012F788 File Offset: 0x0012D988
		private static List<DebugMenuOption> Options_Hediff_BodyParts(Pawn p, HediffDef def)
		{
			if (p == null)
			{
				throw new ArgumentNullException("p");
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("(no body part)", DebugMenuOptionMode.Action, delegate()
			{
				p.health.AddHediff(def, null, null, null);
			}));
			foreach (BodyPartRecord localPart2 in from pa in p.RaceProps.body.AllParts
			orderby pa.Label
			select pa)
			{
				BodyPartRecord localPart = localPart2;
				list.Add(new DebugMenuOption(localPart.LabelCap, DebugMenuOptionMode.Action, delegate()
				{
					p.health.AddHediff(def, localPart, null, null);
				}));
			}
			return list;
		}

		// Token: 0x06002C81 RID: 11393 RVA: 0x0012F888 File Offset: 0x0012DA88
		public static List<DebugMenuOption> Options_RemoveHediff(Pawn pawn)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Hediff localH2 in pawn.health.hediffSet.hediffs)
			{
				Hediff localH = localH2;
				list.Add(new DebugMenuOption(localH.LabelCap + ((localH.Part != null) ? (" (" + localH.Part.def + ")") : ""), DebugMenuOptionMode.Action, delegate()
				{
					pawn.health.RemoveHediff(localH);
				}));
			}
			return list;
		}
	}
}
