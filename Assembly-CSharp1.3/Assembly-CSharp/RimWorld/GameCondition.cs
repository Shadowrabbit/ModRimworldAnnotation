using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000BD8 RID: 3032
	public class GameCondition : IExposable, ILoadReferenceable
	{
		// Token: 0x17000C71 RID: 3185
		// (get) Token: 0x0600472E RID: 18222 RVA: 0x0017946A File Offset: 0x0017766A
		protected Map SingleMap
		{
			get
			{
				return this.gameConditionManager.ownerMap;
			}
		}

		// Token: 0x17000C72 RID: 3186
		// (get) Token: 0x0600472F RID: 18223 RVA: 0x00179477 File Offset: 0x00177677
		public virtual string Label
		{
			get
			{
				return this.def.label;
			}
		}

		// Token: 0x17000C73 RID: 3187
		// (get) Token: 0x06004730 RID: 18224 RVA: 0x00179484 File Offset: 0x00177684
		public virtual string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.def);
			}
		}

		// Token: 0x17000C74 RID: 3188
		// (get) Token: 0x06004731 RID: 18225 RVA: 0x00179497 File Offset: 0x00177697
		public virtual string LetterText
		{
			get
			{
				return this.def.letterText;
			}
		}

		// Token: 0x17000C75 RID: 3189
		// (get) Token: 0x06004732 RID: 18226 RVA: 0x001794A4 File Offset: 0x001776A4
		public virtual bool Expired
		{
			get
			{
				return !this.Permanent && Find.TickManager.TicksGame > this.startTick + this.Duration;
			}
		}

		// Token: 0x17000C76 RID: 3190
		// (get) Token: 0x06004733 RID: 18227 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool ElectricityDisabled
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C77 RID: 3191
		// (get) Token: 0x06004734 RID: 18228 RVA: 0x001794C9 File Offset: 0x001776C9
		public int TicksPassed
		{
			get
			{
				return Find.TickManager.TicksGame - this.startTick;
			}
		}

		// Token: 0x17000C78 RID: 3192
		// (get) Token: 0x06004735 RID: 18229 RVA: 0x001794DC File Offset: 0x001776DC
		public virtual string Description
		{
			get
			{
				return this.def.description;
			}
		}

		// Token: 0x17000C79 RID: 3193
		// (get) Token: 0x06004736 RID: 18230 RVA: 0x001794E9 File Offset: 0x001776E9
		public virtual int TransitionTicks
		{
			get
			{
				return 300;
			}
		}

		// Token: 0x17000C7A RID: 3194
		// (get) Token: 0x06004737 RID: 18231 RVA: 0x001794F0 File Offset: 0x001776F0
		// (set) Token: 0x06004738 RID: 18232 RVA: 0x0017951C File Offset: 0x0017771C
		public int TicksLeft
		{
			get
			{
				if (this.Permanent)
				{
					Log.ErrorOnce("Trying to get ticks left of a permanent condition.", 384767654);
					return 360000000;
				}
				return this.Duration - this.TicksPassed;
			}
			set
			{
				this.Duration = this.TicksPassed + value;
			}
		}

		// Token: 0x17000C7B RID: 3195
		// (get) Token: 0x06004739 RID: 18233 RVA: 0x0017952C File Offset: 0x0017772C
		// (set) Token: 0x0600473A RID: 18234 RVA: 0x00179534 File Offset: 0x00177734
		public bool Permanent
		{
			get
			{
				return this.permanent;
			}
			set
			{
				if (value)
				{
					this.duration = -1;
				}
				this.permanent = value;
			}
		}

		// Token: 0x17000C7C RID: 3196
		// (get) Token: 0x0600473B RID: 18235 RVA: 0x00179547 File Offset: 0x00177747
		// (set) Token: 0x0600473C RID: 18236 RVA: 0x0017956C File Offset: 0x0017776C
		public int Duration
		{
			get
			{
				if (this.Permanent)
				{
					Log.ErrorOnce("Trying to get duration of a permanent condition.", 100394867);
					return 360000000;
				}
				return this.duration;
			}
			set
			{
				this.permanent = false;
				this.duration = value;
			}
		}

		// Token: 0x17000C7D RID: 3197
		// (get) Token: 0x0600473D RID: 18237 RVA: 0x0017957C File Offset: 0x0017777C
		public virtual string TooltipString
		{
			get
			{
				string text = this.def.LabelCap;
				if (this.Permanent)
				{
					text += "\n" + "Permanent".Translate().CapitalizeFirst();
				}
				else
				{
					Vector2 location;
					if (this.SingleMap != null)
					{
						location = Find.WorldGrid.LongLatOf(this.SingleMap.Tile);
					}
					else if (Find.CurrentMap != null)
					{
						location = Find.WorldGrid.LongLatOf(Find.CurrentMap.Tile);
					}
					else if (Find.AnyPlayerHomeMap != null)
					{
						location = Find.WorldGrid.LongLatOf(Find.AnyPlayerHomeMap.Tile);
					}
					else
					{
						location = Vector2.zero;
					}
					text += "\n" + "Started".Translate() + ": " + GenDate.DateFullStringAt((long)GenDate.TickGameToAbs(this.startTick), location);
					text += "\n" + "Lasted".Translate() + ": " + this.TicksPassed.ToStringTicksToPeriod(true, false, true, true).Colorize(ColoredText.DateTimeColor);
				}
				text += "\n";
				text = text + "\n" + this.Description;
				text += "\n";
				text += "\n";
				if (this.conditionCauser != null && CameraJumper.CanJump(this.conditionCauser))
				{
					text += this.def.jumpToSourceKey.Translate();
				}
				else if (this.quest != null)
				{
					text += "CausedByQuest".Translate(this.quest.name);
				}
				else
				{
					text += "SourceUnknown".Translate();
				}
				return text;
			}
		}

		// Token: 0x17000C7E RID: 3198
		// (get) Token: 0x0600473E RID: 18238 RVA: 0x00179770 File Offset: 0x00177970
		public List<Map> AffectedMaps
		{
			get
			{
				if (!GenCollection.ListsEqual<Map>(this.cachedAffectedMapsForMaps, Find.Maps))
				{
					this.cachedAffectedMapsForMaps.Clear();
					this.cachedAffectedMapsForMaps.AddRange(Find.Maps);
					this.cachedAffectedMaps.Clear();
					if (this.gameConditionManager.ownerMap != null)
					{
						this.cachedAffectedMaps.Add(this.gameConditionManager.ownerMap);
					}
					GameCondition.tmpGameConditionManagers.Clear();
					this.gameConditionManager.GetChildren(GameCondition.tmpGameConditionManagers);
					for (int i = 0; i < GameCondition.tmpGameConditionManagers.Count; i++)
					{
						if (GameCondition.tmpGameConditionManagers[i].ownerMap != null)
						{
							this.cachedAffectedMaps.Add(GameCondition.tmpGameConditionManagers[i].ownerMap);
						}
					}
					GameCondition.tmpGameConditionManagers.Clear();
				}
				return this.cachedAffectedMaps;
			}
		}

		// Token: 0x0600473F RID: 18239 RVA: 0x00179848 File Offset: 0x00177A48
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.uniqueID, "uniqueID", -1, false);
			Scribe_Values.Look<bool>(ref this.suppressEndMessage, "suppressEndMessage", false, false);
			Scribe_Defs.Look<GameConditionDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
			Scribe_Values.Look<bool>(ref this.permanent, "permanent", false, false);
			Scribe_References.Look<Quest>(ref this.quest, "quest", false);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06004740 RID: 18240 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void GameConditionTick()
		{
		}

		// Token: 0x06004741 RID: 18241 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void GameConditionDraw(Map map)
		{
		}

		// Token: 0x06004742 RID: 18242 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Init()
		{
		}

		// Token: 0x06004743 RID: 18243 RVA: 0x001798D6 File Offset: 0x00177AD6
		public virtual void End()
		{
			if (!this.suppressEndMessage && this.def.endMessage != null)
			{
				Messages.Message(this.def.endMessage, MessageTypeDefOf.NeutralEvent, true);
			}
			this.gameConditionManager.ActiveConditions.Remove(this);
		}

		// Token: 0x06004744 RID: 18244 RVA: 0x0001F15E File Offset: 0x0001D35E
		public virtual float SkyGazeChanceFactor(Map map)
		{
			return 1f;
		}

		// Token: 0x06004745 RID: 18245 RVA: 0x0001F15E File Offset: 0x0001D35E
		public virtual float SkyGazeJoyGainFactor(Map map)
		{
			return 1f;
		}

		// Token: 0x06004746 RID: 18246 RVA: 0x000682C5 File Offset: 0x000664C5
		public virtual float TemperatureOffset()
		{
			return 0f;
		}

		// Token: 0x06004747 RID: 18247 RVA: 0x000682C5 File Offset: 0x000664C5
		public virtual float SkyTargetLerpFactor(Map map)
		{
			return 0f;
		}

		// Token: 0x06004748 RID: 18248 RVA: 0x00179918 File Offset: 0x00177B18
		public virtual SkyTarget? SkyTarget(Map map)
		{
			return null;
		}

		// Token: 0x06004749 RID: 18249 RVA: 0x0001F15E File Offset: 0x0001D35E
		public virtual float AnimalDensityFactor(Map map)
		{
			return 1f;
		}

		// Token: 0x0600474A RID: 18250 RVA: 0x0001F15E File Offset: 0x0001D35E
		public virtual float PlantDensityFactor(Map map)
		{
			return 1f;
		}

		// Token: 0x0600474B RID: 18251 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool AllowEnjoyableOutsideNow(Map map)
		{
			return true;
		}

		// Token: 0x0600474C RID: 18252 RVA: 0x00002688 File Offset: 0x00000888
		public virtual List<SkyOverlay> SkyOverlays(Map map)
		{
			return null;
		}

		// Token: 0x0600474D RID: 18253 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void DoCellSteadyEffects(IntVec3 c, Map map)
		{
		}

		// Token: 0x0600474E RID: 18254 RVA: 0x00002688 File Offset: 0x00000888
		public virtual WeatherDef ForcedWeather()
		{
			return null;
		}

		// Token: 0x0600474F RID: 18255 RVA: 0x0017992E File Offset: 0x00177B2E
		public virtual void PostMake()
		{
			this.uniqueID = Find.UniqueIDsManager.GetNextGameConditionID();
		}

		// Token: 0x06004750 RID: 18256 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void RandomizeSettings(float points, Map map, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
		}

		// Token: 0x06004751 RID: 18257 RVA: 0x00179940 File Offset: 0x00177B40
		public string GetUniqueLoadID()
		{
			return string.Format("{0}_{1}", base.GetType().Name, this.uniqueID.ToString());
		}

		// Token: 0x04002BC8 RID: 11208
		public GameConditionManager gameConditionManager;

		// Token: 0x04002BC9 RID: 11209
		public Thing conditionCauser;

		// Token: 0x04002BCA RID: 11210
		public GameConditionDef def;

		// Token: 0x04002BCB RID: 11211
		public int uniqueID = -1;

		// Token: 0x04002BCC RID: 11212
		public int startTick;

		// Token: 0x04002BCD RID: 11213
		public bool suppressEndMessage;

		// Token: 0x04002BCE RID: 11214
		private int duration = -1;

		// Token: 0x04002BCF RID: 11215
		private bool permanent;

		// Token: 0x04002BD0 RID: 11216
		private List<Map> cachedAffectedMaps = new List<Map>();

		// Token: 0x04002BD1 RID: 11217
		private List<Map> cachedAffectedMapsForMaps = new List<Map>();

		// Token: 0x04002BD2 RID: 11218
		public Quest quest;

		// Token: 0x04002BD3 RID: 11219
		private static List<GameConditionManager> tmpGameConditionManagers = new List<GameConditionManager>();
	}
}
