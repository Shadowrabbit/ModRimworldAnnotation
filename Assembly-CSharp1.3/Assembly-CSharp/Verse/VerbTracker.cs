using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004F6 RID: 1270
	public class VerbTracker : IExposable
	{
		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x06002683 RID: 9859 RVA: 0x000EEEB5 File Offset: 0x000ED0B5
		public List<Verb> AllVerbs
		{
			get
			{
				if (this.verbs == null)
				{
					this.InitVerbsFromZero();
				}
				return this.verbs;
			}
		}

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x06002684 RID: 9860 RVA: 0x000EEECC File Offset: 0x000ED0CC
		public Verb PrimaryVerb
		{
			get
			{
				if (this.verbs == null)
				{
					this.InitVerbsFromZero();
				}
				for (int i = 0; i < this.verbs.Count; i++)
				{
					if (this.verbs[i].verbProps.isPrimary)
					{
						return this.verbs[i];
					}
				}
				return null;
			}
		}

		// Token: 0x06002685 RID: 9861 RVA: 0x000EEF23 File Offset: 0x000ED123
		public VerbTracker(IVerbOwner directOwner)
		{
			this.directOwner = directOwner;
		}

		// Token: 0x06002686 RID: 9862 RVA: 0x000EEF34 File Offset: 0x000ED134
		public void VerbsTick()
		{
			if (this.verbs == null)
			{
				return;
			}
			for (int i = 0; i < this.verbs.Count; i++)
			{
				this.verbs[i].VerbTick();
			}
		}

		// Token: 0x06002687 RID: 9863 RVA: 0x000EEF71 File Offset: 0x000ED171
		public IEnumerable<Command> GetVerbsCommands(KeyCode hotKey = KeyCode.None)
		{
			CompEquippable ce = this.directOwner as CompEquippable;
			if (ce == null)
			{
				yield break;
			}
			Thing ownerThing = ce.parent;
			List<Verb> verbs = this.AllVerbs;
			int num;
			for (int i = 0; i < verbs.Count; i = num + 1)
			{
				Verb verb = verbs[i];
				if (verb.verbProps.hasStandardCommand)
				{
					yield return this.CreateVerbTargetCommand(ownerThing, verb);
				}
				num = i;
			}
			if (!this.directOwner.Tools.NullOrEmpty<Tool>() && ce != null && ce.parent.def.IsMeleeWeapon)
			{
				yield return this.CreateVerbTargetCommand(ownerThing, (from v in verbs
				where v.verbProps.IsMeleeAttack
				select v).FirstOrDefault<Verb>());
			}
			yield break;
		}

		// Token: 0x06002688 RID: 9864 RVA: 0x000EEF84 File Offset: 0x000ED184
		private Command_VerbTarget CreateVerbTargetCommand(Thing ownerThing, Verb verb)
		{
			Command_VerbTarget command_VerbTarget = new Command_VerbTarget();
			ThingStyleDef styleDef = ownerThing.StyleDef;
			command_VerbTarget.defaultDesc = ownerThing.LabelCap + ": " + ownerThing.def.description.CapitalizeFirst();
			command_VerbTarget.icon = ((styleDef != null && styleDef.UIIcon != null) ? styleDef.UIIcon : ownerThing.def.uiIcon);
			command_VerbTarget.iconAngle = ownerThing.def.uiIconAngle;
			command_VerbTarget.iconOffset = ownerThing.def.uiIconOffset;
			command_VerbTarget.tutorTag = "VerbTarget";
			command_VerbTarget.verb = verb;
			if (verb.caster.Faction != Faction.OfPlayer)
			{
				command_VerbTarget.Disable("CannotOrderNonControlled".Translate());
			}
			else if (verb.CasterIsPawn)
			{
				string reason;
				if (verb.CasterPawn.WorkTagIsDisabled(WorkTags.Violent))
				{
					command_VerbTarget.Disable("IsIncapableOfViolence".Translate(verb.CasterPawn.LabelShort, verb.CasterPawn));
				}
				else if (!verb.CasterPawn.drafter.Drafted)
				{
					command_VerbTarget.Disable("IsNotDrafted".Translate(verb.CasterPawn.LabelShort, verb.CasterPawn));
				}
				else if (verb is Verb_LaunchProjectile)
				{
					Apparel apparel = verb.FirstApparelPreventingShooting();
					if (apparel != null)
					{
						command_VerbTarget.Disable("ApparelPreventsShooting".Translate(verb.CasterPawn.Named("PAWN"), apparel.Named("APPAREL")).CapitalizeFirst());
					}
				}
				else if (EquipmentUtility.RolePreventsFromUsing(verb.CasterPawn, verb.EquipmentSource, out reason))
				{
					command_VerbTarget.Disable(reason);
				}
			}
			return command_VerbTarget;
		}

		// Token: 0x06002689 RID: 9865 RVA: 0x000EF14C File Offset: 0x000ED34C
		public Verb GetVerb(VerbCategory category)
		{
			List<Verb> allVerbs = this.AllVerbs;
			if (allVerbs != null)
			{
				for (int i = 0; i < allVerbs.Count; i++)
				{
					if (allVerbs[i].verbProps.category == category)
					{
						return allVerbs[i];
					}
				}
			}
			return null;
		}

		// Token: 0x0600268A RID: 9866 RVA: 0x000EF194 File Offset: 0x000ED394
		public void ExposeData()
		{
			Scribe_Collections.Look<Verb>(ref this.verbs, "verbs", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs && this.verbs != null)
			{
				if (this.verbs.RemoveAll((Verb x) => x == null) != 0)
				{
					Log.Error("Some verbs were null after loading. directOwner=" + this.directOwner.ToStringSafe<IVerbOwner>());
				}
				List<Verb> sources = this.verbs;
				this.verbs = new List<Verb>();
				this.InitVerbs(delegate(Type type, string id)
				{
					Verb verb = sources.FirstOrDefault((Verb v) => v.loadID == id && v.GetType() == type);
					if (verb == null)
					{
						Log.Warning(string.Format("Replaced verb {0}/{1}; may have been changed through a version update or a mod change", type, id));
						verb = (Verb)Activator.CreateInstance(type);
					}
					this.verbs.Add(verb);
					return verb;
				});
			}
		}

		// Token: 0x0600268B RID: 9867 RVA: 0x000EF246 File Offset: 0x000ED446
		public void InitVerbsFromZero()
		{
			this.verbs = new List<Verb>();
			this.InitVerbs(delegate(Type type, string id)
			{
				Verb verb = (Verb)Activator.CreateInstance(type);
				this.verbs.Add(verb);
				return verb;
			});
		}

		// Token: 0x0600268C RID: 9868 RVA: 0x000EF268 File Offset: 0x000ED468
		private void InitVerbs(Func<Type, string, Verb> creator)
		{
			List<VerbProperties> verbProperties = this.directOwner.VerbProperties;
			if (verbProperties != null)
			{
				for (int i = 0; i < verbProperties.Count; i++)
				{
					try
					{
						VerbProperties verbProperties2 = verbProperties[i];
						string text = Verb.CalculateUniqueLoadID(this.directOwner, i);
						this.InitVerb(creator(verbProperties2.verbClass, text), verbProperties2, null, null, text);
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not instantiate Verb (directOwner=",
							this.directOwner.ToStringSafe<IVerbOwner>(),
							"): ",
							ex
						}));
					}
				}
			}
			List<Tool> tools = this.directOwner.Tools;
			if (tools != null)
			{
				for (int j = 0; j < tools.Count; j++)
				{
					Tool tool = tools[j];
					foreach (ManeuverDef maneuverDef in tool.Maneuvers)
					{
						try
						{
							VerbProperties verb = maneuverDef.verb;
							string text2 = Verb.CalculateUniqueLoadID(this.directOwner, tool, maneuverDef);
							this.InitVerb(creator(verb.verbClass, text2), verb, tool, maneuverDef, text2);
						}
						catch (Exception ex2)
						{
							Log.Error(string.Concat(new object[]
							{
								"Could not instantiate Verb (directOwner=",
								this.directOwner.ToStringSafe<IVerbOwner>(),
								"): ",
								ex2
							}));
						}
					}
				}
			}
		}

		// Token: 0x0600268D RID: 9869 RVA: 0x000EF400 File Offset: 0x000ED600
		private void InitVerb(Verb verb, VerbProperties properties, Tool tool, ManeuverDef maneuver, string id)
		{
			verb.loadID = id;
			verb.verbProps = properties;
			verb.verbTracker = this;
			verb.tool = tool;
			verb.maneuver = maneuver;
			verb.caster = this.directOwner.ConstantCaster;
		}

		// Token: 0x0600268E RID: 9870 RVA: 0x000EF438 File Offset: 0x000ED638
		public void VerbsNeedReinitOnLoad()
		{
			this.verbs = null;
		}

		// Token: 0x04001826 RID: 6182
		public IVerbOwner directOwner;

		// Token: 0x04001827 RID: 6183
		private List<Verb> verbs;
	}
}
