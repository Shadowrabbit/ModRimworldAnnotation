using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001396 RID: 5014
	public class CompAbilityEffect_Teleport : CompAbilityEffect_WithDest
	{
		// Token: 0x170010CE RID: 4302
		// (get) Token: 0x06006CC5 RID: 27845 RVA: 0x00049F98 File Offset: 0x00048198
		public new CompProperties_AbilityTeleport Props
		{
			get
			{
				return (CompProperties_AbilityTeleport)this.props;
			}
		}

		// Token: 0x06006CC6 RID: 27846 RVA: 0x00049FA5 File Offset: 0x000481A5
		public override IEnumerable<PreCastAction> GetPreCastActions()
		{
			yield return new PreCastAction
			{
				action = delegate(LocalTargetInfo t, LocalTargetInfo d)
				{
					if (!this.parent.def.HasAreaOfEffect)
					{
						Pawn pawn = t.Pawn;
						if (pawn != null)
						{
							MoteMaker.MakeAttachedOverlay(pawn, ThingDefOf.Mote_PsycastSkipFlashEntry, Vector3.zero, 1f, -1f).detachAfterTicks = 5;
						}
						else
						{
							MoteMaker.MakeStaticMote(t.CenterVector3, this.parent.pawn.Map, ThingDefOf.Mote_PsycastSkipFlashEntry, 1f);
						}
						MoteMaker.MakeStaticMote(d.Cell, this.parent.pawn.Map, ThingDefOf.Mote_PsycastSkipInnerExit, 1f);
					}
					if (this.Props.destination != AbilityEffectDestination.RandomInRange)
					{
						MoteMaker.MakeStaticMote(d.Cell, this.parent.pawn.Map, ThingDefOf.Mote_PsycastSkipOuterRingExit, 1f);
					}
					if (!this.parent.def.HasAreaOfEffect)
					{
						SoundDefOf.Psycast_Skip_Entry.PlayOneShot(new TargetInfo(t.Cell, this.parent.pawn.Map, false));
						SoundDefOf.Psycast_Skip_Exit.PlayOneShot(new TargetInfo(d.Cell, this.parent.pawn.Map, false));
					}
				},
				ticksAwayFromCast = 5
			};
			yield break;
		}

		// Token: 0x06006CC7 RID: 27847 RVA: 0x00216420 File Offset: 0x00214620
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (target.HasThing)
			{
				base.Apply(target, dest);
				LocalTargetInfo destination = base.GetDestination(dest.IsValid ? dest : target);
				if (destination.IsValid)
				{
					Pawn pawn = this.parent.pawn;
					if (!this.parent.def.HasAreaOfEffect)
					{
						this.parent.AddEffecterToMaintain(EffecterDefOf.Skip_Entry.Spawn(target.Thing, pawn.Map, 1f), target.Thing.Position, 60);
					}
					else
					{
						this.parent.AddEffecterToMaintain(EffecterDefOf.Skip_EntryNoDelay.Spawn(target.Thing, pawn.Map, 1f), target.Thing.Position, 60);
					}
					if (this.Props.destination == AbilityEffectDestination.Selected)
					{
						this.parent.AddEffecterToMaintain(EffecterDefOf.Skip_Exit.Spawn(destination.Cell, pawn.Map, 1f), destination.Cell, 60);
					}
					else
					{
						this.parent.AddEffecterToMaintain(EffecterDefOf.Skip_ExitNoDelay.Spawn(destination.Cell, pawn.Map, 1f), destination.Cell, 60);
					}
					CompCanBeDormant compCanBeDormant = target.Thing.TryGetComp<CompCanBeDormant>();
					if (compCanBeDormant != null)
					{
						compCanBeDormant.WakeUp();
					}
					target.Thing.Position = destination.Cell;
					Pawn pawn2 = target.Thing as Pawn;
					if (pawn2 != null)
					{
						pawn2.stances.stunner.StunFor_NewTmp(this.Props.stunTicks.RandomInRange, this.parent.pawn, false, false);
						pawn2.Notify_Teleported(true, true);
					}
					if (this.Props.destClamorType != null)
					{
						GenClamor.DoClamor(pawn, target.Cell, (float)this.Props.destClamorRadius, this.Props.destClamorType);
					}
				}
			}
		}

		// Token: 0x06006CC8 RID: 27848 RVA: 0x00049FB5 File Offset: 0x000481B5
		public override bool CanHitTarget(LocalTargetInfo target)
		{
			return base.CanPlaceSelectedTargetAt(target) && base.CanHitTarget(target);
		}
	}
}
