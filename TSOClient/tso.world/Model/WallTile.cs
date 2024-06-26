﻿namespace FSO.LotView.Model
{
    public struct WallTile
    {
        public WallSegments Segments;

        //the patterns of each side of the tile's wall.
        public ushort TopLeftPattern;
        public ushort TopRightPattern;
        public ushort BottomLeftPattern;
        public ushort BottomRightPattern;

        //the style of the wall at the top left and top right. bottom left and bottom right are to be obtained from the tiles in those directions.
        //1 generally means "normal wall". Not sure how to deal with cutouts while keeping these as "normal wall".
        public ushort TopLeftStyle;
        public ushort TopRightStyle;

        public bool TopLeftSolid
        {
            get {
                return (Segments & WallSegments.TopLeft) > 0 && !TopLeftDoor;
            }
        }

        public bool TopRightSolid
        {
            get
            {
                return (Segments & WallSegments.TopRight) > 0 && !TopRightDoor;
            }
        }

        public bool TopLeftThick
        {
            get
            {
                return (Segments & WallSegments.TopLeft) > 0 && (TopLeftStyle == 1 || TopLeftStyle == 255);
            }
        }

        public bool TopRightThick
        {
            get
            {
                return (Segments & WallSegments.TopRight) > 0 && (TopRightStyle == 1 || TopRightStyle == 255);
            }
        }

        //Placement never seems to be not zero, so it's not included for now.
        //don't include below information when serializing as it will be generated by the client upon load

        public ushort ObjSetTLStyle; //custom styles set by objects (windows and doors)
        public ushort ObjSetTRStyle;

        public bool TopLeftDoor;
        public bool TopRightDoor;

        public WallSegments OccupiedWalls;
    }
}
