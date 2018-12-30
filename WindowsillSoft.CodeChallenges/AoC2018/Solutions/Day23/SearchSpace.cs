using System.Collections.Generic;
using System.Linq;

namespace WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day23
{
    public class SearchSpace
    {
        private Drone[] _drones;
        private int _X, _Y, _Z;
        public int Size { get; }
        public (int x, int y, int z) Location => (_X, _Y, _Z);

        public SearchSpace(List<Drone> drones)
        {
            _drones = drones.ToArray();
            _X = drones.Min(p => p.X);
            _Y = drones.Min(p => p.Y);
            _Z = drones.Min(p => p.Z);

            var maxWidth = new[] {
                drones.Max(p => p.X) - _X,
                drones.Max(p => p.Y) - _Y,
                drones.Max(p => p.Z) - _Z }
                .Max();

            //var size = Math.Pow(2, Math.Ceiling(Math.Log(maxWidth, 2)));
            //potential overflow due to Double conversion - switching to silly version instead.
            Size = 1;
            while (Size < maxWidth) Size <<= 1;
        }

        private SearchSpace(Drone[] drones, int x, int y, int z, int size)
        {
            (_X, _Y, _Z, Size) = (x, y, z, size);
            _drones = drones.Where(p => p.OverlapsCube(x, y, z, size)).ToArray();
        }

        internal int BestCoverageEstimate()
        {
            return _drones.Count();
        }

        internal IEnumerable<SearchSpace> Partition()
        {
            yield return new SearchSpace(_drones, _X, _Y, _Z, Size / 2);
            yield return new SearchSpace(_drones, _X + Size / 2, _Y, _Z, Size / 2);
            yield return new SearchSpace(_drones, _X, _Y + Size / 2, _Z, Size / 2);
            yield return new SearchSpace(_drones, _X, _Y, _Z + Size / 2, Size / 2);
            yield return new SearchSpace(_drones, _X + Size / 2, _Y + Size / 2, _Z, Size / 2);
            yield return new SearchSpace(_drones, _X + Size / 2, _Y, _Z + Size / 2, Size / 2);
            yield return new SearchSpace(_drones, _X, _Y + Size / 2, _Z + Size / 2, Size / 2);
            yield return new SearchSpace(_drones, _X + Size / 2, _Y + Size / 2, _Z + Size / 2, Size / 2);
        }

        public override string ToString()
            => $"{(_X, _Y, _Z)},{Size};{_drones.Length}";
    }
}
