﻿using MatrixExtensions;
using NeuralNetBuilder.CostFunctions;
using System.Linq;
using System.Threading.Tasks;

namespace NeuralNetBuilder
{
    public interface ILearningNet : IBaseNet
    {
        ILearningLayer[] Layers { get; }
        ICostFunction CostFunction { get; }
        float CurrentTotalCost { get; }
        Task PropagateBackAsync(float[] target);
        Task AdjustWeightsAndBiasesAsync(float learningRate);
        INet GetNet();
    }

    public class LearningNet : ILearningNet
    {
        #region ctor & fields

        internal LearningNet() { }

        #endregion

        #region IBaseNet

        public async Task FeedForwardAsync(float[] input)
        {
            await Task.Run(() =>
            {
                Layers[0].ProcessInput(input);
                Output = Layers.Last().Output;
            });
        }

        #endregion

        #region ILearningNet

        public ILearningLayer[] Layers { get; internal set; }
        public float[] Output { get; internal set; }
        public ICostFunction CostFunction { get; internal set; }
        public float CurrentTotalCost { get; internal set; }
        public async Task PropagateBackAsync(float[] target)
        {
            await Task.Run(() =>
            {
                Layers.Last().ProcessDelta(target, CostFunction);
                CurrentTotalCost = CostFunction.GetTotalCost(Layers.Last().Output, target);
            });
        }
        public async Task AdjustWeightsAndBiasesAsync(float learningRate)
        {
            await Task.Run(() =>
            {
                Layers.ElementAt(1).AdaptWeightsAndBiases(learningRate);
            });
        }
        public INet GetNet()
        {
            return FactoriesAndParameters.NetFactory.GetNet(this);
        }

        #endregion

        #region ILoggable

        public string LoggableName => "LearningNet";
        public string ToLog()
        {
            string result = LoggableName;

            result += $" (Layers: {Layers.Length}";
            result += $", {nameof(CostFunction)}: {CostFunction.GetType().Name}";
            result += $", {nameof(CurrentTotalCost)}: {CurrentTotalCost})\n";

            foreach (var layer in Layers)
            {
                result += $"\n{layer.ToLog()}";
            }

            result += $"{Output?.ToLog(nameof(Output))}";
            return result;
        }

        #endregion
    }
}
