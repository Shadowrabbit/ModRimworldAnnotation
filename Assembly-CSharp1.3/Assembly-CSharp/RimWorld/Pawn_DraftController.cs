using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000E2A RID: 3626
	public class Pawn_DraftController : IExposable
	{
		// Token: 0x17000E41 RID: 3649
		// (get) Token: 0x060053CF RID: 21455 RVA: 0x001C5E4F File Offset: 0x001C404F
		// (set) Token: 0x060053D0 RID: 21456 RVA: 0x001C5E58 File Offset: 0x001C4058
		public bool Drafted
		{
			get
			{
				return this.draftedInt;
			}
			set
			{
				if (value == this.draftedInt)
				{
					return;
				}
				this.pawn.mindState.priorityWork.ClearPrioritizedWorkAndJobQueue();
				this.fireAtWillInt = true;
				this.draftedInt = value;
				if (!value && this.pawn.Spawned)
				{
					this.pawn.Map.pawnDestinationReservationManager.ReleaseAllClaimedBy(this.pawn);
				}
				this.pawn.jobs.ClearQueuedJobs(true);
				if (this.pawn.jobs.curJob != null && this.pawn.jobs.IsCurrentJobPlayerInterruptible())
				{
					JobDriver_Ingest jobDriver_Ingest;
					if ((jobDriver_Ingest = (this.pawn.jobs.curDriver as JobDriver_Ingest)) != null && jobDriver_Ingest.EatingFromInventory)
					{
						this.pawn.inventory.innerContainer.TryAddRangeOrTransfer(this.pawn.carryTracker.innerContainer, true, false);
					}
					this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
				if (this.draftedInt)
				{
					Lord lord = this.pawn.GetLord();
					if (lord != null && lord.LordJob is LordJob_VoluntarilyJoinable)
					{
						lord.Notify_PawnLost(this.pawn, PawnLostCondition.Drafted, null);
					}
					this.autoUndrafter.Notify_Drafted();
				}
				else if (this.pawn.playerSettings != null)
				{
					this.pawn.playerSettings.animalsReleased = false;
				}
				foreach (Pawn pawn in PawnUtility.SpawnedMasteredPawns(this.pawn))
				{
					pawn.jobs.Notify_MasterDraftedOrUndrafted();
				}
			}
		}

		// Token: 0x17000E42 RID: 3650
		// (get) Token: 0x060053D1 RID: 21457 RVA: 0x001C5FFC File Offset: 0x001C41FC
		// (set) Token: 0x060053D2 RID: 21458 RVA: 0x001C6004 File Offset: 0x001C4204
		public bool FireAtWill
		{
			get
			{
				return this.fireAtWillInt;
			}
			set
			{
				this.fireAtWillInt = value;
				if (!this.fireAtWillInt && this.pawn.stances.curStance is Stance_Warmup)
				{
					this.pawn.stances.CancelBusyStanceSoft();
				}
			}
		}

		// Token: 0x060053D3 RID: 21459 RVA: 0x001C603C File Offset: 0x001C423C
		public Pawn_DraftController(Pawn pawn)
		{
			this.pawn = pawn;
			this.autoUndrafter = new AutoUndrafter(pawn);
		}

		// Token: 0x060053D4 RID: 21460 RVA: 0x001C6060 File Offset: 0x001C4260
		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.draftedInt, "drafted", false, false);
			Scribe_Values.Look<bool>(ref this.fireAtWillInt, "fireAtWill", true, false);
			Scribe_Deep.Look<AutoUndrafter>(ref this.autoUndrafter, "autoUndrafter", new object[]
			{
				this.pawn
			});
		}

		// Token: 0x060053D5 RID: 21461 RVA: 0x001C60B0 File Offset: 0x001C42B0
		public void DraftControllerTick()
		{
			this.autoUndrafter.AutoUndraftTick();
		}

		// Token: 0x060053D6 RID: 21462 RVA: 0x001C60BD File Offset: 0x001C42BD
		internal IEnumerable<Gizmo> GetGizmos()
		{
			Command_Toggle command_Toggle = new Command_Toggle();
			command_Toggle.hotKey = KeyBindingDefOf.Command_ColonistDraft;
			command_Toggle.isActive = (() => this.Drafted);
			command_Toggle.toggleAction = delegate()
			{
				this.Drafted = !this.Drafted;
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Drafting, KnowledgeAmount.SpecificInteraction);
				if (this.Drafted)
				{
					LessonAutoActivator.TeachOpportunity(ConceptDefOf.QueueOrders, OpportunityType.GoodToKnow);
				}
			};
			command_Toggle.defaultDesc = "CommandToggleDraftDesc".Translate();
			command_Toggle.icon = TexCommand.Draft;
			command_Toggle.turnOnSound = SoundDefOf.DraftOn;
			command_Toggle.turnOffSound = SoundDefOf.DraftOff;
			command_Toggle.groupKey = 81729172;
			command_Toggle.defaultLabel = (this.Drafted ? "CommandUndraftLabel" : "CommandDraftLabel").Translate();
			if (this.pawn.Downed)
			{
				command_Toggle.Disable("IsIncapped".Translate(this.pawn.LabelShort, this.pawn));
			}
			if (!this.Drafted)
			{
				command_Toggle.tutorTag = "Draft";
			}
			else
			{
				command_Toggle.tutorTag = "Undraft";
			}
			yield return command_Toggle;
			if (this.Drafted && this.pawn.equipment.Primary != null && this.pawn.equipment.Primary.def.IsRangedWeapon)
			{
				yield return new Command_Toggle
				{
					hotKey = KeyBindingDefOf.Misc6,
					isActive = (() => this.FireAtWill),
					toggleAction = delegate()
					{
						this.FireAtWill = !this.FireAtWill;
					},
					icon = TexCommand.FireAtWill,
					defaultLabel = "CommandFireAtWillLabel".Translate(),
					defaultDesc = "CommandFireAtWillDesc".Translate(),
					tutorTag = "FireAtWillToggle"
				};
			}
			yield break;
		}

		// Token: 0x060053D7 RID: 21463 RVA: 0x001C60CD File Offset: 0x001C42CD
		internal void Notify_PrimaryWeaponChanged()
		{
			this.fireAtWillInt = true;
		}

		// Token: 0x04003153 RID: 12627
		public Pawn pawn;

		// Token: 0x04003154 RID: 12628
		private bool draftedInt;

		// Token: 0x04003155 RID: 12629
		private bool fireAtWillInt = true;

		// Token: 0x04003156 RID: 12630
		private AutoUndrafter autoUndrafter;
	}
}
