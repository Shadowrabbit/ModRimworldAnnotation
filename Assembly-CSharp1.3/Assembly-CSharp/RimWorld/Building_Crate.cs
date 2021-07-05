using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001075 RID: 4213
	public class Building_Crate : Building_Casket
	{
		// Token: 0x17001114 RID: 4372
		// (get) Token: 0x060063FD RID: 25597 RVA: 0x001E6FD4 File Offset: 0x001E51D4
		public override int OpenTicks
		{
			get
			{
				return 100;
			}
		}

		// Token: 0x17001115 RID: 4373
		// (get) Token: 0x060063FF RID: 25599 RVA: 0x0021BF60 File Offset: 0x0021A160
		public override bool CanOpen
		{
			get
			{
				CompHackable comp = base.GetComp<CompHackable>();
				return (comp == null || comp.IsHacked) && base.CanOpen;
			}
		}

		// Token: 0x06006400 RID: 25600 RVA: 0x0021BF87 File Offset: 0x0021A187
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (!ModLister.CheckIdeology("Ancient security crate"))
			{
				return;
			}
			base.SpawnSetup(map, respawningAfterLoad);
		}

		// Token: 0x06006401 RID: 25601 RVA: 0x0021BF9E File Offset: 0x0021A19E
		public override bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
		{
			if (base.TryAcceptThing(thing, allowSpecialEffects))
			{
				base.BroadcastCompSignal("CrateContentsChanged");
				return true;
			}
			return false;
		}

		// Token: 0x06006402 RID: 25602 RVA: 0x0021BFB8 File Offset: 0x0021A1B8
		public override void EjectContents()
		{
			this.OccupiedRect();
			this.innerContainer.TryDropAll(base.Position, base.Map, ThingPlaceMode.Near, null, (IntVec3 c) => c.GetEdifice(base.Map) == null, true);
			this.contentsKnown = true;
			if (this.def.building.openingEffect != null)
			{
				Effecter effecter = this.def.building.openingEffect.Spawn();
				effecter.Trigger(new TargetInfo(base.Position, base.Map, false), null);
				effecter.Cleanup();
			}
			base.BroadcastCompSignal("CrateContentsChanged");
		}

		// Token: 0x06006403 RID: 25603 RVA: 0x0021C04F File Offset: 0x0021A24F
		protected override void ReceiveCompSignal(string signal)
		{
			base.ReceiveCompSignal(signal);
			if (signal == "Hackend")
			{
				this.Open();
			}
		}

		// Token: 0x06006404 RID: 25604 RVA: 0x0021C06B File Offset: 0x0021A26B
		public override void Open()
		{
			if (!this.CanOpen)
			{
				return;
			}
			base.Open();
		}

		// Token: 0x06006405 RID: 25605 RVA: 0x0021C07C File Offset: 0x0021A27C
		public override IEnumerable<FloatMenuOption> GetMultiSelectFloatMenuOptions(List<Pawn> selPawns)
		{
			foreach (FloatMenuOption floatMenuOption in base.GetMultiSelectFloatMenuOptions(selPawns))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			if (!this.CanOpen)
			{
				yield break;
			}
			Building_Crate.tmpAllowedPawns.Clear();
			for (int i = 0; i < selPawns.Count; i++)
			{
				if (selPawns[i].CanReach(this, PathEndMode.InteractionCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					Building_Crate.tmpAllowedPawns.Add(selPawns[i]);
				}
			}
			if (Building_Crate.tmpAllowedPawns.Count <= 0)
			{
				yield return new FloatMenuOption("CannotOpen".Translate(this) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			Building_Crate.tmpAllowedPawns.Clear();
			for (int j = 0; j < selPawns.Count; j++)
			{
				if (HackUtility.IsCapableOfHacking(selPawns[j]))
				{
					Building_Crate.tmpAllowedPawns.Add(selPawns[j]);
				}
			}
			if (Building_Crate.tmpAllowedPawns.Count <= 0)
			{
				yield return new FloatMenuOption("CannotOpen".Translate(this.Label) + ": " + "IncapableOfHacking".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			Building_Crate.tmpAllowedPawns.Clear();
			for (int k = 0; k < selPawns.Count; k++)
			{
				if (HackUtility.IsCapableOfHacking(selPawns[k]) && selPawns[k].CanReach(this, PathEndMode.InteractionCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					Building_Crate.tmpAllowedPawns.Add(selPawns[k]);
				}
			}
			if (Building_Crate.tmpAllowedPawns.Count > 0)
			{
				yield return new FloatMenuOption("Open".Translate(this), delegate()
				{
					Building_Crate.tmpAllowedPawns[0].jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.Open, this), new JobTag?(JobTag.Misc), false);
					for (int l = 1; l < Building_Crate.tmpAllowedPawns.Count; l++)
					{
						FloatMenuMakerMap.PawnGotoAction(base.Position, Building_Crate.tmpAllowedPawns[l], RCellFinder.BestOrderedGotoDestNear(base.Position, Building_Crate.tmpAllowedPawns[l], null));
					}
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			yield break;
			yield break;
		}

		// Token: 0x0400387E RID: 14462
		private static IntVec3 ejectPositionOffset = new IntVec3(1, 0, 0);

		// Token: 0x0400387F RID: 14463
		public const string CrateContentsChanged = "CrateContentsChanged";

		// Token: 0x04003880 RID: 14464
		private static List<Pawn> tmpAllowedPawns = new List<Pawn>();
	}
}
