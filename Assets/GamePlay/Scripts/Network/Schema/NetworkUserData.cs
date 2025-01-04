using System;
using Colyseus.Schema;

namespace MyGame.Schema
{
    public partial class NetworkUserData : Colyseus.Schema.Schema
    {
        [Colyseus.Schema.Type(0, "string")]
		public string userId = default(string);

		[Colyseus.Schema.Type(1, "string")]
		public string userName = default(string);

		[Colyseus.Schema.Type(2, "uint32")]
		public uint score = default(uint);
		
		[Colyseus.Schema.Type(3, "boolean")]
		public bool isReady = default(bool);

		[Colyseus.Schema.Type(4, "string")]
		public string avatar = default(string);
		/*
		 * Support for individual property change callbacks below...
		 */

		
		protected event PropertyChangeHandler<bool> __isReadyChange;
		public Action OnIsReadyChange(PropertyChangeHandler<bool> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.isReady));
			__isReadyChange += __handler;
			if (__immediate && this.isReady != default(bool)) { __handler(this.isReady, default(bool)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(this.isReady));
				__isReadyChange -= __handler;
			};
		}

		protected override void TriggerFieldChange(DataChange change) {
			
			switch (change.Field) {
				case nameof(isReady): __isReadyChange?.Invoke((bool)change.Value, (bool)change.PreviousValue);
					break;
				default: break;
			}
		}
	}
    
}