using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DA3 RID: 3491
	public static class PawnBanishUtility
	{
		// Token: 0x060050F1 RID: 20721 RVA: 0x001B15C0 File Offset: 0x001AF7C0
		public static void Banish(Pawn pawn, int tile = -1)
		{
			if (pawn.Faction != Faction.OfPlayer && pawn.HostFaction != Faction.OfPlayer)
			{
				Log.Warning("Tried to banish " + pawn + " but he's neither a colonist, tame animal, nor prisoner.");
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
				pawn.guest.SetGuestStatus(null, GuestStatus.Guest);
			}
			if (pawn.Faction == Faction.OfPlayer)
			{
				Faction faction;
				if (!pawn.Spawned && Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out faction, pawn.Faction != null && pawn.Faction.def.techLevel >= TechLevel.Medieval, false, TechLevel.Undefined, false))
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

		// Token: 0x060050F2 RID: 20722 RVA: 0x001B1710 File Offset: 0x001AF910
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

		// Token: 0x060050F3 RID: 20723 RVA: 0x001B17A4 File Offset: 0x001AF9A4
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
			if (!banishedPawn.IsQuestLodger() && (banishedPawn.guilt == null || !banishedPawn.guilt.IsGuilty))
			{
				PawnDiedOrDownedThoughtsUtility.BuildMoodThoughtsListString(banishedPawn, null, flag ? PawnDiedOrDownedThoughtsKind.BanishedToDie : PawnDiedOrDownedThoughtsKind.Banished, stringBuilder, "\n\n" + "ConfirmBanishPawnDialog_IndividualThoughts".Translate(banishedPawn.LabelShort, banishedPawn), "\n\n" + "ConfirmBanishPawnDialog_AllColonistsThoughts".Translate());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060050F4 RID: 20724 RVA: 0x001B1A24 File Offset: 0x001AFC24
		public static void ShowBanishPawnConfirmationDialog(Pawn pawn, Action onConfirm = null)
		{
			Dialog_MessageBox window = Dialog_MessageBox.CreateConfirmation(PawnBanishUtility.GetBanishPawnDialogText(pawn), delegate
			{
				PawnBanishUtility.Banish(pawn, -1);
				Action onConfirm2 = onConfirm;
				if (onConfirm2 == null)
				{
					return;
				}
				onConfirm2();
			}, true, null);
			Find.WindowStack.Add(window);
		}

		// Token: 0x060050F5 RID: 20725 RVA: 0x001B1A74 File Offset: 0x001AFC74
		public static string GetBanishButtonTip(Pawn pawn)
		{
			if (PawnBanishUtility.WouldBeLeftToDie(pawn, pawn.Tile))
			{
				return "BanishTip".Translate() + "\n\n" + "BanishTipWillDie".Translate(pawn.LabelShort, pawn).CapitalizeFirst();
			}
			return "BanishTip".Translate();
		}

		// Token: 0x060050F6 RID: 20726 RVA: 0x001B1AE0 File Offset: 0x001AFCE0
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

		// Token: 0x060050F7 RID: 20727 RVA: 0x001B1B7F File Offset: 0x001AFD7F
		private static bool WillTakeInventoryIfBanished(Pawn pawn)
		{
			return !pawn.IsCaravanMember();
		}

		// Token: 0x04003003 RID: 12291
		private const float DeathChanceForCaravanPawnBanishedToDie = 0.8f;

		// Token: 0x04003004 RID: 12292
		private static List<Hediff> tmpHediffs = new List<Hediff>();
	}
}
