﻿using System.IO;
using FSO.LotView.Model;
using FSO.SimAntics.Model;
using FSO.SimAntics.Model.TSOPlatform;

namespace FSO.SimAntics.NetPlay.Model.Commands
{
    public class VMNetMoveObjectCmd : VMNetCommandBodyAbstract
    {
        public short ObjectID;
        public short x;
        public short y;
        public sbyte level;
        public Direction dir;

        public override bool Execute(VM vm, VMAvatar caller)
        {
            VMEntity obj = vm.GetObjectById(ObjectID);
            if (!vm.TS1) {
                if (obj == null || caller == null || (obj is VMAvatar) ||
                    (caller.AvatarState.Permissions < VMTSOAvatarPermissions.Admin
                    && obj.IsUserMovable(vm.Context, false) != VMPlacementError.Success))
                    return false;
                if (!vm.PlatformState.Validator.CanMoveObject(caller, obj)) return false;
            } else if (obj == null) return false;
            var result = obj.SetPosition(new LotTilePos(x, y, level), dir, vm.Context, VMPlaceRequestFlags.UserPlacement);
            if (result.Status == VMPlacementError.Success)
            {
                obj.MultitileGroup.ExecuteEntryPoint(11, vm.Context); //User Placement

                vm.SignalChatEvent(new VMChatEvent(caller, VMChatEventType.Arch,
                    caller?.Name ?? "Unknown",
                    vm.GetUserIP(caller?.PersistID ?? 0),
                    "moved " + obj.ToString() +" to (" + x / 16f + ", " + y / 16f + ", " + level + ")"
                ));

                return true;
            } else
            {
                return false;
            }
        }

        #region VMSerializable Members

        public override void SerializeInto(BinaryWriter writer)
        {
            base.SerializeInto(writer);
            writer.Write(ObjectID);
            writer.Write(x);
            writer.Write(y);
            writer.Write(level);
            writer.Write((byte)dir);
        }

        public override void Deserialize(BinaryReader reader)
        {
            base.Deserialize(reader);
            ObjectID = reader.ReadInt16();
            x = reader.ReadInt16();
            y = reader.ReadInt16();
            level = reader.ReadSByte();
            dir = (Direction)reader.ReadByte();
        }

        #endregion
    }
}
