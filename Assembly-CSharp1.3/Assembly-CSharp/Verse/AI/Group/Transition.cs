using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x02000678 RID: 1656
	public class Transition
	{
		// Token: 0x170008D1 RID: 2257
		// (get) Token: 0x06002EE2 RID: 12002 RVA: 0x001176AF File Offset: 0x001158AF
		public Map Map
		{
			get
			{
				return this.target.Map;
			}
		}

		// Token: 0x06002EE3 RID: 12003 RVA: 0x001176BC File Offset: 0x001158BC
		public Transition(LordToil firstSource, LordToil target, bool canMoveToSameState = false, bool updateDutiesIfMovedToSameState = true)
		{
			this.canMoveToSameState = canMoveToSameState;
			this.updateDutiesIfMovedToSameState = updateDutiesIfMovedToSameState;
			this.target = target;
			this.sources = new List<LordToil>();
			this.AddSource(firstSource);
		}

		// Token: 0x06002EE4 RID: 12004 RVA: 0x00117720 File Offset: 0x00115920
		public void AddSource(LordToil source)
		{
			if (this.sources.Contains(source))
			{
				Log.Error("Double-added source to Transition: " + source);
				return;
			}
			if (!this.canMoveToSameState && this.target == source)
			{
				Log.Error("Transition !canMoveToSameState and target is source: " + source);
			}
			this.sources.Add(source);
		}

		// Token: 0x06002EE5 RID: 12005 RVA: 0x0011777C File Offset: 0x0011597C
		public void AddSources(IEnumerable<LordToil> sources)
		{
			foreach (LordToil source in sources)
			{
				this.AddSource(source);
			}
		}

		// Token: 0x06002EE6 RID: 12006 RVA: 0x001177C4 File Offset: 0x001159C4
		public void AddSources(params LordToil[] sources)
		{
			for (int i = 0; i < sources.Length; i++)
			{
				this.AddSource(sources[i]);
			}
		}

		// Token: 0x06002EE7 RID: 12007 RVA: 0x001177E8 File Offset: 0x001159E8
		public void AddTrigger(Trigger trigger)
		{
			this.triggers.Add(trigger);
		}

		// Token: 0x06002EE8 RID: 12008 RVA: 0x001177F6 File Offset: 0x001159F6
		public void AddPreAction(TransitionAction action)
		{
			this.preActions.Add(action);
		}

		// Token: 0x06002EE9 RID: 12009 RVA: 0x00117804 File Offset: 0x00115A04
		public void AddPostAction(TransitionAction action)
		{
			this.postActions.Add(action);
		}

		// Token: 0x06002EEA RID: 12010 RVA: 0x00117814 File Offset: 0x00115A14
		public void SourceToilBecameActive(Transition transition, LordToil previousToil)
		{
			for (int i = 0; i < this.triggers.Count; i++)
			{
				this.triggers[i].SourceToilBecameActive(transition, previousToil);
			}
		}

		// Token: 0x06002EEB RID: 12011 RVA: 0x0011784C File Offset: 0x00115A4C
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
							goto IL_E6;
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
						}));
					}
					this.Execute(lord);
					return true;
				}
				IL_E6:;
			}
			return false;
		}

		// Token: 0x06002EEC RID: 12012 RVA: 0x00117958 File Offset: 0x00115B58
		public void Execute(Lord lord)
		{
			if (!this.canMoveToSameState && this.target == lord.CurLordToil)
			{
				return;
			}
			for (int i = 0; i < this.preActions.Count; i++)
			{
				try
				{
					this.preActions[i].DoAction(this);
				}
				catch (Exception arg)
				{
					Log.Error("Error in lord's preAction: " + arg);
				}
			}
			if (this.target != lord.CurLordToil || this.updateDutiesIfMovedToSameState)
			{
				lord.GotoToil(this.target);
			}
			for (int j = 0; j < this.postActions.Count; j++)
			{
				try
				{
					this.postActions[j].DoAction(this);
				}
				catch (Exception arg2)
				{
					Log.Error("Error in lord's postAction: " + arg2);
				}
			}
		}

		// Token: 0x06002EED RID: 12013 RVA: 0x00117A38 File Offset: 0x00115C38
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

		// Token: 0x04001CAA RID: 7338
		public List<LordToil> sources;

		// Token: 0x04001CAB RID: 7339
		public LordToil target;

		// Token: 0x04001CAC RID: 7340
		public List<Trigger> triggers = new List<Trigger>();

		// Token: 0x04001CAD RID: 7341
		public List<TransitionAction> preActions = new List<TransitionAction>();

		// Token: 0x04001CAE RID: 7342
		public List<TransitionAction> postActions = new List<TransitionAction>();

		// Token: 0x04001CAF RID: 7343
		public bool canMoveToSameState;

		// Token: 0x04001CB0 RID: 7344
		public bool updateDutiesIfMovedToSameState = true;
	}
}
