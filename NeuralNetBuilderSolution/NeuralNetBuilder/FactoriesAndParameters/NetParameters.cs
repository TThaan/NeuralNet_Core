﻿using NeuralNetBuilder.FactoriesAndParameters.JsonConverters;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace NeuralNetBuilder.FactoriesAndParameters
{
    public interface INetParameters : IParametersBase
    {
        string FileName { get; set; }   // redundant?
        ObservableCollection<ILayerParameters> LayerParametersCollection { get; set; }
        WeightInitType WeightInitType { get; set; }
    }

    [Serializable]
    public class NetParameters : ParametersBase, INetParameters
    {
        public string FileName { get; set; } = Path.GetTempFileName();   // redundant?
        [JsonProperty(ItemConverterType = typeof(GenericJsonConverter<LayerParameters>))]
        public ObservableCollection<ILayerParameters> LayerParametersCollection { get; set; } = new ObservableCollection<ILayerParameters>();
        public WeightInitType WeightInitType { get; set; } = WeightInitType.None;
    }
}
