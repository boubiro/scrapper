using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using scrapper.Scrapper.Entities.Mechanics;

namespace scrapper.Scrapper.Maps
{
    public class MapData : IEnumerable
    {
        private readonly DataPoint[] _dataPoints;

        public MapData(uint dataPointCount)
        {
            _dataPoints = new DataPoint[dataPointCount];
        }

        public MapData()
        {
            _dataPoints = new DataPoint[uint.MaxValue];
        }

        public DataPoint this[int i] => _dataPoints[i];

        public uint Length { get; private set; }

        public IEnumerator GetEnumerator()
        {
            return _dataPoints.GetEnumerator();
        }

        public void Add(DataPoint dataPoint)
        {
            AddRange(new[] {dataPoint});
        }

        public void AddRange(IEnumerable<DataPoint> dataPoints)
        {
            foreach (var dataPoint in dataPoints) _dataPoints[Length++] = dataPoint;
        }

        public struct DataPoint
        {
            public Vector2 Position;
            public EMechanicType Type;
            public IMechanicSettings Settings;
        }
    }
}