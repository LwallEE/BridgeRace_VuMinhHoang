// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 2.0.34
// 

using Colyseus.Schema;
using Action = System.Action;

namespace MyGame.Schema {
	public partial class BridgeData : Colyseus.Schema.Schema {
		[Type(0, "ref", typeof(Vect3))]
		public Vect3 bridgePosition = new Vect3();

		[Type(1, "map", typeof(MapSchema<BridgeSlotData>))]
		public MapSchema<BridgeSlotData> bridgeSlots = new MapSchema<BridgeSlotData>();

		[Type(2, "uint8")]
		public byte numberOfSlot = default(byte);

		/*
		 * Support for individual property change callbacks below...
		 */

		protected event PropertyChangeHandler<Vect3> __bridgePositionChange;
		public Action OnBridgePositionChange(PropertyChangeHandler<Vect3> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.bridgePosition));
			__bridgePositionChange += __handler;
			if (__immediate && this.bridgePosition != null) { __handler(this.bridgePosition, null); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(bridgePosition));
				__bridgePositionChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<MapSchema<BridgeSlotData>> __bridgeSlotsChange;
		public Action OnBridgeSlotsChange(PropertyChangeHandler<MapSchema<BridgeSlotData>> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.bridgeSlots));
			__bridgeSlotsChange += __handler;
			if (__immediate && this.bridgeSlots != null) { __handler(this.bridgeSlots, null); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(bridgeSlots));
				__bridgeSlotsChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<byte> __numberOfSlotChange;
		public Action OnNumberOfSlotChange(PropertyChangeHandler<byte> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.numberOfSlot));
			__numberOfSlotChange += __handler;
			if (__immediate && this.numberOfSlot != default(byte)) { __handler(this.numberOfSlot, default(byte)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(numberOfSlot));
				__numberOfSlotChange -= __handler;
			};
		}

		protected override void TriggerFieldChange(DataChange change) {
			switch (change.Field) {
				//case nameof(bridgePosition): __bridgePositionChange?.Invoke((Vect3) change.Value, (Vect3) change.PreviousValue); break;
				case nameof(bridgeSlots): __bridgeSlotsChange?.Invoke((MapSchema<BridgeSlotData>) change.Value, (MapSchema<BridgeSlotData>) change.PreviousValue); break;
				//case nameof(numberOfSlot): __numberOfSlotChange?.Invoke((byte) change.Value, (byte) change.PreviousValue); break;
				default: break;
			}
		}
	}
}
