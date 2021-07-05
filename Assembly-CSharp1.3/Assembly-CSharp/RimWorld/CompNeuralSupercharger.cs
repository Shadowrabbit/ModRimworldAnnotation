using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009F0 RID: 2544
	[StaticConstructorOnStartup]
	public class CompNeuralSupercharger : CompRechargeable
	{
		// Token: 0x17000AF0 RID: 2800
		// (get) Token: 0x06003EB1 RID: 16049 RVA: 0x00156F34 File Offset: 0x00155134
		private static Texture2D ColonistOnlyCommandTex
		{
			get
			{
				if (CompNeuralSupercharger.colonistOnlyCommandTex == null)
				{
					CompNeuralSupercharger.colonistOnlyCommandTex = ContentFinder<Texture2D>.Get("UI/Gizmos/NeuralSupercharger_AllowGuests", true);
				}
				return CompNeuralSupercharger.colonistOnlyCommandTex;
			}
		}

		// Token: 0x17000AF1 RID: 2801
		// (get) Token: 0x06003EB2 RID: 16050 RVA: 0x00156F58 File Offset: 0x00155158
		private CompProperties_NeuralSupercharger Props
		{
			get
			{
				return (CompProperties_NeuralSupercharger)this.props;
			}
		}

		// Token: 0x06003EB3 RID: 16051 RVA: 0x00156F68 File Offset: 0x00155168
		public bool CanAutoUse(Pawn pawn)
		{
			if (!this.allowGuests && pawn.IsQuestLodger())
			{
				return false;
			}
			switch (this.autoUseMode)
			{
			case CompNeuralSupercharger.AutoUseMode.NoAutoUse:
				return false;
			case CompNeuralSupercharger.AutoUseMode.AutoUseWithDesire:
				return pawn.Ideo != null && pawn.Ideo.HasPrecept(PreceptDefOf.NeuralSupercharge_Preferred);
			case CompNeuralSupercharger.AutoUseMode.AutoUseForEveryone:
				return true;
			default:
				Log.Error(string.Format("Unknown auto use mode: {0}", this.autoUseMode));
				return false;
			}
		}

		// Token: 0x06003EB4 RID: 16052 RVA: 0x00156FDC File Offset: 0x001551DC
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.autoUseMode = CompNeuralSupercharger.AutoUseMode.AutoUseWithDesire;
		}

		// Token: 0x06003EB5 RID: 16053 RVA: 0x00156FEC File Offset: 0x001551EC
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<CompNeuralSupercharger.AutoUseMode>(ref this.autoUseMode, "autoUseMode", CompNeuralSupercharger.AutoUseMode.AutoUseWithDesire, false);
			Scribe_Values.Look<bool>(ref this.allowGuests, "allowGuests", false, false);
		}

		// Token: 0x06003EB6 RID: 16054 RVA: 0x00157018 File Offset: 0x00155218
		public override void CompTick()
		{
			base.CompTick();
			if (this.Props.effectCharged != null && this.parent.Spawned)
			{
				if (base.Charged)
				{
					if (this.effecterCharged == null)
					{
						this.effecterCharged = this.Props.effectCharged.Spawn();
						this.effecterCharged.Trigger(this.parent, new TargetInfo(this.parent.InteractionCell, this.parent.Map, false));
					}
					this.effecterCharged.EffectTick(this.parent, new TargetInfo(this.parent.InteractionCell, this.parent.Map, false));
				}
				if (!base.Charged && this.effecterCharged != null)
				{
					this.effecterCharged.Cleanup();
					this.effecterCharged = null;
				}
			}
		}

		// Token: 0x06003EB7 RID: 16055 RVA: 0x001570FA File Offset: 0x001552FA
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			Effecter effecter = this.effecterCharged;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.effecterCharged = null;
		}

		// Token: 0x06003EB8 RID: 16056 RVA: 0x0015711B File Offset: 0x0015531B
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			if (!ModLister.CheckIdeology("Neural supercharger"))
			{
				yield break;
			}
			if (!base.Charged)
			{
				yield return new FloatMenuOption(this.Props.jobString + " (" + "NeuralSuperchargeNotReady".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			if (selPawn.CurJob != null && selPawn.CurJob.def == JobDefOf.GetNeuralSupercharge && selPawn.CurJob.targetA.Thing == this.parent)
			{
				yield return new FloatMenuOption(this.Props.jobString + " (" + "NeuralSuperchargeAlreadyGetting".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(this.Props.jobString, delegate()
			{
				Job job = JobMaker.MakeJob(JobDefOf.GetNeuralSupercharge, this.parent);
				selPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), selPawn, this.parent, "ReservedBy");
			yield break;
		}

		// Token: 0x06003EB9 RID: 16057 RVA: 0x00157132 File Offset: 0x00155332
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			yield return new Command_SetNeuralSuperchargerAutoUse(this);
			yield return new Command_Toggle
			{
				defaultLabel = "CommandNeuralSuperchargerAllowGuests".Translate(),
				defaultDesc = "CommandNeuralSuperchargerAllowGuestsDescription".Translate(),
				icon = CompNeuralSupercharger.ColonistOnlyCommandTex,
				isActive = (() => this.allowGuests),
				toggleAction = delegate()
				{
					this.allowGuests = !this.allowGuests;
				},
				activateSound = SoundDefOf.Tick_Tiny
			};
			yield break;
			yield break;
		}

		// Token: 0x04002188 RID: 8584
		private static Texture2D colonistOnlyCommandTex;

		// Token: 0x04002189 RID: 8585
		public CompNeuralSupercharger.AutoUseMode autoUseMode = CompNeuralSupercharger.AutoUseMode.AutoUseWithDesire;

		// Token: 0x0400218A RID: 8586
		public bool allowGuests;

		// Token: 0x0400218B RID: 8587
		private Effecter effecterCharged;

		// Token: 0x02001FDC RID: 8156
		public enum AutoUseMode
		{
			// Token: 0x040079CF RID: 31183
			NoAutoUse,
			// Token: 0x040079D0 RID: 31184
			AutoUseWithDesire,
			// Token: 0x040079D1 RID: 31185
			AutoUseForEveryone
		}
	}
}
