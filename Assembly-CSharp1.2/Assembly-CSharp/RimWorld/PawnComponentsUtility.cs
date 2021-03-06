using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020013EA RID: 5098
	public class PawnComponentsUtility
	{
		// Token: 0x06006E46 RID: 28230 RVA: 0x0021BDBC File Offset: 0x00219FBC
		public static void CreateInitialComponents(Pawn pawn)
		{
			if (pawn.ageTracker == null)
			{
				pawn.ageTracker = new Pawn_AgeTracker(pawn);
			}
			if (pawn.health == null)
			{
				pawn.health = new Pawn_HealthTracker(pawn);
			}
			if (pawn.records == null)
			{
				pawn.records = new Pawn_RecordsTracker(pawn);
			}
			if (pawn.inventory == null)
			{
				pawn.inventory = new Pawn_InventoryTracker(pawn);
			}
			if (pawn.meleeVerbs == null)
			{
				pawn.meleeVerbs = new Pawn_MeleeVerbs(pawn);
			}
			if (pawn.verbTracker == null)
			{
				pawn.verbTracker = new VerbTracker(pawn);
			}
			if (pawn.carryTracker == null)
			{
				pawn.carryTracker = new Pawn_CarryTracker(pawn);
			}
			if (pawn.needs == null)
			{
				pawn.needs = new Pawn_NeedsTracker(pawn);
			}
			if (pawn.mindState == null)
			{
				pawn.mindState = new Pawn_MindState(pawn);
			}
			if (pawn.RaceProps.ToolUser)
			{
				if (pawn.equipment == null)
				{
					pawn.equipment = new Pawn_EquipmentTracker(pawn);
				}
				if (pawn.apparel == null)
				{
					pawn.apparel = new Pawn_ApparelTracker(pawn);
				}
			}
			if (pawn.RaceProps.Humanlike)
			{
				if (pawn.ownership == null)
				{
					pawn.ownership = new Pawn_Ownership(pawn);
				}
				if (pawn.skills == null)
				{
					pawn.skills = new Pawn_SkillTracker(pawn);
				}
				if (pawn.story == null)
				{
					pawn.story = new Pawn_StoryTracker(pawn);
				}
				if (pawn.guest == null)
				{
					pawn.guest = new Pawn_GuestTracker(pawn);
				}
				if (pawn.guilt == null)
				{
					pawn.guilt = new Pawn_GuiltTracker(pawn);
				}
				if (pawn.workSettings == null)
				{
					pawn.workSettings = new Pawn_WorkSettings(pawn);
				}
				if (pawn.royalty == null)
				{
					pawn.royalty = new Pawn_RoyaltyTracker(pawn);
				}
				if (pawn.abilities == null)
				{
					pawn.abilities = new Pawn_AbilityTracker(pawn);
				}
			}
			if (pawn.RaceProps.IsFlesh)
			{
				if (pawn.relations == null)
				{
					pawn.relations = new Pawn_RelationsTracker(pawn);
				}
				if (pawn.psychicEntropy == null)
				{
					pawn.psychicEntropy = new Pawn_PsychicEntropyTracker(pawn);
				}
			}
			PawnComponentsUtility.AddAndRemoveDynamicComponents(pawn, false);
		}

		// Token: 0x06006E47 RID: 28231 RVA: 0x0021BFA0 File Offset: 0x0021A1A0
		public static void AddComponentsForSpawn(Pawn pawn)
		{
			if (pawn.rotationTracker == null)
			{
				pawn.rotationTracker = new Pawn_RotationTracker(pawn);
			}
			if (pawn.pather == null)
			{
				pawn.pather = new Pawn_PathFollower(pawn);
			}
			if (pawn.thinker == null)
			{
				pawn.thinker = new Pawn_Thinker(pawn);
			}
			if (pawn.jobs == null)
			{
				pawn.jobs = new Pawn_JobTracker(pawn);
			}
			if (pawn.stances == null)
			{
				pawn.stances = new Pawn_StanceTracker(pawn);
			}
			if (pawn.natives == null)
			{
				pawn.natives = new Pawn_NativeVerbs(pawn);
			}
			if (pawn.filth == null)
			{
				pawn.filth = new Pawn_FilthTracker(pawn);
			}
			if (pawn.RaceProps.intelligence <= Intelligence.ToolUser && pawn.caller == null)
			{
				pawn.caller = new Pawn_CallTracker(pawn);
			}
			if (pawn.RaceProps.IsFlesh)
			{
				if (pawn.interactions == null)
				{
					pawn.interactions = new Pawn_InteractionsTracker(pawn);
				}
				if (pawn.psychicEntropy == null)
				{
					pawn.psychicEntropy = new Pawn_PsychicEntropyTracker(pawn);
				}
				if (pawn.abilities == null)
				{
					pawn.abilities = new Pawn_AbilityTracker(pawn);
				}
			}
			PawnComponentsUtility.AddAndRemoveDynamicComponents(pawn, true);
		}

		// Token: 0x06006E48 RID: 28232 RVA: 0x0004AC07 File Offset: 0x00048E07
		public static void RemoveComponentsOnKilled(Pawn pawn)
		{
			pawn.carryTracker = null;
			pawn.needs = null;
			pawn.mindState = null;
			pawn.workSettings = null;
			pawn.trader = null;
		}

		// Token: 0x06006E49 RID: 28233 RVA: 0x0021C0AC File Offset: 0x0021A2AC
		public static void RemoveComponentsOnDespawned(Pawn pawn)
		{
			pawn.rotationTracker = null;
			pawn.pather = null;
			pawn.thinker = null;
			pawn.jobs = null;
			pawn.stances = null;
			pawn.natives = null;
			pawn.filth = null;
			pawn.caller = null;
			pawn.interactions = null;
			pawn.drafter = null;
		}

		// Token: 0x06006E4A RID: 28234 RVA: 0x0021C100 File Offset: 0x0021A300
		public static void AddAndRemoveDynamicComponents(Pawn pawn, bool actAsIfSpawned = false)
		{
			bool flag = pawn.Faction != null && pawn.Faction.IsPlayer;
			bool flag2 = pawn.HostFaction != null && pawn.HostFaction.IsPlayer;
			if (pawn.RaceProps.Humanlike && !pawn.Dead)
			{
				if (pawn.mindState.wantsToTradeWithColony)
				{
					if (pawn.trader == null)
					{
						pawn.trader = new Pawn_TraderTracker(pawn);
					}
				}
				else
				{
					pawn.trader = null;
				}
			}
			if (pawn.RaceProps.Humanlike)
			{
				if ((flag || flag2) && pawn.foodRestriction == null)
				{
					pawn.foodRestriction = new Pawn_FoodRestrictionTracker(pawn);
				}
				if (flag)
				{
					if (pawn.outfits == null)
					{
						pawn.outfits = new Pawn_OutfitTracker(pawn);
					}
					if (pawn.drugs == null)
					{
						pawn.drugs = new Pawn_DrugPolicyTracker(pawn);
					}
					if (pawn.timetable == null)
					{
						pawn.timetable = new Pawn_TimetableTracker(pawn);
					}
					if ((pawn.Spawned || actAsIfSpawned) && pawn.drafter == null)
					{
						pawn.drafter = new Pawn_DraftController(pawn);
					}
				}
				else
				{
					pawn.drafter = null;
				}
			}
			if ((flag || flag2) && pawn.playerSettings == null)
			{
				pawn.playerSettings = new Pawn_PlayerSettings(pawn);
			}
			if (pawn.RaceProps.intelligence <= Intelligence.ToolUser && pawn.Faction != null && !pawn.RaceProps.IsMechanoid && pawn.training == null)
			{
				pawn.training = new Pawn_TrainingTracker(pawn);
			}
			if (pawn.needs != null)
			{
				pawn.needs.AddOrRemoveNeedsAsAppropriate();
			}
		}

		// Token: 0x06006E4B RID: 28235 RVA: 0x0004AC2C File Offset: 0x00048E2C
		public static bool HasSpawnedComponents(Pawn p)
		{
			return p.pather != null;
		}
	}
}
