using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200170B RID: 5899
	public abstract class QuestNode
	{
		// Token: 0x0600883D RID: 34877 RVA: 0x0030F7F4 File Offset: 0x0030D9F4
		public QuestNode()
		{
			this.myTypeShort = base.GetType().Name;
			if (this.myTypeShort.StartsWith("QuestNode_"))
			{
				this.myTypeShort = this.myTypeShort.Substring("QuestNode_".Length);
			}
		}

		// Token: 0x0600883E RID: 34878 RVA: 0x0030F848 File Offset: 0x0030DA48
		public float SelectionWeight(Slate slate)
		{
			float? value = this.selectionWeight.GetValue(slate);
			if (value == null)
			{
				return 1f;
			}
			return value.GetValueOrDefault();
		}

		// Token: 0x0600883F RID: 34879 RVA: 0x0030F878 File Offset: 0x0030DA78
		public void Run()
		{
			if (DeepProfiler.enabled)
			{
				DeepProfiler.Start(this.ToString());
			}
			try
			{
				this.RunInt();
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception running ",
					base.GetType().Name,
					": ",
					ex,
					"\n\nSlate vars:\n",
					QuestGen.slate.ToString()
				}));
			}
			if (DeepProfiler.enabled)
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06008840 RID: 34880 RVA: 0x0030F90C File Offset: 0x0030DB0C
		public bool TestRun(Slate slate)
		{
			bool result;
			try
			{
				Action<QuestNode, Slate> action;
				if (slate.TryGet<Action<QuestNode, Slate>>("testRunCallback", out action, false) && action != null)
				{
					action(this, slate);
				}
				result = this.TestRunInt(slate);
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception test running ",
					base.GetType().Name,
					": ",
					ex,
					"\n\nSlate vars:\n",
					slate.ToString()
				}));
				result = false;
			}
			finally
			{
			}
			return result;
		}

		// Token: 0x06008841 RID: 34881
		protected abstract void RunInt();

		// Token: 0x06008842 RID: 34882
		protected abstract bool TestRunInt(Slate slate);

		// Token: 0x06008843 RID: 34883 RVA: 0x0014DE58 File Offset: 0x0014C058
		public override string ToString()
		{
			return base.GetType().Name;
		}

		// Token: 0x0400562A RID: 22058
		[Unsaved(false)]
		[TranslationHandle]
		public string myTypeShort;

		// Token: 0x0400562B RID: 22059
		public SlateRef<float?> selectionWeight;
	}
}
