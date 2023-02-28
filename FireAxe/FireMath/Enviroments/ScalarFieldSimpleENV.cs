
using Agent.models.Nodes;
using FireAxe.FireMath.Enviroments.DataViewers;
using FireAxe.Models;
using System.Diagnostics;

namespace FireAxe.FireMath.Enviroments
{
    public class ScalarFieldSimpleENV
    {
        public ScalarField field;

        private ScalarViewer scalarViewer;

        public Double3m position;
        private SimpleNode baseNode;
        public float lastCost;



        public ScalarFieldSimpleENV(ScalarField field, Double3m position, SimpleNode baseNode)
        {
            this.baseNode = baseNode;
            this.field = field;
            this.position = position;
            lastCost = float.MaxValue;

            scalarViewer = new ScalarViewer();
        }
        private bool Supported(ScalarField field, Double3m point)
        {
            /// return true if one of the underlying 3x3 points are 0
            if (point.Z.Equals(0))
            {
                return true;
            }
            for (int x = -1; x < 2; x++)
                for (int y = -1; y < 2; y++)
                {
                    Double3m offset = new Double3m(x, y, -1);
                    if (field.GetPoint(point + offset).Equals(0))
                    {
                        return true;
                    }
                }
            return false;
        }

        public List<Double3m> ComputeMoves(ScalarField field, Double3m start, SimpleNode node)
        {
            ScalarField scalarField = field.DeepCopy();

            List<Double3m> moves = new List<Double3m>();
            Double3m lastMove = 1;

            int maxmoves = (int)(field.sum() * 1.5);
            while (lastMove != 0 && moves.Count < maxmoves)
            {
                if (new Double3m(scalarViewer.position) << 0) { break; }
                var output = node.Compute(scalarViewer.View(scalarField).ToArray());
                float maxvalue = 0;
                int maxindex = 0;
                for (int i = 0; i < output.Length; i++)
                {
                    if (output[i] > maxvalue)
                    {
                        maxvalue = output[i];
                        maxindex = i;
                    }
                }

                switch (maxindex)
                {
                    case 1:
                        lastMove = new Double3m(-1, -1, -1);
                        break;
                    case 2:
                        lastMove = new Double3m(-1, -1, 0);
                        break;
                    case 3:
                        lastMove = new Double3m(-1, -1, 1);
                        break;

                    case 4:
                        lastMove = new Double3m(-1, 0, -1);
                        break;
                    case 5:
                        lastMove = new Double3m(-1, 0, 0);
                        break;
                    case 6:
                        lastMove = new Double3m(-1, 0, 1);
                        break;

                    case 7:
                        lastMove = new Double3m(-1, 1, -1);
                        break;
                    case 8:
                        lastMove = new Double3m(-1, 1, 0);
                        break;
                    case 9:
                        lastMove = new Double3m(-1, 1, 1);
                        break;

                    case 10:
                        lastMove = new Double3m(0, -1, -1);
                        break;
                    case 11:
                        lastMove = new Double3m(0, -1, 0);
                        break;
                    case 12:
                        lastMove = new Double3m(0, -1, 1);
                        break;

                    case 13:
                        lastMove = new Double3m(0, 0, -1);
                        break;
                    case 14:
                        lastMove = new Double3m(0, 0, 0);
                        break;
                    case 15:
                        lastMove = new Double3m(0, 0, 1);
                        break;

                    case 16:
                        lastMove = new Double3m(0, 1, -1);
                        break;
                    case 17:
                        lastMove = new Double3m(0, 1, 0);
                        break;
                    case 18:
                        lastMove = new Double3m(0, 1, 1);
                        break;

                    case 19:
                        lastMove = new Double3m(1, -1, -1);
                        break;
                    case 20:
                        lastMove = new Double3m(1, -1, 0);
                        break;
                    case 21:
                        lastMove = new Double3m(1, -1, 1);
                        break;

                    case 22:
                        lastMove = new Double3m(1, 0, -1);
                        break;
                    case 23:
                        lastMove = new Double3m(1, 0, 0);
                        break;
                    case 24:
                        lastMove = new Double3m(1, 0, 1);
                        break;

                    case 25:
                        lastMove = new Double3m(1, 1, -1);
                        break;
                    case 26:
                        lastMove = new Double3m(1, 1, 0);
                        break;
                    case 0:
                        lastMove = new Double3m(1, 1, 1);
                        break;
                }
                moves.Add(lastMove);
                start += lastMove;
                scalarField.SetPoint(start, 0);
                scalarViewer.MovePosition(lastMove);


            }
            return moves;
        }
        public float CalculateCost(ScalarField field, Double3m start, Double3m[] moves)
        {
            {
                float Ztravel = moves.Sum(x => Math.Abs(x.Z));
                float summed = field.sum();
                float cost = 1;
                float faults = 0;

                Double3m current = start;

                field.SetPoint(current, 0);

                foreach (Double3m move in moves)
                {
                    current += move;
                    var value = field.GetPoint(current);
                    if (value.Equals(1))
                    {
                        if (Supported(field, current))
                        {
                            cost += value;
                        }
                        else
                        {
                            faults++;
                        }
                        field.SetPoint(current, 0);
                    }
                    else if (value.Equals(-1) || value.Equals(float.NaN))
                    {
                        faults++;
                    }
                }
                var missed = summed - cost;
                return (Ztravel * faults) + (missed * missed);
            }




        }

        public void Loop()
        {
            this.lastCost = CalculateCost(field, position, ComputeMoves(field, position, baseNode).ToArray());
            this.baseNode.LastCost = this.lastCost;

        }
        public static List<SimpleNode> Train(int count, SimpleNode baseNode, int generations, ScalarField scalarField)
        {
            List<SimpleNode> nodes = new List<SimpleNode>();
            for (int i = 0; i < count; i++)
            {
                nodes.Add(baseNode.ShapeCopy());
            }
            return Train(nodes, generations, scalarField);
        }
        static Random random = new Random();
        public static List<SimpleNode> Train(List<SimpleNode> nodes, int generations, ScalarField scalarField)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Double3m start = 0;

            for (int i = 0; i != generations; i++)
            {
                var batch = nodes.AsParallel().Select(x => new ScalarFieldSimpleENV(scalarField.DeepCopy(random.Next(4)), start, x.DeepCopy())).ToList();
                Parallel.ForEach(batch, i => i.Loop());

                var postbatch = batch.OrderBy(x => x.lastCost);

                Debug.WriteLine($"gen:{i},{stopwatch.Elapsed.TotalSeconds.ToString()}: Best CostScore {postbatch.First().lastCost.ToString()} Worst: {postbatch.Last().lastCost.ToString()} averaging: {postbatch.Average(x => x.lastCost)}");
                nodes = postbatch.Select(x => x.baseNode).ToList();


                int half = (int)Math.Round((float)batch.Count() / 2f);
                float odds = 0.2f;
                float factor = 0.3f;

                Parallel.For(0, half, (k) =>
                {
                    nodes[k + half] = nodes[k].DeepCopy();
                    nodes[k].UpdateWeights(factor, odds);
                });

            }
            stopwatch.Stop();

            return nodes;
        }
    }
}
