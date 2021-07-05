using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BDB RID: 3035
	public sealed class GameConditionManager : IExposable
	{
		// Token: 0x17000C7F RID: 3199
		// (get) Token: 0x06004758 RID: 18264 RVA: 0x00179A79 File Offset: 0x00177C79
		public List<GameCondition> ActiveConditions
		{
			get
			{
				return this.activeConditions;
			}
		}

		// Token: 0x17000C80 RID: 3200
		// (get) Token: 0x06004759 RID: 18265 RVA: 0x00179A81 File Offset: 0x00177C81
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

		// Token: 0x17000C81 RID: 3201
		// (get) Token: 0x0600475A RID: 18266 RVA: 0x00179A98 File Offset: 0x00177C98
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

		// Token: 0x0600475B RID: 18267 RVA: 0x00179B08 File Offset: 0x00177D08
		public GameConditionManager(Map map)
		{
			this.ownerMap = map;
		}

		// Token: 0x0600475C RID: 18268 RVA: 0x00179B22 File Offset: 0x00177D22
		public GameConditionManager(World world)
		{
		}

		// Token: 0x0600475D RID: 18269 RVA: 0x00179B35 File Offset: 0x00177D35
		public void RegisterCondition(GameCondition cond)
		{
			this.activeConditions.Add(cond);
			cond.startTick = Mathf.Max(cond.startTick, Find.TickManager.TicksGame);
			cond.gameConditionManager = this;
			cond.Init();
		}

		// Token: 0x0600475E RID: 18270 RVA: 0x00179B6C File Offset: 0x00177D6C
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

		// Token: 0x0600475F RID: 18271 RVA: 0x00179BC0 File Offset: 0x00177DC0
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

		// Token: 0x06004760 RID: 18272 RVA: 0x00179C08 File Offset: 0x00177E08
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

		// Token: 0x06004761 RID: 18273 RVA: 0x00179C54 File Offset: 0x00177E54
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

		// Token: 0x06004762 RID: 18274 RVA: 0x00179C9F File Offset: 0x00177E9F
		public bool ConditionIsActive(GameConditionDef def)
		{
			return this.GetActiveCondition(def) != null;
		}

		// Token: 0x06004763 RID: 18275 RVA: 0x00179CAC File Offset: 0x00177EAC
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

		// Token: 0x06004764 RID: 18276 RVA: 0x00179D08 File Offset: 0x00177F08
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

		// Token: 0x06004765 RID: 18277 RVA: 0x00179D6C File Offset: 0x00177F6C
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

		// Token: 0x06004766 RID: 18278 RVA: 0x00179DC0 File Offset: 0x00177FC0
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

		// Token: 0x06004767 RID: 18279 RVA: 0x00179E04 File Offset: 0x00178004
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

		// Token: 0x06004768 RID: 18280 RVA: 0x00179E68 File Offset: 0x00178068
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

		// Token: 0x06004769 RID: 18281 RVA: 0x0017A00C File Offset: 0x0017820C
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

		// Token: 0x0600476A RID: 18282 RVA: 0x0017A058 File Offset: 0x00178258
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

		// Token: 0x0600476B RID: 18283 RVA: 0x0017A0AC File Offset: 0x001782AC
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

		// Token: 0x0600476C RID: 18284 RVA: 0x0017A104 File Offset: 0x00178304
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

		// Token: 0x0600476D RID: 18285 RVA: 0x0017A15C File Offset: 0x0017835C
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

		// Token: 0x0600476E RID: 18286 RVA: 0x0017A1B4 File Offset: 0x001783B4
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

		// Token: 0x0600476F RID: 18287 RVA: 0x0017A20C File Offset: 0x0017840C
		internal bool AllowEnjoyableOutsideNow(Map map)
		{
			GameConditionDef gameConditionDef;
			return this.AllowEnjoyableOutsideNow(map, out gameConditionDef);
		}

		// Token: 0x06004770 RID: 18288 RVA: 0x0017A224 File Offset: 0x00178424
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

		// Token: 0x06004771 RID: 18289 RVA: 0x0017A284 File Offset: 0x00178484
		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (GameCondition saveable in this.activeConditions)
			{
				stringBuilder.AppendLine(Scribe.saver.DebugOutputFor(saveable));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04002BD4 RID: 11220
		public Map ownerMap;

		// Token: 0x04002BD5 RID: 11221
		private List<GameCondition> activeConditions = new List<GameCondition>();

		// Token: 0x04002BD6 RID: 11222
		private const float TextPadding = 6f;
	}
}
