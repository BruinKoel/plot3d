using Agent.models.Nodes;
using FireAxe.FireMath.Enviroments;
using FireAxe.FireMath.Enviroments.DataViewers;
using FireAxe.Models;
using System.Diagnostics;

namespace FireAxe.FireMath.Data
{
    public class TrainingFactory
    {
        private static readonly ScalarViewer scalarViewer = new ScalarViewer();
        public static ScalarField GenerateTrainingField(int size)
        {
            // generate a list of points inside a cubic lattice of whole numbers
            // the cube is of width size
            List<Double3m> points = new List<Double3m>();

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    for (int z = 0; z < size; z++)
                    {
                        points.Add(new Double3m(x, y, z));
                    }
                }
            }
            return new ScalarField(points, 1);
        }
        public static SimpleNode Optimise(int generations)
        {
            SimpleNode baseNode = new SimpleNode(scalarViewer.ViewSize, new[] { 39, 44, 54, 34, 27 });
            Random random = new Random();

            int i = 2;
            int maxSize = 3;

            var field = GenerateTrainingField(i);
            List<SimpleNode> nodes = ScalarFieldSimpleENV.Train(512, baseNode, 2, field.DeepCopy());


            while (nodes.First().LastCost > 0 || i < 7)
            {

                int size = ++i;
                if (i > maxSize)
                {
                    if (nodes.First().LastCost < size * size)
                    {
                        maxSize++;
                    }
                    else
                    {
                        field = GenerateTrainingField(size + 1);
                        Debug.WriteLine($"starting {size} rounds on a {size + 1} sized cube");
                        nodes = ScalarFieldSimpleENV.Train(nodes, 3*size, field);
                        maxSize--;
                    }
                    i = 1;
                    //i = (i == 2) ? 1 : i - 2;
                }
                Debug.WriteLine($"starting 3 rounds on a {size} sized cube");
                field = GenerateTrainingField(size);
                nodes = ScalarFieldSimpleENV.Train(nodes, 5, field);
            }


            return nodes.FirstOrDefault();

        }
    }
}
