using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001360 RID: 4960
	public class Targeter
	{
		// Token: 0x1700152C RID: 5420
		// (get) Token: 0x0600784F RID: 30799 RVA: 0x002A6604 File Offset: 0x002A4804
		public bool IsTargeting
		{
			get
			{
				return this.targetingSource != null || this.action != null;
			}
		}

		// Token: 0x06007850 RID: 30800 RVA: 0x002A661C File Offset: 0x002A481C
		public void BeginTargeting(ITargetingSource source, ITargetingSource parent = null)
		{
			if (source.Targetable)
			{
				this.targetingSource = source;
				this.targetingSourceAdditionalPawns = new List<Pawn>();
			}
			else
			{
				Verb getVerb = source.GetVerb;
				if (getVerb.verbProps.nonInterruptingSelfCast)
				{
					getVerb.TryStartCastOn(getVerb.Caster, false, true, false);
					return;
				}
				Job job = JobMaker.MakeJob(JobDefOf.UseVerbOnThing, getVerb.Caster);
				job.verbToUse = getVerb;
				source.CasterPawn.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false, false);
			}
			this.action = null;
			this.caster = null;
			this.targetParams = null;
			this.actionWhenFinished = null;
			this.mouseAttachment = null;
			this.targetingSourceParent = parent;
			this.needsStopTargetingCall = false;
		}

		// Token: 0x06007851 RID: 30801 RVA: 0x002A66E0 File Offset: 0x002A48E0
		public void BeginTargeting(TargetingParameters targetParams, Action<LocalTargetInfo> action, Pawn caster = null, Action actionWhenFinished = null, Texture2D mouseAttachment = null)
		{
			this.targetingSource = null;
			this.targetingSourceParent = null;
			this.targetingSourceAdditionalPawns = null;
			this.action = action;
			this.targetParams = targetParams;
			this.caster = caster;
			this.actionWhenFinished = actionWhenFinished;
			this.mouseAttachment = mouseAttachment;
			this.highlightAction = null;
			this.targetValidator = null;
			this.needsStopTargetingCall = false;
			this.playSoundOnAction = true;
		}

		// Token: 0x06007852 RID: 30802 RVA: 0x002A6744 File Offset: 0x002A4944
		public void BeginTargeting(TargetingParameters targetParams, Action<LocalTargetInfo> action, Action<LocalTargetInfo> highlightAction, Func<LocalTargetInfo, bool> targetValidator, Pawn caster = null, Action actionWhenFinished = null, Texture2D mouseAttachment = null, bool playSoundOnAction = true)
		{
			this.targetingSource = null;
			this.targetingSourceParent = null;
			this.targetingSourceAdditionalPawns = null;
			this.action = action;
			this.targetParams = targetParams;
			this.caster = caster;
			this.actionWhenFinished = actionWhenFinished;
			this.mouseAttachment = mouseAttachment;
			this.highlightAction = highlightAction;
			this.targetValidator = targetValidator;
			this.playSoundOnAction = playSoundOnAction;
			this.needsStopTargetingCall = false;
		}

		// Token: 0x06007853 RID: 30803 RVA: 0x002A67AC File Offset: 0x002A49AC
		public void BeginTargeting(TargetingParameters targetParams, ITargetingSource ability, Action<LocalTargetInfo> action, Action actionWhenFinished = null, Texture2D mouseAttachment = null)
		{
			this.targetingSource = null;
			this.targetingSourceParent = null;
			this.targetingSourceAdditionalPawns = null;
			this.action = action;
			this.actionWhenFinished = actionWhenFinished;
			this.caster = null;
			this.targetParams = targetParams;
			this.mouseAttachment = mouseAttachment;
			this.targetingSource = ability;
			this.highlightAction = null;
			this.targetValidator = null;
			this.needsStopTargetingCall = false;
			this.playSoundOnAction = true;
		}

		// Token: 0x06007854 RID: 30804 RVA: 0x002A6816 File Offset: 0x002A4A16
		public void StopTargeting()
		{
			if (this.actionWhenFinished != null)
			{
				Action action = this.actionWhenFinished;
				this.actionWhenFinished = null;
				action();
			}
			this.targetingSource = null;
			this.action = null;
			this.targetParams = null;
			this.highlightAction = null;
			this.targetValidator = null;
		}

		// Token: 0x06007855 RID: 30805 RVA: 0x002A6858 File Offset: 0x002A4A58
		public void ProcessInputEvents()
		{
			this.ConfirmStillValid();
			if (this.IsTargeting)
			{
				if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
				{
					LocalTargetInfo localTargetInfo = this.CurrentTargetUnderMouse(false);
					this.needsStopTargetingCall = true;
					if (this.targetingSource != null)
					{
						if (!this.targetingSource.ValidateTarget(localTargetInfo, true))
						{
							Event.current.Use();
							return;
						}
						this.OrderVerbForceTarget();
					}
					if (this.action != null)
					{
						if (this.targetValidator != null)
						{
							if (this.targetValidator(localTargetInfo))
							{
								this.action(localTargetInfo);
							}
							else
							{
								this.needsStopTargetingCall = false;
							}
						}
						else if (localTargetInfo.IsValid)
						{
							this.action(localTargetInfo);
						}
					}
					if (this.playSoundOnAction)
					{
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					}
					if (this.targetingSource != null)
					{
						if (this.targetingSource.DestinationSelector != null)
						{
							this.BeginTargeting(this.targetingSource.DestinationSelector, this.targetingSource);
						}
						else if (this.targetingSource.MultiSelect && Event.current.shift)
						{
							this.BeginTargeting(this.targetingSource, null);
						}
						else if (this.targetingSourceParent != null && this.targetingSourceParent.MultiSelect && Event.current.shift)
						{
							this.BeginTargeting(this.targetingSourceParent, null);
						}
					}
					if (this.needsStopTargetingCall)
					{
						this.StopTargeting();
					}
					Event.current.Use();
				}
				if ((Event.current.type == EventType.MouseDown && Event.current.button == 1) || KeyBindingDefOf.Cancel.KeyDownEvent)
				{
					SoundDefOf.CancelMode.PlayOneShotOnCamera(null);
					this.StopTargeting();
					Event.current.Use();
				}
			}
		}

		// Token: 0x06007856 RID: 30806 RVA: 0x002A6A04 File Offset: 0x002A4C04
		public void TargeterOnGUI()
		{
			if (this.targetingSource != null)
			{
				LocalTargetInfo target = this.CurrentTargetUnderMouse(true);
				this.targetingSource.OnGUI(target);
			}
			if (this.action != null)
			{
				GenUI.DrawMouseAttachment(this.mouseAttachment ?? TexCommand.Attack);
			}
		}

		// Token: 0x06007857 RID: 30807 RVA: 0x002A6A4C File Offset: 0x002A4C4C
		public void TargeterUpdate()
		{
			if (this.targetingSource != null)
			{
				this.targetingSource.DrawHighlight(this.CurrentTargetUnderMouse(true));
			}
			if (this.action != null)
			{
				LocalTargetInfo localTargetInfo = this.CurrentTargetUnderMouse(false);
				if (this.highlightAction != null)
				{
					this.highlightAction(localTargetInfo);
					return;
				}
				if (localTargetInfo.IsValid)
				{
					GenDraw.DrawTargetHighlight(localTargetInfo);
				}
			}
		}

		// Token: 0x06007858 RID: 30808 RVA: 0x002A6AA8 File Offset: 0x002A4CA8
		public bool IsPawnTargeting(Pawn p)
		{
			if (this.caster == p)
			{
				return true;
			}
			if (this.targetingSource != null && this.targetingSource.CasterIsPawn)
			{
				if (this.targetingSource.CasterPawn == p)
				{
					return true;
				}
				for (int i = 0; i < this.targetingSourceAdditionalPawns.Count; i++)
				{
					if (this.targetingSourceAdditionalPawns[i] == p)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06007859 RID: 30809 RVA: 0x002A6B10 File Offset: 0x002A4D10
		private void ConfirmStillValid()
		{
			if (this.caster != null && (this.caster.Map != Find.CurrentMap || this.caster.Destroyed || !Find.Selector.IsSelected(this.caster)))
			{
				this.StopTargeting();
			}
			if (this.targetingSource != null)
			{
				Selector selector = Find.Selector;
				CompAbilityEffect_WithDest compAbilityEffect_WithDest;
				if (this.targetingSource.Caster.Map != Find.CurrentMap || this.targetingSource.Caster.Destroyed || !selector.IsSelected(this.targetingSource.Caster) || (this.targetingSource.GetVerb != null && !this.targetingSource.GetVerb.Available()) || ((compAbilityEffect_WithDest = (this.targetingSource as CompAbilityEffect_WithDest)) != null && compAbilityEffect_WithDest.SelectedTargetInvalidated()))
				{
					this.StopTargeting();
					return;
				}
				for (int i = 0; i < this.targetingSourceAdditionalPawns.Count; i++)
				{
					if (this.targetingSourceAdditionalPawns[i].Destroyed || !selector.IsSelected(this.targetingSourceAdditionalPawns[i]))
					{
						this.StopTargeting();
						return;
					}
				}
			}
		}

		// Token: 0x0600785A RID: 30810 RVA: 0x002A6C2C File Offset: 0x002A4E2C
		private void OrderVerbForceTarget()
		{
			if (this.targetingSource.CasterIsPawn)
			{
				this.OrderPawnForceTarget(this.targetingSource);
				for (int i = 0; i < this.targetingSourceAdditionalPawns.Count; i++)
				{
					Verb targetingVerb = this.GetTargetingVerb(this.targetingSourceAdditionalPawns[i]);
					if (targetingVerb != null)
					{
						this.OrderPawnForceTarget(targetingVerb);
					}
				}
				return;
			}
			int numSelected = Find.Selector.NumSelected;
			List<object> selectedObjects = Find.Selector.SelectedObjects;
			for (int j = 0; j < numSelected; j++)
			{
				Building_Turret building_Turret = selectedObjects[j] as Building_Turret;
				if (building_Turret != null && building_Turret.Map == Find.CurrentMap)
				{
					LocalTargetInfo targ = this.CurrentTargetUnderMouse(true);
					building_Turret.OrderAttack(targ);
				}
			}
		}

		// Token: 0x0600785B RID: 30811 RVA: 0x002A6CE0 File Offset: 0x002A4EE0
		public void OrderPawnForceTarget(ITargetingSource targetingSource)
		{
			LocalTargetInfo target = this.CurrentTargetUnderMouse(true);
			if (!target.IsValid)
			{
				return;
			}
			targetingSource.OrderForceTarget(target);
		}

		// Token: 0x0600785C RID: 30812 RVA: 0x002A6D08 File Offset: 0x002A4F08
		private LocalTargetInfo CurrentTargetUnderMouse(bool mustBeHittableNowIfNotMelee)
		{
			if (!this.IsTargeting)
			{
				return LocalTargetInfo.Invalid;
			}
			TargetingParameters targetingParameters = (this.targetingSource != null) ? this.targetingSource.targetParams : this.targetParams;
			LocalTargetInfo localTargetInfo = GenUI.TargetsAtMouse(targetingParameters, false, this.targetingSource).FirstOrFallback(LocalTargetInfo.Invalid);
			if (localTargetInfo.IsValid && this.targetingSource != null)
			{
				if (mustBeHittableNowIfNotMelee && !(localTargetInfo.Thing is Pawn) && !this.targetingSource.IsMeleeAttack)
				{
					if (this.targetingSourceAdditionalPawns != null && this.targetingSourceAdditionalPawns.Any<Pawn>())
					{
						bool flag = false;
						for (int i = 0; i < this.targetingSourceAdditionalPawns.Count; i++)
						{
							Verb targetingVerb = this.GetTargetingVerb(this.targetingSourceAdditionalPawns[i]);
							if (targetingVerb != null && targetingVerb.CanHitTarget(localTargetInfo))
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							localTargetInfo = LocalTargetInfo.Invalid;
						}
					}
					else if (!this.targetingSource.CanHitTarget(localTargetInfo))
					{
						localTargetInfo = LocalTargetInfo.Invalid;
					}
				}
				if (localTargetInfo == this.targetingSource.Caster && !targetingParameters.canTargetSelf)
				{
					localTargetInfo = LocalTargetInfo.Invalid;
				}
			}
			return localTargetInfo;
		}

		// Token: 0x0600785D RID: 30813 RVA: 0x002A6E28 File Offset: 0x002A5028
		private Verb GetTargetingVerb(Pawn pawn)
		{
			Pawn_EquipmentTracker equipment = pawn.equipment;
			Verb verb = (equipment != null) ? equipment.AllEquipmentVerbs.FirstOrDefault(new Func<Verb, bool>(this.<GetTargetingVerb>g__SameVerb|27_0)) : null;
			if (verb != null)
			{
				return verb;
			}
			Pawn_ApparelTracker apparel = pawn.apparel;
			if (apparel == null)
			{
				return null;
			}
			return apparel.AllApparelVerbs.FirstOrDefault(new Func<Verb, bool>(this.<GetTargetingVerb>g__SameVerb|27_0));
		}

		// Token: 0x0600785F RID: 30815 RVA: 0x002A6E8F File Offset: 0x002A508F
		[CompilerGenerated]
		private bool <GetTargetingVerb>g__SameVerb|27_0(Verb x)
		{
			return x.verbProps == this.targetingSource.GetVerb.verbProps;
		}

		// Token: 0x040042D8 RID: 17112
		public ITargetingSource targetingSource;

		// Token: 0x040042D9 RID: 17113
		public ITargetingSource targetingSourceParent;

		// Token: 0x040042DA RID: 17114
		public List<Pawn> targetingSourceAdditionalPawns;

		// Token: 0x040042DB RID: 17115
		private Action<LocalTargetInfo> action;

		// Token: 0x040042DC RID: 17116
		private Pawn caster;

		// Token: 0x040042DD RID: 17117
		private TargetingParameters targetParams;

		// Token: 0x040042DE RID: 17118
		private Action actionWhenFinished;

		// Token: 0x040042DF RID: 17119
		private Texture2D mouseAttachment;

		// Token: 0x040042E0 RID: 17120
		private Action<LocalTargetInfo> highlightAction;

		// Token: 0x040042E1 RID: 17121
		private Func<LocalTargetInfo, bool> targetValidator;

		// Token: 0x040042E2 RID: 17122
		private bool playSoundOnAction = true;

		// Token: 0x040042E3 RID: 17123
		private bool needsStopTargetingCall;
	}
}
