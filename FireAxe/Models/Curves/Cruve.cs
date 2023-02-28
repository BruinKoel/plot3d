using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireAxe.Models
{
    public abstract class Curve
    {
        /// <summary>
        /// return Point at <paramref name="T"/>
        /// </summary>
        /// <param name="T"></param>
        /// <returns></returns>
        public abstract Double3m GetPoint(float T);

        /// <summary>
        /// Gets or sets the spatial offset of the <see cref="Curve"/>
        /// </summary>
        public Double3m Offset { get; set; }

        /// <summary>
        /// Recommended Sampling Interval.
        /// </summary>
        public abstract float RecommendedInterval { get; }

        /// <summary>
        /// Returns boundingboxes the confine this <see cref="Curve"/>
        /// </summary>
        public abstract IEnumerable<(Double3m, Double3m)> BoundingBoxes { get; }
    }
}
