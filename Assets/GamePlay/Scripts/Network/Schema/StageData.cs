// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 2.0.34
// 

using Colyseus.Schema;
using Action = System.Action;

namespace MyGame.Schema {
	public partial class StageData : Colyseus.Schema.Schema {
		[Type(0, "map", typeof(MapSchema<BrickData>))]
		public MapSchema<BrickData> bricks = new MapSchema<BrickData>();

		[Type(1, "array", typeof(ArraySchema<BridgeData>))]
		public ArraySchema<BridgeData> bridges = new ArraySchema<BridgeData>();

		[Type(2, "ref", typeof(Vect3))]
		public Vect3 spawnBrickPosition = new Vect3();

		[Type(3, "uint8")]
		public byte row = default(byte);

		[Type(4, "uint8")]
		public byte column = default(byte);
		
		[Type(5, "ref", typeof(Vect3))]
		public Vect3 stagePosition = new Vect3();
		/*
		 * Support for individual property change callbacks below...
		 */

		protected event PropertyChangeHandler<MapSchema<BrickData>> __bricksChange;
		public Action OnBricksChange(PropertyChangeHandler<MapSchema<BrickData>> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.bricks));
			__bricksChange += __handler;
			if (__immediate && this.bricks != null) { __handler(this.bricks, null); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(bricks));
				__bricksChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<ArraySchema<BridgeData>> __bridgesChange;
		public Action OnBridgesChange(PropertyChangeHandler<ArraySchema<BridgeData>> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.bridges));
			__bridgesChange += __handler;
			if (__immediate && this.bridges != null) { __handler(this.bridges, null); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(bridges));
				__bridgesChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<Vect3> __spawnBrickPositionChange;
		public Action OnSpawnBrickPositionChange(PropertyChangeHandler<Vect3> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.spawnBrickPosition));
			__spawnBrickPositionChange += __handler;
			if (__immediate && this.spawnBrickPosition != null) { __handler(this.spawnBrickPosition, null); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(spawnBrickPosition));
				__spawnBrickPositionChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<byte> __rowChange;
		public Action OnRowChange(PropertyChangeHandler<byte> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.row));
			__rowChange += __handler;
			if (__immediate && this.row != default(byte)) { __handler(this.row, default(byte)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(row));
				__rowChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<byte> __columnChange;
		public Action OnColumnChange(PropertyChangeHandler<byte> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.column));
			__columnChange += __handler;
			if (__immediate && this.column != default(byte)) { __handler(this.column, default(byte)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(column));
				__columnChange -= __handler;
			};
		}

		protected override void TriggerFieldChange(DataChange change) {
			switch (change.Field) {
				case nameof(bricks): __bricksChange?.Invoke((MapSchema<BrickData>) change.Value, (MapSchema<BrickData>) change.PreviousValue); break;
				case nameof(bridges): __bridgesChange?.Invoke((ArraySchema<BridgeData>) change.Value, (ArraySchema<BridgeData>) change.PreviousValue); break;
				//case nameof(spawnBrickPosition): __spawnBrickPositionChange?.Invoke((Vect3) change.Value, (Vect3) change.PreviousValue); break;
				//case nameof(row): __rowChange?.Invoke((byte) change.Value, (byte) change.PreviousValue); break;
				//case nameof(column): __columnChange?.Invoke((byte) change.Value, (byte) change.PreviousValue); break;
				default: break;
			}
		}
	}
}
