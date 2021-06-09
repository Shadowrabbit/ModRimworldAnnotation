using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001168 RID: 4456
	public struct SignalArgs
	{
		// Token: 0x17000F55 RID: 3925
		// (get) Token: 0x060061FD RID: 25085 RVA: 0x0004369F File Offset: 0x0004189F
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x17000F56 RID: 3926
		// (get) Token: 0x060061FE RID: 25086 RVA: 0x000436A7 File Offset: 0x000418A7
		public IEnumerable<NamedArgument> Args
		{
			get
			{
				if (this.count == 0)
				{
					yield break;
				}
				if (this.args != null)
				{
					int num;
					for (int i = 0; i < this.args.Length; i = num + 1)
					{
						yield return this.args[i];
						num = i;
					}
				}
				else
				{
					yield return this.arg1;
					if (this.count >= 2)
					{
						yield return this.arg2;
					}
					if (this.count >= 3)
					{
						yield return this.arg3;
					}
					if (this.count >= 4)
					{
						yield return this.arg4;
					}
				}
				yield break;
			}
		}

		// Token: 0x060061FF RID: 25087 RVA: 0x001E9E7C File Offset: 0x001E807C
		public SignalArgs(SignalArgs args)
		{
			this.count = args.count;
			this.arg1 = args.arg1;
			this.arg2 = args.arg2;
			this.arg3 = args.arg3;
			this.arg4 = args.arg4;
			this.args = args.args;
		}

		// Token: 0x06006200 RID: 25088 RVA: 0x000436BC File Offset: 0x000418BC
		public SignalArgs(NamedArgument arg1)
		{
			this.count = 1;
			this.arg1 = arg1;
			this.arg2 = default(NamedArgument);
			this.arg3 = default(NamedArgument);
			this.arg4 = default(NamedArgument);
			this.args = null;
		}

		// Token: 0x06006201 RID: 25089 RVA: 0x000436F7 File Offset: 0x000418F7
		public SignalArgs(NamedArgument arg1, NamedArgument arg2)
		{
			this.count = 2;
			this.arg1 = arg1;
			this.arg2 = arg2;
			this.arg3 = default(NamedArgument);
			this.arg4 = default(NamedArgument);
			this.args = null;
		}

		// Token: 0x06006202 RID: 25090 RVA: 0x0004372D File Offset: 0x0004192D
		public SignalArgs(NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
		{
			this.count = 3;
			this.arg1 = arg1;
			this.arg2 = arg2;
			this.arg3 = arg3;
			this.arg4 = default(NamedArgument);
			this.args = null;
		}

		// Token: 0x06006203 RID: 25091 RVA: 0x0004375E File Offset: 0x0004195E
		public SignalArgs(NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4)
		{
			this.count = 4;
			this.arg1 = arg1;
			this.arg2 = arg2;
			this.arg3 = arg3;
			this.arg4 = arg4;
			this.args = null;
		}

		// Token: 0x06006204 RID: 25092 RVA: 0x001E9ED4 File Offset: 0x001E80D4
		public SignalArgs(params NamedArgument[] args)
		{
			this.count = args.Length;
			if (args.Length > 4)
			{
				this.arg1 = default(NamedArgument);
				this.arg2 = default(NamedArgument);
				this.arg3 = default(NamedArgument);
				this.arg4 = default(NamedArgument);
				this.args = new NamedArgument[args.Length];
				for (int i = 0; i < args.Length; i++)
				{
					this.args[i] = args[i];
				}
				return;
			}
			if (args.Length == 1)
			{
				this.arg1 = args[0];
				this.arg2 = default(NamedArgument);
				this.arg3 = default(NamedArgument);
				this.arg4 = default(NamedArgument);
			}
			else if (args.Length == 2)
			{
				this.arg1 = args[0];
				this.arg2 = args[1];
				this.arg3 = default(NamedArgument);
				this.arg4 = default(NamedArgument);
			}
			else if (args.Length == 3)
			{
				this.arg1 = args[0];
				this.arg2 = args[1];
				this.arg3 = args[2];
				this.arg4 = default(NamedArgument);
			}
			else if (args.Length == 4)
			{
				this.arg1 = args[0];
				this.arg2 = args[1];
				this.arg3 = args[2];
				this.arg4 = args[3];
			}
			else
			{
				this.arg1 = default(NamedArgument);
				this.arg2 = default(NamedArgument);
				this.arg3 = default(NamedArgument);
				this.arg4 = default(NamedArgument);
			}
			this.args = null;
		}

		// Token: 0x06006205 RID: 25093 RVA: 0x001EA078 File Offset: 0x001E8278
		public bool TryGetArg(int index, out NamedArgument arg)
		{
			if (index < 0 || index >= this.count)
			{
				arg = default(NamedArgument);
				return false;
			}
			if (this.args != null)
			{
				arg = this.args[index];
			}
			else if (index == 0)
			{
				arg = this.arg1;
			}
			else if (index == 1)
			{
				arg = this.arg2;
			}
			else if (index == 2)
			{
				arg = this.arg3;
			}
			else
			{
				arg = this.arg4;
			}
			return true;
		}

		// Token: 0x06006206 RID: 25094 RVA: 0x001EA0FC File Offset: 0x001E82FC
		public bool TryGetArg(string name, out NamedArgument arg)
		{
			if (this.count == 0)
			{
				arg = default(NamedArgument);
				return false;
			}
			if (this.args != null)
			{
				for (int i = 0; i < this.args.Length; i++)
				{
					if (this.args[i].label == name)
					{
						arg = this.args[i];
						return true;
					}
				}
			}
			else
			{
				if (this.count >= 1 && this.arg1.label == name)
				{
					arg = this.arg1;
					return true;
				}
				if (this.count >= 2 && this.arg2.label == name)
				{
					arg = this.arg2;
					return true;
				}
				if (this.count >= 3 && this.arg3.label == name)
				{
					arg = this.arg3;
					return true;
				}
				if (this.count >= 4 && this.arg4.label == name)
				{
					arg = this.arg4;
					return true;
				}
			}
			arg = default(NamedArgument);
			return false;
		}

		// Token: 0x06006207 RID: 25095 RVA: 0x001EA218 File Offset: 0x001E8418
		public bool TryGetArg<T>(string name, out T arg)
		{
			NamedArgument namedArgument;
			if (!this.TryGetArg(name, out namedArgument) || !(namedArgument.arg is T))
			{
				arg = default(T);
				return false;
			}
			arg = (T)((object)namedArgument.arg);
			return true;
		}

		// Token: 0x06006208 RID: 25096 RVA: 0x001EA258 File Offset: 0x001E8458
		public NamedArgument GetArg(int index)
		{
			NamedArgument result;
			if (this.TryGetArg(index, out result))
			{
				return result;
			}
			throw new ArgumentOutOfRangeException("index");
		}

		// Token: 0x06006209 RID: 25097 RVA: 0x001EA27C File Offset: 0x001E847C
		public NamedArgument GetArg(string name)
		{
			NamedArgument result;
			if (this.TryGetArg(name, out result))
			{
				return result;
			}
			throw new ArgumentException("Could not find arg named " + name);
		}

		// Token: 0x0600620A RID: 25098 RVA: 0x001EA2A8 File Offset: 0x001E84A8
		public T GetArg<T>(string name)
		{
			T result;
			if (this.TryGetArg<T>(name, out result))
			{
				return result;
			}
			throw new ArgumentException("Could not find arg named " + name + " of type " + typeof(T).Name);
		}

		// Token: 0x0600620B RID: 25099 RVA: 0x001EA2E8 File Offset: 0x001E84E8
		public TaggedString GetFormattedText(TaggedString text)
		{
			if (this.count == 0)
			{
				return text.Formatted(Array.Empty<NamedArgument>());
			}
			if (this.args != null)
			{
				return text.Formatted(this.args);
			}
			if (this.count == 1)
			{
				return text.Formatted(this.arg1);
			}
			if (this.count == 2)
			{
				return text.Formatted(this.arg1, this.arg2);
			}
			if (this.count == 3)
			{
				return text.Formatted(this.arg1, this.arg2, this.arg3);
			}
			return text.Formatted(this.arg1, this.arg2, this.arg3, this.arg4);
		}

		// Token: 0x0600620C RID: 25100 RVA: 0x001EA390 File Offset: 0x001E8590
		public TaggedString GetTranslatedText(string textKey)
		{
			if (this.count == 0)
			{
				return textKey.Translate();
			}
			if (this.args != null)
			{
				return textKey.Translate(this.args);
			}
			if (this.count == 1)
			{
				return textKey.Translate(this.arg1);
			}
			if (this.count == 2)
			{
				return textKey.Translate(this.arg1, this.arg2);
			}
			if (this.count == 3)
			{
				return textKey.Translate(this.arg1, this.arg2, this.arg3);
			}
			return textKey.Translate(this.arg1, this.arg2, this.arg3, this.arg4);
		}

		// Token: 0x0600620D RID: 25101 RVA: 0x001EA434 File Offset: 0x001E8634
		public void Add(NamedArgument arg)
		{
			if (this.args != null)
			{
				NamedArgument[] array = new NamedArgument[this.args.Length + 1];
				for (int i = 0; i < this.args.Length; i++)
				{
					array[i] = this.args[i];
				}
				array[array.Length - 1] = arg;
				this.args = array;
				this.count = this.args.Length;
				return;
			}
			if (this.count == 0)
			{
				this.arg1 = arg;
			}
			else if (this.count == 1)
			{
				this.arg2 = arg;
			}
			else if (this.count == 2)
			{
				this.arg3 = arg;
			}
			else if (this.count == 3)
			{
				this.arg4 = arg;
			}
			else
			{
				this.args = new NamedArgument[5];
				this.args[0] = this.arg1;
				this.args[1] = this.arg2;
				this.args[2] = this.arg3;
				this.args[3] = this.arg4;
				this.args[4] = arg;
				this.arg1 = default(NamedArgument);
				this.arg2 = default(NamedArgument);
				this.arg3 = default(NamedArgument);
				this.arg4 = default(NamedArgument);
			}
			this.count++;
		}

		// Token: 0x0600620E RID: 25102 RVA: 0x0004378B File Offset: 0x0004198B
		public void Add(NamedArgument arg1, NamedArgument arg2)
		{
			this.Add(arg1);
			this.Add(arg2);
		}

		// Token: 0x0600620F RID: 25103 RVA: 0x0004379B File Offset: 0x0004199B
		public void Add(NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
		{
			this.Add(arg1);
			this.Add(arg2);
			this.Add(arg3);
		}

		// Token: 0x06006210 RID: 25104 RVA: 0x000437B2 File Offset: 0x000419B2
		public void Add(NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4)
		{
			this.Add(arg1);
			this.Add(arg2);
			this.Add(arg3);
			this.Add(arg4);
		}

		// Token: 0x06006211 RID: 25105 RVA: 0x001EA594 File Offset: 0x001E8794
		public void Add(params NamedArgument[] args)
		{
			for (int i = 0; i < args.Length; i++)
			{
				this.Add(args[i]);
			}
		}

		// Token: 0x06006212 RID: 25106 RVA: 0x001EA5BC File Offset: 0x001E87BC
		public void Add(SignalArgs args)
		{
			if (args.count == 0)
			{
				return;
			}
			if (args.args != null)
			{
				for (int i = 0; i < args.args.Length; i++)
				{
					this.Add(args.args[i]);
				}
				return;
			}
			if (args.count >= 1)
			{
				this.Add(args.arg1);
			}
			if (args.count >= 2)
			{
				this.Add(args.arg2);
			}
			if (args.count >= 3)
			{
				this.Add(args.arg3);
			}
			if (args.count >= 4)
			{
				this.Add(args.arg4);
			}
		}

		// Token: 0x04004194 RID: 16788
		private int count;

		// Token: 0x04004195 RID: 16789
		private NamedArgument arg1;

		// Token: 0x04004196 RID: 16790
		private NamedArgument arg2;

		// Token: 0x04004197 RID: 16791
		private NamedArgument arg3;

		// Token: 0x04004198 RID: 16792
		private NamedArgument arg4;

		// Token: 0x04004199 RID: 16793
		private NamedArgument[] args;
	}
}
