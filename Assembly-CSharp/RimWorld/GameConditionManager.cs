using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200117C RID: 4476
	public sealed class GameConditionManager : IExposable
	{
		// Token: 0x17000F79 RID: 3961
		// (get) Token: 0x060062A3 RID: 25251 RVA: 0x00043E46 File Offset: 0x00042046
		public List<GameCondition> ActiveConditions
		{
			get
			{
				return this.activeConditions;
			}
		}

		// Token: 0x17000F7A RID: 3962
		// (get) Token: 0x060062A4 RID: 25252 RVA: 0x00043E4E File Offset: 0x0004204E
		public GameConditionManager Parent
		{
			get
			{
				if (this.ownerMap != null)
				{
					return Find.World.gameConditionManager;
				}
				return null;
			}
		}

		// Token: 0x17000F7B RID: 3963
		// (get) Token: 0x060062A5 RID: 25253 RVA: 0x001EC878 File Offset: 0x001EAA78
		public bool ElectricityDisabled
		{
			get
			{
				using (List<GameCondition>.Enumerator enumerator = this.activeConditions.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.ElectricityDisabled)
						{
							return true;
						}
					}
				}
				return this.Parent != null && this.Parent.ElectricityDisabled;
			}
		}

		// Token: 0x060062A6 RID: 25254 RVA: 0x00043E64 File Offset: 0x00042064
		public GameConditionManager(Map map)
		{
			this.ownerMap = map;
		}

		// Token: 0x060062A7 RID: 25255 RVA: 0x00043E7E File Offset: 0x0004207E
		public GameConditionManager(World world)
		{
		}

		// Token: 0x060062A8 RID: 25256 RVA: 0x00043E91 File Offset: 0x00042091
		public void RegisterCondition(GameCondition cond)
		{
			this.activeConditions.Add(cond);
			cond.startTick = Mathf.Max(cond.startTick, Find.TickManager.TicksGame);
			cond.gameConditionManager = this;
			cond.Init();
		}

		// Token: 0x060062A9 RID: 25257 RVA: 0x001EC8E8 File Offset: 0x001EAAE8
		public void ExposeData()
		{
			Scribe_Collections.Look<GameCondition>(ref this.activeConditions, "activeConditions", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				for (int i = 0; i < this.activeConditions.Count; i++)
				{
					this.activeConditions[i].gameConditionManager = this;
				}
			}
		}

		// Token: 0x060062AA RID: 25258 RVA: 0x001EC93C File Offset: 0x001EAB3C
		public void GameConditionManagerTick()
		{
			for (int i = this.activeConditions.Count - 1; i >= 0; i--)
			{
				GameCondition gameCondition = this.activeConditions[i];
				if (gameCondition.Expired)
				{
					gameCondition.End();
				}
				else
				{
					gameCondition.GameConditionTick();
				}
			}
		}

		// Token: 0x060062AB RID: 25259 RVA: 0x001EC984 File Offset: 0x001EAB84
		public void GameConditionManagerDraw(Map map)
		{
			for (int i = this.activeConditions.Count - 1; i >= 0; i--)
			{
				this.activeConditions[i].GameConditionDraw(map);
			}
			if (this.Parent != null)
			{
				this.Parent.GameConditionManagerDraw(map);
			}
		}

		// Token: 0x060062AC RID: 25260 RVA: 0x001EC9D0 File Offset: 0x001EABD0
		public void DoSteadyEffects(IntVec3 c, Map map)
		{
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				this.activeConditions[i].DoCellSteadyEffects(c, map);
			}
			if (this.Parent != null)
			{
				this.Parent.DoSteadyEffects(c, map);
			}
		}

		// Token: 0x060062AD RID: 25261 RVA: 0x00043EC7 File Offset: 0x000420C7
		public bool ConditionIsActive(GameConditionDef def)
		{
			return this.GetActiveCondition(def) != null;
		}

		// Token: 0x060062AE RID: 25262 RVA: 0x001ECA1C File Offset: 0x001EAC1C
		public GameCondition GetActiveCondition(GameConditionDef def)
		{
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				if (def == this.activeConditions[i].def)
				{
					return this.activeConditions[i];
				}
			}
			if (this.Parent != null)
			{
				return this.Parent.GetActiveCondition(def);
			}
			return null;
		}

		// Token: 0x060062AF RID: 25263 RVA: 0x001ECA78 File Offset: 0x001EAC78
		public T GetActiveCondition<T>() where T : GameCondition
		{
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				T t = this.activeConditions[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			if (this.Parent != null)
			{
				return this.Parent.GetActiveCondition<T>();
			}
			return default(T);
		}

		// Token: 0x060062B0 RID: 25264 RVA: 0x001ECADC File Offset: 0x001EACDC
		public PsychicDroneLevel GetHighestPsychicDroneLevelFor(Gender gender)
		{
			PsychicDroneLevel psychicDroneLevel = PsychicDroneLevel.None;
			for (int i = 0; i < this.ActiveConditions.Count; i++)
			{
				GameCondition_PsychicEmanation gameCondition_PsychicEmanation = this.activeConditions[i] as GameCondition_PsychicEmanation;
				if (gameCondition_PsychicEmanation != null && gameCondition_PsychicEmanation.gender == gender && gameCondition_PsychicEmanation.level > psychicDroneLevel)
				{
					psychicDroneLevel = gameCondition_PsychicEmanation.level;
				}
			}
			return psychicDroneLevel;
		}

		// Token: 0x060062B1 RID: 25265 RVA: 0x001ECB30 File Offset: 0x001EAD30
		public void GetChildren(List<GameConditionManager> outChildren)
		{
			if (this == Find.World.gameConditionManager)
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					outChildren.Add(maps[i].gameConditionManager);
				}
			}
		}

		// Token: 0x060062B2 RID: 25266 RVA: 0x001ECB74 File Offset: 0x001EAD74
		public float TotalHeightAt(float width)
		{
			float num = 0f;
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				num += Text.CalcHeight(this.activeConditions[i].LabelCap, width - 6f);
			}
			if (this.Parent != null)
			{
				num += this.Parent.TotalHeightAt(width);
			}
			return num;
		}

		// Token: 0x060062B3 RID: 25267 RVA: 0x001ECBD8 File Offset: 0x001EADD8
		public void DoConditionsUI(Rect rect)
		{
			GUI.BeginGroup(rect);
			float num = 0f;
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				string labelCap = this.activeConditions[i].LabelCap;
				Rect rect2 = new Rect(0f, num, rect.width, Text.CalcHeight(labelCap, rect.width - 6f));
				Text.Font = GameFont.Small;
				Text.Anchor = TextAnchor.MiddleRight;
				Widgets.DrawHighlightIfMouseover(rect2);
				Rect rect3 = rect2;
				rect3.width -= 6f;
				Widgets.Label(rect3, labelCap);
				if (Mouse.IsOver(rect2))
				{
					TooltipHandler.TipRegion(rect2, new TipSignal(this.activeConditions[i].TooltipString, 976090154 ^ i));
				}
				if (Widgets.ButtonInvisible(rect2, true))
				{
					if (this.activeConditions[i].conditionCauser != null && CameraJumper.CanJump(this.activeConditions[i].conditionCauser))
					{
						CameraJumper.TryJumpAndSelect(this.activeConditions[i].conditionCauser);
					}
					else if (this.activeConditions[i].quest != null)
					{
						Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
						((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(this.activeConditions[i].quest);
					}
				}
				num += rect2.height;
			}
			rect.yMin += num;
			GUI.EndGroup();
			Text.Anchor = TextAnchor.UpperLeft;
			if (this.Parent != null)
			{
				this.Parent.DoConditionsUI(rect);
			}
		}

		// Token: 0x060062B4 RID: 25268 RVA: 0x001ECD7C File Offset: 0x001EAF7C
		public void GetAllGameConditionsAffectingMap(Map map, List<GameCondition> listToFill)
		{
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				listToFill.Add(this.activeConditions[i]);
			}
			if (this.Parent != null)
			{
				this.Parent.GetAllGameConditionsAffectingMap(map, listToFill);
			}
		}

		// Token: 0x060062B5 RID: 25269 RVA: 0x001ECDC8 File Offset: 0x001EAFC8
		internal float AggregateTemperatureOffset()
		{
			float num = 0f;
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				num += this.activeConditions[i].TemperatureOffset();
			}
			if (this.Parent != null)
			{
				num += this.Parent.AggregateTemperatureOffset();
			}
			return num;
		}

		// Token: 0x060062B6 RID: 25270 RVA: 0x001ECE1C File Offset: 0x001EB01C
		internal float AggregateAnimalDensityFactor(Map map)
		{
			float num = 1f;
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				num *= this.activeConditions[i].AnimalDensityFactor(map);
			}
			if (this.Parent != null)
			{
				num *= this.Parent.AggregateAnimalDensityFactor(map);
			}
			return num;
		}

		// Token: 0x060062B7 RID: 25271 RVA: 0x001ECE74 File Offset: 0x001EB074
		internal float AggregatePlantDensityFactor(Map map)
		{
			float num = 1f;
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				num *= this.activeConditions[i].PlantDensityFactor(map);
			}
			if (this.Parent != null)
			{
				num *= this.Parent.AggregatePlantDensityFactor(map);
			}
			return num;
		}

		// Token: 0x060062B8 RID: 25272 RVA: 0x001ECECC File Offset: 0x001EB0CC
		internal float AggregateSkyGazeJoyGainFactor(Map map)
		{
			float num = 1f;
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				num *= this.activeConditions[i].SkyGazeJoyGainFactor(map);
			}
			if (this.Parent != null)
			{
				num *= this.Parent.AggregateSkyGazeJoyGainFactor(map);
			}
			return num;
		}

		// Token: 0x060062B9 RID: 25273 RVA: 0x001ECF24 File Offset: 0x001EB124
		internal float AggregateSkyGazeChanceFactor(Map map)
		{
			float num = 1f;
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				num *= this.activeConditions[i].SkyGazeChanceFactor(map);
			}
			if (this.Parent != null)
			{
				num *= this.Parent.AggregateSkyGazeChanceFactor(map);
			}
			return num;
		}

		// Token: 0x060062BA RID: 25274 RVA: 0x001ECF7C File Offset: 0x001EB17C
		internal bool AllowEnjoyableOutsideNow(Map map)
		{
			GameConditionDef gameConditionDef;
			return this.AllowEnjoyableOutsideNow(map, out gameConditionDef);
		}

		// Token: 0x060062BB RID: 25275 RVA: 0x001ECF94 File Offset: 0x001EB194
		internal bool AllowEnjoyableOutsideNow(Map map, out GameConditionDef reason)
		{
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				GameCondition gameCondition = this.activeConditions[i];
				if (!gameCondition.AllowEnjoyableOutsideNow(map))
				{
					reason = gameCondition.def;
					return false;
				}
			}
			reason = null;
			return this.Parent == null || this.Parent.AllowEnjoyableOutsideNow(map, out reason);
		}

		// Token: 0x060062BC RID: 25276 RVA: 0x001ECFF4 File Offset: 0x001EB1F4
		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (GameCondition saveable in this.activeConditions)
			{
				stringBuilder.AppendLine(Scribe.saver.DebugOutputFor(saveable));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04004219 RID: 16921
		public Map ownerMap;

		// Token: 0x0400421A RID: 16922
		private List<GameCondition> activeConditions = new List<GameCondition>();

		// Token: 0x0400421B RID: 16923
		private const float TextPadding = 6f;
	}
}
