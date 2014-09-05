using System;
using JetBrains.Annotations;

namespace widemeadows.MachineLearning.Classification.Scores.Probabilities.Combiners
{
    /// <summary>
    /// Class EvidenceCombiner.
    /// </summary>
    public sealed class LogEvidenceCombiner : IEvidenceCombiner
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="LogEvidenceCombiner"/> class from being created.
        /// </summary>
        private LogEvidenceCombiner() { }

        /// <summary>
        /// The default factory, lazy-evaluated
        /// </summary>
        private static readonly Lazy<IEvidenceCombinerFactory> DefaultFactoryLazy = new Lazy<IEvidenceCombinerFactory>(() => new InternalFactory());

        /// <summary>
        /// Gets the default.
        /// </summary>
        /// <value>The default.</value>
        [NotNull]
        public static IEvidenceCombinerFactory Factory { get { return DefaultFactoryLazy.Value; } }

        /// <summary>
        /// The 1st term, used for numerator and denominator
        /// </summary>
        private double _term1;

        /// <summary>
        /// The 2nd term, used for the denominator
        /// </summary>
        private double _term2;

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            _term1 = _term2 = 0;
        }

        /// <summary>
        /// Aggregates the specified probability.
        /// </summary>
        /// <param name="p">The p.</param>
        public void Combine(IProbability p)
        {
            var value = p.Value;

            _term1 += Math.Log(value);
            _term2 += Math.Log(1.0D - value);
        }

        /// <summary>
        /// Aggregates the specified probability.
        /// </summary>
        /// <param name="p">The p.</param>
        public void Combine(ILogProbability p)
        {
            var value = p.Value;

            _term1 += value;
            _term2 += Math.Log(1.0D - Math.Exp(value));
        }

        /// <summary>
        /// Calculates the combined probability.
        /// </summary>
        /// <returns>IProbability.</returns>
        [NotNull]
        public IProbability Calculate()
        {
            // Combined probability                                 
            // ====================                                 
            // p = a/(a+b)                                      (1) 
            //  for a = p1*p2*...pn                                 
            //      b = (1-p1)(1-p2)*...*(1-pn)                     
            //                                                      
            // Logarithm rules                                      
            // ===============                                      
            // p1*p2*...*pn = log(p1)+log(p2)+...+log(pn)       (2) 
            // a            = exp(log(a))                       (3) 
            // log(b/a)     = log(b) - log(a)                   (4) 
            // log(a+b)     = log(a) + log(b/a + 1)             (5) 
            //                if a,b positive                       
            //                                                      
            // Transformation                                       
            // ==============                                       
            // inserting (4) in (5) using (3)                       
            // log(a+b) = log(a)+log(exp(log(b)-log(a))+1)      (6) 
            //                                                      
            // inserting (6) in (1) using (3)                       
            // p = a/exp(log(a) + log(exp(log(b)-log(a)) + 1))  (7) 
            //                                                      
            // taking the log of (7)                                
            // log(p) = log(a)-(log(a)+log(exp(log(b)-log(a))+1))   
            //        = log(a)- log(a)-log(exp(log(b)-log(a))+1)    
            //        = -log(exp(log(b)-log(a))+1)              (8) 
            //                                                      
            // raising e to (8)                                     
            // p =  exp(-log(exp(log(b)-log(a))+1))                 
            //   = 1/exp(log(exp(log(b)-log(a))+1))                 
            //   = 1/(exp(log(b)-log(a))+1)                    (10) 
            //                                                      
            //                                              q.e.d.  

            var probability = 1.0D/(1.0D + Math.Exp(_term2 - _term1));
            return new Probability(probability);
        }

        /// <summary>
        /// Class Factory. This class cannot be inherited.
        /// </summary>
        private sealed class InternalFactory : IEvidenceCombinerFactory
        {
            /// <summary>
            /// Creates this instance.
            /// </summary>
            /// <returns>IEvidenceCombiner.</returns>
            public IEvidenceCombiner Create()
            {
                return new LogEvidenceCombiner();
            }
        }
    }
}
