using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020008AD RID: 2221
	public class VerbTracker : IExposable
	{
		// Token: 0x170008AE RID: 2222
		// (get) Token: 0x0600373F RID: 14143 RVA: 0x0002ACF0 File Offset: 0x00028EF0
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

		// Token: 0x170008AF RID: 2223
		// (get) Token: 0x06003740 RID: 14144 RVA: 0x0015FFC4 File Offset: 0x0015E1C4
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

		// Token: 0x06003741 RID: 14145 RVA: 0x0002AD06 File Offset: 0x00028F06
		public VerbTracker(IVerbOwner directOwner)
		{
			this.directOwner = directOwner;
		}

		// Token: 0x06003742 RID: 14146 RVA: 0x0016001C File Offset: 0x0015E21C
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

		// Token: 0x06003743 RID: 14147 RVA: 0x0002AD15 File Offset: 0x00028F15
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

		// Token: 0x06003744 RID: 14148 RVA: 0x0016005C File Offset: 0x0015E25C
		private Command_VerbTarget CreateVerbTargetCommand(Thing ownerThing, Verb verb)
		{
			Command_VerbTarget command_VerbTarget = new Command_VerbTarget();
			command_VerbTarget.defaultDesc = ownerThing.LabelCap + ": " + ownerThing.def.description.CapitalizeFirst();
			command_VerbTarget.icon = ownerThing.def.uiIcon;
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
				if (verb.CasterPawn.WorkTagIsDisabled(WorkTags.Violent))
				{
					command_VerbTarget.Disable("IsIncapableOfViolence".Translate(verb.CasterPawn.LabelShort, verb.CasterPawn));
				}
				else if (!verb.CasterPawn.drafter.Drafted)
				{
					command_VerbTarget.Disable("IsNotDrafted".Translate(verb.CasterPawn.LabelShort, verb.CasterPawn));
				}
			}
			return command_VerbTarget;
		}

		// Token: 0x06003745 RID: 14149 RVA: 0x00160194 File Offset: 0x0015E394
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

		// Token: 0x06003746 RID: 14150 RVA: 0x001601DC File Offset: 0x0015E3DC
		public void ExposeData()
		{
			Scribe_Collections.Look<Verb>(ref this.verbs, "verbs", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs && this.verbs != null)
			{
				if (this.verbs.RemoveAll((Verb x) => x == null) != 0)
				{
					Log.Error("Some verbs were null after loading. directOwner=" + this.directOwner.ToStringSafe<IVerbOwner>(), false);
				}
				List<Verb> sources = this.verbs;
				this.verbs = new List<Verb>();
				this.InitVerbs(delegate(Type type, string id)
				{
					Verb verb = sources.FirstOrDefault((Verb v) => v.loadID == id && v.GetType() == type);
					if (verb == null)
					{
						Log.Warning(string.Format("Replaced verb {0}/{1}; may have been changed through a version update or a mod change", type, id), false);
						verb = (Verb)Activator.CreateInstance(type);
					}
					this.verbs.Add(verb);
					return verb;
				});
			}
		}

		// Token: 0x06003747 RID: 14151 RVA: 0x0002AD25 File Offset: 0x00028F25
		private void InitVerbsFromZero()
		{
			this.verbs = new List<Verb>();
			this.InitVerbs(delegate(Type type, string id)
			{
				Verb verb = (Verb)Activator.CreateInstance(type);
				this.verbs.Add(verb);
				return verb;
			});
		}

		// Token: 0x06003748 RID: 14152 RVA: 0x00160290 File Offset: 0x0015E490
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
						}), false);
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
							}), false);
						}
					}
				}
			}
		}

		// Token: 0x06003749 RID: 14153 RVA: 0x0002AD44 File Offset: 0x00028F44
		private void InitVerb(Verb verb, VerbProperties properties, Tool tool, ManeuverDef maneuver, string id)
		{
			verb.loadID = id;
			verb.verbProps = properties;
			verb.verbTracker = this;
			verb.tool = tool;
			verb.maneuver = maneuver;
			verb.caster = this.directOwner.ConstantCaster;
		}

		// Token: 0x0600374A RID: 14154 RVA: 0x0002AD7C File Offset: 0x00028F7C
		public void VerbsNeedReinitOnLoad()
		{
			this.verbs = null;
		}

		// Token: 0x04002674 RID: 9844
		public IVerbOwner directOwner;

		// Token: 0x04002675 RID: 9845
		private List<Verb> verbs;
	}
}
