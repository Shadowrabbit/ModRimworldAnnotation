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
	// Token: 0x02001B2E RID: 6958
	public class Targeter
	{
		// Token: 0x1700182E RID: 6190
		// (get) Token: 0x06009938 RID: 39224 RVA: 0x00066225 File Offset: 0x00064425
		public bool IsTargeting
		{
			get
			{
				return this.targetingSource != null || this.action != null;
			}
		}

		// Token: 0x06009939 RID: 39225 RVA: 0x002D00E0 File Offset: 0x002CE2E0
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
					getVerb.TryStartCastOn(getVerb.Caster, false, true);
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

		// Token: 0x0600993A RID: 39226 RVA: 0x002D01A4 File Offset: 0x002CE3A4
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
		}

		// Token: 0x0600993B RID: 39227 RVA: 0x002D0200 File Offset: 0x002CE400
		public void BeginTargeting(TargetingParameters targetParams, Action<LocalTargetInfo> action, Action<LocalTargetInfo> highlightAction, Func<LocalTargetInfo, bool> targetValidator, Pawn caster = null, Action actionWhenFinished = null, Texture2D mouseAttachment = null)
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
			this.needsStopTargetingCall = false;
		}

		// Token: 0x0600993C RID: 39228 RVA: 0x002D0260 File Offset: 0x002CE460
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
		}

		// Token: 0x0600993D RID: 39229 RVA: 0x0006623A File Offset: 0x0006443A
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

		// Token: 0x0600993E RID: 39230 RVA: 0x002D02C4 File Offset: 0x002CE4C4
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
						if (!this.targetingSource.ValidateTarget(localTargetInfo))
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
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
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

		// Token: 0x0600993F RID: 39231 RVA: 0x002D0468 File Offset: 0x002CE668
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

		// Token: 0x06009940 RID: 39232 RVA: 0x002D04B0 File Offset: 0x002CE6B0
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

		// Token: 0x06009941 RID: 39233 RVA: 0x002D050C File Offset: 0x002CE70C
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

		// Token: 0x06009942 RID: 39234 RVA: 0x002D0574 File Offset: 0x002CE774
		private void ConfirmStillValid()
		{
			if (this.caster != null && (this.caster.Map != Find.CurrentMap || this.caster.Destroyed || !Find.Selector.IsSelected(this.caster)))
			{
				this.StopTargeting();
			}
			if (this.targetingSource != null)
			{
				Selector selector = Find.Selector;
				if (this.targetingSource.Caster.Map != Find.CurrentMap || this.targetingSource.Caster.Destroyed || !selector.IsSelected(this.targetingSource.Caster) || (this.targetingSource.GetVerb != null && !this.targetingSource.GetVerb.Available()))
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

		// Token: 0x06009943 RID: 39235 RVA: 0x002D0678 File Offset: 0x002CE878
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

		// Token: 0x06009944 RID: 39236 RVA: 0x002D072C File Offset: 0x002CE92C
		public void OrderPawnForceTarget(ITargetingSource targetingSource)
		{
			LocalTargetInfo target = this.CurrentTargetUnderMouse(true);
			if (!target.IsValid)
			{
				return;
			}
			targetingSource.OrderForceTarget(target);
		}

		// Token: 0x06009945 RID: 39237 RVA: 0x002D0754 File Offset: 0x002CE954
		private LocalTargetInfo CurrentTargetUnderMouse(bool mustBeHittableNowIfNotMelee)
		{
			if (!this.IsTargeting)
			{
				return LocalTargetInfo.Invalid;
			}
			TargetingParameters targetingParameters = (this.targetingSource != null) ? this.targetingSource.targetParams : this.targetParams;
			LocalTargetInfo localTargetInfo = GenUI.TargetsAtMouse_NewTemp(targetingParameters, false, this.targetingSource).FirstOrFallback(LocalTargetInfo.Invalid);
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

		// Token: 0x06009946 RID: 39238 RVA: 0x002D0874 File Offset: 0x002CEA74
		private Verb GetTargetingVerb(Pawn pawn)
		{
			Pawn_EquipmentTracker equipment = pawn.equipment;
			Verb verb = (equipment != null) ? equipment.AllEquipmentVerbs.FirstOrDefault(new Func<Verb, bool>(this.<GetTargetingVerb>g__SameVerb|26_0)) : null;
			if (verb != null)
			{
				return verb;
			}
			Pawn_ApparelTracker apparel = pawn.apparel;
			if (apparel == null)
			{
				return null;
			}
			return apparel.AllApparelVerbs.FirstOrDefault(new Func<Verb, bool>(this.<GetTargetingVerb>g__SameVerb|26_0));
		}

		// Token: 0x06009948 RID: 39240 RVA: 0x00066279 File Offset: 0x00064479
		[CompilerGenerated]
		private bool <GetTargetingVerb>g__SameVerb|26_0(Verb x)
		{
			return x.verbProps == this.targetingSource.GetVerb.verbProps;
		}

		// Token: 0x040061E7 RID: 25063
		public ITargetingSource targetingSource;

		// Token: 0x040061E8 RID: 25064
		public ITargetingSource targetingSourceParent;

		// Token: 0x040061E9 RID: 25065
		public List<Pawn> targetingSourceAdditionalPawns;

		// Token: 0x040061EA RID: 25066
		private Action<LocalTargetInfo> action;

		// Token: 0x040061EB RID: 25067
		private Pawn caster;

		// Token: 0x040061EC RID: 25068
		private TargetingParameters targetParams;

		// Token: 0x040061ED RID: 25069
		private Action actionWhenFinished;

		// Token: 0x040061EE RID: 25070
		private Texture2D mouseAttachment;

		// Token: 0x040061EF RID: 25071
		private Action<LocalTargetInfo> highlightAction;

		// Token: 0x040061F0 RID: 25072
		private Func<LocalTargetInfo, bool> targetValidator;

		// Token: 0x040061F1 RID: 25073
		private bool needsStopTargetingCall;
	}
}
