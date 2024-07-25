// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 2.0.34
// 

using Colyseus.Schema;
using Action = System.Action;

namespace MyGame.Schema {
	public partial class BrickData : EntityData {
		[Type(4, "boolean")]
		public bool isActive = default(bool);

		[Type(5, "uint8")]
		public byte color = default(byte);

		[Type(6, "string")]
		public string ownerId = default(string);

		[Type(7, "uint8")]
		public byte state = default(byte);

		/*
		 * Support for individual property change callbacks below...
		 */

		protected event PropertyChangeHandler<bool> __isActiveChange;
		public Action OnIsActiveChange(PropertyChangeHandler<bool> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.isActive));
			__isActiveChange += __handler;
			if (__immediate && this.isActive != default(bool)) { __handler(this.isActive, default(bool)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(isActive));
				__isActiveChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<byte> __colorChange;
		public Action OnColorChange(PropertyChangeHandler<byte> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.color));
			__colorChange += __handler;
			if (__immediate && this.color != default(byte)) { __handler(this.color, default(byte)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(color));
				__colorChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<string> __ownerIdChange;
		public Action OnOwnerIdChange(PropertyChangeHandler<string> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.ownerId));
			__ownerIdChange += __handler;
			if (__immediate && this.ownerId != default(string)) { __handler(this.ownerId, default(string)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(ownerId));
				__ownerIdChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<byte> __stateChange;
		public Action OnStateChange(PropertyChangeHandler<byte> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.state));
			__stateChange += __handler;
			if (__immediate && this.state != default(byte)) { __handler(this.state, default(byte)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(state));
				__stateChange -= __handler;
			};
		}

		protected override void TriggerFieldChange(DataChange change) {
			switch (change.Field) {
				case nameof(isActive): __isActiveChange?.Invoke((bool) change.Value, (bool) change.PreviousValue); break;
				case nameof(color): __colorChange?.Invoke((byte) change.Value, (byte) change.PreviousValue); break;
				case nameof(ownerId): __ownerIdChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
				case nameof(state): __stateChange?.Invoke((byte) change.Value, (byte) change.PreviousValue); break;
				default: break;
			}
		}
	}
}
