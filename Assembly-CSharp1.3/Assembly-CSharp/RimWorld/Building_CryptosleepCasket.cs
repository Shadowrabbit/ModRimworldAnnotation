using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001076 RID: 4214
	public class Building_CryptosleepCasket : Building_Casket
	{
		// Token: 0x0600640A RID: 25610 RVA: 0x0021C145 File Offset: 0x0021A345
		public override bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
		{
			if (base.TryAcceptThing(thing, allowSpecialEffects))
			{
				if (allowSpecialEffects)
				{
					SoundDefOf.CryptosleepCasket_Accept.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600640B RID: 25611 RVA: 0x0021C178 File Offset: 0x0021A378
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
		{
			if (myPawn.IsQuestLodger())
			{
				FloatMenuOption floatMenuOption = new FloatMenuOption("CannotUseReason".Translate("CryptosleepCasketGuestsNotAllowed".Translate()), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield return floatMenuOption;
				yield break;
			}
			foreach (FloatMenuOption floatMenuOption2 in base.GetFloatMenuOptions(myPawn))
			{
				yield return floatMenuOption2;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			if (this.innerContainer.Count == 0)
			{
				if (!myPawn.CanReach(this, PathEndMode.InteractionCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					FloatMenuOption floatMenuOption3 = new FloatMenuOption("CannotUseNoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					yield return floatMenuOption3;
				}
				else
				{
					JobDef jobDef = JobDefOf.EnterCryptosleepCasket;
					string label = "EnterCryptosleepCasket".Translate();
					Action action = delegate()
					{
						Job job = JobMaker.MakeJob(jobDef, this);
						myPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
					};
					yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(label, action, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), myPawn, this, "ReservedBy");
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0600640C RID: 25612 RVA: 0x0021C18F File Offset: 0x0021A38F
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (base.Faction == Faction.OfPlayer && this.innerContainer.Count > 0 && this.def.building.isPlayerEjectable)
			{
				Command_Action command_Action = new Command_Action();
				command_Action.action = new Action(this.EjectContents);
				command_Action.defaultLabel = "CommandPodEject".Translate();
				command_Action.defaultDesc = "CommandPodEjectDesc".Translate();
				if (this.innerContainer.Count == 0)
				{
					command_Action.Disable("CommandPodEjectFailEmpty".Translate());
				}
				command_Action.hotKey = KeyBindingDefOf.Misc8;
				command_Action.icon = ContentFinder<Texture2D>.Get("UI/Commands/PodEject", true);
				yield return command_Action;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600640D RID: 25613 RVA: 0x0021C1A0 File Offset: 0x0021A3A0
		public override void EjectContents()
		{
			ThingDef filth_Slime = ThingDefOf.Filth_Slime;
			foreach (Thing thing in ((IEnumerable<Thing>)this.innerContainer))
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null)
				{
					PawnComponentsUtility.AddComponentsForSpawn(pawn);
					pawn.filth.GainFilth(filth_Slime);
					if (pawn.RaceProps.IsFlesh)
					{
						pawn.health.AddHediff(HediffDefOf.CryptosleepSickness, null, null, null);
					}
				}
			}
			if (!base.Destroyed)
			{
				SoundDefOf.CryptosleepCasket_Eject.PlayOneShot(SoundInfo.InMap(new TargetInfo(base.Position, base.Map, false), MaintenanceType.None));
			}
			base.EjectContents();
		}

		// Token: 0x0600640E RID: 25614 RVA: 0x0021C260 File Offset: 0x0021A460
		public static Building_CryptosleepCasket FindCryptosleepCasketFor(Pawn p, Pawn traveler, bool ignoreOtherReservations = false)
		{
			Predicate<Thing> <>9__1;
			foreach (ThingDef singleDef in from def in DefDatabase<ThingDef>.AllDefs
			where def.IsCryptosleepCasket
			select def)
			{
				IntVec3 position = p.Position;
				Map map = p.Map;
				ThingRequest thingReq = ThingRequest.ForDef(singleDef);
				PathEndMode peMode = PathEndMode.InteractionCell;
				TraverseParms traverseParams = TraverseParms.For(traveler, Danger.Deadly, TraverseMode.ByPawn, false, false, false);
				float maxDistance = 9999f;
				Predicate<Thing> validator;
				if ((validator = <>9__1) == null)
				{
					validator = (<>9__1 = ((Thing x) => !((Building_CryptosleepCasket)x).HasAnyContents && traveler.CanReserve(x, 1, -1, null, ignoreOtherReservations)));
				}
				Building_CryptosleepCasket building_CryptosleepCasket = (Building_CryptosleepCasket)GenClosest.ClosestThingReachable(position, map, thingReq, peMode, traverseParams, maxDistance, validator, null, 0, -1, false, RegionType.Set_Passable, false);
				if (building_CryptosleepCasket != null)
				{
					return building_CryptosleepCasket;
				}
			}
			return null;
		}
	}
}
