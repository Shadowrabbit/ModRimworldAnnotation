using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020013DD RID: 5085
	public static class PawnBanishUtility
	{
		// Token: 0x06006E1B RID: 28187 RVA: 0x0021B3B8 File Offset: 0x002195B8
		public static void Banish(Pawn pawn, int tile = -1)
		{
			if (pawn.Faction != Faction.OfPlayer && pawn.HostFaction != Faction.OfPlayer)
			{
				Log.Warning("Tried to banish " + pawn + " but he's neither a colonist, tame animal, nor prisoner.", false);
				return;
			}
			if (tile == -1)
			{
				tile = pawn.Tile;
			}
			bool flag = PawnBanishUtility.WouldBeLeftToDie(pawn, tile);
			if (!pawn.IsQuestLodger())
			{
				PawnDiedOrDownedThoughtsUtility.TryGiveThoughts(pawn, null, flag ? PawnDiedOrDownedThoughtsKind.BanishedToDie : PawnDiedOrDownedThoughtsKind.Banished);
			}
			Caravan caravan = pawn.GetCaravan();
			if (caravan != null)
			{
				CaravanInventoryUtility.MoveAllInventoryToSomeoneElse(pawn, caravan.PawnsListForReading, null);
				caravan.RemovePawn(pawn);
				if (flag)
				{
					if (Rand.Value < 0.8f)
					{
						pawn.Kill(null, null);
					}
					else
					{
						PawnBanishUtility.HealIfPossible(pawn);
					}
				}
			}
			if (pawn.guest != null)
			{
				pawn.guest.SetGuestStatus(null, false);
			}
			if (pawn.Faction == Faction.OfPlayer)
			{
				Faction faction;
				if (!pawn.Spawned && Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction_NewTemp(out faction, pawn.Faction != null && pawn.Faction.def.techLevel >= TechLevel.Medieval, false, TechLevel.Undefined, false))
				{
					if (pawn.Faction != faction)
					{
						pawn.SetFaction(faction, null);
					}
				}
				else if (pawn.Faction != null)
				{
					pawn.SetFaction(null, null);
				}
			}
			QuestUtility.SendQuestTargetSignals(pawn.questTags, "Banished", pawn.Named("SUBJECT"));
		}

		// Token: 0x06006E1C RID: 28188 RVA: 0x0021B508 File Offset: 0x00219708
		public static bool WouldBeLeftToDie(Pawn p, int tile)
		{
			if (p.Downed)
			{
				return true;
			}
			if (p.health.hediffSet.BleedRateTotal > 0.4f)
			{
				return true;
			}
			if (tile != -1)
			{
				float f = GenTemperature.AverageTemperatureAtTileForTwelfth(tile, GenLocalDate.Twelfth(p));
				if (!p.SafeTemperatureRange().Includes(f))
				{
					return true;
				}
			}
			List<Hediff> hediffs = p.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				HediffStage curStage = hediffs[i].CurStage;
				if (curStage != null && curStage.lifeThreatening)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006E1D RID: 28189 RVA: 0x0021B59C File Offset: 0x0021979C
		public static string GetBanishPawnDialogText(Pawn banishedPawn)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = PawnBanishUtility.WouldBeLeftToDie(banishedPawn, banishedPawn.Tile);
			stringBuilder.Append("ConfirmBanishPawnDialog".Translate(banishedPawn.Label, banishedPawn).Resolve());
			if (flag)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				stringBuilder.Append("ConfirmBanishPawnDialog_LeftToDie".Translate(banishedPawn.LabelShort, banishedPawn).Resolve().CapitalizeFirst());
			}
			List<ThingWithComps> list = (banishedPawn.equipment != null) ? banishedPawn.equipment.AllEquipmentListForReading : null;
			List<Apparel> list2 = (banishedPawn.apparel != null) ? banishedPawn.apparel.WornApparel : null;
			ThingOwner<Thing> thingOwner = (banishedPawn.inventory != null && PawnBanishUtility.WillTakeInventoryIfBanished(banishedPawn)) ? banishedPawn.inventory.innerContainer : null;
			if (!list.NullOrEmpty<ThingWithComps>() || !list2.NullOrEmpty<Apparel>() || !thingOwner.NullOrEmpty<Thing>())
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				stringBuilder.Append("ConfirmBanishPawnDialog_Items".Translate(banishedPawn.LabelShort, banishedPawn).Resolve().CapitalizeFirst().AdjustedFor(banishedPawn, "PAWN", true));
				stringBuilder.AppendLine();
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						stringBuilder.AppendLine();
						stringBuilder.Append("  - " + list[i].LabelCap);
					}
				}
				if (list2 != null)
				{
					for (int j = 0; j < list2.Count; j++)
					{
						stringBuilder.AppendLine();
						stringBuilder.Append("  - " + list2[j].LabelCap);
					}
				}
				if (thingOwner != null)
				{
					for (int k = 0; k < thingOwner.Count; k++)
					{
						stringBuilder.AppendLine();
						stringBuilder.Append("  - " + thingOwner[k].LabelCap);
					}
				}
			}
			if (!banishedPawn.IsQuestLodger())
			{
				PawnDiedOrDownedThoughtsUtility.BuildMoodThoughtsListString(banishedPawn, null, flag ? PawnDiedOrDownedThoughtsKind.BanishedToDie : PawnDiedOrDownedThoughtsKind.Banished, stringBuilder, "\n\n" + "ConfirmBanishPawnDialog_IndividualThoughts".Translate(banishedPawn.LabelShort, banishedPawn), "\n\n" + "ConfirmBanishPawnDialog_AllColonistsThoughts".Translate());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06006E1E RID: 28190 RVA: 0x0021B808 File Offset: 0x00219A08
		public static void ShowBanishPawnConfirmationDialog(Pawn pawn)
		{
			Dialog_MessageBox window = Dialog_MessageBox.CreateConfirmation(PawnBanishUtility.GetBanishPawnDialogText(pawn), delegate
			{
				PawnBanishUtility.Banish(pawn, -1);
			}, true, null);
			Find.WindowStack.Add(window);
		}

		// Token: 0x06006E1F RID: 28191 RVA: 0x0021B854 File Offset: 0x00219A54
		public static string GetBanishButtonTip(Pawn pawn)
		{
			if (PawnBanishUtility.WouldBeLeftToDie(pawn, pawn.Tile))
			{
				return "BanishTip".Translate() + "\n\n" + "BanishTipWillDie".Translate(pawn.LabelShort, pawn).CapitalizeFirst();
			}
			return "BanishTip".Translate();
		}

		// Token: 0x06006E20 RID: 28192 RVA: 0x0021B8C0 File Offset: 0x00219AC0
		private static void HealIfPossible(Pawn p)
		{
			PawnBanishUtility.tmpHediffs.Clear();
			PawnBanishUtility.tmpHediffs.AddRange(p.health.hediffSet.hediffs);
			for (int i = 0; i < PawnBanishUtility.tmpHediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury = PawnBanishUtility.tmpHediffs[i] as Hediff_Injury;
				if (hediff_Injury != null && !hediff_Injury.IsPermanent())
				{
					p.health.RemoveHediff(hediff_Injury);
				}
				else
				{
					ImmunityRecord immunityRecord = p.health.immunity.GetImmunityRecord(PawnBanishUtility.tmpHediffs[i].def);
					if (immunityRecord != null)
					{
						immunityRecord.immunity = 1f;
					}
				}
			}
		}

		// Token: 0x06006E21 RID: 28193 RVA: 0x0004AB2F File Offset: 0x00048D2F
		private static bool WillTakeInventoryIfBanished(Pawn pawn)
		{
			return !pawn.IsCaravanMember();
		}

		// Token: 0x040048B3 RID: 18611
		private const float DeathChanceForCaravanPawnBanishedToDie = 0.8f;

		// Token: 0x040048B4 RID: 18612
		private static List<Hediff> tmpHediffs = new List<Hediff>();
	}
}
