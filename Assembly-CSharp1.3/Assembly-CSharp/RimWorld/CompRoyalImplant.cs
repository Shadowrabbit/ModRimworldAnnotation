using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200118C RID: 4492
	public class CompRoyalImplant : ThingComp
	{
		// Token: 0x170012A9 RID: 4777
		// (get) Token: 0x06006C0F RID: 27663 RVA: 0x00243BCE File Offset: 0x00241DCE
		public CompProperties_RoyalImplant Props
		{
			get
			{
				return (CompProperties_RoyalImplant)this.props;
			}
		}

		// Token: 0x06006C10 RID: 27664 RVA: 0x00243BDB File Offset: 0x00241DDB
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			Pair<Faction, RoyalTitleDef> minTitleForImplantAllFactions = Faction.GetMinTitleForImplantAllFactions(this.Props.implantHediff);
			if (minTitleForImplantAllFactions.First != null)
			{
				Faction first = minTitleForImplantAllFactions.First;
				StringBuilder stringBuilder = new StringBuilder("Stat_Thing_MinimumRoyalTitle_Desc".Translate(first.Named("FACTION")));
				if (typeof(Hediff_Level).IsAssignableFrom(this.Props.implantHediff.hediffClass))
				{
					stringBuilder.Append("\n\n" + "Stat_Thing_MinimumRoyalTitle_Level_Desc".Translate(first.Named("FACTION")) + ":\n\n");
					int num = 1;
					while ((float)num <= this.Props.implantHediff.maxSeverity)
					{
						stringBuilder.Append(string.Concat(new object[]
						{
							" -  x",
							num,
							", ",
							first.GetMinTitleForImplant(this.Props.implantHediff, num).GetLabelCapForBothGenders(),
							"\n"
						}));
						num++;
					}
				}
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsNonPawnImportant, "Stat_Thing_MinimumRoyalTitle_Name".Translate(first.Named("FACTION")).Resolve(), minTitleForImplantAllFactions.Second.GetLabelCapForBothGenders(), stringBuilder.ToTaggedString().Resolve(), 2100, null, null, false);
			}
			yield break;
		}

		// Token: 0x06006C11 RID: 27665 RVA: 0x00243BEC File Offset: 0x00241DEC
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			Pair<Faction, RoyalTitleDef> minTitleForImplantAllFactions = Faction.GetMinTitleForImplantAllFactions(this.Props.implantHediff);
			if (minTitleForImplantAllFactions.First != null)
			{
				stringBuilder.AppendLine("MinimumRoyalTitleInspectString".Translate(minTitleForImplantAllFactions.First.Named("FACTION"), minTitleForImplantAllFactions.Second.GetLabelCapForBothGenders().Named("TITLE")).Resolve());
			}
			if (stringBuilder.Length <= 0)
			{
				return null;
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x06006C12 RID: 27666 RVA: 0x00243C70 File Offset: 0x00241E70
		public static TaggedString CheckForViolations(Pawn pawn, HediffDef hediff, int levelOffset)
		{
			if (levelOffset < 0)
			{
				return "";
			}
			if (pawn.Faction != Faction.OfPlayer || !hediff.HasComp(typeof(HediffComp_RoyalImplant)))
			{
				return "";
			}
			Hediff_Level hediff_Level = (Hediff_Level)pawn.health.hediffSet.hediffs.FirstOrDefault((Hediff h) => h.def == hediff);
			int num = (levelOffset != 0 && hediff_Level != null) ? (hediff_Level.level + levelOffset) : 0;
			foreach (Faction faction in Find.FactionManager.AllFactionsListForReading)
			{
				if (pawn.Faction != null && !faction.Hidden && !faction.HostileTo(Faction.OfPlayer) && ThingRequiringRoyalPermissionUtility.IsViolatingRulesOf(hediff, pawn, faction, num))
				{
					RoyalTitleDef minTitleForImplant = faction.GetMinTitleForImplant(hediff, num);
					HediffCompProperties_RoyalImplant hediffCompProperties_RoyalImplant = hediff.CompProps<HediffCompProperties_RoyalImplant>();
					string arg = hediff.label + ((num == 0) ? "" : (" (" + num + "x)"));
					TaggedString taggedString = hediffCompProperties_RoyalImplant.violationTriggerDescriptionKey.Translate(pawn.Named("PAWN"));
					TaggedString taggedString2 = "RoyalImplantIllegalUseWarning".Translate(pawn.Named("PAWN"), arg.Named("IMPLANT"), faction.Named("FACTION"), minTitleForImplant.GetLabelCapFor(pawn).Named("TITLE"), taggedString.Named("VIOLATIONTRIGGER"));
					if (levelOffset != 0)
					{
						taggedString2 += "\n\n" + "RoyalImplantUpgradeConfirmation".Translate();
					}
					else
					{
						taggedString2 += "\n\n" + "RoyalImplantInstallConfirmation".Translate();
					}
					return taggedString2;
				}
			}
			return "";
		}
	}
}
