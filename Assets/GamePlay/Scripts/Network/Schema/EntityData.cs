// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 2.0.34
// 

using Colyseus.Schema;
using Action = System.Action;

namespace MyGame.Schema {
	public partial class EntityData : Colyseus.Schema.Schema {
		[Type(0, "string")]
		public string entityId = default(string);

		[Type(1, "ref", typeof(Vect3))]
		public Vect3 position = new Vect3();

		[Type(2, "float32")]
		public float yRotation = default(float);

		/*
		 * Support for individual property change callbacks below...
		 */

		protected event PropertyChangeHandler<string> __entityIdChange;
		public Action OnEntityIdChange(PropertyChangeHandler<string> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.entityId));
			__entityIdChange += __handler;
			if (__immediate && this.entityId != default(string)) { __handler(this.entityId, default(string)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(entityId));
				__entityIdChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<Vect3> __positionChange;
		public Action OnPositionChange(PropertyChangeHandler<Vect3> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.position));
			__positionChange += __handler;
			if (__immediate && this.position != null) { __handler(this.position, null); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(position));
				__positionChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<float> __yRotationChange;
		public Action OnYRotationChange(PropertyChangeHandler<float> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.yRotation));
			__yRotationChange += __handler;
			if (__immediate && this.yRotation != default(float)) { __handler(this.yRotation, default(float)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(yRotation));
				__yRotationChange -= __handler;
			};
		}

		protected override void TriggerFieldChange(DataChange change) {
			switch (change.Field) {
				case nameof(entityId): __entityIdChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
				case nameof(position): __positionChange?.Invoke((Vect3) change.Value, (Vect3) change.PreviousValue); break;
				case nameof(yRotation): __yRotationChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
				default: break;
			}
		}

		protected void InvokePositionChange(Vect3 value, Vect3 previousValue)
		{
			__positionChange?.Invoke(value, previousValue);
		}

		protected void InvokeRotationChange(float value, float previousValue)
		{
			__yRotationChange?.Invoke(value, previousValue);
		}
	}
}
