using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001823 RID: 6179
	public class CompReloadable : ThingComp, IVerbOwner
	{
		// Token: 0x17001562 RID: 5474
		// (get) Token: 0x060088DD RID: 35037 RVA: 0x0005BEA0 File Offset: 0x0005A0A0
		public CompProperties_Reloadable Props
		{
			get
			{
				return this.props as CompProperties_Reloadable;
			}
		}

		// Token: 0x17001563 RID: 5475
		// (get) Token: 0x060088DE RID: 35038 RVA: 0x0005BEAD File Offset: 0x0005A0AD
		public int RemainingCharges
		{
			get
			{
				return this.remainingCharges;
			}
		}

		// Token: 0x17001564 RID: 5476
		// (get) Token: 0x060088DF RID: 35039 RVA: 0x0005BEB5 File Offset: 0x0005A0B5
		public int MaxCharges
		{
			get
			{
				return this.Props.maxCharges;
			}
		}

		// Token: 0x17001565 RID: 5477
		// (get) Token: 0x060088E0 RID: 35040 RVA: 0x0005BEC2 File Offset: 0x0005A0C2
		public ThingDef AmmoDef
		{
			get
			{
				return this.Props.ammoDef;
			}
		}

		// Token: 0x17001566 RID: 5478
		// (get) Token: 0x060088E1 RID: 35041 RVA: 0x0005BECF File Offset: 0x0005A0CF
		public bool CanBeUsed
		{
			get
			{
				return this.remainingCharges > 0;
			}
		}

		// Token: 0x17001567 RID: 5479
		// (get) Token: 0x060088E2 RID: 35042 RVA: 0x0005BEDA File Offset: 0x0005A0DA
		public Pawn Wearer
		{
			get
			{
				return ReloadableUtility.WearerOf(this);
			}
		}

		// Token: 0x17001568 RID: 5480
		// (get) Token: 0x060088E3 RID: 35043 RVA: 0x0001D3E0 File Offset: 0x0001B5E0
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return this.parent.def.Verbs;
			}
		}

		// Token: 0x17001569 RID: 5481
		// (get) Token: 0x060088E4 RID: 35044 RVA: 0x0001D3F2 File Offset: 0x0001B5F2
		public List<Tool> Tools
		{
			get
			{
				return this.parent.def.tools;
			}
		}

		// Token: 0x1700156A RID: 5482
		// (get) Token: 0x060088E5 RID: 35045 RVA: 0x0004935B File Offset: 0x0004755B
		public ImplementOwnerTypeDef ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.NativeVerb;
			}
		}

		// Token: 0x1700156B RID: 5483
		// (get) Token: 0x060088E6 RID: 35046 RVA: 0x0005BEE2 File Offset: 0x0005A0E2
		public Thing ConstantCaster
		{
			get
			{
				return this.Wearer;
			}
		}

		// Token: 0x060088E7 RID: 35047 RVA: 0x0005BEEA File Offset: 0x0005A0EA
		public string UniqueVerbOwnerID()
		{
			return "Reloadable_" + this.parent.ThingID;
		}

		// Token: 0x060088E8 RID: 35048 RVA: 0x0005BF01 File Offset: 0x0005A101
		public bool VerbsStillUsableBy(Pawn p)
		{
			return this.Wearer == p;
		}

		// Token: 0x1700156C RID: 5484
		// (get) Token: 0x060088E9 RID: 35049 RVA: 0x0005BF0C File Offset: 0x0005A10C
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

		// Token: 0x1700156D RID: 5485
		// (get) Token: 0x060088EA RID: 35050 RVA: 0x0005BF28 File Offset: 0x0005A128
		public string LabelRemaining
		{
			get
			{
				return string.Format("{0} / {1}", this.RemainingCharges, this.MaxCharges);
			}
		}

		// Token: 0x1700156E RID: 5486
		// (get) Token: 0x060088EB RID: 35051 RVA: 0x0005BF4A File Offset: 0x0005A14A
		public List<Verb> AllVerbs
		{
			get
			{
				return this.VerbTracker.AllVerbs;
			}
		}

		// Token: 0x060088EC RID: 35052 RVA: 0x0005BF57 File Offset: 0x0005A157
		public override void PostPostMake()
		{
			base.PostPostMake();
			this.remainingCharges = this.MaxCharges;
		}

		// Token: 0x060088ED RID: 35053 RVA: 0x0005BF6B File Offset: 0x0005A16B
		public override string CompInspectStringExtra()
		{
			return "ChargesRemaining".Translate(this.Props.ChargeNounArgument) + ": " + this.LabelRemaining;
		}

		// Token: 0x060088EE RID: 35054 RVA: 0x0005BF9C File Offset: 0x0005A19C
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

		// Token: 0x060088EF RID: 35055 RVA: 0x002807A4 File Offset: 0x0027E9A4
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

		// Token: 0x060088F0 RID: 35056 RVA: 0x0005BFAC File Offset: 0x0005A1AC
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

		// Token: 0x060088F1 RID: 35057 RVA: 0x00280808 File Offset: 0x0027EA08
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

		// Token: 0x060088F2 RID: 35058 RVA: 0x002809C8 File Offset: 0x0027EBC8
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

		// Token: 0x060088F3 RID: 35059 RVA: 0x00280A78 File Offset: 0x0027EC78
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

		// Token: 0x060088F4 RID: 35060 RVA: 0x00280ACC File Offset: 0x0027ECCC
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

		// Token: 0x060088F5 RID: 35061 RVA: 0x0005BFBC File Offset: 0x0005A1BC
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

		// Token: 0x060088F6 RID: 35062 RVA: 0x0005BFED File Offset: 0x0005A1ED
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

		// Token: 0x060088F7 RID: 35063 RVA: 0x0005C02C File Offset: 0x0005A22C
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

		// Token: 0x060088F8 RID: 35064 RVA: 0x00280BC8 File Offset: 0x0027EDC8
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

		// Token: 0x040057E9 RID: 22505
		private int remainingCharges;

		// Token: 0x040057EA RID: 22506
		private VerbTracker verbTracker;
	}
}
