using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000D74 RID: 3444
	public class CompAbilityEffect_Teleport : CompAbilityEffect_WithDest
	{
		// Token: 0x17000DD4 RID: 3540
		// (get) Token: 0x06004FDD RID: 20445 RVA: 0x001AB62F File Offset: 0x001A982F
		public new CompProperties_AbilityTeleport Props
		{
			get
			{
				return (CompProperties_AbilityTeleport)this.props;
			}
		}

		// Token: 0x06004FDE RID: 20446 RVA: 0x001AB63C File Offset: 0x001A983C
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
							FleckCreationData dataAttachedOverlay = FleckMaker.GetDataAttachedOverlay(pawn, FleckDefOf.PsycastSkipFlashEntry, Vector3.zero, 1f, -1f);
							dataAttachedOverlay.link.detachAfterTicks = 5;
							pawn.Map.flecks.CreateFleck(dataAttachedOverlay);
						}
						else
						{
							FleckMaker.Static(t.CenterVector3, this.parent.pawn.Map, FleckDefOf.PsycastSkipFlashEntry, 1f);
						}
						FleckMaker.Static(d.Cell, this.parent.pawn.Map, FleckDefOf.PsycastSkipInnerExit, 1f);
					}
					if (this.Props.destination != AbilityEffectDestination.RandomInRange)
					{
						FleckMaker.Static(d.Cell, this.parent.pawn.Map, FleckDefOf.PsycastSkipOuterRingExit, 1f);
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

		// Token: 0x06004FDF RID: 20447 RVA: 0x001AB64C File Offset: 0x001A984C
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
						this.parent.AddEffecterToMaintain(EffecterDefOf.Skip_Entry.Spawn(target.Thing, pawn.Map, 1f), target.Thing.Position, 60, null);
					}
					else
					{
						this.parent.AddEffecterToMaintain(EffecterDefOf.Skip_EntryNoDelay.Spawn(target.Thing, pawn.Map, 1f), target.Thing.Position, 60, null);
					}
					if (this.Props.destination == AbilityEffectDestination.Selected)
					{
						this.parent.AddEffecterToMaintain(EffecterDefOf.Skip_Exit.Spawn(destination.Cell, pawn.Map, 1f), destination.Cell, 60, null);
					}
					else
					{
						this.parent.AddEffecterToMaintain(EffecterDefOf.Skip_ExitNoDelay.Spawn(destination.Cell, pawn.Map, 1f), destination.Cell, 60, null);
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
						pawn2.stances.stunner.StunFor(this.Props.stunTicks.RandomInRange, this.parent.pawn, false, false);
						pawn2.Notify_Teleported(true, true);
					}
					if (this.Props.destClamorType != null)
					{
						GenClamor.DoClamor(pawn, target.Cell, (float)this.Props.destClamorRadius, this.Props.destClamorType);
					}
				}
			}
		}

		// Token: 0x06004FE0 RID: 20448 RVA: 0x001AB82B File Offset: 0x001A9A2B
		public override bool CanHitTarget(LocalTargetInfo target)
		{
			return base.CanPlaceSelectedTargetAt(target) && base.CanHitTarget(target);
		}
	}
}
