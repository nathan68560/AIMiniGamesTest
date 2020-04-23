using UnityEngine;
using System;

public class NeuralNetwork
{
    private int[] layer;
    private Layer[] layers;
    private float learningRate;
    private bool useBias = true;

    public NeuralNetwork(int[] layer, float learningRate = 0.0033f, bool useBias = true)
    {
        this.useBias = useBias;
        this.learningRate = learningRate;
        this.layer = new int[layer.Length];
        for (int i = 0; i < layer.Length; i++)
        {
            this.layer[i] = layer[i];
        }

        layers = new Layer[layer.Length - 1];

        for (int i = 0; i < layers.Length; i++)
        {
            layers[i] = new Layer(layer[i], layer[i + 1]);
        }
    }

    public float[] feedForward(float[] inputs)
    {
        layers[0].feedForward(inputs, useBias);
        for (int i = 1; i < layers.Length; i++)
        {
            layers[i].feedForward(layers[i - 1].outputs, useBias);
        }
        return layers[layers.Length - 1].outputs;
    }

    public void backProp(float[] expected)
    {
        for (int i = layers.Length - 1; i >= 0; i--)
        {
            if (i == layers.Length - 1)
            {
                layers[i].backPropOutput(expected);
            }
            else
            {
                layers[i].backPropHiddenLayer(layers[i + 1].gamma, layers[i + 1].weights);
            }
        }

        for (int i = 1; i < layers.Length; i++)
        {
            layers[i].updateWeights(learningRate);
        }
    }

    public class Layer
    {
        int numberInputs; // num of neurons in prev layer
        int numberOutputs; // num of neurons in current layer

        public float[] outputs;
        public float[] inputs;
        public float[,] weights;
        public float[,] weightsDelta;
        public float[] bias;
        public float[] biasDelta;
        public float[] gamma;
        public float[] error;

        private System.Random random = new System.Random(System.DateTime.Today.Millisecond);

        public Layer(int numberInputs, int numberOutputs)
        {
            this.numberInputs = numberInputs;
            this.numberOutputs = numberOutputs;

            outputs = new float[numberOutputs];
            inputs = new float[numberInputs];
            weights = new float[numberOutputs, numberInputs];
            weightsDelta = new float[numberOutputs, numberInputs];
            bias = new float[numberOutputs];
            biasDelta = new float[numberOutputs];
            error = new float[numberOutputs];
            gamma = new float[numberOutputs];

            initWeights();
        }

        public void initWeights()
        {
            for (int i = 0; i < numberOutputs; i++)
            {
                for (int j = 0; j < numberInputs; j++)
                {
                    weights[i, j] = (float)random.NextDouble() - 0.5f;
                }
                bias[i] = (float)random.NextDouble() - 0.5f;
            }
        }

        public float[] feedForward(float[] inputs, bool useBias)
        {
            this.inputs = inputs;

            for (int i = 0; i < numberOutputs; i++)
            {
                outputs[i] = 0;

                for (int j = 0; j < numberInputs; j++)
                {
                    outputs[i] += inputs[j] * weights[i, j] + ((useBias)?bias[i]:0);
                }
                outputs[i] = (float)Math.Tanh(outputs[i]);
            }

            return outputs;
        }

        public float tanhDerivative(float value)
        {
            return 1 - (value * value);
        }

        public void backPropOutput(float[] expected)
        {
            for (int i = 0; i < numberOutputs; i++)
            {
                error[i] = 2*(outputs[i] - expected[i]); // dC/da
                gamma[i] = error[i] * tanhDerivative(outputs[i]); // (dC/da) * (da/dz)
                for (int j = 0; j < numberInputs; j++)
                {
                    weightsDelta[i, j] = gamma[i] * inputs[j];
                } // weights delta = (dC/da) * (da/dz) * (a)
                biasDelta[i] = gamma[i]; // The input doesn't affect the bias whatsoever
            }
        }

        public void backPropHiddenLayer(float[] gammaForward, float[,] weigthsForward)
        {
            for (int i = 0; i < numberOutputs; i++)
            {
                gamma[i] = 0;
                for (int j = 0; j < gammaForward.Length; j++)
                {
                    gamma[i] += gammaForward[j] * weigthsForward[j, i]; 
                } // dC/da : sum of all previous mutliplied each by it's connected weights
                gamma[i] *= tanhDerivative(outputs[i]); // (dC/da) * (da/dz)

                for (int j = 0; j < numberInputs; j++)
                {
                    weightsDelta[i, j] = gamma[i] * inputs[j];
                } // weights delta = (dC/da) * (da/dz) * (a)
                biasDelta[i] = gamma[i]; // The input doesn't affect the bias whatsoever
            }
        }

        public void updateWeights(float learningRate)
        {
            for (int i = 0; i < numberOutputs; i++)
            {
                for (int j = 0; j < numberInputs; j++)
                {
                    weights[i, j] -= weightsDelta[i, j] * learningRate;
                }
                bias[i] = biasDelta[i] * learningRate;
            }
        }
    }
}