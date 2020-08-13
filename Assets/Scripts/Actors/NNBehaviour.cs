using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Barracuda;
using Unity.MLAgents.Policies;

[System.Serializable]
//public abstract class NNBehavior {
public class NNBehaviour {
    public string name;
    public NNModel neuralNetwork;
    public InferenceDevice inferenceDevice;

    //abstract public string GetName();
    //abstract public NNModel GetNeuralNetwork();
    //abstract public InferenceDevice GetInferenceDevice();
}
