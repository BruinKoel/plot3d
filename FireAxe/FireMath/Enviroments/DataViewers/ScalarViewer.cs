using FireAxe.Models;

namespace FireAxe.FireMath.Enviroments.DataViewers
{
    public class ScalarViewer
    {
        private Tuple<int, int, int>[][] visorCache;

        public Tuple<int, int, int> position;

        public int ViewSize
        {
            get
            {
                return visorCache.GetLength(0);
            }
        }
        public ScalarViewer()
        {
            Init(3, 10);
        }

        public void MovePosition(Double3m move)
        {
            position =new Tuple<int, int, int>( 
                position.Item1 + (int)move.X , 
                position.Item2 + (int)move.Y, 
                position.Item3 + (int)move.Z);

        }

        public Double3m GetPosition
        {
            get
            {
                return new Double3m(position);
            }
        }
        public Double3m[] GetPositions(int radius = 1)
        {


            /// the GetPositions function generates a list of positions.
            /// the radius defines a ball at position (0,0,0) of radius radius.
            /// The outer shell of any position in the ball is returned. 
            /// radius 0 is positions {(0,0,0)}
            /// radius 1 is positions { (1,0,0), (0,1,0), (0,0,1), (-1,0,0), (0,-1,0), (0,0,-1) }

            List<Double3m> positions = new List<Double3m>();
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    for (int z = -radius; z <= radius; z++)
                    {
                        if (x == 0 && y == 0 && z == 0)
                        {
                            continue;
                        }
                        if ((int)(new Double3m(x, y, z).Length) == radius)
                        {
                            positions.Add(new Double3m(x, y, z));
                        }
                    }
                }
            }
            return positions.ToArray();
        }

        public Double3m[] GetLinePositions(Double3m vector, double length)
        {
            /// The GetLinePostitions returns all positions along vector where the position components are whole numbers and not more distant than length from (0,0,0).

            vector = vector.Normal;
            List<Double3m> positions = new List<Double3m>();
            for (int i = 1; i < length; i++)
            {
                var pos = vector * i;
                positions.Add(pos.Rounded);
            }
            return positions.ToArray();
        }

        public List<Double3m[]> GetViewCloud(int radius, double viewDistance)
        {
            /// the GetViewCloud function returns a list of rays consisting of positions from the GetLinePositions function for every position of the GetPositions function.
            return GetPositions(radius).Select(x => GetLinePositions(x, viewDistance)).ToList();
        }

        public void Init(int radius, double viewDistance)
        {
            position = new Tuple<int, int, int>(0, 0, 0);
            visorCache = GetViewCloud(radius, viewDistance).Select(x =>
                x.Select(y =>
                    new Tuple<int, int, int>((int)y.X, (int)y.X, (int)y.Z))
                .ToArray()).ToArray();
        }


        public List<double> View(ScalarField field)
        {

            /// the View function returns an array of doubles.
            /// for every array of tuples in visorCache you multiply the value of this position in the sclarfield by 1/(2^j) and then add it
            List<double> result = new List<double>();
            
            for (int i = 0; i < visorCache.GetLength(0); i++)
            {
                result.Add(0);
                for (int j = 0; j < visorCache[i].Length; j++)
                {
                    var pos = visorCache[i][j];
                    pos = new (pos.Item1 + position.Item1, pos.Item2 + position.Item2, pos.Item3 + position.Item3);
                    double value = field.GetPoint(pos);
                    double weight = 1d / Math.Pow(2, j+1);
                    result[i] += value * weight;

                }

            }
            return result;

        }
    }
}
