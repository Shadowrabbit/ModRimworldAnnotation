using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200182F RID: 6191
	public class CompRoyalImplant : ThingComp
	{
		// Token: 0x1700157D RID: 5501
		// (get) Token: 0x06008941 RID: 35137 RVA: 0x0005C2E7 File Offset: 0x0005A4E7
		public CompProperties_RoyalImplant Props
		{
			get
			{
				return (CompProperties_RoyalImplant)this.props;
			}
		}

		// Token: 0x06008942 RID: 35138 RVA: 0x0005C2F4 File Offset: 0x0005A4F4
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			Pair<Faction, RoyalTitleDef> minTitleForImplantAllFactions = Faction.GetMinTitleForImplantAllFactions(this.Props.implantHediff);
			if (minTitleForImplantAllFactions.First != null)
			{
				Faction first = minTitleForImplantAllFactions.First;
				StringBuilder stringBuilder = new StringBuilder("Stat_Thing_MinimumRoyalTitle_Desc".Translate(first.Named("FACTION")));
				if (typeof(Hediff_ImplantWithLevel).IsAssignableFrom(this.Props.implantHediff.hediffClass))
				{
					stringBuilder.Append("\n\n" + "Stat_Thing_MinimumRoyalTitle_ImplantWithLevel_Desc".Translate(first.Named("FACTION")) + ":\n\n");
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

		// Token: 0x06008943 RID: 35139 RVA: 0x00281B30 File Offset: 0x0027FD30
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

		// Token: 0x06008944 RID: 35140 RVA: 0x00281BB4 File Offset: 0x0027FDB4
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
			Hediff_ImplantWithLevel hediff_ImplantWithLevel = (Hediff_ImplantWithLevel)pawn.health.hediffSet.hediffs.FirstOrDefault((Hediff h) => h.def == hediff);
			int num = (levelOffset != 0 && hediff_ImplantWithLevel != null) ? (hediff_ImplantWithLevel.level + levelOffset) : 0;
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
