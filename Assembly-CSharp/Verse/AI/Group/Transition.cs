using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x02000ADF RID: 2783
	public class Transition
	{
		// Token: 0x17000A3E RID: 2622
		// (get) Token: 0x060041C3 RID: 16835 RVA: 0x00030F5A File Offset: 0x0002F15A
		public Map Map
		{
			get
			{
				return this.target.Map;
			}
		}

		// Token: 0x060041C4 RID: 16836 RVA: 0x001885CC File Offset: 0x001867CC
		public Transition(LordToil firstSource, LordToil target, bool canMoveToSameState = false, bool updateDutiesIfMovedToSameState = true)
		{
			this.canMoveToSameState = canMoveToSameState;
			this.updateDutiesIfMovedToSameState = updateDutiesIfMovedToSameState;
			this.target = target;
			this.sources = new List<LordToil>();
			this.AddSource(firstSource);
		}

		// Token: 0x060041C5 RID: 16837 RVA: 0x00188630 File Offset: 0x00186830
		public void AddSource(LordToil source)
		{
			if (this.sources.Contains(source))
			{
				Log.Error("Double-added source to Transition: " + source, false);
				return;
			}
			if (!this.canMoveToSameState && this.target == source)
			{
				Log.Error("Transition !canMoveToSameState and target is source: " + source, false);
			}
			this.sources.Add(source);
		}

		// Token: 0x060041C6 RID: 16838 RVA: 0x0018868C File Offset: 0x0018688C
		public void AddSources(IEnumerable<LordToil> sources)
		{
			foreach (LordToil source in sources)
			{
				this.AddSource(source);
			}
		}

		// Token: 0x060041C7 RID: 16839 RVA: 0x001886D4 File Offset: 0x001868D4
		public void AddSources(params LordToil[] sources)
		{
			for (int i = 0; i < sources.Length; i++)
			{
				this.AddSource(sources[i]);
			}
		}

		// Token: 0x060041C8 RID: 16840 RVA: 0x00030F67 File Offset: 0x0002F167
		public void AddTrigger(Trigger trigger)
		{
			this.triggers.Add(trigger);
		}

		// Token: 0x060041C9 RID: 16841 RVA: 0x00030F75 File Offset: 0x0002F175
		public void AddPreAction(TransitionAction action)
		{
			this.preActions.Add(action);
		}

		// Token: 0x060041CA RID: 16842 RVA: 0x00030F83 File Offset: 0x0002F183
		public void AddPostAction(TransitionAction action)
		{
			this.postActions.Add(action);
		}

		// Token: 0x060041CB RID: 16843 RVA: 0x001886F8 File Offset: 0x001868F8
		public void SourceToilBecameActive(Transition transition, LordToil previousToil)
		{
			for (int i = 0; i < this.triggers.Count; i++)
			{
				this.triggers[i].SourceToilBecameActive(transition, previousToil);
			}
		}

		// Token: 0x060041CC RID: 16844 RVA: 0x00188730 File Offset: 0x00186930
		public bool CheckSignal(Lord lord, TriggerSignal signal)
		{
			for (int i = 0; i < this.triggers.Count; i++)
			{
				if (this.triggers[i].ActivateOn(lord, signal))
				{
					if (this.triggers[i].filters != null)
					{
						bool flag = true;
						for (int j = 0; j < this.triggers[i].filters.Count; j++)
						{
							if (!this.triggers[i].filters[j].AllowActivation(lord, signal))
							{
								flag = false;
								break;
							}
						}
						if (!flag)
						{
							goto IL_E7;
						}
					}
					if (DebugViewSettings.logLordToilTransitions)
					{
						Log.Message(string.Concat(new object[]
						{
							"Transitioning ",
							this.sources,
							" to ",
							this.target,
							" by trigger ",
							this.triggers[i],
							" on signal ",
							signal
						}), false);
					}
					this.Execute(lord);
					return true;
				}
				IL_E7:;
			}
			return false;
		}

		// Token: 0x060041CD RID: 16845 RVA: 0x0018883C File Offset: 0x00186A3C
		public void Execute(Lord lord)
		{
			if (!this.canMoveToSameState && this.target == lord.CurLordToil)
			{
				return;
			}
			for (int i = 0; i < this.preActions.Count; i++)
			{
				this.preActions[i].DoAction(this);
			}
			if (this.target != lord.CurLordToil || this.updateDutiesIfMovedToSameState)
			{
				lord.GotoToil(this.target);
			}
			for (int j = 0; j < this.postActions.Count; j++)
			{
				this.postActions[j].DoAction(this);
			}
		}

		// Token: 0x060041CE RID: 16846 RVA: 0x001888D4 File Offset: 0x00186AD4
		public override string ToString()
		{
			string text = this.sources.NullOrEmpty<LordToil>() ? "null" : this.sources[0].ToString();
			int num = (this.sources == null) ? 0 : this.sources.Count;
			string text2 = (this.target == null) ? "null" : this.target.ToString();
			return string.Concat(new object[]
			{
				text,
				"(",
				num,
				")->",
				text2
			});
		}

		// Token: 0x04002D3B RID: 11579
		public List<LordToil> sources;

		// Token: 0x04002D3C RID: 11580
		public LordToil target;

		// Token: 0x04002D3D RID: 11581
		public List<Trigger> triggers = new List<Trigger>();

		// Token: 0x04002D3E RID: 11582
		public List<TransitionAction> preActions = new List<TransitionAction>();

		// Token: 0x04002D3F RID: 11583
		public List<TransitionAction> postActions = new List<TransitionAction>();

		// Token: 0x04002D40 RID: 11584
		public bool canMoveToSameState;

		// Token: 0x04002D41 RID: 11585
		public bool updateDutiesIfMovedToSameState = true;
	}
}
