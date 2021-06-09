using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200116F RID: 4463
	public class GameCondition : IExposable, ILoadReferenceable
	{
		// Token: 0x17000F5B RID: 3931
		// (get) Token: 0x0600622C RID: 25132 RVA: 0x00043892 File Offset: 0x00041A92
		protected Map SingleMap
		{
			get
			{
				return this.gameConditionManager.ownerMap;
			}
		}

		// Token: 0x17000F5C RID: 3932
		// (get) Token: 0x0600622D RID: 25133 RVA: 0x0004389F File Offset: 0x00041A9F
		public virtual string Label
		{
			get
			{
				return this.def.label;
			}
		}

		// Token: 0x17000F5D RID: 3933
		// (get) Token: 0x0600622E RID: 25134 RVA: 0x000438AC File Offset: 0x00041AAC
		public virtual string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.def);
			}
		}

		// Token: 0x17000F5E RID: 3934
		// (get) Token: 0x0600622F RID: 25135 RVA: 0x000438BF File Offset: 0x00041ABF
		public virtual string LetterText
		{
			get
			{
				return this.def.letterText;
			}
		}

		// Token: 0x17000F5F RID: 3935
		// (get) Token: 0x06006230 RID: 25136 RVA: 0x000438CC File Offset: 0x00041ACC
		public virtual bool Expired
		{
			get
			{
				return !this.Permanent && Find.TickManager.TicksGame > this.startTick + this.Duration;
			}
		}

		// Token: 0x17000F60 RID: 3936
		// (get) Token: 0x06006231 RID: 25137 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool ElectricityDisabled
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000F61 RID: 3937
		// (get) Token: 0x06006232 RID: 25138 RVA: 0x000438F1 File Offset: 0x00041AF1
		public int TicksPassed
		{
			get
			{
				return Find.TickManager.TicksGame - this.startTick;
			}
		}

		// Token: 0x17000F62 RID: 3938
		// (get) Token: 0x06006233 RID: 25139 RVA: 0x00043904 File Offset: 0x00041B04
		public virtual string Description
		{
			get
			{
				return this.def.description;
			}
		}

		// Token: 0x17000F63 RID: 3939
		// (get) Token: 0x06006234 RID: 25140 RVA: 0x00043911 File Offset: 0x00041B11
		public virtual int TransitionTicks
		{
			get
			{
				return 300;
			}
		}

		// Token: 0x17000F64 RID: 3940
		// (get) Token: 0x06006235 RID: 25141 RVA: 0x00043918 File Offset: 0x00041B18
		// (set) Token: 0x06006236 RID: 25142 RVA: 0x00043945 File Offset: 0x00041B45
		public int TicksLeft
		{
			get
			{
				if (this.Permanent)
				{
					Log.ErrorOnce("Trying to get ticks left of a permanent condition.", 384767654, false);
					return 360000000;
				}
				return this.Duration - this.TicksPassed;
			}
			set
			{
				this.Duration = this.TicksPassed + value;
			}
		}

		// Token: 0x17000F65 RID: 3941
		// (get) Token: 0x06006237 RID: 25143 RVA: 0x00043955 File Offset: 0x00041B55
		// (set) Token: 0x06006238 RID: 25144 RVA: 0x0004395D File Offset: 0x00041B5D
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

		// Token: 0x17000F66 RID: 3942
		// (get) Token: 0x06006239 RID: 25145 RVA: 0x00043970 File Offset: 0x00041B70
		// (set) Token: 0x0600623A RID: 25146 RVA: 0x00043996 File Offset: 0x00041B96
		public int Duration
		{
			get
			{
				if (this.Permanent)
				{
					Log.ErrorOnce("Trying to get duration of a permanent condition.", 100394867, false);
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

		// Token: 0x17000F67 RID: 3943
		// (get) Token: 0x0600623B RID: 25147 RVA: 0x001EB670 File Offset: 0x001E9870
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

		// Token: 0x17000F68 RID: 3944
		// (get) Token: 0x0600623C RID: 25148 RVA: 0x001EB864 File Offset: 0x001E9A64
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

		// Token: 0x0600623D RID: 25149 RVA: 0x001EB93C File Offset: 0x001E9B3C
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

		// Token: 0x0600623E RID: 25150 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void GameConditionTick()
		{
		}

		// Token: 0x0600623F RID: 25151 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void GameConditionDraw(Map map)
		{
		}

		// Token: 0x06006240 RID: 25152 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Init()
		{
		}

		// Token: 0x06006241 RID: 25153 RVA: 0x000439A6 File Offset: 0x00041BA6
		public virtual void End()
		{
			if (!this.suppressEndMessage && this.def.endMessage != null)
			{
				Messages.Message(this.def.endMessage, MessageTypeDefOf.NeutralEvent, true);
			}
			this.gameConditionManager.ActiveConditions.Remove(this);
		}

		// Token: 0x06006242 RID: 25154 RVA: 0x0000CE6C File Offset: 0x0000B06C
		public virtual float SkyGazeChanceFactor(Map map)
		{
			return 1f;
		}

		// Token: 0x06006243 RID: 25155 RVA: 0x0000CE6C File Offset: 0x0000B06C
		public virtual float SkyGazeJoyGainFactor(Map map)
		{
			return 1f;
		}

		// Token: 0x06006244 RID: 25156 RVA: 0x00016647 File Offset: 0x00014847
		public virtual float TemperatureOffset()
		{
			return 0f;
		}

		// Token: 0x06006245 RID: 25157 RVA: 0x00016647 File Offset: 0x00014847
		public virtual float SkyTargetLerpFactor(Map map)
		{
			return 0f;
		}

		// Token: 0x06006246 RID: 25158 RVA: 0x001EB9CC File Offset: 0x001E9BCC
		public virtual SkyTarget? SkyTarget(Map map)
		{
			return null;
		}

		// Token: 0x06006247 RID: 25159 RVA: 0x0000CE6C File Offset: 0x0000B06C
		public virtual float AnimalDensityFactor(Map map)
		{
			return 1f;
		}

		// Token: 0x06006248 RID: 25160 RVA: 0x0000CE6C File Offset: 0x0000B06C
		public virtual float PlantDensityFactor(Map map)
		{
			return 1f;
		}

		// Token: 0x06006249 RID: 25161 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool AllowEnjoyableOutsideNow(Map map)
		{
			return true;
		}

		// Token: 0x0600624A RID: 25162 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual List<SkyOverlay> SkyOverlays(Map map)
		{
			return null;
		}

		// Token: 0x0600624B RID: 25163 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void DoCellSteadyEffects(IntVec3 c, Map map)
		{
		}

		// Token: 0x0600624C RID: 25164 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual WeatherDef ForcedWeather()
		{
			return null;
		}

		// Token: 0x0600624D RID: 25165 RVA: 0x000439E5 File Offset: 0x00041BE5
		public virtual void PostMake()
		{
			this.uniqueID = Find.UniqueIDsManager.GetNextGameConditionID();
		}

		// Token: 0x0600624E RID: 25166 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void RandomizeSettings(float points, Map map, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
		}

		// Token: 0x0600624F RID: 25167 RVA: 0x000439F7 File Offset: 0x00041BF7
		public string GetUniqueLoadID()
		{
			return string.Format("{0}_{1}", base.GetType().Name, this.uniqueID.ToString());
		}

		// Token: 0x040041DB RID: 16859
		public GameConditionManager gameConditionManager;

		// Token: 0x040041DC RID: 16860
		public Thing conditionCauser;

		// Token: 0x040041DD RID: 16861
		public GameConditionDef def;

		// Token: 0x040041DE RID: 16862
		public int uniqueID = -1;

		// Token: 0x040041DF RID: 16863
		public int startTick;

		// Token: 0x040041E0 RID: 16864
		public bool suppressEndMessage;

		// Token: 0x040041E1 RID: 16865
		private int duration = -1;

		// Token: 0x040041E2 RID: 16866
		private bool permanent;

		// Token: 0x040041E3 RID: 16867
		private List<Map> cachedAffectedMaps = new List<Map>();

		// Token: 0x040041E4 RID: 16868
		private List<Map> cachedAffectedMapsForMaps = new List<Map>();

		// Token: 0x040041E5 RID: 16869
		public Quest quest;

		// Token: 0x040041E6 RID: 16870
		private static List<GameConditionManager> tmpGameConditionManagers = new List<GameConditionManager>();
	}
}
