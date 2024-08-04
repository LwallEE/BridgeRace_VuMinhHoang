// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 2.0.34
// 

using Colyseus.Schema;
using UnityEngine;
using Action = System.Action;

namespace MyGame.Schema {
	public partial class PlayerData : EntityData {
		[Type(4, "string")]
		public string name = default(string);

		[Type(5, "uint8")]
		public byte color = default(byte);

		[Type(6, "uint16")]
		public ushort numberOfBrick = default(ushort);
		
		[Type(7, "string")]
		public string animName = default(string);

		[Type(8, "boolean")] public bool isFall = default(bool);
		
		/*
		 * Support for individual property change callbacks below...
		 */

		protected event PropertyChangeHandler<string> __nameChange;
		public Action OnNameChange(PropertyChangeHandler<string> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.name));
			__nameChange += __handler;
			if (__immediate && this.name != default(string)) { __handler(this.name, default(string)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(name));
				__nameChange -= __handler;
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

		protected event PropertyChangeHandler<ushort> __numberOfBrickChange;
		public Action OnNumberOfBrickChange(PropertyChangeHandler<ushort> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.numberOfBrick));
			__numberOfBrickChange += __handler;
			if (__immediate && this.numberOfBrick != default(ushort)) { __handler(this.numberOfBrick, default(ushort)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(numberOfBrick));
				__numberOfBrickChange -= __handler;
			};
		}
		
		protected event PropertyChangeHandler<string> __animNameChange;
		public Action OnAnimNameChange(PropertyChangeHandler<string> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.animName));
			__animNameChange += __handler;
			if (__immediate && this.animName != default(string)) { __handler(this.animName, default(string)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(animName));
				__animNameChange -= __handler;
			};
		}
		protected event PropertyChangeHandler<bool> __isFallChange;
		public Action OnIsFallChange(PropertyChangeHandler<bool> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.isFall));
			__isFallChange += __handler;
			if (__immediate && this.isFall != default(bool)) { __handler(this.isFall, default(bool)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(this.isFall));
				__isFallChange -= __handler;
			};
		}

		protected override void TriggerFieldChange(DataChange change) {
			
			switch (change.Field) {
				case nameof(yRotation): InvokeRotationChange((float)change.Value, (float)change.PreviousValue);
					break;
				case nameof(position): InvokePositionChange((Vect3)change.Value, (Vect3)change.PreviousValue);
					break;
				case nameof(name): __nameChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
				case nameof(color): __colorChange?.Invoke((byte) change.Value, (byte) change.PreviousValue); break;
				case nameof(numberOfBrick): __numberOfBrickChange?.Invoke((ushort) change.Value, (ushort) change.PreviousValue); break;
				case nameof(animName): __animNameChange?.Invoke((string)change.Value, (string)change.PreviousValue);
					break;
				case nameof(isFall): __isFallChange?.Invoke((bool)change.Value, (bool)change.PreviousValue);
					break;
				default: break;
			}
		}
	}
}
