using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x020003BB RID: 955
	public static class DebugTools_Health
	{
		// Token: 0x06001D88 RID: 7560 RVA: 0x000B8674 File Offset: 0x000B6874
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

		// Token: 0x06001D89 RID: 7561 RVA: 0x000B8730 File Offset: 0x000B6930
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

		// Token: 0x06001D8A RID: 7562 RVA: 0x000B87B4 File Offset: 0x000B69B4
		private static List<DebugMenuOption> Options_Damage_BodyParts(Pawn p, DamageDef def)
		{
			if (p == null)
			{
				throw new ArgumentNullException("p");
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("(no body part)", DebugMenuOptionMode.Action, delegate()
			{
				p.TakeDamage(new DamageInfo(def, 5f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true));
			}));
			foreach (BodyPartRecord localPart2 in p.RaceProps.body.AllParts)
			{
				BodyPartRecord localPart = localPart2;
				list.Add(new DebugMenuOption(localPart.LabelCap, DebugMenuOptionMode.Action, delegate()
				{
					p.TakeDamage(new DamageInfo(def, 5f, 0f, -1f, null, localPart, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true));
				}));
			}
			return list;
		}

		// Token: 0x06001D8B RID: 7563 RVA: 0x000B8898 File Offset: 0x000B6A98
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

		// Token: 0x06001D8C RID: 7564 RVA: 0x000B8940 File Offset: 0x000B6B40
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

		// Token: 0x06001D8D RID: 7565 RVA: 0x000B8A40 File Offset: 0x000B6C40
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
