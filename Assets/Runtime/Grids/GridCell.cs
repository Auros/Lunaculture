using System;

namespace Lunaculture.Grids
{
    public struct GridCell
    {
        public long Id { get; }
        
        public float X { get; }
        
        public float Y { get; }

        public GridCell(float x, float y)
        {
            X = x;
            Y = y;
            var xInt = BitConverter.SingleToInt32Bits(x);
            var yInt = BitConverter.SingleToInt32Bits(y);
            long combined = (long)xInt << 32 | (uint)yInt;
            Id = combined;
        }

        public GridCell(long id)
        {
            int xInt = (int)(id & int.MaxValue);
            int yInt = (int)(id >> 32);
            X = BitConverter.Int32BitsToSingle(xInt);
            Y = BitConverter.Int32BitsToSingle(yInt);
            Id = id;
        }

        public void Deconstruct(out float x, out float y, out long id)
        {
            x = X;
            y = Y;
            id = Id;
        }
    }
}