using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001180 RID: 4480
	public class CompReloadable : ThingComp, IVerbOwner
	{
		// Token: 0x17001294 RID: 4756
		// (get) Token: 0x06006BBE RID: 27582 RVA: 0x00242886 File Offset: 0x00240A86
		public CompProperties_Reloadable Props
		{
			get
			{
				return this.props as CompProperties_Reloadable;
			}
		}

		// Token: 0x17001295 RID: 4757
		// (get) Token: 0x06006BBF RID: 27583 RVA: 0x00242893 File Offset: 0x00240A93
		public int RemainingCharges
		{
			get
			{
				return this.remainingCharges;
			}
		}

		// Token: 0x17001296 RID: 4758
		// (get) Token: 0x06006BC0 RID: 27584 RVA: 0x0024289B File Offset: 0x00240A9B
		public int MaxCharges
		{
			get
			{
				return this.Props.maxCharges;
			}
		}

		// Token: 0x17001297 RID: 4759
		// (get) Token: 0x06006BC1 RID: 27585 RVA: 0x002428A8 File Offset: 0x00240AA8
		public ThingDef AmmoDef
		{
			get
			{
				return this.Props.ammoDef;
			}
		}

		// Token: 0x17001298 RID: 4760
		// (get) Token: 0x06006BC2 RID: 27586 RVA: 0x002428B5 File Offset: 0x00240AB5
		public bool CanBeUsed
		{
			get
			{
				return this.remainingCharges > 0;
			}
		}

		// Token: 0x17001299 RID: 4761
		// (get) Token: 0x06006BC3 RID: 27587 RVA: 0x002428C0 File Offset: 0x00240AC0
		public Pawn Wearer
		{
			get
			{
				return ReloadableUtility.WearerOf(this);
			}
		}

		// Token: 0x1700129A RID: 4762
		// (get) Token: 0x06006BC4 RID: 27588 RVA: 0x00099807 File Offset: 0x00097A07
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return this.parent.def.Verbs;
			}
		}

		// Token: 0x1700129B RID: 4763
		// (get) Token: 0x06006BC5 RID: 27589 RVA: 0x00099819 File Offset: 0x00097A19
		public List<Tool> Tools
		{
			get
			{
				return this.parent.def.tools;
			}
		}

		// Token: 0x1700129C RID: 4764
		// (get) Token: 0x06006BC6 RID: 27590 RVA: 0x001A4583 File Offset: 0x001A2783
		public ImplementOwnerTypeDef ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.NativeVerb;
			}
		}

		// Token: 0x1700129D RID: 4765
		// (get) Token: 0x06006BC7 RID: 27591 RVA: 0x002428C8 File Offset: 0x00240AC8
		public Thing ConstantCaster
		{
			get
			{
				return this.Wearer;
			}
		}

		// Token: 0x06006BC8 RID: 27592 RVA: 0x002428D0 File Offset: 0x00240AD0
		public string UniqueVerbOwnerID()
		{
			return "Reloadable_" + this.parent.ThingID;
		}

		// Token: 0x06006BC9 RID: 27593 RVA: 0x002428E7 File Offset: 0x00240AE7
		public bool VerbsStillUsableBy(Pawn p)
		{
			return this.Wearer == p;
		}

		// Token: 0x1700129E RID: 4766
		// (get) Token: 0x06006BCA RID: 27594 RVA: 0x002428F2 File Offset: 0x00240AF2
		public VerbTracker VerbTracker
		{
			get
			{
				if (this.verbTracker == null)
				{
					this.verbTracker = new VerbTracker(this);
				}
				return this.verbTracker;
			}
		}

		// Token: 0x1700129F RID: 4767
		// (get) Token: 0x06006BCB RID: 27595 RVA: 0x0024290E File Offset: 0x00240B0E
		public string LabelRemaining
		{
			get
			{
				return string.Format("{0} / {1}", this.RemainingCharges, this.MaxCharges);
			}
		}

		// Token: 0x170012A0 RID: 4768
		// (get) Token: 0x06006BCC RID: 27596 RVA: 0x00242930 File Offset: 0x00240B30
		public List<Verb> AllVerbs
		{
			get
			{
				return this.VerbTracker.AllVerbs;
			}
		}

		// Token: 0x06006BCD RID: 27597 RVA: 0x0024293D File Offset: 0x00240B3D
		public override void PostPostMake()
		{
			base.PostPostMake();
			this.remainingCharges = this.MaxCharges;
		}

		// Token: 0x06006BCE RID: 27598 RVA: 0x00242951 File Offset: 0x00240B51
		public override string CompInspectStringExtra()
		{
			return "ChargesRemaining".Translate(this.Props.ChargeNounArgument) + ": " + this.LabelRemaining;
		}

		// Token: 0x06006BCF RID: 27599 RVA: 0x00242982 File Offset: 0x00240B82
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			IEnumerable<StatDrawEntry> enumerable = base.SpecialDisplayStats();
			if (enumerable != null)
			{
				foreach (StatDrawEntry statDrawEntry in enumerable)
				{
					yield return statDrawEntry;
				}
				IEnumerator<StatDrawEntry> enumerator = null;
			}
			yield return new StatDrawEntry(StatCategoryDefOf.Apparel, "Stat_Thing_ReloadChargesRemaining_Name".Translate(this.Props.ChargeNounArgument), this.LabelRemaining, "Stat_Thing_ReloadChargesRemaining_Desc".Translate(this.Props.ChargeNounArgument), 2749, null, null, false);
			yield break;
			yield break;
		}

		// Token: 0x06006BD0 RID: 27600 RVA: 0x00242994 File Offset: 0x00240B94
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.remainingCharges, "remainingCharges", -999, false);
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
			{
				this
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.remainingCharges == -999)
			{
				this.remainingCharges = this.MaxCharges;
			}
		}

		// Token: 0x06006BD1 RID: 27601 RVA: 0x002429F8 File Offset: 0x00240BF8
		public override IEnumerable<Gizmo> CompGetWornGizmosExtra()
		{
			foreach (Gizmo gizmo in base.CompGetWornGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			bool drafted = this.Wearer.Drafted;
			if ((drafted && !this.Props.displayGizmoWhileDrafted) || (!drafted && !this.Props.displayGizmoWhileUndrafted))
			{
				yield break;
			}
			ThingWithComps gear = this.parent;
			foreach (Verb verb in this.VerbTracker.AllVerbs)
			{
				if (verb.verbProps.hasStandardCommand)
				{
					yield return this.CreateVerbTargetCommand(gear, verb);
				}
			}
			List<Verb>.Enumerator enumerator2 = default(List<Verb>.Enumerator);
			if (Prefs.DevMode)
			{
				yield return new Command_Action
				{
					defaultLabel = "Debug: Reload to full",
					action = delegate()
					{
						this.remainingCharges = this.MaxCharges;
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x06006BD2 RID: 27602 RVA: 0x00242A08 File Offset: 0x00240C08
		private Command_Reloadable CreateVerbTargetCommand(Thing gear, Verb verb)
		{
			Command_Reloadable command_Reloadable = new Command_Reloadable(this);
			command_Reloadable.defaultDesc = gear.def.description;
			command_Reloadable.hotKey = this.Props.hotKey;
			command_Reloadable.defaultLabel = verb.verbProps.label;
			command_Reloadable.verb = verb;
			if (verb.verbProps.defaultProjectile != null && verb.verbProps.commandIcon == null)
			{
				command_Reloadable.icon = verb.verbProps.defaultProjectile.uiIcon;
				command_Reloadable.iconAngle = verb.verbProps.defaultProjectile.uiIconAngle;
				command_Reloadable.iconOffset = verb.verbProps.defaultProjectile.uiIconOffset;
				command_Reloadable.overrideColor = new Color?(verb.verbProps.defaultProjectile.graphicData.color);
			}
			else
			{
				command_Reloadable.icon = ((verb.UIIcon != BaseContent.BadTex) ? verb.UIIcon : gear.def.uiIcon);
				command_Reloadable.iconAngle = gear.def.uiIconAngle;
				command_Reloadable.iconOffset = gear.def.uiIconOffset;
				command_Reloadable.defaultIconColor = gear.DrawColor;
			}
			if (!this.Wearer.IsColonistPlayerControlled)
			{
				command_Reloadable.Disable(null);
			}
			else if (verb.verbProps.violent && this.Wearer.WorkTagIsDisabled(WorkTags.Violent))
			{
				command_Reloadable.Disable("IsIncapableOfViolenceLower".Translate(this.Wearer.LabelShort, this.Wearer).CapitalizeFirst() + ".");
			}
			else if (!this.CanBeUsed)
			{
				command_Reloadable.Disable(this.DisabledReason(this.MinAmmoNeeded(false), this.MaxAmmoNeeded(false)));
			}
			return command_Reloadable;
		}

		// Token: 0x06006BD3 RID: 27603 RVA: 0x00242BC8 File Offset: 0x00240DC8
		public string DisabledReason(int minNeeded, int maxNeeded)
		{
			string result;
			if (this.AmmoDef == null)
			{
				result = "CommandReload_NoCharges".Translate(this.Props.ChargeNounArgument);
			}
			else
			{
				string arg;
				if (this.Props.ammoCountToRefill != 0)
				{
					arg = this.Props.ammoCountToRefill.ToString();
				}
				else
				{
					arg = ((minNeeded == maxNeeded) ? minNeeded.ToString() : string.Format("{0}-{1}", minNeeded, maxNeeded));
				}
				result = "CommandReload_NoAmmo".Translate(this.Props.ChargeNounArgument, this.AmmoDef.Named("AMMO"), arg.Named("COUNT"));
			}
			return result;
		}

		// Token: 0x06006BD4 RID: 27604 RVA: 0x00242C78 File Offset: 0x00240E78
		public bool NeedsReload(bool allowForcedReload)
		{
			if (this.AmmoDef == null)
			{
				return false;
			}
			if (this.Props.ammoCountToRefill == 0)
			{
				return this.RemainingCharges != this.MaxCharges;
			}
			if (!allowForcedReload)
			{
				return this.remainingCharges == 0;
			}
			return this.RemainingCharges != this.MaxCharges;
		}

		// Token: 0x06006BD5 RID: 27605 RVA: 0x00242CCC File Offset: 0x00240ECC
		public void ReloadFrom(Thing ammo)
		{
			if (!this.NeedsReload(true))
			{
				return;
			}
			if (this.Props.ammoCountToRefill != 0)
			{
				if (ammo.stackCount < this.Props.ammoCountToRefill)
				{
					return;
				}
				ammo.SplitOff(this.Props.ammoCountToRefill).Destroy(DestroyMode.Vanish);
				this.remainingCharges = this.MaxCharges;
			}
			else
			{
				if (ammo.stackCount < this.Props.ammoCountPerCharge)
				{
					return;
				}
				int num = Mathf.Clamp(ammo.stackCount / this.Props.ammoCountPerCharge, 0, this.MaxCharges - this.RemainingCharges);
				ammo.SplitOff(num * this.Props.ammoCountPerCharge).Destroy(DestroyMode.Vanish);
				this.remainingCharges += num;
			}
			if (this.Props.soundReload != null)
			{
				this.Props.soundReload.PlayOneShot(new TargetInfo(this.Wearer.Position, this.Wearer.Map, false));
			}
		}

		// Token: 0x06006BD6 RID: 27606 RVA: 0x00242DC8 File Offset: 0x00240FC8
		public int MinAmmoNeeded(bool allowForcedReload)
		{
			if (!this.NeedsReload(allowForcedReload))
			{
				return 0;
			}
			if (this.Props.ammoCountToRefill != 0)
			{
				return this.Props.ammoCountToRefill;
			}
			return this.Props.ammoCountPerCharge;
		}

		// Token: 0x06006BD7 RID: 27607 RVA: 0x00242DF9 File Offset: 0x00240FF9
		public int MaxAmmoNeeded(bool allowForcedReload)
		{
			if (!this.NeedsReload(allowForcedReload))
			{
				return 0;
			}
			if (this.Props.ammoCountToRefill != 0)
			{
				return this.Props.ammoCountToRefill;
			}
			return this.Props.ammoCountPerCharge * (this.MaxCharges - this.RemainingCharges);
		}

		// Token: 0x06006BD8 RID: 27608 RVA: 0x00242E38 File Offset: 0x00241038
		public int MaxAmmoAmount()
		{
			if (this.AmmoDef == null)
			{
				return 0;
			}
			if (this.Props.ammoCountToRefill == 0)
			{
				return this.Props.ammoCountPerCharge * this.MaxCharges;
			}
			return this.Props.ammoCountToRefill;
		}

		// Token: 0x06006BD9 RID: 27609 RVA: 0x00242E70 File Offset: 0x00241070
		public void UsedOnce()
		{
			if (this.remainingCharges > 0)
			{
				this.remainingCharges--;
			}
			if (this.Props.destroyOnEmpty && this.remainingCharges == 0 && !this.parent.Destroyed)
			{
				this.parent.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x04003C07 RID: 15367
		private int remainingCharges;

		// Token: 0x04003C08 RID: 15368
		private VerbTracker verbTracker;
	}
}
